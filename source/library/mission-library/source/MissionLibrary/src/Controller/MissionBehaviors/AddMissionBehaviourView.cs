﻿using System.Collections.Generic;
using MissionLibrary.Extension;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Missions;

namespace MissionLibrary.Controller.MissionBehaviors
{
    [DefaultView]
    class AddMissionBehaviourView : MissionView
    {
        public override void OnCreated()
        {
            base.OnCreated();

            Global.GetProvider<AMissionStartingManager>().OnCreated(this);
        }

        public override void OnPreMissionTick(float dt)
        {
            base.OnPreMissionTick(dt);

            Global.GetProvider<AMissionStartingManager>().OnPreMissionTick(this, dt);

            var self = Mission.GetMissionBehaviour<AddMissionBehaviourView>();
            if (self == this)
            {
                Mission.RemoveMissionBehaviour(self);
            }
        }
    }
}
