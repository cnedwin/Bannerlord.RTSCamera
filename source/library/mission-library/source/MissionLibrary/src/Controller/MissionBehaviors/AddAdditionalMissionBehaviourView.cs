using System.Collections.Generic;
using MissionLibrary.Extension;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Missions;

namespace MissionLibrary.Controller.MissionBehaviors
{
    [DefaultView]
    class AddAdditionalMissionBehaviourView : MissionView
    {
        public override void OnCreated()
        {
            base.OnCreated();

            //foreach (var extension in RTSCameraExtension.Extensions)
            //{
            //    foreach (var missionBehaviour in extension.CreateMissionBehaviours(Mission))
            //    {
            //        AddMissionBehaviour(missionBehaviour);
            //    }
            //}

            foreach (var extension in MissionExtensionCollection.Extensions)
            {
                foreach (var missionBehaviour in extension.CreateMissionBehaviours(Mission))
                {
                    AddMissionBehaviour(missionBehaviour);
                }
            }
        }

        private void AddMissionBehaviour(MissionBehaviour behaviour)
        {
            behaviour.OnAfterMissionCreated();
            Mission.AddMissionBehaviour(behaviour);
        }
    }
}
