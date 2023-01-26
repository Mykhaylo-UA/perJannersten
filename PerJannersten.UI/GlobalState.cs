using System.IO;
using Microsoft.Win32;

namespace PerJannersten.UI;

public class GlobalState
{
    public string DefaultSettingFileName { get; set; } = "BTC.ini";
    public string DefaultSettingPath { get; set; } = @"C:\BOS\BTC";
    public string RegistryKeyPath { get; set; } = @"Software\Jannersten Forlag AB\BOS\Settings";
    public string KeyName { get; set; } = "BOS_path";
    public string BwsPath { get; set; } = @"C:\\BOS\\Events\\BWS_test\\DATA\\bws_test.bws";
    public string Path
    {
        get
        {
            string path = DefaultSettingPath;
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey pathKey = currentUserKey.OpenSubKey(RegistryKeyPath);
            if (pathKey is not null) path = pathKey.GetValue(KeyName) as string ?? "";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
    }
}