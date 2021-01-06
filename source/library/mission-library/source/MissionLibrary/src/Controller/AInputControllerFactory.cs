using MissionLibrary.Provider;
using TaleWorlds.MountAndBlade;

namespace MissionLibrary.Controller
{
    public abstract class AInputControllerFactoryTag : ITag<AInputControllerFactoryTag>
    {
        public AInputControllerFactoryTag Self => this;
    }
    public abstract class AInputControllerFactory : AInputControllerFactoryTag
    {
        public abstract MissionLogic CreateInputController(Mission mission);
    }
}
