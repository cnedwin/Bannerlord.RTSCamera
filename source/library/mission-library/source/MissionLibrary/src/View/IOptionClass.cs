using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace MissionLibrary.View
{
    public interface IOptionClass : IViewModelProvider<ViewModel>
    {
        string Id { get; }

        void AddOptionCategory(int column, IOptionCategory optionCategory);
    }
}
