namespace Farcin.Editor.Forms {
    partial class DateForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnInsertDate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbShortDate = new System.Windows.Forms.RadioButton();
            this.rdbLongDate = new System.Windows.Forms.RadioButton();
            this.chkConvertToShamsi = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "تاریخ دلخواه را انتخاب کنید :";
            // 
            // datePicker
            // 
            this.datePicker.Location = new System.Drawing.Point(185, 23);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(251, 25);
            this.datePicker.TabIndex = 1;
            this.datePicker.ValueChanged += new System.EventHandler(this.datePicker_ValueChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(460, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 31);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnInsertDate
            // 
            this.btnInsertDate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnInsertDate.Location = new System.Drawing.Point(460, 23);
            this.btnInsertDate.Name = "btnInsertDate";
            this.btnInsertDate.Size = new System.Drawing.Size(87, 31);
            this.btnInsertDate.TabIndex = 3;
            this.btnInsertDate.Text = "درج";
            this.btnInsertDate.UseVisualStyleBackColor = true;
            this.btnInsertDate.Click += new System.EventHandler(this.btnInsertDate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbShortDate);
            this.groupBox1.Controls.Add(this.rdbLongDate);
            this.groupBox1.Location = new System.Drawing.Point(18, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(417, 128);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "فرمت :";
            // 
            // rdbShortDate
            // 
            this.rdbShortDate.Location = new System.Drawing.Point(53, 64);
            this.rdbShortDate.Name = "rdbShortDate";
            this.rdbShortDate.Size = new System.Drawing.Size(300, 27);
            this.rdbShortDate.TabIndex = 0;
            this.rdbShortDate.TabStop = true;
            this.rdbShortDate.Text = "radioButton1";
            this.rdbShortDate.UseVisualStyleBackColor = true;
            this.rdbShortDate.CheckedChanged += new System.EventHandler(this.rdbShortDate_CheckedChanged);
            // 
            // rdbLongDate
            // 
            this.rdbLongDate.Location = new System.Drawing.Point(53, 32);
            this.rdbLongDate.Name = "rdbLongDate";
            this.rdbLongDate.Size = new System.Drawing.Size(300, 25);
            this.rdbLongDate.TabIndex = 0;
            this.rdbLongDate.TabStop = true;
            this.rdbLongDate.Text = "radioButton1";
            this.rdbLongDate.UseVisualStyleBackColor = true;
            this.rdbLongDate.CheckedChanged += new System.EventHandler(this.rdbLongDate_CheckedChanged);
            // 
            // chkConvertToShamsi
            // 
            this.chkConvertToShamsi.AutoSize = true;
            this.chkConvertToShamsi.Location = new System.Drawing.Point(18, 196);
            this.chkConvertToShamsi.Name = "chkConvertToShamsi";
            this.chkConvertToShamsi.Size = new System.Drawing.Size(141, 21);
            this.chkConvertToShamsi.TabIndex = 6;
            this.chkConvertToShamsi.Text = "تبدیل به تاریخ شمسی";
            this.chkConvertToShamsi.UseVisualStyleBackColor = true;
            this.chkConvertToShamsi.CheckedChanged += new System.EventHandler(this.chkConvertToShamsi_CheckedChanged);
            // 
            // DateForm
            // 
            this.AcceptButton = this.btnInsertDate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 231);
            this.Controls.Add(this.chkConvertToShamsi);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnInsertDate);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DateForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "درج تاریخ دلخواه";
            this.Activated += new System.EventHandler(this.DateForm_Activated);
            this.Deactivate += new System.EventHandler(this.DateForm_Deactivate);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnInsertDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbShortDate;
        private System.Windows.Forms.RadioButton rdbLongDate;
        private System.Windows.Forms.CheckBox chkConvertToShamsi;
    }
}