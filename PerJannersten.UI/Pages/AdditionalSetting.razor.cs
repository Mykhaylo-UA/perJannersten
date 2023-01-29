using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using PerJannersten.UI.WpfPages;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;
using Size = MudBlazor.Size;

namespace PerJannersten.UI.Pages;

public partial class AdditionalSetting : ComponentBase
{
    [Inject] IAdditionalSettingService AdditionalSettingService { get; set; }
    [Inject] ISettingService SettingService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] GlobalState GlobalState { get; set; }
    AdditionalSettingWindow AdditionalSettingWindow { get; set; }
    [Inject] IStringLocalizer<AdditionalSetting> Localizer { get; set; }

    AdditionalSettingViewModel _viewModel = new();
    const Size DefaultSize = Size.Small;
    const Color DefaultColor = Color.Primary;
    const Color DefaultColorSecondary = Color.Secondary;
    
    protected override async Task OnInitializedAsync()
    {
        AdditionalSettingWindow = Application.Current.Windows.OfType<AdditionalSettingWindow>().Last();
        _viewModel = AdditionalSettingWindow.AdditionalSettingViewModel;
        AdditionalSettingWindow.Title = AdditionalSettingWindow.DefaultSetting ? Localizer["title_default"] : Localizer["title"];
    }

    void SaveSettings()
    {
        if (AdditionalSettingWindow.DefaultSetting)
        {
            AdditionalSettingService.SaveDefaultAdditionalSetting(_viewModel, GlobalState.DefaultFullPath);
        }
        else
        {
            AdditionalSettingService.SaveAdditionalSetting(_viewModel, GlobalState.BwsPath);
        }
        Snackbar.Add("Data is saved", Severity.Success);
    }

    void OpenSettingWindow()
    {
        SaveSettings();
        SettingWindow window = new()
        {
            DefaultSetting = false,
            SettingViewModel = SettingService.GetSetting(GlobalState.BwsPath, GlobalState.DefaultFullPath)
        };
        window.Show();
    }
}