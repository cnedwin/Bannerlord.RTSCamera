using MissionLibrary.Provider;
using TaleWorlds.Library;

namespace MissionLibrary.View
{
    public abstract class AMenuManager : ITag<AMenuManager>
    {
        public static AMenuManager Get()
        {
            return Global.GetProvider<AMenuManager>();
        }
        
        public abstract IMenuClassCollection MenuClassCollection { get; }

        public AMenuManager Self => this;
    }
}
