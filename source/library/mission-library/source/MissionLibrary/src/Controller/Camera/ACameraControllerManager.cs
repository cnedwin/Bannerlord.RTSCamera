using MissionLibrary.Provider;

namespace MissionLibrary.Controller.Camera
{
    public abstract class ACameraControllerManagerTag : ITag<ACameraControllerManagerTag>
    {
        public ACameraControllerManagerTag Self => this;
    }
    public abstract class ACameraControllerManager : ACameraControllerManagerTag
    {
        public static ACameraControllerManager Get()
        {
            return Global.GetProvider<ACameraControllerManagerTag, ACameraControllerManager>();
        }

        public abstract ICameraController Instance { get; set; }
        public abstract void Clear();
    }
}
