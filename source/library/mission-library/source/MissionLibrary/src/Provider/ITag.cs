using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionLibrary.Provider
{
    public interface ITag<out T> where T : class, ITag<T>
    {
        T Self { get; }
    }
}
