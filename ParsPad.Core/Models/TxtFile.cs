using System.IO;
using Farcin.Editor.Core.Exceptions;

namespace Farcin.Editor.Core.Models {
    public class TxtFile {

        private FileInfo _fileInfo;

        public TxtFile() {
            Changed = SavedToHard = CancelSaveDialog = false;
            CanClose = true;
        }

        public TxtFile(string path) {
            Path = path;
            Changed = CancelSaveDialog = false;
        }

        private string _path;
        public string Path {
            get => _path;
            set {
                _path = value;
                if (!HasValidFilePath)
                    throw new FilePathNotExist();
                SavedToHard = CanClose = true;
                _fileInfo = new FileInfo(_path);
            }
        }

        private string _name;
        public string Name { 
            get {
                if (IsTempFile)
                    return _name;

                if(!string.IsNullOrEmpty(Path))
                    return Path.Substring(Path.LastIndexOf("\\")+1);

                return _name;
            }
            set { _name = value; }
        }
        public string Text { get; set; }
        public bool SavedToHard { get; set; }
        public bool Changed { get; set; }
        public int LastFindIndex { get; set; }
        public string FindString { get; set; }
        public string ReplaceString { get; set; }
        public bool CancelSaveDialog { get; set; }
        public bool CanClose { get; set; }
        public bool IsTempFile { get; set; }
        public bool HasValidFilePath {
            get {
                if (string.IsNullOrEmpty(Path))
                    return false;
                return System.IO.File.Exists(Path);
            }
        }

        public FileInfo Info => _fileInfo;

        public string FileSizeDisplay {
            get {
                if (Info == null)
                    return string.Empty;

                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double len = Info.Length;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1) {
                    order++;
                    len = len / 1024;
                }

                // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                // show a single decimal place, and no space.
                return string.Format(
                    "{0:0.##} {1}", 
                    len, sizes[order]);
            }
        }

        #region Methods

        

        #endregion
    }
}
