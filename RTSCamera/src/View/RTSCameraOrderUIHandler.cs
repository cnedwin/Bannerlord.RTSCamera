using RTSCamera.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions.Order;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.View.Screen;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;

namespace RTSCamera.View
{
    [OverrideView(typeof(MissionOrderUIHandler))]
    public class RTSCameraOrderUIHandler : MissionView, ISiegeDeploymentView
    {
        private bool isGamepadActive
        {
            get
            {
                return TaleWorlds.InputSystem.Input.IsControllerConnected && !TaleWorlds.InputSystem.Input.IsMouseActive;
            }
        }

        private void RegisterReload()
        {
            MissionEvent.PreSwitchTeam += OnPreSwitchTeam;
            MissionEvent.PostSwitchTeam += OnPostSwitchTeam;
        }

        private void UnregisterReload()
        {
            MissionEvent.PreSwitchTeam -= OnPreSwitchTeam;
            MissionEvent.PostSwitchTeam -= OnPostSwitchTeam;
        }
        private void OnPreSwitchTeam()
        {
            dataSource.TryCloseToggleOrder();
            FinalizeViewAndVM();
        }

        private void OnPostSwitchTeam()
        {
            InitailizeViewAndVM();
            OnMissionScreenActivate();
        }

        public bool exitWithRightClick = true;

        private SiegeMissionView _siegeMissionView;
        private List<DeploymentSiegeMachineVM> _deploymentPointDataSources;
        private RTSCameraOrderTroopPlacer _orderTroopPlacer;
        public GauntletLayer gauntletLayer;
        public MissionOrderVM dataSource;
        private GauntletMovie _viewMovie;
        private SiegeDeploymentHandler _siegeDeploymentHandler;
        public bool IsDeployment;
        private bool isInitialized;
        private bool _isTransferEnabled;
        private const string _radialOrderMovieName = "OrderRadial";
        private const string _barOrderMovieName = "OrderBar";
        private float _holdTime;
        private bool _holdExecuted;
        private Vec2 _deploymentPointWidgetSize;

        private float _minHoldTimeForActivation
        {
            get
            {
                return 0f;
            }
        }



        public RTSCameraOrderUIHandler()
        {
            ViewOrderPriorty = 19;
        }
        public void OnActivateToggleOrder()
        {
            this.SetLayerEnabled(true);
        }

        public void OnDeactivateToggleOrder()
        {
            if (!dataSource.TroopController.IsTransferActive)
            {
                this.SetLayerEnabled(false);
            }
        }

        public void OnTransferTroopsFinisedDelegate()
        {
            this.SetLayerEnabled(false);
        }

        public override void OnMissionScreenInitialize()
        {
            base.OnMissionScreenInitialize();

            RegisterReload();
            InitailizeViewAndVM();
        }

        private void OnTransferFinished()
        {
            this.SetLayerEnabled(false);
        }

