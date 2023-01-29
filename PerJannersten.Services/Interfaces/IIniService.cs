using IniParser.Model;
using PerJannersten.Services.Data.Abstraction;

namespace PerJannersten.Services.Interfaces;

public interface IIniService
{
    string Read(string section, string key, string iniString);
    void Write(IniData iniData, string path);
    T Parse<T>(string iniFilePath) where T : new();
    void Save(string path, params IIni[] iniViewModels);
}