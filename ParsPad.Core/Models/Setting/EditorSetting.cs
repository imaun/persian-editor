using System;
using System.Collections.Generic;
using System.Text;

namespace Farcin.Editor.Core.Models.Setting
{
    public class EditorSetting
    {

        #region Properties

        public bool AutoSave { get; set; }
        /// <summary>
        /// AutoSave delay in milliseconds
        /// </summary>
        public int AutoSaveInterval { get; set; }
        public string FontName { get; set; }
        public bool WordWrap { get; set; }

        #endregion
    }
}
