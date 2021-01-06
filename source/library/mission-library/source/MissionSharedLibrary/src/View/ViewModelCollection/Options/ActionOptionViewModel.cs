using MissionLibrary.View;
using MissionSharedLibrary.View.ViewModelCollection.Options;
using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MissionSharedLibrary.src.View.ViewModelCollection.Options
{
    public class ActionOptionViewModel : OptionViewModel, IOption
    {
        private readonly Action _onAction;

        public ActionOptionViewModel(TextObject name, TextObject description, Action onAction) 
            : base(name, description, OptionsVM.OptionsDataType.ActionOption)
        {
            _onAction = onAction;
        }

        private void ExecuteAction()
        {
            _onAction?.DynamicInvokeWithLog();
        }

        public ViewModel GetViewModel()
        {
            return this;
        }

        public void Commit()
        { }

        public void Cancel()
        { }
    }
}
