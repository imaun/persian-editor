using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Farasun.PersianLib.PersianDateTime;
using Farasun.PersianLib.PersianString;
using PersianEditor.Models;

namespace PersianEditor
{
    public partial class MainForm : Form
    {
        #region Declerations

        public List<Clip> MyClipboard = new List<Clip>(); 

        public List<PartexFile> Files = new List<PartexFile>();

        FindForm findForm = new FindForm();

        private const string NEW_FILE_TITLE = "نوشته";

        private int fileCounter = 1;


        #endregion

        #region Properties

        public string Status
        {
            set { lblStatus.Text = value; }
        }

        public TextBox ActiveEditor
        {
            get 
            { 
                if(Tabs.SelectedTab != null)
                    return (TextBox) Tabs.SelectedTab.Controls[0]; 
                return null; 
            }
        }


        public int SelectedFileIndex
        {
            get { return Tabs.SelectedIndex; }
        }
        

        public PartexFile ActiveFile
        {
            get 
            { 
                try
                {
                    return Files[SelectedFileIndex];
                }
                catch
                {
                    return null;
                }
            }
        }

        public Color CurrentForeColor
        {
            get { return ActiveEditor.ForeColor; }
            set { ActiveEditor.ForeColor = value; }
        }

        public Font CurrentFont
        {
            get { return ActiveEditor.Font; }
            set { ActiveEditor.Font = value; }
        }

        #endregion

        #region Methods

        private void StartUp()
        {
            MainWindowState.FullScreen = false;
            MainWindowState.LastHeight = Height;
            MainWindowState.LastWidth = Width;
            MainWindowState.LastLeftPosition = Left;
            MainWindowState.LastTopPosition = Top;
            
        }

        private string OpenFile(string path)
        {
            string result = string.Empty;
            using (var reader = new StreamReader(path, Encoding.Default))
            {
                result = reader.ReadToEnd();
                reader.Close();
                
            }
            return result;
        }

        private void SaveToFile(string path)
        {
            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(ActiveEditor.Text);
                writer.Flush();
                writer.Close();
            }
        }

        private void SaveToFile(PartexFile file)
        {
            using (var writer = new StreamWriter(file.Path, false, Encoding.UTF8))
            {
                writer.Write(ActiveEditor.Text);
                writer.Flush();
                writer.Close();
            }
        }

        private void CreateTab()
        {
            if (Files.Count == 0)
                fileCounter = 1;
            //var tab = new SuperTabItem(); {Text = NEW_FILE_TITLE + fileCounter.ToString()};
            
            var tab = new TabPage(NEW_FILE_TITLE + fileCounter.ToString());
            var editor = new TextBox {Multiline = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical};
            editor.TextChanged += Editor_TextChanged;
            tab.Controls.Add(editor);
            Tabs.TabPages.Add(tab);
            Tabs.SelectedIndex = Tabs.TabPages.Count - 1;
            var file = new PartexFile {Name = tab.Text};
            Files.Add(file);
            fileCounter++;
            editor.Focus();
        }

        

