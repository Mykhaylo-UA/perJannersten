namespace PerJannersten.Services.Data.Attributes;

public class IniSectionAttribute: Attribute
{
    public string Name { get; set; }

    public IniSectionAttribute(string name)
    {
        Name = name;
    }
}