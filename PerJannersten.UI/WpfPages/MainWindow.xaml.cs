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
        SettingWindow window = new()
        {
            DefaultSetting = defaultSetting,
            Owner = this,
            SettingViewModel = defaultSetting
                ? _settingService.GetDefaultSetting(_globalState.DefaultFullPath)
                : _settingService.GetSetting(_globalState.BwsPath, _globalState.DefaultFullPath)
        };
        window.ShowDialog();
    }


    void OpenAdditionalCurrentSettings(object sender, RoutedEventArgs e) => OpenAdditionalSetting(false);
    void OpenAdditionalDefaultSettings(object sender, RoutedEventArgs e) => OpenAdditionalSetting(true);

    void OpenAdditionalSetting(bool defaultSetting)
    {
        AdditionalSettingWindow window = new()
        {
            DefaultSetting = defaultSetting,
            Owner = this,
            AdditionalSettingViewModel = defaultSetting
                ? _additionalSettingService.GetDefaultAdditionalSetting(_globalState.DefaultFullPath)
                : _additionalSettingService.GetAdditionalSetting(_globalState.BwsPath, _globalState.DefaultFullPath)
        };
        window.ShowDialog();
    }
}