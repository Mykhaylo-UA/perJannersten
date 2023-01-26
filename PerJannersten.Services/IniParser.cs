using System.Reflection;
using PerJannersten.Services.Data.Attributes;
using PerJannersten.Services.Interfaces;

namespace PerJannersten.Services;

public class IniParser : IIniParser
{
    readonly IIniService _iniService;

    public IniParser(IIniService iniService)
    {
        _iniService = iniService;
    }

    public T Parse<T>(string iniString) where T : new()
    {
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

                string iniField = GetIni(section, iniFieldAttribute.Name, iniString);
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

    string GetIni(string section, string key, string iniString) => _iniService.Read(section, key, iniString);
}