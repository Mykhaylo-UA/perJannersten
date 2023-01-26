using PerJannersten.ViewModel;

namespace PerJannersten.UiServices.Interfaces;

public interface ISettingService
{
    SettingViewModel GetDefaultSetting(string pathFile);
    void SaveDefaultSetting(SettingViewModel settingViewModel, string path);
    SettingViewModel GetSetting(string filePath, string iniString);
    void SaveSetting(SettingViewModel settingViewModel, string path);
}