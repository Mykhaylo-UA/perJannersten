using PerJannersten.Services.Interfaces;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;

namespace PerJannersten.UiServices;

public class AdditionalSettingService : IAdditionalSettingService
{
    readonly IIniService _iniService;
    readonly IBwsService _bwsService;

    public AdditionalSettingService(IIniService iniService, IBwsService bwsService)
    {
        _iniService = iniService;
        _bwsService = bwsService;
    }

    public AdditionalSettingViewModel GetDefaultAdditionalSetting(string iniFilePath)
    {
        AdditionalSettingViewModel defaultSetting = _iniService.Parse<AdditionalSettingViewModel>(iniFilePath);
        return defaultSetting;
    }

    public void SaveDefaultAdditionalSetting(AdditionalSettingViewModel additionalSetting, string path)
    {
        _iniService.Save(additionalSetting, path);
    }

    public AdditionalSettingViewModel GetAdditionalSetting(string bwsFilePath, string iniFilePath)
    {
        AdditionalSettingViewModel defaultSetting = GetDefaultAdditionalSetting(iniFilePath);
        AdditionalSettingViewModel setting = _bwsService.Parse<AdditionalSettingViewModel>(bwsFilePath, defaultSetting);
        return setting;
    }

    public void SaveAdditionalSetting(AdditionalSettingViewModel additionalSetting, string path)
    {
        _bwsService.Save(additionalSetting, path);
    }
}