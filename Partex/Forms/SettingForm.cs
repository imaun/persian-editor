using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Farcin.Editor.Forms {

    public partial class SettingForm : Form {

        private readonly FontConverter _fontConv;

        public SettingForm() {
            InitializeComponent();
            _fontConv = new FontConverter();
        }

        #region Properties

        public Color DefaultEditorFontColor { get; set; }
        public string DefaultEditorFontName {
            get => ddlFonts.Text;
            set {
                ddlFonts.Text = value;
            }
        }
        public Font DefaultEditorFont {
            get => (Font)_fontConv.ConvertFromString(DefaultEditorFontName);
            set => DefaultEditorFontName = value.FontFamily.Name;
        }


        #endregion

        private void SettingForm_Load(object sender, EventArgs e) {

        }

        private void btnForeColor_Click(object sender, EventArgs e) {
            using(var colorDlg = new ColorDialog()) {
                colorDlg.Color = btnForeColor.BackColor;
                if(colorDlg.ShowDialog() == DialogResult.OK) {

                }
            }
        }
    }
}
