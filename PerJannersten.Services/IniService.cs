using System.Reflection;
using IniParser;
using IniParser.Model;
using PerJannersten.Services.Data.Abstraction;
using PerJannersten.Services.Data.Attributes;
using PerJannersten.Services.Interfaces;

namespace PerJannersten.Services;

public class IniService : IIniService
{
    readonly FileIniDataParser _dataParser;

    public IniService()
    {
        _dataParser = new();
    }

    public void Write(IniData iniData, string path)
    {
        _dataParser.WriteFile(path, iniData);
    }

    public string Read(string section, string key, string iniString)
    {
        IniData data = _dataParser.Parser.Parse(iniString);
        KeyData result = data.Sections.FirstOrDefault(s => s.SectionName == section)?.Keys
            .FirstOrDefault(p => p.KeyName == key);
        if (result is not null) return result.Value;
        result = data.Global.FirstOrDefault(p => p.KeyName == key);
        return result?.Value;
    }
    
    public T Parse<T>(string iniFilePath) where T : new()
    {
        using Stream fileStream = new FileStream(iniFilePath, FileMode.Open);
        using StreamReader reader = new(fileStream);
        string content = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        fileStream.Close();
        fileStream.Dispose();
        
        T obj = new();
        Type objectType = obj.GetType();
        IniSectionAttribute mainIniSectionAttribute = objectType.GetCustomAttribute<IniSectionAttribute>();
        if (mainIniSectionAttribute is not null)
        {
            string section = mainIniSectionAttribute.Name;
            return (T)GetIniField(obj, section);
        }

        IEnumerable<PropertyInfo> properties = objectType.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            IniSectionAttribute iniSectionAttribute = property.GetCustomAttribute<IniSectionAttribute>();
            if (iniSectionAttribute is null) continue;
            string section = iniSectionAttribute.Name;
            object propertyValue = property.GetValue(obj);
            if (propertyValue is null) continue;
            property.SetValue(obj, GetIniField(propertyValue, section));
        }

        return obj;

        object GetIniField(object o, string section)
        {
            PropertyInfo[] internalProperties = o.GetType().GetProperties();
            foreach (PropertyInfo internalProperty in internalProperties)
            {
                IniFieldAttribute iniFieldAttribute = internalProperty.GetCustomAttribute<IniFieldAttribute>();
                if (iniFieldAttribute is null) continue;
                if (!string.IsNullOrEmpty(iniFieldAttribute.Section))
                {
                    section = iniFieldAttribute.Section;
                }

                string iniField = Read(section, iniFieldAttribute.Name, content);
                if (string.IsNullOrEmpty(iniField)) iniField = "0";
                object value;
                if (internalProperty.PropertyType == typeof(bool)) value = iniField != "0";
                else if (internalProperty.PropertyType == typeof(byte?)) value = byte.Parse(iniField);
                else if (internalProperty.PropertyType == typeof(int)) value = int.Parse(iniField);
                else value = Convert.ChangeType(iniField, internalProperty.PropertyType);
                internalProperty.SetValue(o, value);
            }

            return o;
        }
    }

    public void Save(IIni iniViewModel, string path)
    {
        string BoolToString(bool value) => value ? "1" : "0";
        Dictionary<(string, string), object> fieldsWithValue = iniViewModel.GetIniDictionary();
        IniData iniData = new();
        foreach (KeyValuePair<(string, string),object> o in fieldsWithValue)
        {
            if (o.Value is bool value)
            {
                iniData[o.Key.Item1][o.Key.Item2] = $"{BoolToString(value)}";
                continue;
            }
            iniData[o.Key.Item1][o.Key.Item2] = $"{o.Value}";
        }
        Write(iniData, path);
    }
}