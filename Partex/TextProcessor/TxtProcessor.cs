using PersianEditor.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PersianEditor.TextProcessor {
    public static class TxtProcessor {

        public static TxtFile ReadFile(string path) {
            string result = string.Empty;
            using (var reader = new StreamReader(path, Encoding.Default)) {
                result = reader.ReadToEnd();
                reader.Close();
            }
            return new TxtFile {
                Text = result,
                Path = path
            };
        }

        public static void WriteToFile(string path, string content) {
            using (var writer = new StreamWriter(path, false, Encoding.UTF8)) {
                writer.Write(content);
                writer.Flush();
                writer.Close();
            }
        }

        public static void SaveFile(TxtFile file) {
            WriteToFile(file.Path, file.Text);
        }

        public static string SaveAs(TxtFile file) {
            string result = string.Empty;
            using (var saveDialog = new SaveFileDialog()) {
                saveDialog.DefaultExt = "txt";
                saveDialog.Filter = "فایل های متنی|*.txt|همه فایل ها|*.*";
                saveDialog.Title = "ذخیره فایل در...";
                if (saveDialog.ShowDialog() == DialogResult.OK) {
                    file.Path = result = saveDialog.FileName;
                    SaveFile(file);
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
                openDialog.DefaultExt = "txt";
                openDialog.Filter = "فایل های متنی|*.txt|همه فایل ها|*.*";
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
