using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionLibrary.Provider
{
    public interface IProviderManager
    {
        void RegisterProvider<T>(IProvider<T> newProvider) where T : class, ITag<T>;
    }
}
