using System;
using System.Collections.Generic;
using System.Text;
using MissionLibrary;
using MissionLibrary.Controller;
using MissionSharedLibrary.View;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Missions;

namespace MissionSharedLibrary.Controller
{
    public class MissionStartingManager : AMissionStartingManager
    {
        private readonly List<AMissionStartingHandler> _handlers = new List<AMissionStartingHandler>();

        public static void AddMissionBehaviour(MissionView entranceView, MissionBehaviour behaviour)
        {
            behaviour.OnAfterMissionCreated();
            entranceView.Mission.AddMissionBehaviour(behaviour);
        }

        public override void OnCreated(MissionView entranceView)
        {
            AddMissionLibraryMissionBehaviors(entranceView);
            foreach (var handler in _handlers)
            {
                handler.OnCreated(entranceView);
            }
        }

        public override void OnPreMissionTick(MissionView entranceView, float dt)
        {
            foreach (var handler in _handlers)
            {
                handler.OnPreMissionTick(entranceView, dt);
            }
        }

        public override void AddHandler(AMissionStartingHandler handler)
        {
            _handlers.Add(handler);
        }

        private void AddMissionLibraryMissionBehaviors(MissionView entranceView)
        {
            AddMissionBehaviour(entranceView, Global.GetProvider<AInputControllerFactory>().CreateInputController(entranceView.Mission));
            AddMissionBehaviour(entranceView, new OptionView(24, new Version(1, 0, 0)));
        }
    }
}
