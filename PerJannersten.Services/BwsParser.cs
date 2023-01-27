using System.Data;
using System.Data.OleDb;
using System.Reflection;
using PerJannersten.Services.Data.Attributes;
using PerJannersten.Services.Interfaces;

namespace PerJannersten.Services;

public class BwsParser: IBwsParser
{
    public T Parse<T>(string path, object defaultObject = null) where T: new()
    {
        path = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={path};Persist Security Info=True";
        T obj = new();
        Type objectType = obj.GetType();
        IEnumerable<PropertyInfo> properties = objectType.GetProperties();
        List<string> additionalQueries = new();
        using OleDbConnection connection = new (path);
        connection.Open();
        
        foreach (PropertyInfo property in properties)
        {
            BwsTableAttribute bwsTableAttribute = property.GetCustomAttribute<BwsTableAttribute>();
            if (bwsTableAttribute is null) continue;
            string table = bwsTableAttribute.Name;
            
            string query = $"SELECT * FROM [{table}]";
            OleDbDataReader reader = new OleDbCommand(query, connection).ExecuteReader();
            reader.Read();
            
            object propertyValue = property.GetValue(obj);
            if (propertyValue is null) continue;
            
            PropertyInfo[] internalProperties = propertyValue.GetType().GetProperties();
            foreach (PropertyInfo internalProperty in internalProperties)
            {
                BwsColumnAttribute bwsColumnAttribute = internalProperty.GetCustomAttribute<BwsColumnAttribute>();
                if (bwsColumnAttribute is null) continue;
                object value = null;
                string columnName = bwsColumnAttribute.Name;
                
                if (!string.IsNullOrEmpty(bwsColumnAttribute.Table))
                {
                    // if another table
                    using OleDbConnection internalConnection = new (path);
                    internalConnection.Open();
                    string internalQuery = $"SELECT * FROM [{bwsColumnAttribute.Table}]";
                    OleDbDataReader internalReader = new OleDbCommand(internalQuery, internalConnection).ExecuteReader();
                    internalReader.Read();
                    ParseValueOrDefault(internalReader, bwsColumnAttribute.Table);
                    internalReader.Close();
                    internalReader.Dispose();
                    internalConnection.Close();
                    internalConnection.Dispose();
                }
                else
                {
                    ParseValueOrDefault(reader, table);
                }
                if (value is DBNull)
                {
                    if (defaultObject is not null)
                    {
                        object propValue = property.GetValue(defaultObject);
                        value = internalProperty.GetValue(propValue);
                    }
                }
                internalProperty.SetValue(propertyValue, Convert.ChangeType(value, internalProperty.PropertyType));
                
                void ParseValueOrDefault(OleDbDataReader dataReader, string tableName)
                {
                    try
                    {
                        value = dataReader.GetFieldValue<object>(columnName);
                    }
                    catch (IndexOutOfRangeException) // not found this column
                    {
                        additionalQueries.Add($"ALTER TABLE [{tableName}] ADD {columnName} {GetSqlType(value)}");
                        if (defaultObject is not null)
                        {
                            object propValue = property.GetValue(defaultObject);
                            value = internalProperty.GetValue(propValue);
                        }
                    }
                }
            }
            reader.Close();
            reader.Dispose();
        }
        foreach (string additionalQuery in additionalQueries) // invoke additional sql
        {
            OleDbCommand additionalCommand = new (additionalQuery, connection);
            additionalCommand.ExecuteNonQuery();
        }
        
        connection.Close();
        connection.Dispose();
        return obj;
    }

    string GetSqlType(object value)
    {
        if (value is bool) return "BIT";
        if (value is string) return "VARCHAR";
        return "INTEGER";
    }
}