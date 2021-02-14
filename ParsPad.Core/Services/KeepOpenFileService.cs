using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Farcin.Editor.Core.Models;
using Farcin.Editor.Core.Models.Setting;
using Farcin.Editor.Core.TextProcessor;

namespace Farcin.Editor.Core.Services {

    public class KeepOpenFileService {

        private string _directory = Application.StartupPath + "\\_keep";

        public IEnumerable<TxtEditor> Editors { get; set; }

        public KeepOpenFileService(IEnumerable<TxtEditor> editors) {
            Editors = editors;
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);
        }

        public IList<TxtFileSetting> CreateOpenedFilesSetting() {
            var result = new List<TxtFileSetting>();
            foreach(var edt in Editors) {
                if (string.IsNullOrEmpty(edt.File.Text))
                    continue;

                var setting = new TxtFileSetting {
                    Name = edt.Title,
                    BackColor = ColorTranslator.ToWin32(edt.BackColor),
                    CurrentLine = edt.CurrentLineIndex,
                    FilePath = edt.File.Path,
                    FontName = edt.Font.Name,
                    FontSize = edt.Font.Size,
                    ForeColor = ColorTranslator.ToWin32(edt.ForeColor),
                    IsRtl = edt.Rtl,
                    Saved = edt.File.SavedToHard,
                    HasPendingChanges = edt.File.Changed
                };
                if(edt.File.Changed) {
                    setting.TempFilePath = $"{_directory}\\{Guid.NewGuid()}.ats";
                    TxtProcessor.WriteToFile(setting.TempFilePath, edt.Text);
                }
                result.Add(setting);
            }

            return result;
        }
    }
}
