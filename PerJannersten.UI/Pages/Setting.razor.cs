using System;
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

public partial class Setting : ComponentBase
{
    [Inject] ISettingService SettingService { get; set; }
    [Inject] IAdditionalSettingService AdditionalSettingService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] GlobalState GlobalState { get; set; }
    SettingWindow SettingWindow { get; set; }
    [Inject] IStringLocalizer<Setting> Localizer { get; set; }

    SettingViewModel _settingViewModel = new();
    const Size DefaultSize = Size.Small;
    const Color DefaultColor = Color.Primary;
    const Color DefaultColorSecondary = Color.Secondary;

    bool _btDefault;

    protected override async Task OnInitializedAsync()
    {
        SettingWindow = Application.Current.Windows.OfType<SettingWindow>().Last();
        _settingViewModel = SettingWindow.SettingViewModel;
        _btDefault = _settingViewModel.Other.BTDefault;
        SettingWindow.Title = SettingWindow.DefaultSetting ? Localizer["title_default"] : Localizer["title"];


        try
        {
            _prevScoringType = (byte) (_settingViewModel.GameAndScoringType.ScoringType == 4
                ? 1
                : _settingViewModel.GameAndScoringType.ScoringType);
            StateHasChanged();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    void SaveSettings()
    {
        if (SettingWindow.DefaultSetting)
        {
            SettingService.SaveDefaultSetting(_settingViewModel, GlobalState.DefaultFullPath);
        }
        else
        {
            SettingService.SaveSetting(_settingViewModel, GlobalState.BwsPath);
        }

        Snackbar.Add("Data is saved", Severity.Success);
    }

    byte _prevScoringType;

    void SelectTeamsScoringType()
    {
        _prevScoringType = _settingViewModel.GameAndScoringType.ScoringType;
        _settingViewModel.GameAndScoringType.ScoringType = 4;
    }

    void ChangeGameType(byte obj)
    {
        _settingViewModel.GameAndScoringType.GameType = obj;
        _settingViewModel.GameAndScoringType.ScoringType = _prevScoringType;
    }

    void MoreSettings()
    {
        SaveSettings();
        AdditionalSettingWindow window = new()
        {
            DefaultSetting = false,
            AdditionalSettingViewModel =
                AdditionalSettingService.GetAdditionalSetting(GlobalState.BwsPath, GlobalState.DefaultFullPath)
        };
        window.Show();
    }
}