using System.Data.OleDb;
using IniParser.Model;
using PerJannersten.Services.Interfaces;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;

namespace PerJannersten.UiServices;

public class SettingService: ISettingService
{
    readonly IIniParser _iniParser;
    readonly IIniService _iniService;
    readonly IBwsParser _bwsParser;

    public SettingService(IIniParser iniParser, IIniService iniService, IBwsParser bwsParser)
    {
        _iniParser = iniParser;
        _iniService = iniService;
        _bwsParser = bwsParser;
    }

    public SettingViewModel GetDefaultSetting(string iniString)
    {
        SettingViewModel defaultSetting = _iniParser.Parse<SettingViewModel>(iniString);
        return FormatSetting(defaultSetting);
    }

    public SettingViewModel GetSetting(string path, string iniString)
    {
        SettingViewModel defaultSetting = GetDefaultSetting(iniString);
        SettingViewModel setting = _bwsParser.Parse<SettingViewModel>(path, defaultSetting);
        return FormatSetting(setting);
    }
    SettingViewModel FormatSetting(SettingViewModel setting)
    {
        Timing timing = setting.Timing;
        timing.UseTimer = timing.ChangeTime != 0 || timing.PlayTime != 0 || timing.RegistrationTime != 0;
        TD td = setting.TD;
        td.Checked = td.Pin != 0;
        return setting;
    }
    
    public void SaveSetting(SettingViewModel settingViewModel, string path)
    {
        path = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={path};Persist Security Info=True";
        Dictionary<string, List<string>> data = settingViewModel.GetUpdateSqlString();
        using OleDbConnection connection = new (path);
        connection.Open();
        foreach (KeyValuePair<string,List<string>> pair in data)
        {
            string query = $"UPDATE [{pair.Key}] SET {string.Join(", ", pair.Value)}";
            OleDbCommand command = new (query, connection);
            command.ExecuteNonQuery();
        }
        connection.Close();
        connection.Dispose();
    }

    public void SaveDefaultSetting(SettingViewModel settingViewModel, string path)
    {
        string BoolToString(bool value) => value ? "1" : "0";
        Dictionary<(string, string), object> fieldsWithValue = settingViewModel.GetIniDictionary();
        IniData iniData = new();
        foreach (KeyValuePair<(string, string),object> o in fieldsWithValue)
        {
            if (o.Value is bool value)
            {
                iniData[o.Key.Item1][o.Key.Item2] = $"{BoolToString(value)}";
                continue;
            }
            iniData[o.Key.Item1][o.Key.Item2] = $"{o.Value}";
        }
        _iniService.Write(iniData, path);
    }
}