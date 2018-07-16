using System.Collections.Generic;

namespace PersianEditor.Models
{
    public class PartexFile
    {

        public PartexFile()
        {
            Changed = SavedToHard = CancelSaveDialog = false;
            CanClose = true;
        }

        public PartexFile(string path)
        {
            Path = path;
            Changed = CancelSaveDialog = false;
            SavedToHard=  CanClose = true;
        }

        public string Path { get; set; }

        private string _name;
        public string Name { get
        {
            if(!string.IsNullOrEmpty(Path))
                return Path.Substring(Path.LastIndexOf("\\")+1);
            return _name;
        }
            set { _name = value; }
        }
        
        public bool SavedToHard { get; set; }
        public bool Changed { get; set; }

        public int LastFindIndex { get; set; }
        public string FindString { get; set; }
        public string ReplaceString { get; set; }

        public bool CancelSaveDialog { get; set; }

        public bool CanClose { get; set; }

        #region Methods

        public static int Exist(IEnumerable<PartexFile> files, string path)
        {
            path = path.ToLower();
            int index = 0;
            foreach (var file in files)
            {
                if (!file.SavedToHard)
                {
                    index++;
                    continue;
                }
                if (file.Path.ToLower() == path)
                    return index;
                index++;
            }
            return -1;
        }

        #endregion
    }
}