        private void SetLayerEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                if (dataSource == null || dataSource.ActiveTargetState == 0)
                {
                    this._orderTroopPlacer.SuspendTroopPlacer = false;
                }
                base.MissionScreen.SetOrderFlagVisibility(true);
                if (gauntletLayer != null)
                {
                    ScreenManager.SetSuspendLayer(gauntletLayer, false);
                }
                Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(true));
                return;
            }
            this._orderTroopPlacer.SuspendTroopPlacer = true;
            base.MissionScreen.SetOrderFlagVisibility(false);
            if (gauntletLayer != null)
            {
                ScreenManager.SetSuspendLayer(gauntletLayer, true);
            }
            base.MissionScreen.SetRadialMenuActiveState(false);
            Game.Current.EventManager.TriggerEvent<MissionPlayerToggledOrderViewEvent>(new MissionPlayerToggledOrderViewEvent(false));
        }

        private void InitailizeViewAndVM()
        {
            MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("MissionOrderHotkeyCategory"));
            MissionScreen.OrderFlag = new OrderFlag(Mission, MissionScreen);
            _orderTroopPlacer = Mission.GetMissionBehaviour<RTSCameraOrderTroopPlacer>();
            MissionScreen.SetOrderFlagVisibility(false);
            _siegeDeploymentHandler = Mission.GetMissionBehaviour<SiegeDeploymentHandler>();
            IsDeployment = _siegeDeploymentHandler != null;
            if (IsDeployment)
            {
                _siegeMissionView = Mission.GetMissionBehaviour<SiegeMissionView>();
                if (_siegeMissionView != null)
                    _siegeMissionView.OnDeploymentFinish += OnDeploymentFinish;
                _deploymentPointDataSources = new List<DeploymentSiegeMachineVM>();
            }
            dataSource = new MissionOrderVM(MissionScreen.CombatCamera,
                IsDeployment ? _siegeDeploymentHandler.DeploymentPoints.ToList() : new List<DeploymentPoint>(),
                ToggleScreenRotation,
                IsDeployment,
                MissionScreen.GetOrderFlagPosition,
                RefreshVisuals,
                SetSuspendTroopPlacer,
                OnActivateToggleOrder,
                OnDeactivateToggleOrder,
                OnTransferTroopsFinisedDelegate,
                false);

            if (IsDeployment)
            {
                foreach (DeploymentPoint deploymentPoint in _siegeDeploymentHandler.DeploymentPoints)
                {
                    DeploymentSiegeMachineVM deploymentSiegeMachineVm = new DeploymentSiegeMachineVM(deploymentPoint, null, MissionScreen.CombatCamera, dataSource.DeploymentController.OnRefreshSelectedDeploymentPoint, dataSource.DeploymentController.OnEntityHover, false);
                    Vec3 origin = deploymentPoint.GameEntity.GetFrame().origin;
                    for (int index = 0; index < deploymentPoint.GameEntity.ChildCount; ++index)
                    {
                        if (deploymentPoint.GameEntity.GetChild(index).Tags.Contains("deployment_point_icon_target"))
                        {
                            Vec3 vec3 = origin + deploymentPoint.GameEntity.GetChild(index).GetFrame().origin;
                            break;
                        }
                    }
                    _deploymentPointDataSources.Add(deploymentSiegeMachineVm);
                    deploymentSiegeMachineVm.RemainingCount = 0;
                    _deploymentPointWidgetSize = new Vec2(75f / Screen.RealScreenResolutionWidth, 75f / Screen.RealScreenResolutionHeight);
                }
            }
            gauntletLayer = new GauntletLayer(ViewOrderPriorty, "GauntletLayer");
            gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
            string movieName = (BannerlordConfig.OrderType == 0) ? "OrderBar" : "OrderRadial";
            _viewMovie = gauntletLayer.LoadMovie(movieName, dataSource);
            MissionScreen.AddLayer(gauntletLayer);
            if (IsDeployment)
                gauntletLayer.InputRestrictions.SetInputRestrictions();
            else if (!dataSource.IsToggleOrderShown)
                ScreenManager.SetSuspendLayer(gauntletLayer, true);
            dataSource.InputRestrictions = gauntletLayer.InputRestrictions;
            ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
            dataSource.AfterInitialize();
        }



        public override void OnMissionScreenFinalize()
        {
            base.OnMissionScreenFinalize();
            FinalizeViewAndVM();
            UnregisterReload();
        }

        private void FinalizeViewAndVM()
        {
            ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
            _deploymentPointDataSources = null;
            _orderTroopPlacer = null;
            _viewMovie = null;
            gauntletLayer = null;
            dataSource.OnFinalize();
            dataSource = null;
            _siegeDeploymentHandler = null;
        }

        private void OnDeploymentFinish()
        {
            IsDeployment = false;
            dataSource.DeploymentController.FinalizeDeployment();
            _deploymentPointDataSources.Clear();
            _orderTroopPlacer.SuspendTroopPlacer = true;
            MissionScreen.SetOrderFlagVisibility(false);
            if (_siegeMissionView == null)
                return;
            SiegeMissionView siegeMissionView = _siegeMissionView;
            siegeMissionView.OnDeploymentFinish = (OnPlayerDeploymentFinishDelegate)Delegate.Remove(siegeMissionView.OnDeploymentFinish, new OnPlayerDeploymentFinishDelegate(this.OnDeploymentFinish));
        }

        public override bool OnEscape()
        {
            bool isToggleOrderShown = dataSource.IsToggleOrderShown;
            dataSource.OnEscape();
            return isToggleOrderShown;
        }

        private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
        {
            if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.OrderType)
            {
                gauntletLayer.ReleaseMovie(_viewMovie);
                string movieName = (BannerlordConfig.OrderType == 0) ? "OrderBar" : "OrderRadial";
                _viewMovie = gauntletLayer.LoadMovie(movieName, dataSource);
            }
        }


        public override void OnMissionScreenTick(float dt)
        {
            base.OnMissionScreenTick(dt);
            TickInput(dt);
            dataSource.Update();
            // TODO: Should the Tick go somewhere else?
            //dataSource.Tick(dt);
            if (dataSource.IsToggleOrderShown)
            {
                if (_orderTroopPlacer.SuspendTroopPlacer && dataSource.ActiveTargetState == 0)
                    _orderTroopPlacer.SuspendTroopPlacer = false;
                _orderTroopPlacer.IsDrawingForced = dataSource.IsMovementSubOrdersShown;
                _orderTroopPlacer.IsDrawingFacing = dataSource.IsFacingSubOrdersShown;
                _orderTroopPlacer.IsDrawingForming = false;
                _orderTroopPlacer.IsDrawingAttaching = cursorState == MissionOrderVM.CursorState.Attach;
                _orderTroopPlacer.UpdateAttachVisuals(cursorState == MissionOrderVM.CursorState.Attach);
                if (cursorState == MissionOrderVM.CursorState.Face)
                {
                    Vec2 orderLookAtDirection = OrderController.GetOrderLookAtDirection
                           (Mission.MainAgent.Team.PlayerOrderController.SelectedFormations, MissionScreen.OrderFlag.Position.AsVec2);
                    base.MissionScreen.OrderFlag.SetArrowVisibility(true, orderLookAtDirection);
                }
                else
                    MissionScreen.OrderFlag.SetArrowVisibility(false, Vec2.Invalid);
                if (cursorState == MissionOrderVM.CursorState.Form)
                {
                    float orderFormCustomWidth = OrderController.GetOrderFormCustomWidth(Mission.MainAgent.Team.PlayerOrderController.SelectedFormations, MissionScreen.OrderFlag.Position);
                    MissionScreen.OrderFlag.SetWidthVisibility(true, orderFormCustomWidth);
                }
                else
                    MissionScreen.OrderFlag.SetWidthVisibility(false, -1f);
                if (isGamepadActive)
                {
                    OrderItemVM lastSelectedOrderItem = dataSource.LastSelectedOrderItem;
                    if (lastSelectedOrderItem == null || lastSelectedOrderItem.IsTitle)
                    {
                        MissionScreen.SetRadialMenuActiveState(false);
                        if (_orderTroopPlacer.SuspendTroopPlacer && dataSource.ActiveTargetState == 0)
                        {
                            _orderTroopPlacer.SuspendTroopPlacer = false;
                        }
                    }
                    else
                    {
                        base.MissionScreen.SetRadialMenuActiveState(true);
                        if (!_orderTroopPlacer.SuspendTroopPlacer)
                        {
                            _orderTroopPlacer.SuspendTroopPlacer = true;
                        }
                    }
                }
            }
            else if (dataSource.TroopController.IsTransferActive)
            {
                gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            }
            else
            {
                if (!_orderTroopPlacer.SuspendTroopPlacer)
                {
                    _orderTroopPlacer.SuspendTroopPlacer = true;
                }
                gauntletLayer.InputRestrictions.ResetInputRestrictions();
            }
            if (IsDeployment)
            {
                if (MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.RightMouseButton) || base.MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.ControllerLTrigger))
                {
                    gauntletLayer.InputRestrictions.SetMouseVisibility(false);
                }
                else
                {
                    this.gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                }
            }
            MissionScreen.OrderFlag.IsTroop = (dataSource.ActiveTargetState == 0);
            MissionScreen.OrderFlag.Tick(dt);
        }

            private void RefreshVisuals()
        {
            if (!IsDeployment)
                return;
            foreach (DeploymentSiegeMachineVM deploymentPointDataSource in _deploymentPointDataSources)
                deploymentPointDataSource.RefreshWithDeployedWeapon();
        }

        public override void OnMissionScreenActivate()
        {
            base.OnMissionScreenActivate();
            dataSource.AfterInitialize();
            isInitialized = true;
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            if (!isInitialized || !agent.IsHuman)
                return;
            dataSource.TroopController.AddTroops(agent);
        }

        public override void OnAgentRemoved(
          Agent affectedAgent,
          Agent affectorAgent,
          AgentState agentState,
          KillingBlow killingBlow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
            if (!affectedAgent.IsHuman)
                return;
            dataSource.TroopController.RemoveTroops(affectedAgent);
        }

        private IOrderable GetFocusedOrderableObject()
        {
            return MissionScreen.OrderFlag.FocusedOrderableObject;
        }

        private void SetSuspendTroopPlacer(bool value)
        {
            _orderTroopPlacer.SuspendTroopPlacer = value;
            MissionScreen.SetOrderFlagVisibility(!value);
        }

        void ISiegeDeploymentView.OnEntityHover(GameEntity hoveredEntity)
        {
            if (gauntletLayer.HitTest())
                return;
            dataSource.DeploymentController.OnEntityHover(hoveredEntity);
        }

        void ISiegeDeploymentView.OnEntitySelection(GameEntity selectedEntity)
        {
            dataSource.DeploymentController.OnEntitySelect(selectedEntity);
        }

        private void ToggleScreenRotation(bool isLocked)
        {
            MissionScreen.SetFixedMissionCameraActive(isLocked);
        }

        [Conditional("DEBUG")]
        private void TickInputDebug()
        {
        }

        public MissionOrderVM.CursorState cursorState
        {
            get
            {
                if (dataSource.IsFacingSubOrdersShown)
                {
                    return MissionOrderVM.CursorState.Face;
                }
                return MissionOrderVM.CursorState.Move;
            }
        }

        private void TickInput(float dt)
        {
            if (base.Input.IsGameKeyDown(77) && !dataSource.IsToggleOrderShown)
            {
                _holdTime += dt;
                if (_holdTime >= this._minHoldTimeForActivation)
                {
                    dataSource.OpenToggleOrder(true, true);
                    _holdExecuted = true;
                }
            }
            else if (!Input.IsGameKeyDown(77))
            {
                if (_holdExecuted && dataSource.IsToggleOrderShown)
                {
                    dataSource.TryCloseToggleOrder();
                    _holdExecuted = false;
                }
                _holdTime = 0f;
            }
            if (dataSource.IsToggleOrderShown)
            {
                if (dataSource.TroopController.IsTransferActive && gauntletLayer.Input.IsHotKeyReleased("Exit"))
                    dataSource.TroopController.IsTransferActive = false;
                if (dataSource.TroopController.IsTransferActive != _isTransferEnabled)
                {
                    _isTransferEnabled = dataSource.TroopController.IsTransferActive;
                    if (!_isTransferEnabled)
                    {
                        gauntletLayer.IsFocusLayer = false;
                        ScreenManager.TryLoseFocus(gauntletLayer);
                    }
                    else
                    {
                        gauntletLayer.IsFocusLayer = true;
                        ScreenManager.TrySetFocus(gauntletLayer);
                    }
                }
                if (dataSource.ActiveTargetState == 0 && (Input.IsKeyReleased(InputKey.LeftMouseButton) || Input.IsKeyReleased(InputKey.ControllerRTrigger)))
                {
                    OrderItemVM lastSelectedOrderItem = dataSource.LastSelectedOrderItem;
                    if (lastSelectedOrderItem != null && !lastSelectedOrderItem.IsTitle && isGamepadActive)
                    {
                        dataSource.ApplySelectedOrder();
                    }
                    else
                    {
                        switch (cursorState)
                        {
                            case MissionOrderVM.CursorState.Move:
                                IOrderable focusedOrderableObject = GetFocusedOrderableObject();
                                if (focusedOrderableObject != null)
                                {
                                    dataSource.OrderController.SetOrderWithOrderableObject(focusedOrderableObject);
                                }
                                break;
                            case MissionOrderVM.CursorState.Face:
                                dataSource.OrderController.SetOrderWithPosition(OrderType.LookAtDirection, new WorldPosition(Mission.Scene, UIntPtr.Zero, MissionScreen.GetOrderFlagPosition(), false));
                                break;
                            case MissionOrderVM.CursorState.Form:
                                dataSource.OrderController.SetOrderWithPosition(OrderType.FormCustom, new WorldPosition(Mission.Scene, UIntPtr.Zero, MissionScreen.GetOrderFlagPosition(), false));
                                break;
                        }
                    }
                }
                //if (this.Input.IsAltDown())
                //{
                //    bool isMouseVisible = this.dataSource.IsTransferActive || !this.gauntletLayer.InputRestrictions.MouseVisibility;
                //    this.gauntletLayer.InputRestrictions.SetInputRestrictions(isMouseVisible, isMouseVisible ? InputUsageMask.Mouse : InputUsageMask.Invalid);
                //}
                if (exitWithRightClick && Input.IsKeyReleased(InputKey.RightMouseButton))
                    dataSource.OnEscape();
            }
            int pressedIndex = -1;
            if ((!isGamepadActive || dataSource.IsToggleOrderShown) && !Input.IsControlDown())
            {
                if (base.Input.IsGameKeyPressed(59))
                {
                    pressedIndex = 0;
                }
                else if (base.Input.IsGameKeyPressed(60))
                {
                    pressedIndex = 1;
                }
                else if (base.Input.IsGameKeyPressed(61))
                {
                    pressedIndex = 2;
                }
                else if (base.Input.IsGameKeyPressed(62))
                {
                    pressedIndex = 3;
                }
                else if (base.Input.IsGameKeyPressed(63))
                {
                    pressedIndex = 4;
                }
                else if (base.Input.IsGameKeyPressed(64))
                {
                    pressedIndex = 5;
                }
                else if (base.Input.IsGameKeyPressed(65))
                {
                    pressedIndex = 6;
                }
                else if (base.Input.IsGameKeyPressed(66))
                {
                    pressedIndex = 7;
                }
                else if (base.Input.IsGameKeyPressed(67))
                {
                    pressedIndex = 8;
                }
            }
            if (pressedIndex > -1)
                dataSource.OnGiveOrder(pressedIndex);
            int formationTroopIndex = -1;
            if (base.Input.IsGameKeyPressed(68))
            {
                formationTroopIndex = 100;
            }
            else if (base.Input.IsGameKeyPressed(69))
            {
                formationTroopIndex = 0;
            }
            else if (base.Input.IsGameKeyPressed(70))
            {
                formationTroopIndex = 1;
            }
            else if (base.Input.IsGameKeyPressed(71))
            {
                formationTroopIndex = 2;
            }
            else if (base.Input.IsGameKeyPressed(72))
            {
                formationTroopIndex = 3;
            }
            else if (base.Input.IsGameKeyPressed(73))
            {
                formationTroopIndex = 4;
            }
            else if (base.Input.IsGameKeyPressed(74))
            {
                formationTroopIndex = 5;
            }
            else if (base.Input.IsGameKeyPressed(75))
            {
                formationTroopIndex = 6;
            }
            else if (base.Input.IsGameKeyPressed(76))
            {
                formationTroopIndex = 7;
            }
            if (base.Input.IsGameKeyPressed(78))
            {
                dataSource.SelectNextTroop(1);
            }
            else if (base.Input.IsGameKeyPressed(79))
            {
                dataSource.SelectNextTroop(-1);
            }
            else if (base.Input.IsGameKeyPressed(80))
            {
                dataSource.ToggleSelectionForCurrentTroop();
            }
            if (formationTroopIndex != -1)
            {
                dataSource.OnSelect(formationTroopIndex);
            }
            if (base.Input.IsGameKeyPressed(58))
            {
                dataSource.ViewOrders();
            }
        }
        public delegate void OnManagedOptionChangedDelegate(ManagedOptions.ManagedOptionsType changedManagedOptionsType);
    }
}
