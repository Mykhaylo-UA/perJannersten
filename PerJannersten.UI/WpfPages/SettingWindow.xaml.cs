using System.Windows;
using PerJannersten.ViewModel;

namespace PerJannersten.UI.WpfPages;

public partial class SettingWindow : Window
{
    public SettingViewModel SettingViewModel { get; set; } = new();
    public bool DefaultSetting { get; set; }
    public SettingWindow()
    {
        InitializeComponent();
    }
}