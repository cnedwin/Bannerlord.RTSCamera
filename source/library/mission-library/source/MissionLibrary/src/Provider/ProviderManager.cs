using System;
using System.Collections.Generic;

namespace MissionLibrary.Provider
{
    public class ProviderManager : IProviderManager
    {
        private readonly Dictionary<Type, IProvider> _providers = new Dictionary<Type, IProvider>();

        public void RegisterProvider<T>(IProvider<T> newProvider) where T : class, ITag<T>
        {
            if (!_providers.TryGetValue(typeof(T), out IProvider oldProvider))
            {
                _providers.Add(typeof(T), newProvider);
            }
            else if (oldProvider.ProviderVersion.CompareTo(newProvider) <= 0)
            {
                _providers[typeof(T)] = newProvider;
            }
        }

        public T GetProvider<T>() where T : class, ITag<T>
        {
            if (!_providers.TryGetValue(typeof(T), out IProvider provider) || !(provider is IProvider<T> tProvider))
            {
                return null;
            }

            return tProvider.Value;
        }

        public TU GetProvider<T, TU>() where T : class, ITag<T> where TU: class, T

        {
            if (!_providers.TryGetValue(typeof(T), out IProvider provider) || !(provider is IProvider<T> tProvider) ||
                !(tProvider.Value is TU result))
            {
                return null;
            }

            return result;
        }
    }
}
