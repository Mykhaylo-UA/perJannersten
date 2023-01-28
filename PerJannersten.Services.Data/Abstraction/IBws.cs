namespace PerJannersten.Services.Data.Abstraction;

public interface IBws
{
    Dictionary<string, List<string>> GetUpdateSqlString();
}