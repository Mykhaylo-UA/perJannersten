namespace PerJannersten.Services.Data.Attributes;

public class IniFieldAttribute: Attribute
{
    public string Name { get; set; }
    public string Section { get; set; }

    public IniFieldAttribute(string name)
    {
        Name = name;
    }
    public IniFieldAttribute(string name, string section)
    {
        Name = name;
        Section = section;
    }
}