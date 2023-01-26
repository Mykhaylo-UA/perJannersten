namespace PerJannersten.Services.Data.Attributes;

public class BwsColumnAttribute: Attribute
{
    public string Name { get; set; }
    public string Table { get; set; }

    public BwsColumnAttribute(string name)
    {
        Name = name;
    }
    
    public BwsColumnAttribute(string name, string table)
    {
        Name = name;
        Table = table;
    }
}