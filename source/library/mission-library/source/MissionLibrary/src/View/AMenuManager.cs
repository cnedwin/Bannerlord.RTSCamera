using MissionLibrary.Provider;

namespace MissionLibrary.View
{
    public abstract class AMenuMangerTag : ITag<AMenuMangerTag>
    {
        public AMenuMangerTag Self => this;
    }

    public abstract class AMenuManager : AMenuMangerTag
    {
        public static AMenuManager Get()
        {
            return Global.GetProvider<AMenuMangerTag, AMenuManager>();
        }
        
        public abstract void AddOptionClass(IOptionClass optionClass);

        public abstract string MovieName { get; }
    }
}