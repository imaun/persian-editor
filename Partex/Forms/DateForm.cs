using Farcin.Editor.Core.Date;
using Farcin.Editor.Core.Models.Types;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Farcin.Editor.Forms {
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

        public string SelectedDateShortFormatPersian =>
            new PersianDateHelper(SelectedDateValue)
                .ToString(PersianDateFormat.ShortDate);

        public string SelectedDateLongFormatPersian =>
            new PersianDateHelper(SelectedDateValue)
                .ToString(PersianDateFormat.LongDate);

        public string SelectedDateCustomText => $"";

        public bool MustConvertToShamsi {
            get => chkConvertToShamsi.Checked;
            set => chkConvertToShamsi.Checked = value;
        }

        private MainForm mainForm => (MainForm)Application.OpenForms["MainForm"];


        #endregion

        private void btnInsertDate_Click(object sender, EventArgs e) {
            if (mainForm.ActiveEditor == null) return;

            var result = string.Empty;            
            if(MustConvertToShamsi) {
                if (DateFormatMode == InsertDateFormValueMode.ShortDateFormat)
                    result = SelectedDateShortFormatPersian;
                else if (DateFormatMode == InsertDateFormValueMode.LongDateFormat)
                    result = SelectedDateLongFormatPersian;
            }
            else {
                result = SelectedDateValue.ToString(CultureInfo.CurrentUICulture);
                if (DateFormatMode == InsertDateFormValueMode.ShortDateFormat)
                    result = SelectedDateShortFormatText;
                else if (DateFormatMode == InsertDateFormValueMode.LongDateFormat)
                    result = SelectedDateLongFormatText;
            }

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
            if(MustConvertToShamsi) {
                rdbLongDate.Text = SelectedDateLongFormatPersian;
                rdbShortDate.Text = SelectedDateShortFormatPersian;
            }
            else {
                rdbLongDate.Text = SelectedDateLongFormatText;
                rdbShortDate.Text = SelectedDateShortFormatText;
            }
        }

        private void chkConvertToShamsi_CheckedChanged(object sender, EventArgs e) {
            datePicker_ValueChanged(sender, e);
        }
    }
}
