using System;
using System.Collections.Generic;
using System.Text;
using MissionLibrary.View;
using TaleWorlds.Library;

namespace MissionSharedLibrary.View
{
    public class MenuManager : AMenuManager
    {
        public override IMenuClassCollection MenuClassCollection { get; } = new MenuClassCollection();
    }
}
