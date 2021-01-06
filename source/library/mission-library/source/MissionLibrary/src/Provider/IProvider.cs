using System;

namespace MissionLibrary.Provider
{
    public interface IProvider
    {
        Version ProviderVersion { get; }
    }

    public interface IProvider<out T>: IProvider where T : class, ITag<T>
    {
        T Value { get; }
    }
}
