using System.IO;
using System.Windows;
using PerJannersten.UiServices.Interfaces;

namespace PerJannersten.UI.WpfPages;

public partial class MainWindow : Window
{
    readonly ISettingService _settingService;
    readonly IAdditionalSettingService _additionalSettingService;
    readonly GlobalState _globalState;

    public MainWindow(ISettingService settingService, GlobalState globalState,
        IAdditionalSettingService additionalSettingService)
    {
        _settingService = settingService;
        _globalState = globalState;
        _additionalSettingService = additionalSettingService;
        InitializeComponent();
    }

    void OpenCurrentSettings(object sender, RoutedEventArgs e) => OpenSetting(false);
    void OpenDefaultSettings(object sender, RoutedEventArgs e) => OpenSetting(true);

    void OpenSetting(bool defaultSetting)
    {
        string path = Path.Combine(_globalState.Path, _globalState.DefaultSettingFileName);
        SettingWindow window = new()
        {
            DefaultSetting = defaultSetting,
            Owner = this,
            SettingViewModel = defaultSetting
                ? _settingService.GetDefaultSetting(path)
                : _settingService.GetSetting(_globalState.BwsPath, path)
        };
        window.ShowDialog();
    }


    void OpenAdditionalCurrentSettings(object sender, RoutedEventArgs e) => OpenAdditionalSetting(false);
    void OpenAdditionalDefaultSettings(object sender, RoutedEventArgs e) => OpenAdditionalSetting(true);

    void OpenAdditionalSetting(bool defaultSetting)
    {
        string path = Path.Combine(_globalState.Path, _globalState.DefaultSettingFileName);
        AdditionalSettingWindow window = new()
        {
            DefaultSetting = defaultSetting,
            Owner = this,
            AdditionalSettingViewModel = defaultSetting
                ? _additionalSettingService.GetDefaultAdditionalSetting(path)
                : _additionalSettingService.GetAdditionalSetting(_globalState.BwsPath, path)
        };
        window.ShowDialog();
    }
}