using System;
using System.Collections.Generic;
using System.Text;

namespace PersianEditor.Models
{
    public static class MainWindowState
    {
        public static bool FullScreen { get; set; }
        public static int LastLeftPosition { get; set; }
        public static int LastTopPosition { get; set; }
        public static int LastWidth { get; set; }
        public static int LastHeight { get; set; }
    }
}
