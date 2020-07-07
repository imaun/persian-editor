using PersianEditor.Exceptions;

namespace PersianEditor.Models {
    public class TxtFile {
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
            }
        }

        private string _name;
        public string Name { 
            get {
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
        public bool HasValidFilePath {
            get {
                if (string.IsNullOrEmpty(Path))
                    return false;
                return System.IO.File.Exists(Path);
            }
        }

        #region Methods

        

        #endregion
    }
}
