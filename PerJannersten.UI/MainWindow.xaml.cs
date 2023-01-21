using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using PerJannersten.Services;
using PerJannersten.Services.Interfaces;
using PerJannersten.UiServices;
using PerJannersten.UiServices.Interfaces;

namespace PerJannersten.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            ServiceCollection serviceCollection = new ();
            serviceCollection.AddWpfBlazorWebView();
            serviceCollection.AddSingleton<IIniService, IniService>();
            serviceCollection.AddSingleton<ISettingService, SettingService>();
            serviceCollection.AddBlazorWebViewDeveloperTools();
            serviceCollection.AddMudServices();
            Resources.Add("services", serviceCollection.BuildServiceProvider());
            
            AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            {
                MessageBox.Show(error.ExceptionObject.ToString(), caption: "Error");
            };
            InitializeComponent();
        }
    }
}
