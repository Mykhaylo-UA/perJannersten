using IniParser.Model;
using PerJannersten.Services.Interfaces;
using PerJannersten.UiServices.Interfaces;
using PerJannersten.ViewModel;

namespace PerJannersten.UiServices;

public class SettingService: ISettingService
{
    readonly IIniService _iniService;

    public SettingService(IIniService iniService)
    {
        _iniService = iniService;
    }
    const string AskForSection = "Ask for";
    const string GameTypeSection = "GameType";
    const string TricksAndPointsSection = "Tricks & points";
    const string TimingSection = "Timing";
    const string EachDealSection = "Each deal";
    const string EndOfRoundSection = "End of round";
    const string SettingSection = "Settings";

    public SettingViewModel GetDefaultSetting(string iniString)
    {
        SettingViewModel defaultSetting = new()
        {
            AskFor = new AskFor
            {
                Id = StringToBool(GetIni(AskForSection, "AskPlayerID")),
                Deal = StringToBool(GetIni(AskForSection,"AskDeal")),
                Bidding = StringToBool(GetIni(AskForSection,"AskBidding")),
                Lead = StringToBool(GetIni(AskForSection,"AskLead")),
                Reconfirm = StringToBool(GetIni(AskForSection,"AskConfirmation")),
                Pin = StringToBool(GetIni(AskForSection,"AskPIN"))
            },
            GameAndScoringType = new GameAndScoringType
            {
                GameType = byte.Parse(GetIni(GameTypeSection, "BTGameType") ?? "0"),
                ScoringType = byte.Parse(GetIni(GameTypeSection, "ScoringType") ?? "0"),
                ScoreAcrossPair = StringToBool(GetIni(GameTypeSection, "ScoreAcross")),
            },
            TrickAndPoint = new TrickAndPoint
            {
                RotateDirections = StringToBool(GetIni(TricksAndPointsSection, "RotateDirections")),
                InputMethod = byte.Parse(GetIni(TricksAndPointsSection, "InputMethod") ?? "0"),
                PointToDeclarer = StringToBool(GetIni(TricksAndPointsSection, "PointToDeclarer"))
            },
            Timing = new Timing
            {
                RegistrationTime = int.Parse(GetIni(TimingSection, "RegistrationTime") ?? "0"),
                PlayTime = int.Parse(GetIni(TimingSection, "PlayTime") ?? "0"),
                ChangeTime = int.Parse(GetIni(TimingSection, "ChangeTime") ?? "0"),
            },
            TD = new TD
            {
                Pin = int.Parse(GetIni(TimingSection, "TDPIN") ?? "0"),
                CallByBt = StringToBool(GetIni(TimingSection, "TDCall"))
            },
            EachDeal = new EachDeal
            {
                ShowScore = StringToBool(GetIni(EachDealSection, "ShowScore")),
                Automatic = StringToBool(GetIni(EachDealSection, "Automatic")),
                ShowDeal = StringToBool(GetIni(EachDealSection, "IncludeDeal")),
                MakableAs = StringToBool(GetIni(EachDealSection, "MakableAs")),
                Contracts = StringToBool(GetIni(EachDealSection, "Contracts"))
            },
            EndOfRound = new EndOfRound
            {
                RoundSummary = StringToBool(GetIni(EndOfRoundSection, "RoundSummary")),
                PermitResultChange = StringToBool(GetIni(EndOfRoundSection, "PermitResultChange")),
                ShowRanking = StringToBool(GetIni(EndOfRoundSection, "ShowRanking")),
                Top10 = StringToBool(GetIni(EndOfRoundSection, "Top10")),
                ShowGoto = StringToBool(GetIni(EndOfRoundSection, "ShowGoto")),
                RecapResult = StringToBool(GetIni(EndOfRoundSection, "RecapResult")),

            },
            Other = new Other
            {
                ShowAtStart = StringToBool(GetIni(SettingSection, "ShowAtStart")),
                BTDefault = StringToBool(GetIni(SettingSection, "BTDefault"))
            }
        };

        
        string GetIni(string section, string key)
        {
            return _iniService.Read(section, key, iniString);
        }

        bool StringToBool(string value)
        {
            if (value != "0") return true;
            return false;
        }

        Timing timing = defaultSetting.Timing;
        timing.UseTimer = timing.ChangeTime != 0 || timing.PlayTime != 0 || timing.RegistrationTime != 0;
        TD td = defaultSetting.TD;
        td.Checked = td.Pin != 0;

        return defaultSetting;
    }

