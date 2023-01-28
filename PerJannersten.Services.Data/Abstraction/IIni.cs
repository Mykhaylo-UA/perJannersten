namespace PerJannersten.Services.Data.Abstraction;

public interface IIni
{
    Dictionary<(string, string), object> GetIniDictionary();
}