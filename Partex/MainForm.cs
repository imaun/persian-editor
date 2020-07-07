using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PersianEditor.Models;
using PersianEditor.TextProcessor;
using PersianEditor.Models.Types;
using PersianEditor.Forms;
using System.Xml.Serialization;
using System.ComponentModel.Design;

namespace PersianEditor {
    public partial class MainForm : Form {
        #region Constants

        private const string APP_TITLE = "فارسینا";

        #endregion

        #region Declerations

        public List<Clip> MyClipboard = new List<Clip>(); 

        public List<TxtEditor> Editors { get; set; }

        FindForm findForm = new FindForm();
        DateForm dateForm = new DateForm();
        
        #endregion

        #region Properties

        public string Status {
            set => lblStatus.Text = value;
        }

        public TxtEditor ActiveEditor {
            get {
                try {
                    return Tabs.SelectedTab != null
                        ? Editors[ActiveEditorIndex]
                        : null;
                }
                catch {
                    return null;
                }
            }  
        }

        public int ActiveEditorIndex => Tabs.SelectedIndex;

        public Color ActiveEditorForeColor {
            get => ActiveEditor.ForeColor;
            set => ActiveEditor.ForeColor = value; 
        }

        public Font ActiveEditorFont {
            get => ActiveEditor.Font;
            set => ActiveEditor.Font = value;
        }

        #endregion

        #region Methods

        private void StartUp() {
            this.Text = APP_TITLE;
            Editors = new List<TxtEditor>();
            findForm.Disposed += FindForm_Disposed;
            MainWindowState.FullScreen = false;
            MainWindowState.LastHeight = Height;
            MainWindowState.LastWidth = Width;
            MainWindowState.LastLeftPosition = Left;
            MainWindowState.LastTopPosition = Top;
            MouseWheel += MainForm_MouseWheel;
        }

        

        private void CreateTab(TxtFile file = null) {
            if(file != null && file.HasValidFilePath) {
                var editor_index = TxtEditor.Exist(Editors, file.Path);
                if (editor_index > -1) {
                    Tabs.SelectedIndex = editor_index;
                    return;
                }
            }

            var tab = new TabPage();
            if (file == null) file = new TxtFile();
            var editor = new TxtEditor(tab, Tabs.TabPages.Count, file, Editors.Count, mnuEditor) {
                Editor_TextChanged = Editor_TextChanged
            };
            tab.ToolTipText = file.Path;
            Tabs.TabPages.Add(tab);
            Tabs.SelectedIndex = Tabs.TabPages.Count - 1;
            editor.Focus();
            Editors.Add(editor);
            UpdateStatus();
        }

        private void CreateNewTabIfEmpty() {
            if (Editors.Count == 0)
                CreateTab();
        }

        private void CloseCurrentTab() {
            if (ActiveEditor == null) return;

            CloseTab(ActiveEditorIndex);
            CreateNewTabIfEmpty();
        }

        private void RemoveEditor(int index) {
            Editors.RemoveAt(index);
            Tabs.TabPages.RemoveAt(index);
        }

