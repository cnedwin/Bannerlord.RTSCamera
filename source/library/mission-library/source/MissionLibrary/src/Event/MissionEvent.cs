using System;
using TaleWorlds.MountAndBlade;

namespace MissionLibrary.Event
{
    public class MissionEvent
    {
        public static event Action<Agent> MainAgentWillBeChangedToAnotherOne;

        public static event Action<bool> ToggleFreeCamera;

        public delegate void SwitchTeamDelegate();

        public static event SwitchTeamDelegate PreSwitchTeam;
        public static event SwitchTeamDelegate PostSwitchTeam;

        public static event Action MissionMenuClosed;

        public static void Clear()
        {
            MainAgentWillBeChangedToAnotherOne = null;
            ToggleFreeCamera = null;
            PreSwitchTeam = null;
            PostSwitchTeam = null;
            MissionMenuClosed = null;
        }

        public static void OnMainAgentWillBeChangedToAnotherOne(Agent newAgent)
        {
            MainAgentWillBeChangedToAnotherOne?.Invoke(newAgent);
        }

        public static void OnToggleFreeCamera(bool freeCamera)
        {
            ToggleFreeCamera?.Invoke(freeCamera);
        }

        public static void OnPreSwitchTeam()
        {
            PreSwitchTeam?.Invoke();
        }

        public static void OnPostSwitchTeam()
        {
            PostSwitchTeam?.Invoke();
        }

        private static void OnMissionMenuClosed()
        {
            MissionMenuClosed?.Invoke();
        }
    }
}
