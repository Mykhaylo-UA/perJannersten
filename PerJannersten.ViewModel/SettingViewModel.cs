namespace PerJannersten.ViewModel;

public class SettingViewModel
{
    public AskFor AskFor { get; set; } = new();
    public GameAndScoringType GameAndScoringType { get; set; } = new();
    public TrickAndPoint TrickAndPoint { get; set; } = new();
    public Timing Timing { get; set; } = new();
    public TD TD { get; set; } = new();
    public EachDeal EachDeal { get; set; } = new();
    public EndOfRound EndOfRound { get; set; } = new();
    public Other Other { get; set; } = new();
}

public class AskFor
{
    public bool Id { get; set; }
    public bool Deal { get; set; }
    public bool Bidding { get; set; }
    public bool Lead { get; set; }
    public bool Reconfirm { get; set; }
    public bool Pin { get; set; }
}

public class GameAndScoringType
{
    public byte? GameType { get; set; }
    public byte ScoringType { get; set; }
    public bool ScoreAcrossPair { get; set; }
}

public class TrickAndPoint
{
    public bool RotateDirections { get; set; }
    public byte InputMethod { get; set; }
    public bool PointToDeclarer { get; set; }
}

public class Timing
{
    public int RegistrationTime { get; set; }
    public int PlayTime { get; set; }
    public int ChangeTime { get; set; }
    public bool UseTimer { get; set; }
}

public class TD
{
    public bool Checked { get; set; }
    public int Pin { get; set; }
    public bool CallByBt { get; set; }
}

public class EachDeal
{
    public bool ShowScore { get; set; }
    public bool Automatic { get; set; }
    public bool ShowDeal { get; set; }
    public bool MakableAs { get; set; }
    public bool Contracts { get; set; }
}

public class EndOfRound
{
    public bool RoundSummary { get; set; }
    public bool PermitResultChange { get; set; }
    public bool ShowRanking { get; set; }
    public bool Top10 { get; set; }
    public bool ShowGoto { get; set; }
    public bool RecapResult { get; set; }
}

public class Other
{
    public bool ShowAtStart { get; set; }
    public bool BTDefault { get; set; }
}