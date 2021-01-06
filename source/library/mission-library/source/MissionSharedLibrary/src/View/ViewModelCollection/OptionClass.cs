using MissionLibrary.View;
using MissionSharedLibrary.View.ViewModelCollection.Basic;
using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MissionSharedLibrary.View.ViewModelCollection
{
    public class OptionClass : ViewModel, IOptionClass
    {
        private readonly int _maxColumnIndex;
        private MBBindingList<OptionColumnViewModel> _optionColumns = new MBBindingList<OptionColumnViewModel>();

        public OptionClass(string id, TextObject name)
        {
            Id = id;
            Name = new TextViewModel(name);
        }

        public string Id { get; }
        public TextViewModel Name { get; }

        [DataSourceProperty]
        public MBBindingList<OptionColumnViewModel> OptionColumns
        {
            get => _optionColumns;
            set
            {
                if (_optionColumns == value)
                    return;
                _optionColumns = value;
                OnPropertyChanged(nameof(OptionColumns));
            }
        }

        public OptionClass(int maxColumnCount = 10)
        {
            _maxColumnIndex = maxColumnCount - 1;

            Refresh();
        }

        public void AddOptionCategory(int column, IOptionCategory optionCategory)
        {
            column = Math.Min(column, _maxColumnIndex);
            if (column < 0 || column >= OptionColumns.Count)
            {
                column = MBMath.ClampInt(column, 0, OptionColumns.Count);
                OptionColumns.Insert(column, new OptionColumnViewModel());
            }

            OptionColumns[column].AddOptionCategory(optionCategory);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            Refresh();
        }

        private void Refresh()
        {
            foreach (var optionColumnViewModel in _optionColumns)
            {
                optionColumnViewModel.RefreshValues();
            }
        }

        public ViewModel GetViewModel()
        {
            return this;
        }
    }
}
