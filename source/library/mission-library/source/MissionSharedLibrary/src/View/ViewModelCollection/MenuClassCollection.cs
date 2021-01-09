using MissionLibrary.View;
using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace MissionSharedLibrary.View
{
    public class MenuClassCollection : ViewModel, IMenuClassCollection
    {
        private readonly List<IOptionClass> _optionClasses = new List<IOptionClass>();

        public void AddOptionClass(IOptionClass optionClass)
        {
            var index = _optionClasses.FindIndex(o => o.Id == optionClass.Id);
            if (index < 0)
                _optionClasses.Add(optionClass);
            else
                _optionClasses[index] = optionClass;
        }
        public ViewModel GetViewModel()
        {
            return this;
        }
    }
}
