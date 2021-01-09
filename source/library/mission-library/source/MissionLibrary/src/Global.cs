using MissionLibrary.Provider;

namespace MissionLibrary
{
    public static class Global
    {
        private static bool _isInitialized;

        private static ProviderManager ProviderManager { get; set; }

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
            ProviderManager = new ProviderManager();
        }
        public static void RegisterProvider<T>(IProvider<T> newProvider) where T : class, ITag<T>
        {
            ProviderManager.RegisterProvider(newProvider);
        }

        public static T GetProvider<T>() where T : class, ITag<T>
        {
            return ProviderManager.GetProvider<T>();
        }

        public static void Clear()
        {
            _isInitialized = false;
            ProviderManager = null;
        }
    }
}
