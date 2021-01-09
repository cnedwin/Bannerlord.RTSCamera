using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Missions;

namespace MissionSharedLibrary.View
{
    public abstract class MissionMenuViewBase : MissionView
    {
        private readonly string _movieName;
        private MissionMenuVMBase _dataSource;
        protected GauntletLayer GauntletLayer;
        private GauntletMovie _movie;

        public bool IsActivated { get; set; }

        public MissionMenuViewBase(int viewOrderPriority, string movieName)
        {
            ViewOrderPriorty = viewOrderPriority;
            _movieName = movieName;
        }

        protected abstract MissionMenuVMBase GetDataSource();

        public override void OnMissionScreenFinalize()
        {
            base.OnMissionScreenFinalize();
            GauntletLayer = null;
            _dataSource?.OnFinalize();
            _dataSource = null;
            _movie = null;
        }

        public void ToggleMenu()
        {
            if (IsActivated)
                DeactivateMenu();
            else
                ActivateMenu();
        }

        public void ActivateMenu()
        {
            IsActivated = true;
            _dataSource = GetDataSource();
            if (_dataSource == null)
                return;
            GauntletLayer = new GauntletLayer(ViewOrderPriorty) { IsFocusLayer = true };
            GauntletLayer.InputRestrictions.SetInputRestrictions();
            GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
            _movie = GauntletLayer.LoadMovie(_movieName, _dataSource);
            MissionScreen.AddLayer(GauntletLayer);
            ScreenManager.TrySetFocus(GauntletLayer);
            PauseGame();
        }

        public void DeactivateMenu()
        {
            _dataSource?.CloseMenu();
        }
        protected void OnCloseMenu()
        {
            IsActivated = false;
            _dataSource.OnFinalize();
            _dataSource = null;
            GauntletLayer.InputRestrictions.ResetInputRestrictions();
            MissionScreen.RemoveLayer(GauntletLayer);
            _movie = null;
            GauntletLayer = null;
            UnpauseGame();
        }

        public override void OnMissionScreenTick(float dt)
        {
            base.OnMissionScreenTick(dt);
            if (IsActivated)
            {
                if (GauntletLayer.Input.IsKeyReleased(InputKey.RightMouseButton) ||
                    GauntletLayer.Input.IsHotKeyReleased("Exit"))
                    DeactivateMenu();
            }
        }

        public override void OnRemoveBehaviour()
        {
            base.OnRemoveBehaviour();

            Game.Current.GameStateManager.ActiveStateDisabledByUser = false;
        }

        private void PauseGame()
        {
            MBCommon.PauseGameEngine();
        }

        private void UnpauseGame()
        {
            MBCommon.UnPauseGameEngine();
        }
    }
}
