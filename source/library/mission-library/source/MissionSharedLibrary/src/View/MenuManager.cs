using System;
using System.Collections.Generic;
using System.Text;
using MissionLibrary;
using MissionLibrary.View;

namespace MissionSharedLibrary.View
{
    public class MenuManager : AMenuManager
    {
        private readonly List<IOptionClass> _optionClasses = new List<IOptionClass>();

        public MenuManager(Version version)
        {
            MovieName = nameof(MissionLibrary) + nameof(MenuManager) + version.ToString(3);
        }

        public override void AddOptionClass(IOptionClass optionClass)
        {
            var index = _optionClasses.FindIndex(o => o.Id == optionClass.Id);
            if (index < 0)
                _optionClasses.Add(optionClass);
            else
                _optionClasses[index] = optionClass;
        }

        public override string MovieName { get; }
    }
}
