using System;
using System.Collections.Generic;
using System.Text;
using MissionLibrary.Config.HotKey;
using MissionSharedLibrary.HotKey.Category;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace MissionSharedLibrary.src.Controller.MissionBehaviors
{
    public class InputController : MissionLogic
    {
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (Input.IsKeyPressed(GeneralGameKeyCategories.GetKey(GeneralGameKey.OpenMenu)))
            {
                Utility.DisplayMessage("L pressed.");
            }
        }
    }
}