        private void CreateTab(PartexFile file)
        {
            var file_index = PartexFile.Exist(Files, file.Path);
            if (file_index > -1)
            {
                Tabs.SelectedIndex = file_index;
                return;
            }

            var tab = new TabPage(file.Name);
            var editor = new TextBox
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.Vertical,
                    Text = OpenFile(file.Path)
                };
            editor.TextChanged += Editor_TextChanged;
            tab.Controls.Add(editor);
            tab.ToolTipText = file.Path;
            Tabs.TabPages.Add(tab);
            Tabs.SelectedIndex = Tabs.TabPages.Count - 1;
            editor.Focus();
            Files.Add(file);
        }

        private void CloseCurrentTab()
        {
            Files.RemoveAt(SelectedFileIndex);
            var curr_index = Tabs.SelectedIndex;
            Tabs.TabPages.Remove(Tabs.SelectedTab);
            if (Tabs.TabCount > 0)
            {
                if (curr_index == 0)
                    curr_index = 1;
                Tabs.SelectedIndex = curr_index - 1;
            }
            else
            {
                CreateTab();
            }
           
        }

        private bool CloseTab(int index)
        {
            var result = false;
            if (index < Files.Count)
            {
                var currIndex = Tabs.SelectedIndex;
                currIndex -= 1;
                if (currIndex == -1)
                    currIndex = 0;
                Files.RemoveAt(index);
                Tabs.TabPages.RemoveAt(index);
                result = true;
                if (Tabs.TabCount > 0)
                    Tabs.SelectedIndex = currIndex;
            }
            
            return result;
        }

        private void UpdateStatus()
        {
            if(ActiveEditor!= null)
                Status = ActiveEditor.Lines.Length + " خط, " + ActiveEditor.Text.Length.ToString() + " کاراکتر";
        }

        public void SetFont(Font font)
        {
            CurrentFont = font;
        }

        public void SetFont(Font font, Color forecolor)
        {
            CurrentFont = font;
            CurrentForeColor = forecolor;
        }

        public void OpenArgumentFile(PartexFile file)
        {
            CreateTab(file);
            this.Activate();
        }

        public void OpenArgumentFiles(IEnumerable<PartexFile> files)
        {
            foreach (var file in files)
            {
                CreateTab(file);
            }
            this.Activate();
        }


        private void SaveCurrentFile()
        {
            if (ActiveFile == null) return;
            if (ActiveFile.SavedToHard)
            {
                SaveToFile(Files[SelectedFileIndex]);
                Files[SelectedFileIndex].Changed = false;
                Files[SelectedFileIndex].SavedToHard = true;
                Tabs.SelectedTab.Text = Files[SelectedFileIndex].Name;
            }
            else
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.DefaultExt = "txt";
                    saveDialog.Filter = "فایل های متنی|*.txt|همه فایل ها|*.*";
                    saveDialog.Title = "ذخیره در";
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        SaveToFile(saveDialog.FileName);
                        Files[SelectedFileIndex].Path = saveDialog.FileName;
                        Files[SelectedFileIndex].Changed = false;
                        Files[SelectedFileIndex].SavedToHard = true;
                        Tabs.SelectedTab.Text = Files[SelectedFileIndex].Name;
                        Files[SelectedFileIndex].CancelSaveDialog = false;
                    }
                    else
                    {
                        Files[SelectedFileIndex].CancelSaveDialog = true;
                    }
                }
            }
        }

        private void CloseAllButThis()
        {
            int selIndex = SelectedFileIndex;
            var count = Files.Count;
            var j = 0;
            for (var i = 0; i < count; i++)
            {
                if (j == selIndex)
                {
                    j++;
                    continue;
                }

                var file = Files[i];
                if (file.CanClose && file.Changed)
                {
                    Tabs.SelectedIndex = i;
                    var result =
                        MessageBox.Show(
                            string.Format("فایل '{0}' تغییر یافته، مایل به ذخیره کردن هستید؟", file.Name)
                            , "ذخیره", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            SaveCurrentFile();
                            if (!ActiveFile.CancelSaveDialog)
                            {
                                CloseTab(i);
                                i--;
                                //index++;
                            }
                            break;
                        case DialogResult.No:
                            CloseTab(i);
                            i--;
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                else
                {
                    CloseTab(i);
                    i--;
                    //index++;
                }
                count = Files.Count;
                j++;
            }
            foreach (PartexFile file in Files)
            {
                file.CanClose = true;
            }
        }

        private void SaveAllFiles()
        {
            int index = 0;
            foreach (var file in Files)
            {
                if (file.SavedToHard)
                {
                    SaveToFile(file);
                    Tabs.TabPages[index].Text = file.Name;
                    file.Changed = false;
                }
                index++;
            }
        }


        private bool CloseAllTabs()
        {
            var count = Files.Count;
            for (var i = 0; i < count; i++)
            {
                var file = Files[i];
                if (file.Changed)
                {
                    Tabs.SelectedIndex = i;
                    DialogResult result =
                        MessageBox.Show(
                            string.Format("فایل '{0}' تغییر یافته، مایل به ذخیره کردن هستید؟", file.Name)
                            , "ذخیره", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            SaveCurrentFile();
                            if (!ActiveFile.CancelSaveDialog)
                            {
                                CloseTab(i);
                                i--;
                            }
                            else
                                return false;
                            break;
                        case DialogResult.No:
                            CloseTab(i);
                            i--;
                            break;
                        case DialogResult.Cancel:
                            return false;
                    }
                }
                else
                {
                    CloseTab(i);
                    i--;
                }
                count = Files.Count;
            }
            if (Tabs.TabPages.Count == 0)
                CreateTab();
            return true;
        }


        #endregion

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
            Files[SelectedFileIndex].Changed = true;
            if(!Tabs.SelectedTab.Text.Contains("*"))
                Tabs.SelectedTab.Text += "*";
        }

        public MainForm()
        {
            InitializeComponent();
            StartUp();
            findForm.Disposed += FindForm_Disposed;
            
        }

        public MainForm(IEnumerable<PartexFile> files)
        {
            InitializeComponent();
            StartUp();
            findForm.Disposed += FindForm_Disposed;
            foreach (var file in files)
            {
                CreateTab(file);
            }
        }

        

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            CreateTab();
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatus();
            if (ActiveEditor != null)
            {
                ActiveEditor.Focus();
                ActiveEditor.SelectionLength = 0;
                mnuViewRightToLeft.Checked = (ActiveEditor.TextAlign == HorizontalAlignment.Right);
                mnuViewLeftToRight.Checked = !mnuViewRightToLeft.Checked;

            }
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.DefaultExt = "txt";
                openDialog.Filter = "فایل های متنی|*.txt|همه فایل ها|*.*";
                openDialog.Title = "گشودن فایل";
                openDialog.Multiselect = true;
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var fileName in openDialog.FileNames)
                    {
                        var file = new PartexFile(fileName);
                        CreateTab(file);
                    }
                }
            }
        }

        private void mnuEditUndo_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.Undo();
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            SaveCurrentFile();
        }

       

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.DefaultExt = "txt";
                saveDialog.Filter = "فایل های متنی|*.txt|همه فایل ها|*.*";
                saveDialog.Title = "ذخیره در";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveToFile(saveDialog.FileName);
                    Files[SelectedFileIndex].Path = saveDialog.FileName;
                    Files[SelectedFileIndex].Changed = false;
                    Files[SelectedFileIndex].SavedToHard = true;
                    Tabs.SelectedTab.Text = Files[SelectedFileIndex].Name;
                    Tabs.SelectedTab.ToolTipText = Files[SelectedFileIndex].Path;
                    Tabs.SelectedTab.Text = Files[SelectedFileIndex].Name;
                    Files[SelectedFileIndex].CancelSaveDialog = false;
                }
                else
                {
                    Files[SelectedFileIndex].CancelSaveDialog = true;
                }
            }
            
        }

        private void mnuFileSaveAll_Click(object sender, EventArgs e)
        {
            SaveAllFiles();
        }

        

        private void mnuFileClose_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if (ActiveFile.Changed)
            {
                var result = MessageBox.Show("فایل تغییر یافته، مایل به ذخیره کردن هستید؟", "ذخیره",
                                                      MessageBoxButtons.YesNoCancel,
                                                      MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveCurrentFile();
                        if(!ActiveFile.CancelSaveDialog)
                            CloseCurrentTab();
                        break;
                    case DialogResult.No:
                        CloseCurrentTab();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
                return;
            }
            CloseCurrentTab();
        }

        private void mnuFileCloseButThis_Click(object sender, EventArgs e)
        {
            CloseAllButThis();
        }

        

        private void mnuFileCloseAll_Click(object sender, EventArgs e)
        {
            CloseAllTabs();
        }

        

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CloseAllTabs();
        }

        private void mnuEditCut_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if (ActiveEditor.SelectionLength > 0)
            {
                MyClipboard.Add(new Clip(ActiveEditor.SelectedText, ClipboardOperation.Cut, DateTime.Now));
            }
            ActiveEditor.Cut();
        }

        private void mnuEditCopy_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if (ActiveEditor.SelectionLength > 0)
            {
                MyClipboard.Add(new Clip(ActiveEditor.SelectedText,
                    ClipboardOperation.Copy, DateTime.Now));
            }
            ActiveEditor.Copy();
        }

        private void mnuEditPaste_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.Paste();
        }

        private void mnuEditDelete_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.SelectedText = string.Empty;
        }

        private void mnuEditSelectAll_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.SelectAll();
        }

        private void mnuEditUnselect_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null)
                ActiveEditor.SelectionLength = 0;
        }

        private void muuEditCopyFromAll_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.SelectAll();
            ActiveEditor.Copy();
            MyClipboard.Add(new Clip(ActiveEditor.SelectedText,
                ClipboardOperation.Copy, DateTime.Now));
            ActiveEditor.SelectionLength = 0;
        }

        private void mnuEditCopyFromPath_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if (Files[SelectedFileIndex].SavedToHard)
                Clipboard.SetText(Files[SelectedFileIndex].Path);
        }

        private void mnuEditCopyFromFilename_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if (Files[SelectedFileIndex].SavedToHard)
                Clipboard.SetText(Files[SelectedFileIndex].Name);
        }

        private void mnuEditLineDuplicate_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if(ActiveEditor != null)
            {
                var currIndex = ActiveEditor.SelectionStart;
                if (ActiveEditor.Lines.Length > 0)
                {
                    string[] lines = ActiveEditor.Lines;
                    int curLine = ActiveEditor.GetLineFromCharIndex(ActiveEditor.SelectionStart);

                    var sb = new StringBuilder();
                    var c = 0;
                    foreach (var line in lines)
                    {
                        sb.AppendLine(line);
                        if (curLine == c)
                            sb.AppendLine(line);
                        c++;
                    }
                    ActiveEditor.Text = sb.ToString();
                    ActiveEditor.SelectionStart = currIndex;
                    ActiveEditor.ScrollToCaret();
                }

            }
        }

        private void mnuEditLineCut_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if (ActiveEditor.Lines.Length > 0)
            {
                int curLine = ActiveEditor.GetLineFromCharIndex(ActiveEditor.SelectionStart);
                string[] lines = ActiveEditor.Lines;
                string lineContent = ActiveEditor.Lines[curLine];
                Clipboard.SetText(lineContent);
                lines[curLine] = string.Empty;
                ActiveEditor.Lines = lines;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if(Tabs.TabCount == 0)
                CreateTab();
        }

        private void mnuSearchFind_Click(object sender, EventArgs e)
        {
            if(findForm.IsDisposed)
                findForm = new FindForm();
            if(findForm.Visible) findForm.Activate();
            else findForm.Show(this);
        }

        private void FindForm_Disposed(object sender, EventArgs e)
        {
            findForm = new FindForm();
        }

        private void btnCloseCurrentTab_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            CloseCurrentTab();
        }

        private void mnuInsertFilename_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.SelectedText = ActiveFile.Name;
        }

        private void mnuInsertFilePath_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.SelectedText = ActiveFile.Path;
        }

        private void mnuInsertTime_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            var now = DateTime.Now;
            ActiveEditor.SelectedText = string.Format("{0}:{1}:{2}",
                                                      now.Hour, now.Minute, now.Second);
        }

        private void mnuInsertDate_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            ActiveEditor.SelectedText = DateTime.Now.ToLongDateString();
        }

        private void mnuSearchGoto_Click(object sender, EventArgs e)
        {
            
        }

        private void mnuViewFont_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            using (var fontDialog = new FontDialog())
            {
                fontDialog.Font = ActiveEditor.Font;
                fontDialog.ShowColor = true;
                fontDialog.Color = ActiveEditor.ForeColor;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    SetFont(fontDialog.Font, fontDialog.Color);
                }
            }
        }

        private void mnuInsertDatePersian_Click(object sender, EventArgs e)
        {
            if(ActiveEditor == null) return;
            ActiveEditor.SelectedText = new PersianDateHelper().Convert(DateTime.Now,PersianDateFormat.Normal);
        }

        private void mnuViewFullScreen_Click(object sender, EventArgs e)
        {
            if (MainWindowState.FullScreen)
            {
                Height = MainWindowState.LastHeight;
                Width = MainWindowState.LastWidth;
                Left = MainWindowState.LastLeftPosition;
                Top = MainWindowState.LastTopPosition;
                MainWindowState.FullScreen = false;
                Toolbar.Visible = true;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                MainWindowState.FullScreen = true;
                MainWindowState.LastHeight = Height;
                MainWindowState.LastWidth = Width;
                MainWindowState.LastLeftPosition = Left;
                MainWindowState.LastTopPosition = Top;
                Toolbar.Visible = false;
                Left = 0;//Screen.PrimaryScreen.WorkingArea.Left;
                Top = 0; //Screen.PrimaryScreen.WorkingArea.Top;
                Width = Screen.PrimaryScreen.WorkingArea.Width;
                Height = Screen.PrimaryScreen.WorkingArea.Height;
                FormBorderStyle = FormBorderStyle.None;
            }
            
        }

        private void mnuSearchInFiles_Click(object sender, EventArgs e)
        {

        }

        private void mnuSearchFindNext_Click(object sender, EventArgs e)
        {
            if(ActiveEditor == null) return;

            if (ActiveFile.LastFindIndex > 0)
            {
                int start = ActiveEditor.Text.IndexOf(ActiveFile.FindString, 
                    Files[SelectedFileIndex].LastFindIndex,
                    StringComparison.CurrentCultureIgnoreCase);
                if (start == -1)
                {
                    MessageBox.Show("در متن یافت نشد");
                    Files[SelectedFileIndex].LastFindIndex = 0;
                    return;
                }
                Files[SelectedFileIndex].LastFindIndex = start + ActiveFile.FindString.Length;
                ActiveEditor.SelectionStart = start;
                ActiveEditor.SelectionLength = ActiveFile.FindString.Length;
                ActiveEditor.ScrollToCaret();
                ActiveEditor.Focus();
                //btnFind.Text = "یافتن بعدی";
            }
            else
            {
                mnuSearchFind_Click(sender, e);
            }
            
        }
        


    }
}
