﻿using System;
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
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] GlobalState GlobalState { get; set; }
    SettingWindow SettingWindow { get; set; }
    [Inject] IStringLocalizer<Setting> Localizer { get; set; }

    SettingViewModel _settingViewModel = new();
    const Size DefaultSize = Size.Small;
    const Color DefaultColor = Color.Primary;
    const Color DefaultColorSecondary = Color.Secondary;
    
    string _path;

    protected override async Task OnInitializedAsync()
    {
        SettingWindow = Application.Current.Windows.OfType<SettingWindow>().Last();
        SettingWindow.Title = SettingWindow.DefaultSetting ? Localizer["title_default"] : Localizer["title"];
        
        _path = Path.Combine(GlobalState.Path, GlobalState.DefaultSettingFileName);

        await using Stream fileStream = new FileStream(_path, FileMode.Open);
        using StreamReader reader = new(fileStream);
        string content = await reader.ReadToEndAsync();

        try
        {
            _settingViewModel = SettingWindow.DefaultSetting ? 
                SettingService.GetDefaultSetting(content) : 
                SettingService.GetSetting(GlobalState.BwsPath, content);
            
            _prevScoringType = (byte) (_settingViewModel.GameAndScoringType.ScoringType == 4
                ? 1
                : _settingViewModel.GameAndScoringType.ScoringType);
            StateHasChanged();
        }
        catch (Exception e)
        {
            _settingViewModel = new();
            MessageBox.Show(e.Message);
        }
    }

    void SaveSettings()
    {
        if (SettingWindow.DefaultSetting)
        {
            SettingService.SaveDefaultSetting(_settingViewModel, _path);
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
        MessageBox.Show("Progress");
    }
}