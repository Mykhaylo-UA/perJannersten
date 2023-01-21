using PerJannersten.ViewModel;

namespace PerJannersten.UiServices.Interfaces;

public interface ISettingService
{
    SettingViewModel GetDefaultSetting(string pathFile);
    void SaveSetting(SettingViewModel settingViewModel, string path);
}