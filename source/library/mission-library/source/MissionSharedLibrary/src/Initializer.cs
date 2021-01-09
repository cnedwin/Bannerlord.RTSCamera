using MissionLibrary;
using MissionLibrary.Provider;
using MissionSharedLibrary.Controller.Camera;
using MissionSharedLibrary.HotKey;
using MissionSharedLibrary.Provider;
using System;
using MissionSharedLibrary.Controller;
using MissionSharedLibrary.HotKey.Category;
using MissionSharedLibrary.View;

namespace MissionSharedLibrary
{
    public class Initializer
    {
        public static void Initialize()
        {
            Global.Initialize();
            RegisterProviders();
            GeneralGameKeyCategories.RegisterGameKeyCategory();
        }

        public static void Clear()
        {
            Global.Clear();
        }

        private static void RegisterProviders()
        {
            RegisterProvider(() => new GameKeyCategoryManager(), new Version(1, 0));
            RegisterProvider(() => new CameraControllerManager(), new Version(1, 0));
            RegisterProvider(() => new InputControllerFactory(), new Version(1, 0));
            RegisterProvider(() => new MissionStartingManager(), new Version(1, 0));
            RegisterProvider(() => new MenuManager(), new Version(1, 0));
        }

        public static void RegisterProvider<T>(Func<ITag<T>> creator, Version providerVersion) where T : class, ITag<T>
        {
            Global.RegisterProvider(ProviderCreator.Create(creator, providerVersion));
        }

    }
}
