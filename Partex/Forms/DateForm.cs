using System;
using System.Globalization;
using System.Windows.Forms;
using PersianEditor.Models.Types;

namespace PersianEditor.Forms {
    public partial class DateForm : Form {
        public DateForm() {
            InitializeComponent();
            datePicker.Format = DateTimePickerFormat.Custom;
            datePicker.CustomFormat = Application.CurrentCulture
                .DateTimeFormat.ShortDatePattern;
            SelectedDateValue = DateTime.Now;
            DateFormatMode = InsertDateFormValueMode.LongDateFormat;

        }

        #region Properties

        public InsertDateFormValueMode DateFormatMode {
            get {
                if (rdbLongDate.Checked)
                    return InsertDateFormValueMode.LongDateFormat;
                if (rdbShortDate.Checked)
                    return InsertDateFormValueMode.ShortDateFormat;

                return InsertDateFormValueMode.LongDateFormat;
            }
            set {
                rdbLongDate.Checked = (value == InsertDateFormValueMode.LongDateFormat);
                rdbShortDate.Checked = (value == InsertDateFormValueMode.ShortDateFormat);
            }
        }

        public DateTime SelectedDateValue {
            get => datePicker.Value;
            set => datePicker.Value = value;
        }

        public string SelectedDateShortFormatText => SelectedDateValue.ToShortDateString();

        public string SelectedDateLongFormatText => SelectedDateValue.ToLongDateString();

        public string SelectedDateCustomText => $"";

        private MainForm mainForm => (MainForm)Application.OpenForms["MainForm"];


        #endregion

        private void btnInsertDate_Click(object sender, EventArgs e) {
            if (mainForm.ActiveEditor == null) return;

            var result = SelectedDateValue.ToString(CultureInfo.CurrentUICulture);
            if (DateFormatMode == InsertDateFormValueMode.ShortDateFormat)
                result = SelectedDateShortFormatText;
            else if (DateFormatMode == InsertDateFormValueMode.LongDateFormat)
                result = SelectedDateLongFormatText;

            mainForm.ActiveEditor.Insert(result);
            mainForm.ActiveEditor.Focus();
        }

        private void DateForm_Deactivate(object sender, EventArgs e) {
            try {
                Opacity = 0.7;
            }
            catch { }
        }

        private void DateForm_Activated(object sender, EventArgs e) {
            Opacity = 1;
        }

        protected override bool ProcessDialogKey(Keys keyData) {
            if (ModifierKeys == Keys.None && keyData == Keys.Escape) {
                Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void rdbLongDate_CheckedChanged(object sender, EventArgs e) {
            if (rdbLongDate.Checked)
                DateFormatMode = InsertDateFormValueMode.LongDateFormat;
        }

        private void rdbShortDate_CheckedChanged(object sender, EventArgs e) {
            if (rdbShortDate.Checked)
                DateFormatMode = InsertDateFormValueMode.ShortDateFormat;
        }

        private void datePicker_ValueChanged(object sender, EventArgs e) {
            rdbLongDate.Text = SelectedDateLongFormatText;
            rdbShortDate.Text = SelectedDateShortFormatText;
        }
    }
}
