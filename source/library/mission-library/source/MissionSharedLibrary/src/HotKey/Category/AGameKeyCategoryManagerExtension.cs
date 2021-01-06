using MissionLibrary.HotKey;
using MissionSharedLibrary.Provider;
using System;

namespace MissionSharedLibrary.HotKey.Category
{
    public static class AGameKeyCategoryManagerExtension
    {
        public static void AddCategory(this AGameKeyCategoryManager categoryManager, Func<AGameKeyCategory> creator,
            Version version, bool addOnlyWhenMissing = true)
        {
            categoryManager.AddCategory(new ConcreteProvider<AGameKeyCategory>(creator, version), addOnlyWhenMissing);
        }
    }
}
