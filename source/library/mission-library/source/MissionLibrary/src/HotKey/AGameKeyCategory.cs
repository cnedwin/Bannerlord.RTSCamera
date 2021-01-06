using System.Collections.Generic;
using MissionLibrary.Provider;
using TaleWorlds.InputSystem;

namespace MissionLibrary.HotKey
{
    public abstract class AGameKeyCategory : ITag<AGameKeyCategory>
    {
        public abstract List<GameKey> GameKeys { get; }

        public abstract string GameKeyCategoryId { get; }

        public abstract InputKey GetKey(int i);

        public abstract void Save();

        public abstract void Load();
        public AGameKeyCategory Self => this;
    }
}
