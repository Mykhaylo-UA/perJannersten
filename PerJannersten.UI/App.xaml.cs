using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using PerJannersten.Services;
using PerJannersten.Services.Interfaces;
using PerJannersten.UI.WpfPages;
using PerJannersten.UiServices;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;

namespace PerJannersten.UI;

public partial class App : Application
{
    readonly ServiceProvider _serviceProvider;

    public App()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddWpfBlazorWebView();
        serviceCollection.AddSingleton<IIniService, IniService>();
        serviceCollection.AddSingleton<ISettingService, SettingService>();
        serviceCollection.AddSingleton<IIniParser, Services.IniParser>();
        serviceCollection.AddSingleton<IBwsParser, BwsParser>();
        serviceCollection.AddBlazorWebViewDeveloperTools();
        serviceCollection.AddMudServices();
        serviceCollection.AddSingleton<GlobalState>();
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddLocalization(o => o.ResourcesPath = "Resources\\Localization");
        _serviceProvider = serviceCollection.BuildServiceProvider();
        Resources.Add("services", _serviceProvider);
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
        
        IBwsParser bwsParser = _serviceProvider.GetService<IBwsParser>();
        SettingViewModel setting = bwsParser.Parse<SettingViewModel>(globalState.BwsPath);
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