using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions;

namespace MissionLibrary.View
{
    public interface IOption : IViewModelProvider<ViewModel>
    {
        void Commit();
        void Cancel();
    }
}
