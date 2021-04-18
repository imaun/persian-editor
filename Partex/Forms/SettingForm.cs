using System;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using Farcin.Editor.Core.Models.Types;
using Farcin.Editor.Core.Models.Setting;

namespace Farcin.Editor.Forms {

    public partial class SettingForm : Form {

        private readonly FontConverter _fontConv;
        private readonly InstalledFontCollection _fontCollection;
        private readonly AppSetting _appSetting;

        public SettingForm() {
            InitializeComponent();
            Tabs.SelectedIndex = 0;
            _appSetting = AppSetting.Load();
            _fontConv = new FontConverter();
            _fontCollection = new InstalledFontCollection();
            foreach(var font in _fontCollection.Families) {
                ddlFonts.Items.Add(font.Name);
            }
            loadAppSettings();
        }

        #region Properties

        public AppSetting Setting => _appSetting;

        public bool KeepOpenFiles {
            get => chkKeepOpenFiles.Checked;
            set => chkKeepOpenFiles.Checked = value;
        }

        public Color DefaultEditorFontColor {
            get => btnForeColor.BackColor;
            set => btnForeColor.BackColor = value;
        }

        public int DefaultEditorFontColorValue
            => ColorTranslator.ToWin32(DefaultEditorFontColor);
        
        public Color DefaultEditorBackColor {
            get => btnBackColor.BackColor;
            set => btnBackColor.BackColor = value;
        }

        public int DefaultEditorBackColorValue
            => ColorTranslator.ToWin32(DefaultEditorBackColor);

        public string DefaultEditorFontName {
            get => ddlFonts.Text;
            set => ddlFonts.Text = value;
        }

        public Font DefaultEditorFont {
            get => (Font)_fontConv.ConvertFromString(DefaultEditorFontName);
            set => DefaultEditorFontName = value.FontFamily.Name;
        }

        public float DefaultEditorFontSize {
            get => (float)numFontSize.Value;
            set => numFontSize.Value = (decimal)value;
        }

        public HorizontalAlignment DefaultTextAlign {
            get {
                if (btnTextAlignRight.Checked)
                    return HorizontalAlignment.Right;
                if (btnTextAlignLeft.Checked)
                    return HorizontalAlignment.Left;
                return HorizontalAlignment.Center;
            }
            set {
                switch(value) {
                    case HorizontalAlignment.Right:
                        btnTextAlignRight.Checked = true;
                        break;
                    case HorizontalAlignment.Center:
                        btnTextAlignCenter.Checked = true;
                        break;
                    case HorizontalAlignment.Left:
                        btnTextAlignLeft.Checked = true;
                        break;
                }
            }
        }

        public TxtDirection DefaultTextDirection {
            get {
                if (rdbDirAuto.Checked)
                    return TxtDirection.Auto;
                else if (rdbDirLtr.Checked)
                    return TxtDirection.Ltr;
                return TxtDirection.Rtl;
            }
            set {
                if (value == TxtDirection.Auto)
                    rdbDirAuto.Checked = true;
                else if (value == TxtDirection.Ltr)
                    rdbDirLtr.Checked = true;
                else
                    rdbDirRtl.Checked = true;
            }
        }

        #endregion

        #region Methods

        private Color selectColor(Color selected) {
            Color result = selected;
            using (var colorDlg = new ColorDialog()) {
                colorDlg.Color = selected;
                if (colorDlg.ShowDialog() == DialogResult.OK) {
                    result = colorDlg.Color;
                }
            }
            return result;
        }

        public void loadAppSettings() {
            KeepOpenFiles = _appSetting.KeepOpenFilesAfterExit;
            DefaultEditorFontColor = _appSetting.DefaultFileSetting.ForeColorValue;
            DefaultEditorBackColor = _appSetting.DefaultFileSetting.BackColorValue;
            DefaultEditorFontSize = _appSetting.DefaultFileSetting.FontSize;
            DefaultEditorFontName = _appSetting.DefaultFileSetting.FontFamilyName;
            DefaultTextAlign = _appSetting.DefaultFileSetting.TextAlign;
            DefaultTextDirection = _appSetting.DefaultFileSetting.Direction;
        }

        public void saveAppSettings() {
            _appSetting.KeepOpenFilesAfterExit = KeepOpenFiles;
            _appSetting.DefaultFileSetting.BackColor = DefaultEditorBackColorValue;
            _appSetting.DefaultFileSetting.FontName = DefaultEditorFontName;
            _appSetting.DefaultFileSetting.ForeColor = DefaultEditorFontColorValue;
            _appSetting.DefaultFileSetting.FontSize = DefaultEditorFontSize;
            _appSetting.DefaultFileSetting.Direction = DefaultTextDirection;
            _appSetting.DefaultFileSetting.TextAlign = DefaultTextAlign;
            _appSetting.Save();
        }

        private void setSampleText() {
            txtEditorSample.Font = new Font(DefaultEditorFontName, DefaultEditorFontSize);
            txtEditorSample.ForeColor = DefaultEditorFontColor;
            txtEditorSample.BackColor = DefaultEditorBackColor;
            if (btnTextAlignCenter.Checked)
                txtEditorSample.TextAlign = HorizontalAlignment.Center;
            else if (btnTextAlignLeft.Checked)
                txtEditorSample.TextAlign = HorizontalAlignment.Left;
            else if (btnTextAlignRight.Checked)
                txtEditorSample.TextAlign = HorizontalAlignment.Right;
            if (rdbDirRtl.Checked)
                txtEditorSample.RightToLeft = RightToLeft.Yes;
            else if (rdbDirLtr.Checked)
                txtEditorSample.RightToLeft = RightToLeft.No;
            else if (rdbDirAuto.Checked)
                txtEditorSample.RightToLeft = RightToLeft.Inherit;
        }

        #endregion

        private void SettingForm_Load(object sender, EventArgs e) {

        }

        private void btnForeColor_Click(object sender, EventArgs e) {
            DefaultEditorFontColor = selectColor(DefaultEditorFontColor);
            setSampleText();
        }
        
        private void btnBackColor_Click(object sender, EventArgs e) {
            DefaultEditorBackColor = selectColor(DefaultEditorBackColor);
            setSampleText();
        }

        private void ddlFonts_SelectedIndexChanged(object sender, EventArgs e) {
            setSampleText();
        }

        private void numFontSize_ValueChanged(object sender, EventArgs e) {
            setSampleText();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            saveAppSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnTextAlignRight_CheckedChanged(object sender, EventArgs e) {
            txtEditorSample.TextAlign = HorizontalAlignment.Right;
        }

        private void btnTextAlignCenter_CheckedChanged(object sender, EventArgs e) {
            txtEditorSample.TextAlign = HorizontalAlignment.Center;
        }

        private void btnTextAlignLeft_CheckedChanged(object sender, EventArgs e) {
            txtEditorSample.TextAlign = HorizontalAlignment.Right;
        }

        private void rdbDirAuto_CheckedChanged(object sender, EventArgs e) {
            setSampleText();
        }

        private void rdbDirRtl_CheckedChanged(object sender, EventArgs e) {
            setSampleText();
        }

        private void rdbDirLtr_CheckedChanged(object sender, EventArgs e) {
            setSampleText();
        }
    }
}
