using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Farcin.Editor.Forms {
    public enum FindFormMode {
        Find, 
        Replace
    }

    public partial class FindForm : Form {

        #region Properties

        public FindFormMode Mode {
            get =>
                Tabs.SelectedTab == tabFind
                    ? FindFormMode.Find
                    : FindFormMode.Replace;
            set => Tabs.SelectedTab = value == FindFormMode.Find ? tabFind : tabReplace;
        }

        //public FindFormMode Mode => Tabs.SelectedTab == tabFind
        //    ? FindFormMode.Find
        //    : FindFormMode.Replace;

        public string FindText => Mode == FindFormMode.Find
            ? txtFind.Text
            : txtReplace.Text;

        public string ReplaceText {
            get => txtReplaceValue.Text;
            set => txtReplaceValue.Text = value;
        }

        public bool MatchWholeWord {
            get => chkMatchWholeWord.Checked;
            set => chkMatchWholeWord.Checked = chkReplaceWholeWord.Checked = value;
        }

        public bool IgnoreCase {
            get => !chkSwitchCap.Checked;
            set => chkSwitchCap.Checked = chkReplaceSwitchCap.Checked = !value;
        }

        private MainForm mainForm => (MainForm)Application.OpenForms["MainForm"];

        //public int StartIndex => mainForm.ActiveEditor.Text.IndexOf(FindText,
        //        mainForm.ActiveEditor.File.LastFindIndex,
        //        StringComparison.CurrentCultureIgnoreCase);

        public int StartIndex {
            get {
                var letterCase = StringComparison.CurrentCultureIgnoreCase;
                var regExOpt = RegexOptions.IgnoreCase;
                if (!IgnoreCase) {
                    letterCase = StringComparison.CurrentCulture;
                    regExOpt = RegexOptions.CultureInvariant;
                }
                
                if(MatchWholeWord) {
                    int regExStartIdx = mainForm.ActiveEditor.File.LastFindIndex;
                    int len = mainForm.ActiveEditor.Text.Length - regExStartIdx;
                    var indx = Regex.Match(mainForm.ActiveEditor.Text.Substring(regExStartIdx, len-1),
                        @"\b"+ FindText + @"\b", regExOpt).Index;
                    if (indx == 0) return -1;
                    return indx;
                }
                return mainForm.ActiveEditor.Text.IndexOf(FindText,
                    mainForm.ActiveEditor.File.LastFindIndex,
                    letterCase); 
            }
        }

        #endregion

        public FindForm(FindFormMode mode = FindFormMode.Find) {
            InitializeComponent();
            Mode = mode;
        }

        #region Helper Methods
        private bool CheckForEmptyInput() {
            if(FindText == string.Empty) {
                MessageBox.Show("لطفاً در کادر جستجو چیزی بنویسید!");
                return true;
            }
            return false;
        }

        private void NotFound() {
            MessageBox.Show("در متن یافت نشد");
            mainForm.ActiveEditor.File.LastFindIndex = 0;
            btnFind.Text = btnReplaceFind.Text = "یافتن";
            if (Mode == FindFormMode.Find) {
                txtFind.SelectAll();
                txtFind.Focus();
            }
            else {
                txtReplace.SelectAll();
                txtReplace.Focus();
            }
        }

        private void GoToNextStartIndex() {
            mainForm.ActiveEditor.File.LastFindIndex = StartIndex + FindText.Length;
        }

        private void HighlightFound() {
            mainForm.ActiveEditor.SelectionStart = StartIndex;
            mainForm.ActiveEditor.SelectionLength = FindText.Length;
        }

        private void ReplaceHighlightedText(string newValue) {
            mainForm.ActiveEditor.SelectedText = newValue;
        }

        #endregion

        #region Methods 
        public int CountAll() {
            if (CheckForEmptyInput())
                return 0;

            int result = 0;
            while (StartIndex > -1) {
                GoToNextStartIndex();
                result++;
            }

            if (result == 0)
                NotFound();
            return result;
        }

        public void DoFind() {
            if (CheckForEmptyInput())
                return;

            if (StartIndex == -1) {
                NotFound();
                return;
            }

            mainForm.ActiveEditor.File.FindString = FindText;
            HighlightFound();
            mainForm.ActiveEditor.ScrollToCaret();
            mainForm.ActiveEditor.Focus();
            btnFind.Text = btnReplaceFind.Text = "یافتن بعدی";
            GoToNextStartIndex();
        }

        public void DoReplace() {
            if (CheckForEmptyInput())
                return;

            if (mainForm.ActiveEditor.File.LastFindIndex < 0) {
                return;
            }

            DoFind();
            if (mainForm.ActiveEditor.SelectionLength > 0) {
                ReplaceHighlightedText(ReplaceText);
            }
        }

        public void ReplaceAll() {
            if (CheckForEmptyInput())
                return;

            mainForm.ActiveEditor.File.LastFindIndex = 0;
            int replacedCount = 0;
            while (StartIndex > -1) {
                HighlightFound();
                ReplaceHighlightedText(ReplaceText);
                GoToNextStartIndex();
                replacedCount++;
            }
            mainForm.ActiveEditor.File.LastFindIndex = 0;
            MessageBox.Show($"تعداد {replacedCount} جایگزینی انجام شد.");
        }

        public void GoToReplaceMode() {
            Mode = FindFormMode.Replace;
            txtReplace.SelectAll();
            txtReplace.Focus();
            Activate();
        }
        #endregion


        private void btnFind_Click(object sender, EventArgs e) {
            DoFind();
        }

        private void FindForm_Load(object sender, EventArgs e) {
            
        }

        private void FindForm_Shown(object sender, EventArgs e) {
            txtFind.Focus();
        }

        private void txtWhatToFind_TextChanged(object sender, EventArgs e) {

        }

        private void txtWhatToFind_Enter(object sender, EventArgs e) {
            AcceptButton = btnFind;
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e) {
            if(Tabs.SelectedTab == tabFind) {
                txtFind.Focus();
                AcceptButton = btnFind;
                return;
            }

            if(Tabs.SelectedTab == tabReplace) {
                txtReplace.Focus();
                AcceptButton = btnReplaceFind;
            }
        }

        private void btnReplaceFind_Click(object sender, EventArgs e) {
            DoFind();
        }

        private void btnReplace_Click(object sender, EventArgs e) {
            DoReplace();
        }

        private void btnFindCount_Click(object sender, EventArgs e) {
            
            MessageBox.Show($"تعداد {CountAll()} برای جستجوی '{FindText}' پیدا شد.");
        }

        private void btnReplaceAll_Click(object sender, EventArgs e) {
            ReplaceAll();
        }

        private void FindForm_Deactivate(object sender, EventArgs e) {
            try {
                Opacity = 0.7;
            }
            catch { }
        }

        private void FindForm_Activated(object sender, EventArgs e) {
            Opacity = 1;
        }

        protected override bool ProcessDialogKey(Keys keyData) {
            if (ModifierKeys == Keys.None && keyData == Keys.Escape) {
                Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void tabFind_Click(object sender, EventArgs e) {

        }

        private void tabReplace_Click(object sender, EventArgs e) {

        }

        private void chkSwitchCap_CheckedChanged(object sender, EventArgs e) {
            chkReplaceSwitchCap.Checked = chkSwitchCap.Checked;
        }

        private void chkMatchWholeWord_CheckedChanged(object sender, EventArgs e) {
            chkReplaceWholeWord.Checked = chkMatchWholeWord.Checked;
        }

        private void chkReplaceSwitchCap_CheckedChanged(object sender, EventArgs e) {
            chkSwitchCap.Checked = chkReplaceSwitchCap.Checked;
        }

        private void chkReplaceWholeWord_CheckedChanged(object sender, EventArgs e) {
            chkMatchWholeWord.Checked = chkReplaceWholeWord.Checked;
        }
    }
}
