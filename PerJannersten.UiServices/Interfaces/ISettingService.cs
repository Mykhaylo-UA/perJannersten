using PerJannersten.ViewModel;

namespace PerJannersten.UiServices.Interfaces;

public interface ISettingService
{
    SettingViewModel GetDefaultSetting(string iniFilePath);
    void SaveDefaultSetting(SettingViewModel setting, string iniFilePath);
    SettingViewModel GetSetting(string bwsFilePath, string iniFilePath);
    void SaveSetting(SettingViewModel setting, string bwsFilePath);
}