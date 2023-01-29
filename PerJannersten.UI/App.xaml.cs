using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PerJannersten.UI.WpfPages;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;

namespace PerJannersten.UI;

public partial class App : Application
{
    readonly IServiceProvider _serviceProvider;

    public App()
    {
        _serviceProvider = this.AddDependencyInjection();
        
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
        
        AppDomain.CurrentDomain.UnhandledException += (_, error) =>
        {
            MessageBox.Show(error.ExceptionObject.ToString(), caption: "Error");
        };
    }

    void OnStartup(object sender, StartupEventArgs e)
    {
        GlobalState globalState = _serviceProvider.GetService<GlobalState>();
        string[] args = Environment.GetCommandLineArgs();
        string f = args.FirstOrDefault(a => a.Contains("/f:"));
        if (f != null)
        {
            string path = f.Replace("/f:","");
            if (!File.Exists(path))
            {
                MessageBox.Show($"BWS file by path '{path}' not found");
                return;
            }
            globalState.BwsPath = path;
        }
        
        MainWindow mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
        
        ISettingService settingService = _serviceProvider.GetService<ISettingService>();
        
        SettingViewModel setting = settingService.GetSetting(globalState.BwsPath, globalState.DefaultFullPath);
        if (setting.Other.ShowAtStart)
        {
            SettingWindow settingWindow = new()
            {
                Owner = mainWindow,
                SettingViewModel = setting
            };
            settingWindow.Show();   
        }
    }
}