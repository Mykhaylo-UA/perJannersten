namespace PerJannersten.Services.Interfaces;

public interface IIniParser
{
    T Parse<T>(string iniString) where T : new();
}