using MissionLibrary;
using MissionLibrary.HotKey;
using System;
using System.Collections.Generic;
using MissionLibrary.Provider;
using MissionSharedLibrary.Provider;

namespace MissionSharedLibrary.HotKey
{
    public class GameKeyCategoryManager : AGameKeyCategoryManager
    {

        public override Dictionary<string, IProvider<AGameKeyCategory>> Categories { get; } = new Dictionary<string, IProvider<AGameKeyCategory>>();

        public override void AddCategory(IProvider<AGameKeyCategory> provider, bool addOnlyWhenMissing = true)
        {
            if (Categories.TryGetValue(provider.Value.GameKeyCategoryId, out IProvider<AGameKeyCategory> existingProvider))
            {
                if (existingProvider.ProviderVersion == provider.ProviderVersion && addOnlyWhenMissing ||
                    existingProvider.ProviderVersion > provider.ProviderVersion)
                    return;

                Categories[provider.Value.GameKeyCategoryId] = provider;
            }

            Categories.Add(provider.Value.GameKeyCategoryId, provider);

            provider.Value.Load();
            provider.Value.Save();
        }

        public override AGameKeyCategory GetCategory(string categoryId)
        {
            if (Categories.TryGetValue(categoryId, out IProvider<AGameKeyCategory> provider))
            {
                return provider.Value;
            }

            return null;
        }

        public override T GetCategory<T>(string categoryId)
        {
            if (Categories.TryGetValue(categoryId, out IProvider<AGameKeyCategory> provider) && provider.Value is T t)
            {
                return t;
            }

            return null;
        }
    }
}
