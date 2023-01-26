namespace PerJannersten.Services.Interfaces;

public interface IBwsParser
{
    T Parse<T>(string path, object defaultObject = null) where T: new();
}