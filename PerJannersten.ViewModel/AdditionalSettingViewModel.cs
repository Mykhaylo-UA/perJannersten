using System.Reflection;
using PerJannersten.Services.Data.Abstraction;
using PerJannersten.Services.Data.Attributes;
using PerJannersten.ViewModel.AdditionalSetting;

namespace PerJannersten.ViewModel
{
    public class AdditionalSettingViewModel : IBws, IIni
    {
        [IniSection("AdditionalSettings")] public Scorer Scorer { get; set; }
        [IniSection("AdditionalSettings")] public COM COM { get; set; }
        [IniSection("AdditionalSettings")] public TD TD { get; set; }
        [IniSection("AdditionalSettings")] public EachDeal EachDeal { get; set; }
        [IniSection("AdditionalSettings")] public Feedback Feedback { get; set; }
        [IniSection("AdditionalSettings")] public AllRounds AllRounds { get; set; }

        public Dictionary<string, List<string>> GetUpdateSqlString()
        {
            Dictionary<string, List<string>> result = new();

            IEnumerable<PropertyInfo> properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                BwsTableAttribute bwsTableAttribute = property.GetCustomAttribute<BwsTableAttribute>();
                if (bwsTableAttribute is null) continue;
                string table = bwsTableAttribute.Name;

                object propertyValue = property.GetValue(this);
                IEnumerable<PropertyInfo> propertyFields = propertyValue.GetType().GetProperties();
                foreach (PropertyInfo propertyField in propertyFields)
                {
                    object value = propertyField.GetValue(propertyValue);
                    BwsColumnAttribute bwsColumnAttribute = propertyField.GetCustomAttribute<BwsColumnAttribute>();
                    if (bwsColumnAttribute is null) continue;

                    if (!string.IsNullOrEmpty(bwsColumnAttribute.Table))
                    {
                        table = bwsColumnAttribute.Table;
                    }

                    if (!result.ContainsKey(table))
                    {
                        result.Add(table, new());
                    }

                    result.First(a => a.Key == table).Value.Add($"{bwsColumnAttribute.Name}={value}");
                }
            }

            return result;
        }

        public Dictionary<(string, string), object> GetIniDictionary()
        {
            Dictionary<(string, string), object> result = new();
            IEnumerable<PropertyInfo> properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object propertyValue = property.GetValue(this);
                IniSectionAttribute iniSectionAttribute =
                    propertyValue.GetType().GetCustomAttribute<IniSectionAttribute>();
                if (iniSectionAttribute is null) continue;
                string section = iniSectionAttribute.Name;
                IEnumerable<PropertyInfo> propertyFields = propertyValue.GetType().GetProperties();
                foreach (PropertyInfo propertyField in propertyFields)
                {
                    IniFieldAttribute iniFieldAttribute = propertyField.GetCustomAttribute<IniFieldAttribute>();
                    if (iniFieldAttribute is null) continue;
                    object value = propertyField.GetValue(propertyValue);
                    result.Add((section, iniFieldAttribute.Name), value);
                }
            }

            return result;
        }
    }
}


namespace PerJannersten.ViewModel.AdditionalSetting
{
    public class Scorer
    {
        [IniField("Master")] public bool Masters { get; set; }
        [IniField("NEMasters")] public byte NEMesters { get; set; }
    }

    public class COM
    {
        [IniField("COM")] public bool Required { get; set; }
        [IniField("UntilRd")] public string UntilRd { get; set; }
    }

    public class TD
    {
        [IniField("BM2TDCall")] public bool SilentCalls { get; set; }
        [IniField("Bluetooth")] public bool Bluetooth { get; set; }
        [IniField("WiFi")] public bool WiFi { get; set; }
    }

    public class EachDeal
    {
        [IniField("BoardOrderVerification")] public bool AscendingBoardOrder { get; set; }
        [IniField("IntermediateResults")] public bool SendContractSeparately { get; set; }
        [IniField("BM2ConfirmNP")] public bool NotifyTdOfOnPlay { get; set; }
        [IniField("BM2ValidateLeadCard")] public bool ValidateLead { get; set; }
        [IniField("BM2EnterHandRecordsWhen")] public bool DealEntryAfterPlay { get; set; }
    }

    public class Feedback
    {
        [IniField("ShowOwnResult")] public bool OwnResult { get; set; }
        [IniField("ShowPercentage")] public bool Percentage { get; set; }
    }

    public class AllRounds
    {
        [IniField("BM2ShowPlayerNames")] public bool ShowNames { get; set; }
        [IniField("BM2NumberEntryEachRound")] public bool RequiredPlayerId { get; set; }
    }
}