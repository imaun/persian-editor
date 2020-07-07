using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PersianEditor.TextProcessor;
using PersianEditor.Models.Types;
using PersianEditor.Helpers;
using System.Runtime.InteropServices;
using ParsPad.Core;

namespace PersianEditor.Models {
    public class TxtEditor {

        [DllImport("user32.dll")]
        static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);

        private const string NEW_FILE_TITLE = "نوشته";

        public TxtEditor(TabPage tabPage, int index, int lastEditorsCount = 0) {
            initialize(tabPage, index, lastEditorsCount);
        }

        public TxtEditor(TabPage tabPage, int index, string filePath, int lastEditorsCount = 0) {
            initialize(tabPage, index, lastEditorsCount);
            File = TxtProcessor.ReadFile(filePath);
        }

        public TxtEditor(TabPage tabPage, 
            int index, 
            TxtFile file, 
            int lastEditorsCount = 0, 
            ContextMenuStrip contextMenu = null) {

            initialize(tabPage, index, lastEditorsCount);
            Editor.ContextMenuStrip = contextMenu;
            File = file;
            if (File.HasValidFilePath)
                tabPage.ToolTipText = File.Path;
            else
                tabPage.ToolTipText = StatusText;
        }

        private void EditorOnTextChanged(object sender, EventArgs e) {
            Editor_TextChanged?.Invoke(sender, e);
            if (FixYeKeAutomatically)
                Text = PersianHelper.ApplyCorrectYeKe(Text);
            File.Text = this.Text;

        }

        #region Properties
        public string Title => File != null && File.HasValidFilePath
            ? File.Name
            : $"{NEW_FILE_TITLE}{CurrentCounter}";

        public TabPage TabPage { get; set; }
        protected TextBox Editor { get; set; }
        public string Text {
            get => Editor.Text;
            set => Editor.Text = value;
        }
        private TxtFile _file;
        public TxtFile File {
            get => _file;
            set {
                _file = value;
                Text = _file.Text;
                TabPage.Text = Title;
                TabPage.ToolTipText = _file.Path;
            }
        }
        public EventHandler Editor_TextChanged { get; set; }
        public int Index { get; set; }
        public int LastEditorsCount { get; set; }
        public int CurrentCounter { get; set; }
        public Font Font {
            get => Editor.Font;
            set => Editor.Font = value;
        }
        public Color ForeColor {
            get => Editor.ForeColor;
            set => Editor.ForeColor = value;
        }
        public string StatusText => getStatus();
        public bool FixYeKeAutomatically {
            get; set;
        }
        #endregion

        #region Editor Properties

        public int CurrentLineNumber => Editor.GetLineFromCharIndex(Editor.SelectionStart);
        public int CurrentColumnNumber => Editor.SelectionStart - Editor.GetFirstCharIndexFromLine(CurrentLineNumber);
        public int TextLength => Editor.Text.Length;
        public int LinesCount => Editor.Lines.Length;
        public string SelectedText {
            get => Editor.SelectedText;
            set => Editor.SelectedText = value;
        }
        public int SelectionStart {
            get => Editor.SelectionStart;
            set => Editor.SelectionStart = value;
        }

        public int SelectionLength {
            get => Editor.SelectionLength;
            set => Editor.SelectionLength = value;
        }
        public HorizontalAlignment TextAlign {
            get => Editor.TextAlign;
            set => Editor.TextAlign = value;
        }
        public bool Rtl {
            get => Editor.RightToLeft == RightToLeft.Yes;
            set {
                if (value)
                    Editor.RightToLeft = RightToLeft.Yes;
                else
                    Editor.RightToLeft = RightToLeft.No;
            }
        }
        public bool CanUndo => Editor.CanUndo;
        public int CurrentLineIndex {
            get => Editor.GetLineFromCharIndex(SelectionStart);
        }
        public string CurrentLineText {
            get => Editor.Lines[CurrentLineIndex];
        }

        #endregion

        #region Methods

        private void initialize(TabPage tabPage, int index, int lastEditorsCount = 0) {
            TabPage = tabPage;
            Index = index;
            LastEditorsCount = lastEditorsCount;
            CurrentCounter = lastEditorsCount + 1;
            Editor = new TextBox {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.Black,
                ForeColor = Color.White
            };
            //CreateCaret(Editor.Handle, IntPtr.Zero, 10, Editor.Height);
            //ShowCaret(Editor.Handle);
            Editor.TextChanged += EditorOnTextChanged;
            TabPage.Controls.Add(Editor);
            File = new TxtFile();
        }

