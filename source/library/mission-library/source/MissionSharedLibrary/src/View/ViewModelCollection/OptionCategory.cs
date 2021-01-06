using MissionLibrary.View;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace MissionSharedLibrary.View.ViewModelCollection
{
    public class OptionCategory : ViewModel, IOptionCategory
    {
        private readonly List<IOption> _options = new List<IOption>();
        private MBBindingList<ViewModel> _optionViewModels;

        public string Id { get; }

        [DataSourceProperty]
        public MBBindingList<ViewModel> OptionViewModels
        {
            get => _optionViewModels;
            set
            {
                if (_optionViewModels == value)
                    return;
                _optionViewModels = value;
                OnPropertyChanged(nameof(OptionViewModels));
            }
        }

        public OptionCategory(string id)
        {
            Id = id;

            Refresh();
        }

        public void AddOption(IOption option)
        {
            _options.Add(option);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            Refresh();
        }

        private void Refresh()
        {
            var optionViewModels = new MBBindingList<ViewModel>();
            foreach (var option in _options)
            {
                optionViewModels.Add(option.GetViewModel());
            }

            OptionViewModels = optionViewModels;
        }

        public ViewModel GetViewModel()
        {
            return this;
        }
    }
}
