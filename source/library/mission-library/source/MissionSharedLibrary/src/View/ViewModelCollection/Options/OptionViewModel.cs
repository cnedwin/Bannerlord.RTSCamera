using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MissionSharedLibrary.View.ViewModelCollection.Options
{
    public abstract class OptionViewModel : ViewModel
    {
        private int _optionTypeId = -1;
        private readonly TextObject _textText;
        private readonly TextObject _descriptionText;
        private string _description;
        private string _name;
        private string[] _imageIDs;

        protected OptionViewModel(
            TextObject name,
            TextObject description,
            OptionsVM.OptionsDataType typeID)
        {
            _textText = name;
            _descriptionText = description;
            OptionTypeID = (int) typeID;
            RefreshValues();
        }

        public virtual void UpdateData(bool initUpdate)
        {
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Name = _textText.ToString();
            Description = _descriptionText.ToString();
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

        [DataSourceProperty]
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name)
                    return;
                _name = value;
                OnPropertyChangedWithValue(value, nameof(Name));
            }
        }

        [DataSourceProperty]
        public string[] ImageIDs
        {
            get => _imageIDs;
            set
            {
                if (value == _imageIDs)
                    return;
                _imageIDs = value;
                OnPropertyChangedWithValue(value, nameof(ImageIDs));
            }
        }

        [DataSourceProperty]
        public int OptionTypeID
        {
            get => _optionTypeId;
            set
            {
                if (value == _optionTypeId)
                    return;
                _optionTypeId = value;
                OnPropertyChangedWithValue(value, nameof(OptionTypeID));
            }
        }
    }
}
