using Farcin.Editor.Core.Models;
using System.ComponentModel;
using System.Windows.Forms;

namespace Farcin.Editor.Forms
{
    public partial class ClipboardForm : Form
    {
        public BindingList<Clip> ClipSource { get; set; }

        public ClipboardForm() {
            InitializeComponent();
            dataGrid.AutoGenerateColumns = false;
            ClipSource = new BindingList<Clip>();
            dataGrid.DataSource = ClipSource;
        }

        public void AddToClipboard(Clip clip) {
            ClipSource.Add(clip);
        }

        private void ClipboardForm_FormClosing(object sender, FormClosingEventArgs e) {
            Hide();
            e.Cancel = true;
        }
    }
}
