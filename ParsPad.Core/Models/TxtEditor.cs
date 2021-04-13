using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Farcin.Editor.Core.Models.Types;
using Farcin.Editor.Core.TextProcessor;
using System.Runtime.InteropServices;
using Farcin.Editor.Core.Date;
using Farcin.Editor.Core.Helpers;
using Farcin.Editor.Core.Models.Setting;

namespace Farcin.Editor.Core.Models {
    public class TxtEditor {

        [DllImport("user32.dll")]
        static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);

        private const string NEW_FILE_TITLE = "نوشته"; 

        public TxtEditor(
            TabPage tabPage, 
            int index, 
            int lastEditorsCount = 0) {

            initialize(tabPage, index, lastEditorsCount);
        }

        public TxtEditor(TabPage tabPage, 
            int index, 
            string filePath, 
            int lastEditorsCount = 0) {

            initialize(tabPage, index, lastEditorsCount);
            File = TxtProcessor.ReadFile(filePath);
        }

        public TxtEditor(
            TabPage tabPage, 
            int index, 
            TxtFile file, 
            int lastEditorsCount = 0, 
            ContextMenuStrip contextMenu = null) {

            initialize(tabPage, index, lastEditorsCount);
            Editor.ContextMenuStrip = contextMenu;
            File = file;
            tabPage.ToolTipText = File.HasValidFilePath 
                ? File.Path 
                : StatusText;
        }

        private void EditorOnTextChanged(object sender, EventArgs e) {
            Editor_TextChanged?.Invoke(sender, e);

            if (FixYeKeAutomatically)
                Text = PersianHelper.ApplyCorrectYeKe(Text);
            File.Text = this.Text;
        }

        #region Properties
        //public string Title =>  File != null && File.HasValidFilePath
        //    ? File.Name
        //    : $"{NEW_FILE_TITLE}{CurrentCounter}";

        public string Title {
            get {
                if (File != null && (File.IsTempFile || File.HasValidFilePath))
                    return File.Name;

                return $"{NEW_FILE_TITLE}{CurrentCounter}";
            }
        }

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
        public EventHandler Editor_StatusChanged { get; set; }
        public int Index { get; set; }
        public int LastEditorsCount { get; set; }
        public int CurrentCounter { get; set; }
        public Font Font {
            get => Editor.Font;
            set => Editor.Font = value;
        }
        public float FontSize {
            get => Editor.Font.Size;
            set => Editor.Font = new Font(Font.FontFamily, value);
        }
        public Color ForeColor {
            get => Editor.ForeColor;
            set => Editor.ForeColor = value;
        }

        public Color BackColor {
            get => Editor.BackColor;
            set => Editor.BackColor = value;
        }

        public string StatusText => getStatus();
        public bool FixYeKeAutomatically {
            get; set;
        }

        #endregion

        #region Editor Properties

        public bool IsEmpty => string.IsNullOrEmpty(Text);

        public int CurrentLineNumber => CurrentLineIndex +1;
        public int CurrentColumnNumber => 
            Editor.SelectionStart - Editor.GetFirstCharIndexFromLine(CurrentLineIndex);
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

        public string CurrentLineText => Editor.Lines[CurrentLineIndex];

        /// <summary>
        /// Calculates Words count, regardless of the language and context.
        /// </summary>
        public int WordsCount {
            get {
                if (TextLength == 0)
                    return 0;

                int textLenght = TextLength;
                int wordCount = 0, index = 0;
                // skip whitespace until first word
                while (index < textLenght && char.IsWhiteSpace(Text[index]))
                    index++;

                while (index < textLenght) {
                    // check if current char is part of a word
                    while (index < textLenght && !char.IsWhiteSpace(Text[index]))
                        index++;

                    wordCount++;
                    // skip whitespace until next word
                    while (index < textLenght && char.IsWhiteSpace(Text[index]))
                        index++;
                }

                return wordCount;
            }
        }

        #endregion

        #region Settings

        public bool AutoSave { get; set; }

        #endregion

        #region Methods

        private void initialize(
            TabPage tabPage, 
            int index, 
            int lastEditorsCount = 0
        ) {
            TabPage = tabPage;
            Index = index;
            LastEditorsCount = lastEditorsCount;
            CurrentCounter = lastEditorsCount + 1;
            Editor = new TextBox {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.Black,
                ForeColor = Color.White,
                AcceptsTab = true
            };
            //CreateCaret(Editor.Handle, IntPtr.Zero, 10, Editor.Height);
            //ShowCaret(Editor.Handle);
            Editor.TextChanged += EditorOnTextChanged;
            Editor.Click += Editor_Click;
            Editor.KeyDown += Editor_KeyDown;
            TabPage.Controls.Add(Editor);
            File = new TxtFile();
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e) {
            Editor_StatusChanged?
                .Invoke(sender, EventArgs.Empty);
        }

        private void Editor_Click(object sender, EventArgs e) {
            Editor_StatusChanged?.Invoke(sender, e);
        }

        private string getStatus() {
            if (string.IsNullOrEmpty(Text))
                return "چیزی بنویسید!";

            return $"{Title} - خط: {CurrentLineNumber}, ستون: {CurrentColumnNumber}; طول متن : {TextLength} کاراکتر; " +
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
            try
            {
                SelectionStart = Editor.GetFirstCharIndexFromLine(lineNum);
                ScrollToCaret();
            }
            catch(Exception e) {

            }
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

        private PrinterSettings _printSetting;
        private PageSettings _pageSettings;
        public DialogResult GetPrintSetting() {
            using(var pageSetup = new PageSetupDialog()) {
                if(_pageSettings == null)
                    _pageSettings = new PageSettings();
                pageSetup.PageSettings = _pageSettings;
                if(pageSetup.ShowDialog() == DialogResult.OK) {
                    _printSetting = pageSetup.PrinterSettings;
                    return DialogResult.OK;
                }
            }

            return DialogResult.Cancel;
        }


        public void Print() {
            if (_printSetting == null && 
                    GetPrintSetting() == DialogResult.Cancel) return;

            PrintDocument printDoc = new PrintDocument {
                PrinterSettings = _printSetting
            };
            printDoc.PrintPage += TxtEditor_PrintPage;

            printDoc.Print();
        }

        private void TxtEditor_PrintPage(object sender, PrintPageEventArgs e) {
            e.Graphics.DrawString(Text, Font, Brushes.Black,10,25);
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

        public void ApplySetting(TxtFileSetting setting) {
            if (!string.IsNullOrEmpty(setting.FontName))
                Font = setting.Font;
            if (setting.FontSize > 0)
                FontSize = setting.FontSize;
            BackColor = setting.BackColorValue;
            ForeColor = setting.ForeColorValue;
            Rtl = setting.IsRtl;
            GoToLine(setting.CurrentLine);
            TextAlign = setting.TextAlign;
        }

       
        #endregion
    }
}
