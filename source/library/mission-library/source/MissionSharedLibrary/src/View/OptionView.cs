using System;
using MissionLibrary;
using MissionLibrary.View;
using MissionSharedLibrary.HotKey.Category;
using TaleWorlds.InputSystem;

namespace MissionSharedLibrary.View
{
    public class OptionView : MissionMenuViewBase
    {
        public OptionView(int viewOrderPriority, Version version)
            : base(viewOrderPriority, "MissionLibrary" + nameof(OptionView) + "-" + version)
        {
        }

        public override void OnMissionScreenTick(float dt)
        {
            base.OnMissionScreenTick(dt);
            if (IsActivated)
            {
                if (GauntletLayer.Input.IsKeyReleased(InputKey.K))
                    DeactivateMenu();
            }
            else if (Input.IsKeyReleased(InputKey.K))
                ActivateMenu();
        }

        protected override MissionMenuVMBase GetDataSource()
        {
            return new OptionVM(AMenuManager.Get().MenuClassCollection, OnCloseMenu);
        }
    }
}
