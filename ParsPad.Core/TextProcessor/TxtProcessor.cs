using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Farcin.Editor.Core.Models;

namespace Farcin.Editor.Core.TextProcessor {

    public static class TxtProcessor {

        private static string _FILTER = "فایل های متنی|*.txt|همه فایل ها|*.*";
        private static string _DEFAULT_EXT = "txt";

        public static TxtFile ReadFile(string path) {
            return new TxtFile {
                Text = ReadFileText(path),
                Path = path
            };
        }

        public static string ReadFileText(string path) {
            string result = string.Empty;
            using (var reader = new StreamReader(path, Encoding.Default)) {
                result = reader.ReadToEnd();
                reader.Close();
            }
            return result;
        }

        public static void WriteToFile(string path, string content) {
            using (var writer = new StreamWriter(path, false, Encoding.UTF8)) {
                writer.Write(content);
                writer.Flush();
                writer.Close();
            }
        }

        public static void SaveFile(TxtFile file) 
            => WriteToFile(file.Path, file.Text);

        public static string SaveAs(TxtFile file) {
            string result = string.Empty;
            using (var saveDialog = new SaveFileDialog()) {
                saveDialog.DefaultExt = _DEFAULT_EXT;
                saveDialog.Filter = _FILTER;
                saveDialog.Title = "ذخیره فایل در...";
                if (saveDialog.ShowDialog() == DialogResult.OK) {
                    result = saveDialog.FileName;
                    WriteToFile(result, file.Text);
                    file.Path = result;
                    file.SavedToHard = true;
                    file.Changed = file.CancelSaveDialog = false;
                }
                else {
                    return null;
                }
            }

            return result;
        }

        public static List<TxtFile> ShowOpenFileDialog() {
            List<TxtFile> result = new List<TxtFile>();

            using (var openDialog = new OpenFileDialog()) {
                openDialog.DefaultExt = _DEFAULT_EXT;
                openDialog.Filter = _FILTER;
                openDialog.Title = "گشودن فایل";
                openDialog.Multiselect = true;
                if (openDialog.ShowDialog() == DialogResult.OK) {
                    foreach (var fileName in openDialog.FileNames) {
                        var file = ReadFile(fileName);
                        result.Add(file);
                    }
                }
            }

            return result;
        }

    }

}
