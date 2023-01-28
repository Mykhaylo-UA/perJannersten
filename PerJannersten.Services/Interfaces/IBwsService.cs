using PerJannersten.Services.Data.Abstraction;

namespace PerJannersten.Services.Interfaces;

public interface IBwsService
{
    T Parse<T>(string path, object defaultObject = null) where T: new();
    void Save(IBws bwsViewModel, string path);
}