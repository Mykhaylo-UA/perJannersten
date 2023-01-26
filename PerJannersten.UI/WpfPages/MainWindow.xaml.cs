using System.Windows;

namespace PerJannersten.UI.WpfPages;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    void OpenCurrentSettings(object sender, RoutedEventArgs e)
    {
        SettingWindow window = new()
        {
            DefaultSetting = false,
            Owner = this
        };
        window.ShowDialog();
    }
    void OpenDefaultSettings(object sender, RoutedEventArgs e)
    {
        SettingWindow window = new()
        {
            DefaultSetting = true,
            Owner = this
        };
        window.ShowDialog();
    }
}