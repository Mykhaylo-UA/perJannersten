using System.IO;
using System.Windows;
using PerJannersten.UiServices.Interfaces;

namespace PerJannersten.UI.WpfPages;

public partial class MainWindow : Window
{
    readonly ISettingService _settingService;
    readonly GlobalState _globalState;

    public MainWindow(ISettingService settingService, GlobalState globalState)
    {
        _settingService = settingService;
        _globalState = globalState;
        InitializeComponent();
    }

    void OpenCurrentSettings(object sender, RoutedEventArgs e)
    {
        string path = Path.Combine(_globalState.Path, _globalState.DefaultSettingFileName);
        SettingWindow window = new()
        {
            DefaultSetting = false,
            Owner = this,
            SettingViewModel = _settingService.GetSetting(_globalState.BwsPath, path)
        };
        window.ShowDialog();
        
    }
    void OpenDefaultSettings(object sender, RoutedEventArgs e)
    {
        string path = Path.Combine(_globalState.Path, _globalState.DefaultSettingFileName);
        SettingWindow window = new()
        {
            DefaultSetting = true,
            Owner = this,
            SettingViewModel = _settingService.GetDefaultSetting(path)
        };
        window.ShowDialog();
    }
}