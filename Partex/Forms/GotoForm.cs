using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Farcin.Editor.Forms
{
    public partial class GotoForm : Form
    {
        public GotoForm(int currentLineNumber = 0) {
            InitializeComponent();
            LineNumber = currentLineNumber;
        }

        #region Properties

        public int LineNumber {
            get => int.Parse(numLineNumber.Value.ToString());
            set => numLineNumber.Value = value;
        }


        #endregion

        private void btnGo_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
