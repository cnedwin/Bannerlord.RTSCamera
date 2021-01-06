using System.Collections.Generic;
using MissionLibrary.View;
using TaleWorlds.Library;

namespace MissionSharedLibrary.View.ViewModelCollection
{
    public class OptionColumnViewModel : ViewModel
    {
        private readonly List<IOptionCategory> _optionCategories = new List<IOptionCategory>();
        private MBBindingList<ViewModel> _categories;

        [DataSourceProperty]
        public MBBindingList<ViewModel> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                    return;
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public OptionColumnViewModel()
        {
            Refresh();
        }

        public void AddOptionCategory(IOptionCategory optionCategory)
        {
            var index = _optionCategories.FindIndex(o => o.Id == optionCategory.Id);
            if (index < 0)
                _optionCategories.Add(optionCategory);
            else
                _optionCategories[index] = optionCategory;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();

            Refresh();
        }

        private void Refresh()
        {
            var categories = new MBBindingList<ViewModel>();
            foreach (var optionCategory in _optionCategories)
            {
                var viewModel = optionCategory.GetViewModel();
                viewModel.RefreshValues();
                categories.Add(viewModel);
            }

            Categories = categories;
        }
    }
}
