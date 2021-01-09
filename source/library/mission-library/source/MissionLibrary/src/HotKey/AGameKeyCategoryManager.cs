using System;
using System.Collections.Generic;
using MissionLibrary.Provider;

namespace MissionLibrary.HotKey
{
    public abstract class AGameKeyCategoryManager : ITag<AGameKeyCategoryManager>
    {
        public static AGameKeyCategoryManager Get()
        {
            return Global.GetProvider<AGameKeyCategoryManager>();
        }

        public abstract Dictionary<string, IProvider<AGameKeyCategory>> Categories { get; }
        public abstract void AddCategory(IProvider<AGameKeyCategory> category, bool addOnlyWhenMissing = true);
        public abstract AGameKeyCategory GetCategory(string categoryId);

        public abstract T GetCategory<T>(string categoryId) where T : AGameKeyCategory;
        public AGameKeyCategoryManager Self => this;
    }
}
