using TaleWorlds.Library;

namespace MissionLibrary.View
{
    public interface IViewModelProvider<out T> where T : ViewModel
    {
        T GetViewModel();
    }
}
