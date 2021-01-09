using System;
using System.Collections.Generic;
using System.Text;
using MissionLibrary.Provider;

namespace MissionSharedLibrary.Provider
{
    public class ConcreteProvider<T> : IProvider<T> where T : class, ITag<T>
    {
        private readonly Func<ITag<T>> _creator;
        private T _value;
        public Version ProviderVersion { get; }

        public T Value => _value ??= _creator?.Invoke().Self;

        public ConcreteProvider(Func<ITag<T>> creator, Version providerVersion)
        {
            ProviderVersion = providerVersion;
            _creator = creator;
        }
    }

    public class ProviderCreator
    {
        public static ConcreteProvider<T> Create<T>(Func<ITag<T>> creator, Version providerVersion) where T : class, ITag<T>
        {
            return new ConcreteProvider<T>(creator, providerVersion);
        }
    }
}
