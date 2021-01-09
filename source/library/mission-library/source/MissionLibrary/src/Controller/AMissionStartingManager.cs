using MissionLibrary.Provider;
using TaleWorlds.MountAndBlade.View.Missions;

namespace MissionLibrary.Controller
{

    public abstract class AMissionStartingHandler
    {
        public abstract void OnCreated(MissionView entranceView);

        public abstract void OnPreMissionTick(MissionView entranceView, float dt);
    }

    public abstract class AMissionStartingManager : ITag<AMissionStartingManager>
    {
        public AMissionStartingManager Self => this;

        public abstract void OnCreated(MissionView entranceView);

        public abstract void OnPreMissionTick(MissionView entranceView, float dt);

        public abstract void AddHandler(AMissionStartingHandler handler);

    }
}
