using System;
using System.Collections.Generic;
using System.IO;
using Farcin.Editor.Core.Models;
using Farcin.Editor.Core.TextProcessor;
using System.Windows.Forms;

namespace Farcin.Editor.Core.Services
{
    public class AutoSaveService
    {

        private string _directory = Application.StartupPath + "\\_auotsave";
        private string _autoSaveCfgPath = Application.StartupPath + "\\_autosave-cfg.json";

        public List<AutoSaveRecord> Records { get; set; }

        public AutoSaveService() {
            Records = new List<AutoSaveRecord>();
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);
        }


        public void Run(TxtEditor editor) {
            if(!editor.File.SavedToHard) {
                var record = new AutoSaveRecord {
                    CreateDate = DateTime.Now,
                    FileName = editor.File.Name,
                    FilePath = editor.File.Path,
                    SavedFileName = $"{_directory}\\{Guid.NewGuid()}.ats"
                };
                TxtProcessor.WriteToFile(
                    record.SavedFileName, 
                    editor.File.Text);
            }
        }


        public void Run(IEnumerable<TxtEditor> editors) {
            Records = new List<AutoSaveRecord>();
            foreach(var edt in editors) {
                if (edt.AutoSave && edt.File.SavedToHard) {
                    TxtProcessor.WriteToFile(edt.File.Path, edt.File.Text);
                    continue;
                }
                else if(edt.AutoSave && !edt.File.SavedToHard) {
                    var record = new AutoSaveRecord {
                        CreateDate = DateTime.Now,
                        FileName = edt.File.Name,
                        FilePath = edt.File.Path,
                        SavedFileName = $"{_directory}\\{Guid.NewGuid()}.ats"
                    };
                    TxtProcessor.WriteToFile(record.SavedFileName, edt.File.Text);
                    Records.Add(record);
                }
            }
        }
    }

    public class AutoSaveRecord
    {
        public DateTime CreateDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string SavedFileName { get; set; }
    }
}