        private bool CloseTab(int index) {
            var result = false;
            if (index < Editors.Count) {
                var currIndex = ActiveEditorIndex;
                currIndex -= 1;
                if (currIndex == -1)
                    currIndex = 0;
                var editor = Editors[index];

                if(editor.File.CanClose && editor.File.Changed) {
                    Tabs.SelectedIndex = index;
                    var msgRes = MessageBox.Show(
                        $"فایل '{editor.File.Name}' تغییر یافته، مایل به ذخیره فایل هستید؟",
                        "ذخیره",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    switch(msgRes) {
                        case DialogResult.Cancel:
                            return false;
                        case DialogResult.Yes:
                            SaveCurrentFile();
                            RemoveEditor(index);
                            break;
                        case DialogResult.No:
                            RemoveEditor(index);
                            break;
                    }
                }
                else {
                    RemoveEditor(index);
                }

                result = true;
                if (Tabs.TabCount > 0)
                    Tabs.SelectedIndex = currIndex;
            }
            
            return result;
        }

        private void UpdateStatus() {
            if (ActiveEditor != null) {
                Status = ActiveEditor.StatusText;
                this.Text = $"{APP_TITLE} [{ActiveEditor.Title}]";
            }
        }

        public void SetFont(Font font) {
            ActiveEditorFont = font;
        }

        public void SetFont(Font font, Color forecolor) {
            ActiveEditorFont = font;
            ActiveEditorForeColor = forecolor;
        }

        public void OpenArgumentFile(TxtFile file) {
            CreateTab(file);
            this.Activate();
        }

        public void OpenArgumentFiles(IEnumerable<string> filePaths) {
            foreach (var path in filePaths) {
                var file = TxtProcessor.ReadFile(path);
                CreateTab(file);
            }
            //this.Activate();
        }


        private void SaveCurrentFile()
        {
            if (ActiveEditor == null) return;

            if (ActiveEditor.File.SavedToHard) {
                TxtProcessor.SaveFile(ActiveEditor.File);
                ActiveEditor.File.Changed = false;
                ActiveEditor.File.SavedToHard = true;
                Tabs.SelectedTab.Text = ActiveEditor.File.Name;
            }
            else {
                var saveAsResult = TxtProcessor.SaveAs(ActiveEditor.File);
                if(!string.IsNullOrEmpty(saveAsResult)) {
                    Tabs.SelectedTab.Text = ActiveEditor.File.Name;
                }
                else {

                }
                
            }
        }

        private void CloseAllButThis() {
            int selIndex = ActiveEditorIndex;
            var count = Editors.Count;
            var j = 0;
            for (var i = 0; i < count; i++) {
                if (j == selIndex) {
                    j++;
                    continue;
                }

                if (!CloseTab(i))
                    return;

                i--;
                count = Editors.Count;
                j++;
            }

            foreach (TxtEditor editor in Editors) {
                editor.File.CanClose = true;
            }
        }

        private void SaveAllFiles() {
            int index = 0;
            foreach (var editor in Editors) {
                if (editor.File.SavedToHard) {
                    TxtProcessor.SaveFile(editor.File);
                    //Tabs.TabPages[index].Text = file.Name;
                    editor.File.Changed = false;
                }
                index++;
            }
        }


        private bool CloseAllTabs(bool onExitApp) {
            var count = Editors.Count;
            for (var i = 0; i < count; i++) {
                
                if (!CloseTab(i))
                    return false;
                i--;
                count = Editors.Count;
            }

            if (!onExitApp && Editors.Count == 0)
                CreateTab();

            return true;
        }


        #endregion

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
            Editors[ActiveEditor.Index].File.Changed = true;
            if(!Tabs.SelectedTab.Text.Contains("*"))
                Tabs.SelectedTab.Text += "*";
        }

        public MainForm() {
            InitializeComponent();
            StartUp();
        }

        public MainForm(IEnumerable<TxtFile> files) {
            InitializeComponent();
            StartUp();
            
            foreach (var file in files) {
                CreateTab(file);
            }
        }

        private void mnuFileNew_Click(object sender, EventArgs e) {
            CreateTab();
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateStatus();
            if (ActiveEditor != null) {
                ActiveEditor.Focus();
                ActiveEditor.SelectionLength = 0;
            }
        }

        private void mnuFileOpen_Click(object sender, EventArgs e) {
            Cursor = Cursors.WaitCursor;
            var openFileResult = TxtProcessor.ShowOpenFileDialog();
            foreach(var file in openFileResult) {
                CreateTab(file);
            }
            Cursor = Cursors.Default;
        }

        private void mnuEditUndo_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            if(ActiveEditor.CanUndo)
                ActiveEditor.Undo();
        }

