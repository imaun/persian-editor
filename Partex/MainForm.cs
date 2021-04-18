using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Farcin.Editor.Core.Models;
using Farcin.Editor.Core.Models.Setting;
using Farcin.Editor.Core.TextProcessor;
using Farcin.Editor.Core.Models.Types;
using Farcin.Editor.Forms;
using Farcin.Editor.Controls;
using Farcin.Editor.Core.Services;
using System.Text;

namespace Farcin.Editor {

    public partial class MainForm : Form {

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

        #region Constants

        private const string APP_TITLE = "فارسین";
        private const string PROPERTIES_TAG = "properties";
        #endregion

        #region Declerations

        public List<TxtEditor> Editors { get; set; }

        private FindForm findForm = new FindForm();
        private DateForm dateForm = new DateForm();
        private ClipboardForm clipboardForm = new ClipboardForm();
        private AppSetting _appSettings;

        #endregion

        #region Properties

        public string Status {
            set => lblStatus.Text = value;
        }

        public TabPage CurrentTabPage => Tabs.TabPages.Count > 0 
            ? Tabs.SelectedTab
            : null;

        public TxtEditor ActiveEditor {
            get {
                try {
                    if (CurrentTabPage == null)
                        return null;
                    return CurrentTabPage != null
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

        public bool AutoFixYeKe {
            get => mnuCorrectYekeAuto.Checked;
            set => mnuCorrectYekeAuto.Checked = value;
        }

        #endregion

        #region Methods

        private void fizzBuzzTest() {
            var sb = new StringBuilder();
            for(int i = 1; i <= 100; i++) {
                string line = "";
                if (i % 3 == 0) line = "Fizz";
                if (i % 5 == 0) line += "Buzz";
                if (string.IsNullOrEmpty(line))
                    line = i.ToString();
                sb.AppendLine(line);
            }

            ActiveEditor.Text = sb.ToString();
        }

        private void StartUp() {
            this.Text = APP_TITLE;
            Editors = new List<TxtEditor>();
            _appSettings = AppSetting.Load();
            findForm.Disposed += FindForm_Disposed;
            MouseWheel += MainForm_MouseWheel;
            UpdateToolbarItems();
            applySettings();
            MainWindowState.FullScreen = false;
            MainWindowState.LastHeight = Height;
            MainWindowState.LastWidth = Width;
            MainWindowState.LastLeftPosition = Left;
            MainWindowState.LastTopPosition = Top;
            //fizzBuzzTest();
        }

        private TxtEditor CreateTab(TxtFile file = null) {
            if(file != null && file.HasValidFilePath) {
                var editor_index = TxtEditor.Exist(Editors, file.Path);
                if (editor_index > -1) {
                    Tabs.SelectedIndex = editor_index;
                    return Editors[editor_index];
                }
            }

            var tab = new TabPage();
            if (file == null) file = new TxtFile();
            var editor = new TxtEditor(tab, Tabs.TabPages.Count, file, Editors.Count, mnuEditor) {
                Editor_TextChanged = Editor_TextChanged,
                Editor_StatusChanged = Editor_StatusChanged
            };
            editor.ApplySetting(_appSettings.DefaultFileSetting);
            Splitter splitter = new Splitter {
                Dock = DockStyle.Left
            };
            tab.Controls.Add(splitter);
            tab.ToolTipText = file.Path;
            if (file.Changed)
                tab.Text += "*";
            Tabs.TabPages.Add(tab);
            Tabs.SelectedIndex = Tabs.TabPages.Count - 1;
            editor.Focus();
            Editors.Add(editor);
            UpdateToolbarItems();

            return editor;
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
            var editorToRemove = Editors[index];
            if(editorToRemove.File.SavedToHard) {
                if (!_appSettings.HasRecentFiles)
                    _appSettings.RecentFiles = new List<TxtFileSetting>();
                _appSettings.RecentFiles.Add(new TxtFileSetting {
                    BackColor = ColorTranslator.ToWin32(editorToRemove.BackColor),
                    CurrentLine = editorToRemove.CurrentLineIndex,
                    FilePath = editorToRemove.File.Path,
                    FontName = editorToRemove.Font.Name,
                    FontSize = editorToRemove.FontSize,
                    ForeColor = ColorTranslator.ToWin32(editorToRemove.ForeColor),
                    IsRtl = editorToRemove.Rtl,
                    Name = editorToRemove.Title
                });
            }
            
            Editors.RemoveAt(index);
            Tabs.TabPages.RemoveAt(index);
        }

        private DialogResult confirmSaveChangedFile(TxtFile file = null) {
            string filename = "";
            if (file != null)
                filename = file.Name;

            return MessageBox.Show(
                        $"فایل '{filename}' تغییر یافته، مایل به ذخیره فایل هستید؟",
                        "ذخیره",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
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
                    var msgRes = confirmSaveChangedFile(editor.File);

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

        private void UpdateToolbarItems() {
            if (ActiveEditor == null) return;

            var hasSelection = ActiveEditor.SelectionLength > 0;
            tolEditCopy.Enabled 
                = tolEditCut.Enabled = tolEditDelete.Enabled
                = hasSelection;

            mnuEditUndo.Enabled = tolEditUndo.Enabled = ActiveEditor.CanUndo;

            tolViewRightToLeft.Checked = ActiveEditor.Rtl;
            tolViewLeftToRight.Checked = !ActiveEditor.Rtl;
            tolViewAlignCenter.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Center;
            if(ActiveEditor.Rtl) {
                tolViewAlignLeft.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Right;
                tolViewAlignRight.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Left;
            }
            else {
                tolViewAlignLeft.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Left;
                tolViewAlignRight.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Right;
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

        private void applySettings() {
            this.Size = new Size(
                _appSettings.UserInterface.Window.Width,
                _appSettings.UserInterface.Window.Height);
            this.Opacity = _appSettings.UserInterface.Window.Opacity;
            if (_appSettings.UserInterface.Window.Maximized)
                this.WindowState = FormWindowState.Maximized;
            this.TopMost = _appSettings.UserInterface.Window.TopMost;
            if(_appSettings.HasOpenedFiles) {
                foreach(var openedFile in _appSettings.OpenedFiles) {
                    var file = new TxtFile {
                        Name = openedFile.Name,
                        Changed = openedFile.HasPendingChanges,
                        IsTempFile = !string.IsNullOrEmpty(openedFile.TempFilePath),
                        SavedToHard = openedFile.Saved,
                    };
                    file.Path = file.IsTempFile
                        ? openedFile.TempFilePath
                        : openedFile.FilePath;
                    file.Text = TxtProcessor.ReadFileText(file.Path);
                    var editor = CreateTab(file);
                    editor.ApplySetting(openedFile);
                }
            }
            UpdateStatus();
            UpdateToolbarItems();
        }


        private void saveSettings() {
            _appSettings.UserInterface.Window.Width = Width;
            _appSettings.UserInterface.Window.Height = Height;
            _appSettings.UserInterface.Window.Opacity = Opacity;
            _appSettings.UserInterface.Window.Maximized = 
                WindowState == FormWindowState.Maximized;
            _appSettings.UserInterface.Window.TopMost = TopMost;
            _appSettings.OpenedFiles = new List<TxtFileSetting>();
            var openedFilesSetting = new KeepOpenFileService(Editors)
                .CreateOpenedFilesSetting();
            _appSettings.OpenedFiles = openedFilesSetting;
            //foreach(var editor in Editors) {
            //    _appSettings.OpenedFiles.Add(new TxtFileSetting {
            //        BackColor = editor.BackColor.ToString(),
            //        CurrentLine = editor.CurrentLineIndex,
            //        FilePath = editor.File.Path,
            //        FontName = editor.Font.Name,
            //        FontSize = editor.Font.Size,
            //        ForeColor = editor.ForeColor.ToString(),
            //        IsRtl = editor.Rtl,
            //        Saved = editor.File.SavedToHard
            //    });
            //}

            _appSettings.Save();
        }

        private FileProperties findFileProperties(TabPage tab) {
            foreach (Control ctrl in tab.Controls)
                if (ctrl.Tag == PROPERTIES_TAG)
                    return (FileProperties)ctrl;

            return null;
        }

        #endregion

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            UpdateStatus();
            UpdateToolbarItems();
            ActiveEditor.File.Changed = true;

            if(!Tabs.SelectedTab.Text.Contains("*"))
                Tabs.SelectedTab.Text += "*";

            var fileProperties = findFileProperties(CurrentTabPage);
            if (fileProperties != null)
                fileProperties.LoadFileInfo(ActiveEditor);
        }

        private void Editor_StatusChanged(object sender, EventArgs e) {
            UpdateStatus();
        }

        private void mnuFileNew_Click(object sender, EventArgs e) {
            CreateTab();
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e) {
            if (ActiveEditor != null) {
                ActiveEditor.Focus();
                ActiveEditor.SelectionLength = 0;
                UpdateStatus();
                UpdateToolbarItems();
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
            UpdateStatus();
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

        private void mnuFileClose_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            if (ActiveEditor.File.Changed) {
                var result = confirmSaveChangedFile(ActiveEditor.File);
                switch (result) {
                    case DialogResult.Yes:
                        SaveCurrentFile();
                        if (!ActiveEditor.File.CancelSaveDialog)
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
            if(_appSettings.KeepOpenFilesAfterExit)
                saveSettings();
            else
                e.Cancel = !CloseAllTabs(onExitApp: true);
        }

        private void mnuEditCut_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            if (ActiveEditor.SelectionLength > 0)
                clipboardForm.AddToClipboard(new Clip(ActiveEditor.SelectedText, 
                    ClipboardOperation.Cut, DateTime.Now));
            
            ActiveEditor.Cut();
            UpdateStatus();
        }

        private void mnuEditCopy_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            if (ActiveEditor.SelectionLength > 0)
                clipboardForm.AddToClipboard(new Clip(ActiveEditor.SelectedText,
                    ClipboardOperation.Copy, DateTime.Now));
            
            ActiveEditor.Copy();
            UpdateStatus();
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
            clipboardForm.AddToClipboard(new Clip(ActiveEditor.SelectedText,
                ClipboardOperation.Copy, DateTime.Now));
            ActiveEditor.ClearSelection();
        }

        private void mnuEditCopyFromPath_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            if (ActiveEditor.File.SavedToHard) {
                Clipboard.SetText(ActiveEditor.File.Path);
                clipboardForm.AddToClipboard(new Clip(ActiveEditor.File.Path,
                    ClipboardOperation.Copy, DateTime.Now));
            }
        }

        private void mnuEditCopyFromFilename_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            if (ActiveEditor.File.SavedToHard) {
                Clipboard.SetText(ActiveEditor.File.Name);
                clipboardForm.AddToClipboard(new Clip(ActiveEditor.File.Name,
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
                clipboardForm.AddToClipboard(
                    new Clip(result, ClipboardOperation.Cut, DateTime.Now));
        }

        private void MainForm_Load(object sender, EventArgs e) {
            if(Tabs.TabCount == 0)
                CreateTab();
        }

        private void mnuSearchFind_Click(object sender, EventArgs e) {
            if(findForm.IsDisposed)
                findForm = new FindForm(mode: FindFormMode.Find);
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
            if (ActiveEditor == null) return;
            using (var form = new GotoForm(ActiveEditor.CurrentLineNumber)) {
                if(form.ShowDialog() == DialogResult.OK) {
                    ActiveEditor.GoToLine(form.LineNumber);
                }
            }
        }

        private void mnuViewFont_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;
            using (var fontDialog = new FontDialog()) {
                fontDialog.Font = ActiveEditor.Font;
                fontDialog.ShowColor = true;
                fontDialog.Color = ActiveEditor.ForeColor;
                if (fontDialog.ShowDialog() == DialogResult.OK) {
                    SetFont(fontDialog.Font, fontDialog.Color);
                }
            }
        }

        private void mnuInsertDatePersian_Click(object sender, EventArgs e) {
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
                int start = ActiveEditor.Text.IndexOf(
                    ActiveEditor.File.FindString, 
                    ActiveEditor.File.LastFindIndex,
                    StringComparison.CurrentCultureIgnoreCase);
                if (start == -1) {
                    MessageBox.Show($"'{ActiveEditor.File.FindString}' !در نوشته یافت نشد");
                    ActiveEditor.File.LastFindIndex = 0;
                    return;
                }
                ActiveEditor.File.LastFindIndex = start + ActiveEditor.File.FindString.Length;
                ActiveEditor.SelectionStart = start;
                ActiveEditor.SelectionLength = ActiveEditor.File.FindString.Length;
                ActiveEditor.ScrollToCaret();
                ActiveEditor.Focus();
            }
            else {
                mnuSearchFind_Click(sender, e);
            }
        }

        private void mnuEdit_DropDownOpening(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            bool hasSelection = ActiveEditor.SelectionLength > 0;
            mnuEditUndo.Enabled = ActiveEditor.CanUndo;
            mnuEditCopy.Enabled = mnuEditCut.Enabled = mnuEditDelete.Enabled
                = hasSelection;
            mnuEditSelectAll.Enabled = !ActiveEditor.IsEmpty;
            mnuEditUnselect.Enabled = hasSelection;
        }

        private void mnuView_DropDownOpening(object sender, EventArgs e) {
            mnuViewToolbar.Checked = Toolbar.Visible;
            mnuViewStatusBar.Checked = Statusbar.Visible;
            mnuViewToolbarEdit.Checked = ToolbarEdit.Visible;
            mnuViewRightToLeft.Checked = ActiveEditor.Rtl;
            mnuViewLeftToRight.Checked = !ActiveEditor.Rtl;
            if(ActiveEditor.Rtl) {
                mnuViewAlignLeft.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Right;
                mnuViewAlignRight.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Left;
                mnuViewAlignCenter.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Center;
            }
            else {
                mnuViewAlignLeft.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Left;
                mnuViewAlignRight.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Right;
                mnuViewAlignCenter.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Center;
            }
        }

        private void mnuViewToolbar_Click(object sender, EventArgs e) {
            mnuViewToolbar.Checked = Toolbar.Visible = !mnuViewToolbar.Checked;
            if(Toolbar.Visible) {
                Tabs.Height -= Toolbar.Height;
                Tabs.Top += Toolbar.Height;
            }
            else {
                Tabs.Height += Toolbar.Height;
                Tabs.Top -= Toolbar.Height;
            }
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

        private void mnuViewRightToLeft_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            ActiveEditor.Rtl = true;
            mnuViewRightToLeft.Checked =
                tolViewRightToLeft.Checked
                = mnuEditorViewRtl.Checked = true;
            mnuViewLeftToRight.Checked =
                tolViewLeftToRight.Checked =
                mnuEditorViewLtr.Checked = false;

            if (mnuViewAlignRight.Checked) {
                ActiveEditor.TextAlign = HorizontalAlignment.Left;
                mnuViewAlignRight.Checked = mnuEditorAlignRight.Checked = true;
                mnuViewAlignLeft.Checked = mnuEditorAlignLeft.Checked = false;
            }
            else if (mnuViewAlignLeft.Checked) {
                ActiveEditor.TextAlign = HorizontalAlignment.Right;
                mnuViewAlignLeft.Checked = mnuEditorAlignLeft.Checked = true;
                mnuViewAlignRight.Checked = mnuEditorAlignRight.Checked = false;
            }

            return;
            if (ActiveEditor.TextAlign == HorizontalAlignment.Left) {
                ActiveEditor.TextAlign = HorizontalAlignment.Right;
                mnuViewAlignLeft.Checked = mnuEditorAlignLeft.Checked = true;
                mnuViewAlignRight.Checked = mnuEditorAlignRight.Checked = false;
            }
            else if(ActiveEditor.TextAlign == HorizontalAlignment.Right) {
                ActiveEditor.TextAlign = HorizontalAlignment.Left;
                mnuViewAlignRight.Checked = mnuEditorAlignRight.Checked = true;
                mnuViewAlignLeft.Checked = mnuEditorAlignLeft.Checked = false;
            }
        }

        private void mnuViewLeftToRight_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            ActiveEditor.Rtl = false;
            mnuViewRightToLeft.Checked = 
                tolViewRightToLeft.Checked = 
                mnuEditorViewRtl.Checked = false;
            mnuViewLeftToRight.Checked = 
                tolViewLeftToRight.Checked =
                mnuEditorViewLtr.Checked = true;

            if (mnuViewAlignRight.Checked) {
                ActiveEditor.TextAlign = HorizontalAlignment.Right;
                mnuViewAlignRight.Checked = mnuEditorAlignRight.Checked = true;
                mnuViewAlignLeft.Checked = mnuEditorAlignLeft.Checked = false;
            }
            else if (mnuViewAlignLeft.Checked) {
                ActiveEditor.TextAlign = HorizontalAlignment.Left;
                mnuViewAlignLeft.Checked = mnuEditorAlignLeft.Checked = true;
                mnuViewAlignRight.Checked = mnuEditorAlignRight.Checked = false;
            }
        }

        private void mnuEditor_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (ActiveEditor == null) return;

            bool hasSelection = ActiveEditor.SelectionLength > 0;
            mnuEditorUndo.Enabled = ActiveEditor.CanUndo;
            mnuEditorViewLtr.Checked = !ActiveEditor.Rtl;
            mnuEditorViewRtl.Checked = ActiveEditor.Rtl;
            mnuEditorCopy.Enabled = mnuEditorCut.Enabled = mnuEditorDelete.Enabled
                = hasSelection;
            mnuEditorSelectAll.Enabled = !ActiveEditor.IsEmpty;

            if (ActiveEditor.Rtl) {
                mnuEditorAlignLeft.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Right;
                mnuEditorAlignRight.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Left;
                mnuEditorAlignCenter.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Center;
            }
            else {
                mnuEditorAlignLeft.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Left;
                mnuEditorAlignRight.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Right;
                mnuEditorAlignCenter.Checked = ActiveEditor.TextAlign == HorizontalAlignment.Center;
            }
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e) {
            if(ModifierKeys == Keys.Control && e.Delta > 0) {
                mnuViewZoomIn_Click(this, null);
            }
            else if(ModifierKeys == Keys.Control && e.Delta <= 0) {
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

        private void mnuSearchReplace_Click(object sender, EventArgs e) {
            if (findForm.IsDisposed)
                findForm = new FindForm(mode: FindFormMode.Replace);
            if (findForm.Visible) 
                findForm.GoToReplaceMode();
            else 
                findForm.Show(this);
        }

        private void mnuFilePrint_Click(object sender, EventArgs e) {
            ActiveEditor?.Print();
        }

        private void mnuFilePageSetup_Click(object sender, EventArgs e) {
            ActiveEditor?.GetPrintSetting();
        }

        private void mnuToolsClipboard_Click(object sender, EventArgs e) {
            if(clipboardForm.IsDisposed)
                clipboardForm = new ClipboardForm();
            if(clipboardForm.Visible)
                clipboardForm.Activate();
            else
                clipboardForm.Show(this);
        }

        private void mnuViewBackColor_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            using(var colorDlg = new ColorDialog()) {
                colorDlg.Color = ActiveEditor.BackColor;
                colorDlg.AnyColor = true;
                if(colorDlg.ShowDialog() == DialogResult.OK) {
                    ActiveEditor.BackColor = colorDlg.Color;
                }
            }
        }

        private void mnuViewToolbarEdit_Click(object sender, EventArgs e) {
            mnuViewToolbarEdit.Checked = 
                ToolbarEdit.Visible = 
                !mnuViewToolbarEdit.Checked;
            if (ToolbarEdit.Visible) {
                Tabs.Height -= ToolbarEdit.Height;
                Tabs.Top += ToolbarEdit.Height;
            }
            else {
                Tabs.Height += ToolbarEdit.Height;
                Tabs.Top -= ToolbarEdit.Height;
            }
        }

        private void mnuViewFileProperties_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            var fileProperties = findFileProperties(CurrentTabPage);
            if (fileProperties != null)
                return;

            fileProperties = new FileProperties(ActiveEditor) {
                Dock = DockStyle.Left,
                Width = 300
            };
            fileProperties.Tag = PROPERTIES_TAG;
            fileProperties.OnWindowClose = FileProperties_OnWindowClose;
            CurrentTabPage.Controls.Add(fileProperties);
        }

        private void FileProperties_OnWindowClose(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            var fileProperties = findFileProperties(CurrentTabPage);
            if(fileProperties != null) {
                CurrentTabPage.Controls.Remove(fileProperties);
            }
            //foreach(Control ctrl in CurrentTabPage.Controls) {
            //    if (ctrl.Tag == PROPERTIES_TAG) {
            //        CurrentTabPage.Controls.Remove(ctrl);
            //        break;
            //    }
            //}
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e) {
            new AboutForm().ShowDialog();
        }

        private void mnuViewAlwaysOnTop_Click(object sender, EventArgs e) {
            TopMost = !TopMost;
            mnuViewAlwaysOnTop.Checked = !mnuViewAlwaysOnTop.Checked;
        }

        private void mnuViewAlignRight_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) 
                    return;

            ActiveEditor.TextAlign = ActiveEditor.Rtl 
                ? HorizontalAlignment.Left
                : HorizontalAlignment.Right;
            mnuViewAlignRight.Checked = mnuEditorAlignRight.Checked 
                = tolViewAlignRight.Checked = true;
            mnuViewAlignCenter.Checked = mnuViewAlignLeft.Checked 
                = mnuEditorAlignCenter.Checked = mnuEditorAlignLeft.Checked
                = tolViewAlignCenter.Checked = tolViewAlignLeft.Checked = false;
        }

        private void mnuViewAlignCenter_Click(object sender, EventArgs e) {
            if (ActiveEditor == null
                || ActiveEditor.TextAlign == HorizontalAlignment.Center)
                    return;

            ActiveEditor.TextAlign = HorizontalAlignment.Center;
            mnuViewAlignCenter.Checked = mnuEditorAlignCenter.Checked
                = tolViewAlignCenter.Checked = true;
            mnuViewAlignRight.Checked = mnuViewAlignLeft.Checked
                = mnuEditorAlignRight.Checked = mnuEditorAlignLeft.Checked 
                = tolViewAlignRight.Checked = tolViewAlignLeft.Checked = false;
        }

        private void mnuViewAlignLeft_Click(object sender, EventArgs e) {
            if (ActiveEditor == null)
                return;

            ActiveEditor.TextAlign = ActiveEditor.Rtl
                ? HorizontalAlignment.Right
                : HorizontalAlignment.Left;
            mnuViewAlignLeft.Checked = mnuViewAlignLeft.Checked 
                = tolViewAlignLeft.Checked = true;
            mnuViewAlignCenter.Checked = mnuViewAlignRight.Checked
                = mnuEditorAlignCenter.Checked = mnuEditorAlignRight.Checked 
                = tolViewAlignCenter.Checked = tolViewAlignRight.Checked = false;
        }

        private void mnuToolsOptions_Click(object sender, EventArgs e) {
            using(var settingForm = new SettingForm()) {
                if(settingForm.ShowDialog() == DialogResult.OK) {
                    _appSettings = settingForm.Setting;
                }
            }
        }

        private void mnuViewForeColor_Click(object sender, EventArgs e) {
            if (ActiveEditor == null) return;

            using(var colorDlg = new ColorDialog()) {
                colorDlg.FullOpen = true;
                colorDlg.Color = ActiveEditor.ForeColor;
                if(colorDlg.ShowDialog() == DialogResult.OK) {
                    ActiveEditor.ForeColor = colorDlg.Color;
                }
            }
        }
    }
}
