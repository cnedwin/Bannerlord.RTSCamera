using MissionLibrary.HotKey;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.InputSystem;

namespace MissionSharedLibrary.Config.HotKey
{
    public class SerializedGameKey
    {
        public string StringId { get; set; }

        public InputKey Key { get; set; }

        public static SerializedGameKey FromGameKey(GameKey gameKey)
        {
            return new SerializedGameKey
            {
                StringId = gameKey.StringId,
                Key = gameKey.KeyboardKey.InputKey
            };
        }
    }

    public class SerializedGameKeyCategory
    {
        public string CategoryId { get; set; } = "DefaultGameKeyCategory";

        public List<SerializedGameKey> GameKeys { get; set; } = new List<SerializedGameKey>();

        public static SerializedGameKeyCategory FromGameKeyCategory(AGameKeyCategory category)
        {
            return new SerializedGameKeyCategory
            {
                CategoryId = category.GameKeyCategoryId,
                GameKeys = category.GameKeys.Select(SerializedGameKey.FromGameKey).ToList()
            };
        }

        public void ToGameKeyCategory(AGameKeyCategory category)
        {
            var dictionary = GameKeys.ToDictionary(serializedGameKey => serializedGameKey.StringId);
            for (var i = 0; i < category.GameKeys.Count; i++)
            {
                var gameKey = category.GameKeys[i];
                if (dictionary.TryGetValue(gameKey.StringId, out SerializedGameKey serializedGameKey))
                {
                    category.GameKeys[i] = new GameKey(gameKey.Id, gameKey.StringId, gameKey.GroupId,
                        serializedGameKey.Key, gameKey.MainCategoryId);
                }
            }
        }

        public SerializedGameKey GetGameKey(string gameKeyId)
        {
            for (int index = 0; index < this.GameKeys.Count; ++index)
            {
                SerializedGameKey gameKey = GameKeys[index];
                if (gameKey != null && gameKey.StringId == gameKeyId)
                    return gameKey;
            }
            return null;
        }
    }
}
