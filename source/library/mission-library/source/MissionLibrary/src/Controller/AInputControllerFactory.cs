using MissionLibrary.Provider;
using TaleWorlds.MountAndBlade;

namespace MissionLibrary.Controller
{
    public abstract class AInputControllerFactory : ITag<AInputControllerFactory>
    {
        public abstract MissionLogic CreateInputController(Mission mission);
        public AInputControllerFactory Self => this;
    }
}