    public void SaveSetting(SettingViewModel settingViewModel, string path)
    {
        string BoolToString(bool value) => value ? "1" : "0";

        IniData iniData = new();
        iniData[AskForSection]["AskPlayerID"] = BoolToString(settingViewModel.AskFor.Id);
        iniData[AskForSection]["AskDeal"] = BoolToString(settingViewModel.AskFor.Deal);
        iniData[AskForSection]["AskBidding"] = BoolToString(settingViewModel.AskFor.Bidding);
        iniData[AskForSection]["AskLead"] = BoolToString(settingViewModel.AskFor.Lead);
        iniData[AskForSection]["AskConfirmation"] = BoolToString(settingViewModel.AskFor.Reconfirm);
        iniData[AskForSection]["AskPIN"] = BoolToString(settingViewModel.AskFor.Pin);
        
        iniData[GameTypeSection]["BTGameType"] = $"{settingViewModel.GameAndScoringType.GameType ?? 1}";
        iniData[GameTypeSection]["ScoringType"] = $"{settingViewModel.GameAndScoringType.ScoringType}";
        iniData[GameTypeSection]["ScoreAcross"] = BoolToString(settingViewModel.GameAndScoringType.ScoreAcrossPair);
        
        iniData[TricksAndPointsSection]["RotateDirections"] = BoolToString(settingViewModel.TrickAndPoint.RotateDirections);
        iniData[TricksAndPointsSection]["InputMethod"] = $"{settingViewModel.TrickAndPoint.InputMethod}";
        iniData[TricksAndPointsSection]["PointToDeclarer"] = BoolToString(settingViewModel.TrickAndPoint.PointToDeclarer);

        bool useTimer = settingViewModel.Timing.UseTimer; 
        iniData[TimingSection]["RegistrationTime"] = $"{(!useTimer ? 0 : settingViewModel.Timing.RegistrationTime)}";
        iniData[TimingSection]["PlayTime"] = $"{(!useTimer ? 0 : settingViewModel.Timing.PlayTime)}";
        iniData[TimingSection]["ChangeTime"] = $"{(!useTimer ? 0 : settingViewModel.Timing.ChangeTime)}";
        
        iniData[TimingSection]["TDPIN"] = $"{(!settingViewModel.TD.Checked ? 0 : settingViewModel.TD.Pin)}";
        iniData[TimingSection]["TDCall"] = BoolToString(settingViewModel.TD.CallByBt);
        
        iniData[EachDealSection]["ShowScore"] = BoolToString(settingViewModel.EachDeal.ShowScore);
        iniData[EachDealSection]["Automatic"] = BoolToString(settingViewModel.EachDeal.Automatic);
        iniData[EachDealSection]["IncludeDeal"] = BoolToString(settingViewModel.EachDeal.ShowDeal);
        iniData[EachDealSection]["MakableAs"] = BoolToString(settingViewModel.EachDeal.MakableAs);
        iniData[EachDealSection]["Contracts"] = BoolToString(settingViewModel.EachDeal.Contracts);
        
        iniData[EndOfRoundSection]["RoundSummary"] = BoolToString(settingViewModel.EndOfRound.RoundSummary);
        iniData[EndOfRoundSection]["PermitResultChange"] = BoolToString(settingViewModel.EndOfRound.PermitResultChange);
        iniData[EndOfRoundSection]["ShowRanking"] = BoolToString(settingViewModel.EndOfRound.ShowRanking);
        iniData[EndOfRoundSection]["Top10"] = BoolToString(settingViewModel.EndOfRound.Top10);
        iniData[EndOfRoundSection]["ShowGoto"] = BoolToString(settingViewModel.EndOfRound.ShowGoto);
        iniData[EndOfRoundSection]["RecapResult"] = BoolToString(settingViewModel.EndOfRound.RecapResult);
        
        iniData[SettingSection]["ShowAtStart"] = BoolToString(settingViewModel.Other.ShowAtStart);
        iniData[SettingSection]["BTDefault"] = BoolToString(settingViewModel.Other.BTDefault);

        _iniService.Write(iniData, path);
    }
}