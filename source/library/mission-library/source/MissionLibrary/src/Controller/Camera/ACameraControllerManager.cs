using MissionLibrary.Provider;

namespace MissionLibrary.Controller.Camera
{
    public abstract class ACameraControllerManager : ITag<ACameraControllerManager>
    {
        public static ACameraControllerManager Get()
        {
            return Global.GetProvider<ACameraControllerManager>();
        }

        public abstract ICameraController Instance { get; set; }
        public abstract void Clear();
        public ACameraControllerManager Self => this;
    }
}
