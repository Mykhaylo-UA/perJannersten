using PerJannersten.ViewModel;

namespace PerJannersten.UiServices.Interfaces;

public interface IAdditionalSettingService
{
    AdditionalSettingViewModel GetDefaultAdditionalSetting(string iniFilePath);
    void SaveDefaultAdditionalSetting(AdditionalSettingViewModel additionalSetting, string path);
    AdditionalSettingViewModel GetAdditionalSetting(string bwsFilePath, string iniFilePath);
    void SaveAdditionalSetting(AdditionalSettingViewModel additionalSetting, string path);
}