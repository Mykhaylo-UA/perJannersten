using IniParser.Model;

namespace PerJannersten.Services.Interfaces;

public interface IIniService
{
    string Read(string section, string key, string iniString);
    void Write(IniData iniData, string path);
}