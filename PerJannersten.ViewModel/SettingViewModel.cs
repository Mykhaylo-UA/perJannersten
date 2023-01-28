using System.Reflection;
using PerJannersten.Services.Data.Abstraction;
using PerJannersten.Services.Data.Attributes;
using PerJannersten.ViewModel.Setting;

namespace PerJannersten.ViewModel
{
    public class SettingViewModel : IBws, IIni
    {
        [IniSection("Ask for")]
        [BwsTable("SETTINGS")]
        public AskFor AskFor { get; set; } = new();

        [IniSection("GameType")]
        [BwsTable("SECTION")]
        public GameAndScoringType GameAndScoringType { get; set; } = new();

        [IniSection("Tricks & points")]
        [BwsTable("SETTINGS")]
        public TrickAndPoint TrickAndPoint { get; set; } = new();

        [IniSection("Timing")]
        [BwsTable("SETTINGS")]
        public Timing Timing { get; set; } = new();

        [IniSection("Timing")]
        [BwsTable("SETTINGS")]
        public TD TD { get; set; } = new();

        [IniSection("Each deal")]
        [BwsTable("SETTINGS")]
        public EachDeal EachDeal { get; set; } = new();

        [IniSection("End of round")]
        [BwsTable("SETTINGS")]
        public EndOfRound EndOfRound { get; set; } = new();

        [IniSection("Settings")]
        [BwsTable("SETTINGS")]
        public Other Other { get; set; } = new();

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
                    if (section == "Timing" && !Timing.UseTimer) value = 0;
                    if (iniFieldAttribute.Name == "TDPIN" && !TD.Checked) value = 0;
                    result.Add((section, iniFieldAttribute.Name), value);
                }
            }

            return result;
        }

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
                    if (property.Name == "Timing" && !Timing.UseTimer) value = 0;
                    BwsColumnAttribute bwsColumnAttribute = propertyField.GetCustomAttribute<BwsColumnAttribute>();
                    if (bwsColumnAttribute is null) continue;
                    if (bwsColumnAttribute.Name == "BM2PINcode" && !TD.Checked) value = 0;

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
    }
}

namespace PerJannersten.ViewModel.Setting
{
    [IniSection("Ask for")]
    public class AskFor
    {
        [BwsColumn("MemberNumbers")]
        [IniField("AskPlayerID")]
        public bool Id { get; set; }

        [BwsColumn("BM2EnterHandRecord")]
        [IniField("AskDeal")]
        public bool Deal { get; set; }

        [BwsColumn("BM2RecordBidding")]
        [IniField("AskBidding")]
        public bool Bidding { get; set; }

        [BwsColumn("LeadCard")]
        [IniField("AskLead")]
        public bool Lead { get; set; }

        [BwsColumn("BTAskConfirmation")]
        [IniField("AskConfirmation")]
        public bool Reconfirm { get; set; }

        [BwsColumn("BTAskPIN")]
        [IniField("AskPIN")]
        public bool Pin { get; set; }
    }

    [IniSection("GameType")]
    public class GameAndScoringType
    {
        [BwsColumn("BTGameType")]
        [IniField("BTGameType")]
        public byte GameType { get; set; }

        [BwsColumn("ScoringType")]
        [IniField("ScoringType")]
        public byte ScoringType { get; set; }

        [BwsColumn("PairsMoveAcrossField", "SESSION")]
        [IniField("ScoreAcross")]
        public bool ScoreAcrossPair { get; set; }
    }

    [IniSection("Tricks & points")]
    public class TrickAndPoint
    {
        [BwsColumn("BTRotateDirections")]
        [IniField("RotateDirections")]
        public bool RotateDirections { get; set; }

        [BwsColumn("EnterResultsMethod")]
        [IniField("InputMethod")]
        public byte InputMethod { get; set; }

        [BwsColumn("ScorePoints")]
        [IniField("PointToDeclarer")]
        public bool PointToDeclarer { get; set; }
    }

    [IniSection("Timing")]
    public class Timing
    {
        [BwsColumn("BTRegistrationTime")]
        [IniField("RegistrationTime")]
        public int RegistrationTime { get; set; }

        [BwsColumn("BTPlayTime")]
        [IniField("PlayTime")]
        public int PlayTime { get; set; }

        [BwsColumn("BTChangeTime")]
        [IniField("ChangeTime")]
        public int ChangeTime { get; set; }

        public bool UseTimer { get; set; }
    }

    [IniSection("Timing")]
    public class TD
    {
        public bool Checked { get; set; }

        [BwsColumn("BM2PINcode")]
        [IniField("TDPIN")]
        public int Pin { get; set; }

        [BwsColumn("BM2TDCall")]
        [IniField("TDCall")]
        public bool CallByBt { get; set; }
    }

    [IniSection("Each deal")]
    public class EachDeal
    {
        [BwsColumn("ShowResults")]
        [IniField("ShowScore")]
        public bool ShowScore { get; set; }

        [BwsColumn("BTAutomatic")]
        [IniField("Automatic")]
        public bool Automatic { get; set; }

        [BwsColumn("BM2ViewHandRecord")]
        [IniField("IncludeDeal")]
        public bool ShowDeal { get; set; }

        [BwsColumn("BTMakableAs")]
        [IniField("MakableAs")]
        public bool MakableAs { get; set; }

        [BwsColumn("BTContracts")]
        [IniField("Contracts")]
        public bool Contracts { get; set; }
    }

    [IniSection("End of round")]
    public class EndOfRound
    {
        [BwsColumn("BM2ScoreRecap")]
        [IniField("RoundSummary")]
        public bool RoundSummary { get; set; }

        [BwsColumn("BM2ScoreCorrection")]
        [IniField("PermitResultChange")]
        public bool PermitResultChange { get; set; }

        [BwsColumn("BM2Ranking")]
        [IniField("ShowRanking")]
        public bool ShowRanking { get; set; }

        [BwsColumn("BTTop10")]
        [IniField("Top10")]
        public bool Top10 { get; set; }

        [BwsColumn("BM2NextSeatings")]
        [IniField("ShowGoto")]
        public bool ShowGoto { get; set; }

        [BwsColumn("BM2GameSummary")]
        [IniField("RecapResult")]
        public bool RecapResult { get; set; }
    }

    [IniSection("Settings")]
    public class Other
    {
        [BwsColumn("BTShow")]
        [IniField("ShowAtStart")]
        public bool ShowAtStart { get; set; }

        [BwsColumn("BTDefault")]
        [IniField("BTDefault")]
        public bool BTDefault { get; set; }
    }
}