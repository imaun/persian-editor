using System;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
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
        }

        public void saveAppSettings() {
            _appSetting.KeepOpenFilesAfterExit = KeepOpenFiles;
            _appSetting.DefaultFileSetting.BackColor = DefaultEditorBackColorValue;
            _appSetting.DefaultFileSetting.FontName = DefaultEditorFontName;
            _appSetting.DefaultFileSetting.ForeColor = DefaultEditorFontColorValue;
            _appSetting.DefaultFileSetting.FontSize = DefaultEditorFontSize;
            _appSetting.Save();
        }

        private void setSampleText() {
            txtEditorSample.Font = new Font(DefaultEditorFontName, DefaultEditorFontSize);
            txtEditorSample.ForeColor = DefaultEditorFontColor;
            txtEditorSample.BackColor = DefaultEditorBackColor;
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
    }
}
