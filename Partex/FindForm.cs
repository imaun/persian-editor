using System;
using System.Windows.Forms;

namespace PersianEditor
{
    public partial class FindForm : Form
    {

        #region Properties

        public string WhatToFind
        {
            get { return txtWhatToFind.Text; }
            set { txtWhatToFind.Text = value; }
        }

        #endregion

        public FindForm()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm) Application.OpenForms["MainForm"];
            if (mainForm == null)
            {
                MessageBox.Show("فرم اصلی برنامه کجاست!؟؟؟");
                return;
            }
            int start = mainForm.ActiveEditor.Text.IndexOf(WhatToFind, mainForm.Files[mainForm.SelectedFileIndex].LastFindIndex, 
                StringComparison.CurrentCultureIgnoreCase);
            if (start == -1)
            {
                MessageBox.Show("در متن یافت نشد");
                mainForm.Files[mainForm.SelectedFileIndex].LastFindIndex = 0;
                txtWhatToFind.SelectAll();
                txtWhatToFind.Focus();
                btnFind.Text = "یافتن";
                return;
            }
            mainForm.Files[mainForm.SelectedFileIndex].FindString = WhatToFind;
            mainForm.Files[mainForm.SelectedFileIndex].LastFindIndex = start + WhatToFind.Length;
            mainForm.ActiveEditor.SelectionStart = start;
            mainForm.ActiveEditor.SelectionLength = WhatToFind.Length;
            mainForm.ActiveEditor.ScrollToCaret();
            mainForm.ActiveEditor.Focus();
            btnFind.Text = "یافتن بعدی";
        }

        private void FindForm_Load(object sender, EventArgs e)
        {
            
        }

        private void FindForm_Shown(object sender, EventArgs e)
        {
            txtWhatToFind.Focus();
        }
    }
}
