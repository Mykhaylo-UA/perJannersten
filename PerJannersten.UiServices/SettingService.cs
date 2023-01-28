using PerJannersten.Services.Interfaces;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;
using PerJannersten.ViewModel.Setting;

namespace PerJannersten.UiServices;

public class SettingService : ISettingService
{
    readonly IIniService _iniService;
    readonly IBwsService _bwsService;

    public SettingService(IIniService iniService, IBwsService bwsService)
    {
        _iniService = iniService;
        _bwsService = bwsService;
    }

    public SettingViewModel GetDefaultSetting(string iniFilePath)
    {
        SettingViewModel defaultSetting = _iniService.Parse<SettingViewModel>(iniFilePath);
        return FormatSetting(defaultSetting);
    }

    public SettingViewModel GetSetting(string bwsFilePath, string iniFilePath)
    {
        SettingViewModel defaultSetting = GetDefaultSetting(iniFilePath);
        SettingViewModel setting = _bwsService.Parse<SettingViewModel>(bwsFilePath, defaultSetting);
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

    public void SaveSetting(SettingViewModel settingViewModel, string bwsFilePath)
    {
        _bwsService.Save(settingViewModel, bwsFilePath);
    }

    public void SaveDefaultSetting(SettingViewModel settingViewModel, string iniFilePath)
    {
        _iniService.Save(settingViewModel, iniFilePath);
    }
}