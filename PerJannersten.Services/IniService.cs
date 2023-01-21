using IniParser;
using IniParser.Model;
using PerJannersten.Services.Interfaces;

namespace PerJannersten.Services;

public class IniService : IIniService
{
    readonly FileIniDataParser _dataParser;

    public IniService()
    {
        _dataParser = new();
    }
    
    public void Write(IniData iniData, string path)
    {
        _dataParser.WriteFile(path,iniData);
    }
    public string Read(string section, string key, string iniString)
    {
        IniData data = _dataParser.Parser.Parse(iniString);
        var result = data.Sections.FirstOrDefault(s => s.SectionName == section)?.Keys.FirstOrDefault(p => p.KeyName == key);
        if (result is not null) return result.Value;
        result = data.Global.FirstOrDefault(p => p.KeyName == key);
        return result?.Value;
    }
}