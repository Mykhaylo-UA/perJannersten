using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.Components;
using Microsoft.Win32;
using MudBlazor;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;
using Size = MudBlazor.Size;

namespace PerJannersten.UI.Pages;

public partial class Index : ComponentBase
{
    [Inject] ISettingService SettingService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    SettingViewModel _settingViewModel = new();
    const Size DefaultSize = Size.Small;
    const Color DefaultColor = Color.Primary;
    const Color DefaultColorSecondary = Color.Secondary;

    const string FileName = "BTC.ini";
    const string DefaultPath = @"C:\BOS";
    const string RegistryKeyPath = @"Software\Jannersten Forlag AB\BOS\Settings";
    const string KeyName = "BOS_path";
    string _path;

    protected override async Task OnInitializedAsync()
    {
        RegistryKey currentUserKey = Registry.CurrentUser;
        RegistryKey pathKey = currentUserKey.OpenSubKey(RegistryKeyPath);
        if (pathKey is null) _path = DefaultPath;
        else _path = pathKey.GetValue(KeyName) as string ?? "";
        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        try
        {
            await using Stream fileStream = new FileStream(Path.Combine(_path, FileName), FileMode.Open);
            using StreamReader reader = new(fileStream);
            string content = await reader.ReadToEndAsync();
            _settingViewModel = SettingService.GetDefaultSetting(content);
            _prevScoringType = (byte) (_settingViewModel.GameAndScoringType.ScoringType == 4 ? 0 : _settingViewModel.GameAndScoringType.ScoringType);
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
        SettingService.SaveSetting(_settingViewModel, Path.Combine(_path, FileName));
        Snackbar.Add("Data is saved", Severity.Success);
    }

    byte _prevScoringType;

    void SelectTeamsScoringType()
    {
        _prevScoringType = _settingViewModel.GameAndScoringType.ScoringType;
        _settingViewModel.GameAndScoringType.ScoringType = 4;
        _settingViewModel.GameAndScoringType.GameType = null;
    }

    void ChangeGameType(byte? obj)
    {
        _settingViewModel.GameAndScoringType.GameType = obj;
        _settingViewModel.GameAndScoringType.ScoringType = _prevScoringType;
    }
}