using System.Windows;
using PerJannersten.ViewModel;

namespace PerJannersten.UI.WpfPages;

public partial class AdditionalSettingWindow : Window
{
    public AdditionalSettingViewModel SettingViewModel { get; set; } = new();
    public bool DefaultSetting { get; set; }
    public AdditionalSettingWindow()
    {
        InitializeComponent();
    }
}