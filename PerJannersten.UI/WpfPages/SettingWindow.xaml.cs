using System.IO;
using System.Windows;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Win32;
using PerJannersten.UI.Pages;

namespace PerJannersten.UI.WpfPages;

public partial class SettingWindow : Window
{
    public bool DefaultSetting { get; set; }
    public SettingWindow()
    {
        InitializeComponent();
    }
}