        private void mnuFileSave_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            SaveCurrentFile();
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            
            TxtProcessor.SaveAs(ActiveEditor.File);
        }

        private void mnuFileSaveAll_Click(object sender, EventArgs e) {
            SaveAllFiles();
        }

        private void mnuFileClose_Click(object sender, EventArgs e)
        {
            if (ActiveEditor == null) return;
            if (ActiveEditor.File.Changed) {
                var result = MessageBox.Show("فایل تغییر یافته، مایل به ذخیره کردن هستید؟", "ذخیره",
                                                      MessageBoxButtons.YesNoCancel,
                                                      MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveCurrentFile();
                        if(!ActiveEditor.File.CancelSaveDialog)
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

        private void mnuFileCloseButThis_Click(object sender, EventArgs e) {
            CloseAllButThis();
        }

        private void mnuFileCloseAll_Click(object sender, EventArgs e) {
            CloseAllTabs(onExitApp: false);
        }

        private void mnuFileExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !CloseAllTabs(onExitApp: true);
        }

        private void mnuEditCut_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            if (ActiveEditor.SelectionLength > 0)
                MyClipboard.Add(new Clip(ActiveEditor.SelectedText, 
                    ClipboardOperation.Cut, DateTime.Now));
            
            ActiveEditor.Cut();
        }

        private void mnuEditCopy_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            if (ActiveEditor.SelectionLength > 0)
                MyClipboard.Add(new Clip(ActiveEditor.SelectedText,
                    ClipboardOperation.Copy, DateTime.Now));
            
            ActiveEditor.Copy();
        }

        private void mnuEditPaste_Click(object sender, EventArgs e) {
            ActiveEditor?.Paste();
        }

        private void mnuEditDelete_Click(object sender, EventArgs e) {
            ActiveEditor?.DeleteSelection();
        }

        private void mnuEditSelectAll_Click(object sender, EventArgs e) {
            ActiveEditor?.SelectAll();
        }

        private void mnuEditUnselect_Click(object sender, EventArgs e) {
            ActiveEditor?.ClearSelection();
        }

        private void muuEditCopyFromAll_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            ActiveEditor.SelectAll();
            ActiveEditor.Copy();
            MyClipboard.Add(new Clip(ActiveEditor.SelectedText,
                ClipboardOperation.Copy, DateTime.Now));
            ActiveEditor.ClearSelection();
        }

        private void mnuEditCopyFromPath_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            if (ActiveEditor.File.SavedToHard) {
                Clipboard.SetText(ActiveEditor.File.Path);
                MyClipboard.Add(new Clip(ActiveEditor.File.Path,
                    ClipboardOperation.Copy, DateTime.Now));
            }
        }

        private void mnuEditCopyFromFilename_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            if (ActiveEditor.File.SavedToHard) {
                Clipboard.SetText(ActiveEditor.File.Name);
                MyClipboard.Add(new Clip(ActiveEditor.File.Name,
                    ClipboardOperation.Copy, DateTime.Now));
            }
        }

        private void mnuEditLineDuplicate_Click(object sender, EventArgs e) {
            ActiveEditor?.DuplicateCurrentLine();
        }

        private void mnuEditLineCut_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            var result = ActiveEditor.CutCurrentLine();
            if(!string.IsNullOrEmpty(result)) 
                MyClipboard.Add(new Clip(result, ClipboardOperation.Cut, DateTime.Now));
        }

        private void MainForm_Load(object sender, EventArgs e) {
            if(Tabs.TabCount == 0)
                CreateTab();
        }

        private void mnuSearchFind_Click(object sender, EventArgs e) {
            if(findForm.IsDisposed)
                findForm = new FindForm();
            if(findForm.Visible) findForm.Activate();
            else findForm.Show(this);
        }

        private void FindForm_Disposed(object sender, EventArgs e) {
            findForm = new FindForm();
        }

        private void btnCloseCurrentTab_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            CloseCurrentTab();
        }

        private void mnuInsertFilename_Click(object sender, EventArgs e) {
            ActiveEditor?.Insert(InsertTextType.FileName);
        }

        private void mnuInsertFilePath_Click(object sender, EventArgs e) {
            ActiveEditor?.Insert(InsertTextType.FilePath);
        }

        private void mnuInsertTime_Click(object sender, EventArgs e) {
            ActiveEditor?.Insert(InsertTextType.CurrentSystemTime);
        }

        private void mnuInsertDate_Click(object sender, EventArgs e) {
            ActiveEditor?.Insert(InsertTextType.CurrentSystemDate);
        }

        private void mnuSearchGoto_Click(object sender, EventArgs e) {
            ActiveEditor?.GoToLine(2);
        }

        private void mnuViewFont_Click(object sender, EventArgs e) {
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

        private void mnuInsertDatePersian_Click(object sender, EventArgs e) {
            //ActiveEditor.SelectedText = new PersianDateHelper().Convert(DateTime.Now,PersianDateFormat.Normal);
            ActiveEditor?.Insert(InsertTextType.CurrentPersianDateNumbers);
        }

        private void mnuViewFullScreen_Click(object sender, EventArgs e) {
            if (MainWindowState.FullScreen) {
                Height = MainWindowState.LastHeight;
                Width = MainWindowState.LastWidth;
                Left = MainWindowState.LastLeftPosition;
                Top = MainWindowState.LastTopPosition;
                MainWindowState.FullScreen = false;
                Menubar.Visible = Toolbar.Visible = true;
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
            }
            else {
                MainWindowState.FullScreen = true;
                MainWindowState.LastHeight = Height;
                MainWindowState.LastWidth = Width;
                MainWindowState.LastLeftPosition = Left;
                MainWindowState.LastTopPosition = Top;
                Menubar.Visible = Toolbar.Visible = false;
                Left = 0;//Screen.PrimaryScreen.WorkingArea.Left;
                Top = 0; //Screen.PrimaryScreen.WorkingArea.Top;
                //Width = Screen.PrimaryScreen.WorkingArea.Width;
                //Height = Screen.PrimaryScreen.WorkingArea.Height;
                Bounds = Screen.PrimaryScreen.Bounds;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            
        }

        private void mnuSearchInFiles_Click(object sender, EventArgs e) {

        }

        private void mnuSearchFindNext_Click(object sender, EventArgs e) {
            if(ActiveEditor == null) return;

            if (ActiveEditor.File.LastFindIndex > 0) {
                int start = ActiveEditor.Text.IndexOf(ActiveEditor.File.FindString, 
                    ActiveEditor.File.LastFindIndex,
                    StringComparison.CurrentCultureIgnoreCase);
                if (start == -1)
                {
                    MessageBox.Show("در متن یافت نشد");
                    ActiveEditor.File.LastFindIndex = 0;
                    return;
                }
                ActiveEditor.File.LastFindIndex = start + ActiveEditor.File.FindString.Length;
                ActiveEditor.SelectionStart = start;
                ActiveEditor.SelectionLength = ActiveEditor.File.FindString.Length;
                ActiveEditor.ScrollToCaret();
                ActiveEditor.Focus();
                //btnFind.Text = "یافتن بعدی";
            }
            else {
                mnuSearchFind_Click(sender, e);
            }
            
        }

        private void mnuEdit_DropDownOpening(object sender, EventArgs e) {
            mnuEditUndo.Enabled = ActiveEditor.CanUndo;

        }

        private void mnuView_DropDownOpening(object sender, EventArgs e) {
            mnuViewToolbar.Checked = Toolbar.Visible;
            mnuViewStatusBar.Checked = Statusbar.Visible;
            mnuViewRightToLeft.Checked = ActiveEditor.Rtl;
            mnuViewLeftToRight.Checked = !ActiveEditor.Rtl;
        }

        private void mnuViewToolbar_Click(object sender, EventArgs e) {
            mnuViewToolbar.Checked = Toolbar.Visible = !mnuViewToolbar.Checked;
        }

        private void mnuViewStatusBar_Click(object sender, EventArgs e) {
            mnuViewStatusBar.Checked = Statusbar.Visible = !mnuViewStatusBar.Checked;
        }

        private void mnuViewZoomIn_Click(object sender, EventArgs e) {
            ActiveEditor?.ZoomIn();
        }

        private void mnuViewZoomOut_Click(object sender, EventArgs e) {
            ActiveEditor?.ZoomOut();
        }

        private void Tabs_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Middle) return;

            int idx = 0;
            foreach (TabPage tab in Tabs.TabPages) {
                if (Tabs.GetTabRect(idx).Contains(e.Location)) {
                    CloseTab(idx);
                    CreateNewTabIfEmpty();
                    return;
                }
                idx++;
            }
        }

        private void Toolbar_MouseDoubleClick(object sender, MouseEventArgs e) {
            CreateTab();
        }

        private void mnuViewRightToLeft_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            ActiveEditor.Rtl = true;
            mnuViewRightToLeft.Checked = mnuEditorViewRtl.Checked = true;
            mnuViewLeftToRight.Checked = mnuEditorViewLtr.Checked = false;
        }

        private void mnuViewLeftToRight_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            ActiveEditor.Rtl = false;
            mnuViewRightToLeft.Checked = mnuEditorViewRtl.Checked = false;
            mnuViewLeftToRight.Checked = mnuEditorViewLtr.Checked = true;
        }

        private void mnuEditor_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (ActiveEditor == null) return;

            mnuEditorUndo.Enabled = ActiveEditor.CanUndo;
            mnuEditorViewLtr.Checked = !ActiveEditor.Rtl;
            mnuEditorViewRtl.Checked = ActiveEditor.Rtl;
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e) {
            if(ModifierKeys == Keys.Control && e.Delta > 0) {
                mnuViewZoomIn_Click(this, null);
            }
            else {
                mnuViewZoomOut_Click(this, null);
            }
        }

        private void mnuInsertHalfSpace_Click(object sender, EventArgs e) {
            ActiveEditor?.AddHalfSpace();
        }

        private void mnuCorrectYekeAuto_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            mnuCorrectYekeAuto.Checked = 
                ActiveEditor.FixYeKeAutomatically = !ActiveEditor.FixYeKeAutomatically;
        }

        private void mnuCorrectYeKe_DropDownOpening(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            mnuCorrectYekeAuto.Checked = ActiveEditor.FixYeKeAutomatically;
        }

        private void mnuCorrectYeKeSelText_Click(object sender, EventArgs e) {
            ActiveEditor?.FixYekeForSelection();
        }

        private void mnuCorrectYeKeAll_Click(object sender, EventArgs e) {
            ActiveEditor?.FixYeKe();
        }

        private void mnuInsertCustomDate_Click(object sender, EventArgs e) {
            if (dateForm.IsDisposed)
                dateForm = new DateForm();
            if (dateForm.Visible) dateForm.Activate();
            else dateForm.Show(this);
        }
    }
}
