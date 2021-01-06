using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MissionLibrary.HotKey.View
{
    public class MissionLibraryGameKeyGroupVM : ViewModel
    {
        private readonly Action<string, int, InputKey> _setAllKeysOfId;
        private readonly string _categoryId;
        private string _description;
        private MBBindingList<GameKeyOptionVM> _gameKeys;

        public MissionLibraryGameKeyGroupVM(
          string categoryId,
          IEnumerable<GameKey> keys,
          Action<GameKeyOptionVM> onKeyBindRequest,
          Action<string, int, InputKey> setAllKeysOfId)
        {
            _setAllKeysOfId = setAllKeysOfId;
            _categoryId = categoryId;
            _gameKeys = new MBBindingList<GameKeyOptionVM>();
            foreach (GameKey key in keys)
            {
                Key validInputKey = FindValidInputKey(key);
                if (validInputKey != null)
                    _gameKeys.Add(new GameKeyOptionVM(key, onKeyBindRequest, SetGameKey));
            }
            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Description = Module.CurrentModule.GlobalTextManager.FindText("str_key_category_name", _categoryId).ToString();
            GameKeys.ApplyActionOnAllItems(x => x.RefreshValues());
        }

        private Key FindValidInputKey(GameKey gameKey)
        {
            return gameKey.KeyboardKey;
        }

        private void SetGameKey(GameKeyOptionVM option, InputKey newKey)
        {
            option.CurrentKey.ChangeKey(newKey);
            option.OptionValueText = Module.CurrentModule.GlobalTextManager.FindText("str_game_key_text", option.CurrentKey.ToString().ToLower()).ToString();
            _setAllKeysOfId(_categoryId, option.CurrentGameKey.Id, newKey);
        }

        public void OnReset()
        {
            foreach (GameKeyOptionVM gameKey in GameKeys)
                gameKey.Reset();
        }

        public void OnDone()
        {
            foreach (GameKeyOptionVM gameKey in GameKeys)
                gameKey.OnDone();
        }

        [DataSourceProperty]
        public MBBindingList<GameKeyOptionVM> GameKeys
        {
            get => _gameKeys;
            set
            {
                if (value == _gameKeys)
                    return;
                _gameKeys = value;
                OnPropertyChangedWithValue(value, nameof(GameKeys));
            }
        }

        [DataSourceProperty]
        public string Description
        {
            get => _description;
            set
            {
                if (value == _description)
                    return;
                _description = value;
                OnPropertyChangedWithValue(value, nameof(Description));
            }
        }
    }
}
