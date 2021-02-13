using System;
using System.Collections.Generic;
using System.Text;

namespace Farcin.Editor.Core.Models.Setting {
    public class UserInterfaceSetting {

        public UserInterfaceSetting() {
            Window = new AppWindowSetting();
        }

        public AppWindowSetting Window { get; set; }
        public int ActiveTabIndex { get; set; }

    }

    public class AppWindowSetting
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Maximized { get; set; }
        public double Opacity { get; set; }
        public bool TopMost { get; set; }
    }
}
