namespace PerJannersten.Services.Data.Attributes;

public class BwsTableAttribute: Attribute
{
    public string Name { get; set; }

    public BwsTableAttribute(string name)
    {
        Name = name;
    }
}