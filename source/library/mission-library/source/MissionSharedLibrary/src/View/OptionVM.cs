using System;
using System.Collections.Generic;
using MissionLibrary.View;

namespace MissionSharedLibrary.View
{
    public class OptionVM : MissionMenuVMBase
    {
        private readonly IMenuClassCollection _menuClassCollection;

        public OptionVM(IMenuClassCollection menuClassCollection, Action closeMenu)
            : base(closeMenu)
        {
            _menuClassCollection = menuClassCollection;
        }
        
    }
}
