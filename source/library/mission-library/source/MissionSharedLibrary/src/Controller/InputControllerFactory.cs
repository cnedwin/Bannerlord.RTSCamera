using MissionLibrary.Controller;
using MissionSharedLibrary.src.Controller.MissionBehaviors;
using TaleWorlds.MountAndBlade;

namespace MissionSharedLibrary.Controller
{
    public class InputControllerFactory : AInputControllerFactory
    {
        public override MissionLogic CreateInputController(Mission mission)
        {
            return new InputController();
        }
    }
}