        private string getStatus() {
            if (string.IsNullOrEmpty(Text))
                return "چیزی بنویسید!";

            return $"{Title} - خط: {CurrentLineNumber-1}, ستون: {CurrentColumnNumber}; طول متن : {TextLength} کاراکتر; " +
                $"کل خطوط : {LinesCount}";
        }

        public void Focus() {
            Editor.Focus();
        }

        public void Undo() {
            Editor.Undo();
        }

        public void Cut() {
            Editor.Cut();
        }

        public void Copy() {
            Editor.Copy();
        }

        public void Paste() {
            Editor.Paste();
        }

        public void DeleteSelection() {
            Editor.SelectedText = string.Empty;
        }

        public void SelectAll() {
            Editor.SelectAll();
        }

        public void ClearSelection() {
            SelectionLength = 0;
        }

        public void DuplicateCurrentLine() {
            var currIndex = SelectionStart;
            if (LinesCount > 0) {
                string[] lines = Editor.Lines;
                int curLine = Editor.GetLineFromCharIndex(SelectionStart);

                var sb = new StringBuilder();
                var c = 0;
                foreach (var line in lines) {
                    sb.AppendLine(line);
                    if (curLine == c)
                        sb.AppendLine(line);
                    c++;
                }
                Editor.Text = sb.ToString();
                SelectionStart = currIndex;
                ScrollToCaret();
            }
        }

        public void ScrollToCaret() {
            Editor.ScrollToCaret();
        }

        public string CutCurrentLine() {
            //int curLine = Editor.GetLineFromCharIndex(SelectionStart);
            //string[] lines = Editor.Lines;
            //string lineContent = Editor.Lines[curLine];
            //Clipboard.SetText(lineContent);
            //lines[curLine] = string.Empty;
            //Editor.Lines = lines;
            if (string.IsNullOrEmpty(CurrentLineText))
                return null;

            Clipboard.SetText(CurrentLineText);
            Editor.Lines[CurrentLineIndex] = string.Empty;
            return CurrentLineText;
        }

        public string CopyCurrentLine() {
            if (string.IsNullOrEmpty(CurrentLineText))
                return null;

            Clipboard.SetText(CurrentLineText);
            return CurrentLineText;
        }

        public void Insert(string text) {
            SelectedText = text;
        }

        public void Insert(InsertTextType textType, string text = "") {
            switch(textType) {
                case InsertTextType.FilePath:
                    Insert(File.Path);
                    break;
                case InsertTextType.FileName:
                    Insert(File.Name);
                    break;
                case InsertTextType.CurrentSystemTime:
                    Insert(DateTimeHelper.GetCurrentSystemTimeStr());
                    break;
                case InsertTextType.CurrentSystemDate:
                    Insert(DateTimeHelper.GetCurrentSystemDateLongStr());
                    break;
                case InsertTextType.CurrentPersianDateNumbers:
                    Insert(DateTimeHelper.GetCurrentPersianDateStr());
                    break;
                case InsertTextType.CurrentPersianDateWithMonthName:
                    Insert(DateTimeHelper.GetCurrentPersianDateStr(InsertDateFormat.DayMonthNameYear));
                    break;
                default:
                    Insert(text);
                    break;
            }
        }

        public void GoToLine(int lineNum) {
            --lineNum;
            SelectionStart = Editor.GetFirstCharIndexFromLine(lineNum);
            ScrollToCaret();
        }

        public void ZoomIn() {
            Editor.Font = new Font(Font.FontFamily, Font.Size + 2);
        }

        public void ZoomOut() {
            if(Editor.Font.Size > 2)
                Editor.Font = new Font(Font.FontFamily, Font.Size - 2);
        }

        public void AddText(string text) {
            Text += text;
            SelectionStart = Text.Length - 1;
        }

        public void AddHalfSpace() {
            AddText(StringHelper.GetHalfSpace());
        }

        public void FixYeKe() {
            File.Text = Text = PersianHelper.ApplyCorrectYeKe(Text);
        }

        public void FixYekeForSelection() {
            SelectedText = PersianHelper.ApplyCorrectYeKe(SelectedText);
        }

        public static int Exist(IEnumerable<TxtEditor> editors, string path) {
            path = path.ToLower();
            int index = 0;
            foreach (var editor in editors) {
                if (!editor.File.SavedToHard) {
                    index++;
                    continue;
                }
                if (editor.File.Path.ToLower() == path)
                    return index;
                index++;
            }
            return -1;
        }

       
        #endregion
    }
}
