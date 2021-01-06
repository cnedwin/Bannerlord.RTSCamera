﻿using System.Collections.Generic;
using System.Linq;
using MissionLibrary.View;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MissionSharedLibrary.View.ViewModelCollection.Options.Selection
{
    public class SelectionOptionViewModel : OptionViewModel, IOption
    {
        private readonly SelectionOptionData _selectionOptionData;
        private SelectorVM<SelectorItemVM> _selector;

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> Selector
        {
            get => _selector;
            set
            {
                if (value == _selector)
                    return;
                _selector = value;
                OnPropertyChangedWithValue(value, nameof(Selector));
            }
        }

        public SelectionOptionViewModel(TextObject name, TextObject description, SelectionOptionData selectionOptionData) 
            : base(name, description, OptionsVM.OptionsDataType.MultipleSelectionOption)
        {
            _selectionOptionData = selectionOptionData;
            Selector = new SelectorVM<SelectorItemVM>(0, null);
            UpdateData(true);
            Selector.SelectedIndex = _selectionOptionData.GetDefaultValue();
        }

        public ViewModel GetViewModel()
        {
            return this;
        }

        public void Commit()
        {
            _selectionOptionData.Commit();
        }

        public void Cancel()
        {
            Selector.SelectedIndex = _selectionOptionData.GetDefaultValue();
            UpdateValue(_selector);
        }

        public override void UpdateData(bool initialUpdate)
        {
            base.UpdateData(initialUpdate);
            IEnumerable<SelectionItem> selectableOptionNames = _selectionOptionData.GetSelectableOptionNames();
            Selector.SetOnChangeAction(null);
            Selector.SelectedIndex = -1;
            var selectionItems = selectableOptionNames as SelectionItem[] ?? selectableOptionNames.ToArray();
            if (selectionItems.Any() && selectionItems.All(n => n.IsLocalizationId))
            {
                List<TextObject> textObjectList = new List<TextObject>();
                foreach (SelectionItem selectionData in selectionItems)
                {
                    TextObject text = Module.CurrentModule.GlobalTextManager.FindText(selectionData.Data, selectionData.Variation);
                    textObjectList.Add(text);
                }
                Selector.Refresh(textObjectList, _selectionOptionData.GetValue(), UpdateValue);
            }
            else
            {
                List<string> stringList = new List<string>();
                foreach (SelectionItem selectionData in selectionItems)
                {
                    if (selectionData.IsLocalizationId)
                    {
                        TextObject text = Module.CurrentModule.GlobalTextManager.FindText(selectionData.Data);
                        stringList.Add(text.ToString());
                    }
                    else
                        stringList.Add(selectionData.Data);
                }
                Selector.Refresh(stringList, _selectionOptionData.GetValue(), UpdateValue);
            }
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Selector?.RefreshValues();
        }

        private void UpdateValue(SelectorVM<SelectorItemVM> selector)
        {
            if (selector.SelectedIndex < 0)
                return;
            this._selectionOptionData.SetValue(selector.SelectedIndex);
        }
    }
}
