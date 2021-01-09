using System;
using MissionLibrary.Provider;
using TaleWorlds.Library;

namespace MissionLibrary.View
{
    public interface IMenuClassCollection
    {
        public abstract void AddOptionClass(IOptionClass optionClass);

        public abstract ViewModel GetViewModel();
    }
}