using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using PerJannersten.Services;
using PerJannersten.Services.Interfaces;
using PerJannersten.UI.WpfPages;
using PerJannersten.UiServices;
using PerJannersten.UiServices.Interfaces;

namespace PerJannersten.UI;

public static class DependencyInjection
{
    public static IServiceProvider AddDependencyInjection(this Application application)
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddWpfBlazorWebView();
        serviceCollection.AddSingleton<IIniService, IniService>();
        serviceCollection.AddSingleton<IBwsService, BwsService>();
        serviceCollection.AddSingleton<ISettingService, SettingService>();
        serviceCollection.AddBlazorWebViewDeveloperTools();
        serviceCollection.AddMudServices();
        serviceCollection.AddSingleton<GlobalState>();
        serviceCollection.AddSingleton<MainWindow>();
        serviceCollection.AddLocalization(o => o.ResourcesPath = "Resources\\Localization");
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        application.Resources.Add("services", serviceProvider);
        return serviceProvider;
    }
}