using System.Drawing;
using Newtonsoft.Json;

namespace Farcin.Editor.Core.Models.Setting {
    
    /// <summary>
    /// Keep settings for a Text File.
    /// </summary>
    public class TxtFileSetting {

        private readonly FontConverter _fontConv;

        public TxtFileSetting() {
            _fontConv = new FontConverter();
        }

        public string Name { get; set; }
        public string FilePath { get; set; }
        public string TempFilePath { get; set; }
        public bool Saved { get; set; }
        public string FontName { get; set; }
        [JsonIgnore]
        public Font Font => (Font)_fontConv.ConvertFromString(FontName);
        public float FontSize { get; set; }
        public int ForeColor { get; set; }
        [JsonIgnore]
        public Color ForeColorValue => ColorTranslator.FromWin32(ForeColor);
        public int BackColor { get; set; }
        [JsonIgnore]
        public Color BackColorValue => ColorTranslator.FromWin32(BackColor);
        public bool IsRtl { get; set; }
        public int CurrentLine { get; set; }
        public bool HasPendingChanges { get; set; }
    }
}
