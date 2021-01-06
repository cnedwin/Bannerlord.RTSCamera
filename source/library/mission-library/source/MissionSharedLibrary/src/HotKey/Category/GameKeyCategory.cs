using MissionLibrary.HotKey;
using MissionSharedLibrary.Config.HotKey;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace MissionSharedLibrary.HotKey.Category
{
    public class GameKeyCategory : AGameKeyCategory
    {
        public sealed override List<GameKey> GameKeys { get; }

        public override string GameKeyCategoryId { get; }

        private readonly IGameKeyConfig _config;

        public override InputKey GetKey(int i)
        {
            if (GameKeys == null || i < 0 || i >= GameKeys.Count)
            {
                return InputKey.Invalid;
            }

            return GameKeys[i]?.KeyboardKey.InputKey ?? InputKey.Invalid;
        }

        public override void Save()
        {
            _config.Category = SerializedGameKeyCategory.FromGameKeyCategory(this);
            _config.Serialize();
        }

        public override void Load()
        {
            _config.Deserialize();
            _config.Category.ToGameKeyCategory(this);
        }

        public GameKeyCategory(string categoryId, int gameKeysCount, IGameKeyConfig config)
        {
            GameKeyCategoryId = categoryId;
            _config = config;
            GameKeys = new List<GameKey>(gameKeysCount);
            for (int index = 0; index < gameKeysCount; ++index)
                GameKeys.Add((GameKey)null);
        }

        public void AddGameKey(GameKey gameKey)
        {
            if (gameKey.Id < 0 || gameKey.Id >= GameKeys.Count)
                return;

            GameKeys[gameKey.Id] = gameKey;
        }
    }
}
