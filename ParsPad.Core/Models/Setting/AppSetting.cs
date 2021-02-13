using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;

namespace Farcin.Editor.Core.Models.Setting {
    
    public class AppSetting {

        public AppSetting() {
            RecentFiles = new List<TxtFileSetting>();
            OpenedFiles = new List<TxtFileSetting>();
            DefaultFileSetting = new TxtFileSetting();
            Editor = new EditorSetting();
            UserInterface = new UserInterfaceSetting();
        }

        #region Properties

        public IList<TxtFileSetting> RecentFiles { get; set; }
        public bool HasRecentFiles => RecentFiles != null && RecentFiles.Count > 0;

        public IList<TxtFileSetting> OpenedFiles { get; set; }
        public bool HasOpenedFiles => OpenedFiles != null && OpenedFiles.Count > 0;

        public TxtFileSetting DefaultFileSetting { get; set; }
        public EditorSetting Editor { get; set; }
        public UserInterfaceSetting UserInterface { get; set; }

        #endregion

        public static string FilePath => 
            $"{Application.StartupPath}\\appsettings.josn";
        
        public static AppSetting Load() {
            AppSetting result = new AppSetting();
            if(File.Exists(FilePath)) {
                return readFromFile(FilePath);
            }
            else {
                result = getDefaultSetting();
                writeToFile(FilePath, result);
            }

            return result;
        }

        public void Save() {
            writeToFile(FilePath, this);
        }

        private static void writeToFile(string filePath, AppSetting setting) {
            using (StreamWriter writer = new StreamWriter(filePath)) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, setting);
            }
        }

        private static AppSetting readFromFile(string filePath) {
            AppSetting result = getDefaultSetting();
            using (var file = File.OpenText(FilePath)) {
                JsonSerializer serializer = new JsonSerializer();
                result = (AppSetting)serializer.Deserialize(
                    file,
                    typeof(AppSetting)
                );
            }

            return result;
        }

        private static AppSetting getDefaultSetting() {
            var fontConverter = new FontConverter();
            var backColor = SystemColors.Window; 
            var foreColor = SystemColors.ControlText;
            var defaultFont = new Font("Segoe UI", 12);

            return new AppSetting {
                DefaultFileSetting = new TxtFileSetting {
                    BackColor = ColorTranslator.ToWin32(backColor),
                    ForeColor = ColorTranslator.ToWin32(foreColor),
                    CurrentLine = 1,
                    FontName = fontConverter.ConvertToString(defaultFont),
                    FontSize = 12,
                    IsRtl = true,
                    Saved = false
                },
                Editor = new EditorSetting {
                    AutoSave = false,
                    AutoSaveInterval = 10000,
                    FontName = fontConverter.ConvertToString(defaultFont),
                    WordWrap = true
                },
                RecentFiles = new List<TxtFileSetting>(),
                UserInterface = new UserInterfaceSetting {
                    Window = new AppWindowSetting {
                        Height = 700,
                        Maximized = false,
                        Opacity = 100,
                        TopMost = false,
                        Width = 950
                    }
                }
            };
        }
    }
}
