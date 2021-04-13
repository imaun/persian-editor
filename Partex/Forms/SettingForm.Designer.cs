
namespace Farcin.Editor.Forms
{
    partial class SettingForm
    {
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
            this.Tabs = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.chkKeepOpenFiles = new System.Windows.Forms.CheckBox();
            this.tabEditor = new System.Windows.Forms.TabPage();
            this.txtEditorSample = new System.Windows.Forms.TextBox();
            this.btnBackColor = new System.Windows.Forms.Button();
            this.btnForeColor = new System.Windows.Forms.Button();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.ddlFonts = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Tabs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.tabGeneral);
            this.Tabs.Controls.Add(this.tabEditor);
            this.Tabs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Tabs.Location = new System.Drawing.Point(7, 13);
            this.Tabs.Name = "Tabs";
            this.Tabs.RightToLeftLayout = true;
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(648, 449);
            this.Tabs.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.chkKeepOpenFiles);
            this.tabGeneral.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.tabGeneral.Location = new System.Drawing.Point(4, 26);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(640, 419);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "عمومی";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // chkKeepOpenFiles
            // 
            this.chkKeepOpenFiles.AutoSize = true;
            this.chkKeepOpenFiles.Location = new System.Drawing.Point(289, 37);
            this.chkKeepOpenFiles.Name = "chkKeepOpenFiles";
            this.chkKeepOpenFiles.Size = new System.Drawing.Size(315, 21);
            this.chkKeepOpenFiles.TabIndex = 0;
            this.chkKeepOpenFiles.Text = "نگهداری از فایل های باز (حتی بعد از بسته شدن برنامه)";
            this.chkKeepOpenFiles.UseVisualStyleBackColor = true;
            // 
            // tabEditor
            // 
            this.tabEditor.Controls.Add(this.txtEditorSample);
            this.tabEditor.Controls.Add(this.btnBackColor);
            this.tabEditor.Controls.Add(this.btnForeColor);
            this.tabEditor.Controls.Add(this.numFontSize);
            this.tabEditor.Controls.Add(this.ddlFonts);
            this.tabEditor.Controls.Add(this.label2);
            this.tabEditor.Controls.Add(this.label4);
            this.tabEditor.Controls.Add(this.label3);
            this.tabEditor.Controls.Add(this.label1);
            this.tabEditor.Location = new System.Drawing.Point(4, 26);
            this.tabEditor.Name = "tabEditor";
            this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabEditor.Size = new System.Drawing.Size(640, 419);
            this.tabEditor.TabIndex = 1;
            this.tabEditor.Text = "ویرایشگر";
            this.tabEditor.UseVisualStyleBackColor = true;
            // 
            // txtEditorSample
            // 
            this.txtEditorSample.Location = new System.Drawing.Point(19, 90);
            this.txtEditorSample.Multiline = true;
            this.txtEditorSample.Name = "txtEditorSample";
            this.txtEditorSample.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEditorSample.Size = new System.Drawing.Size(436, 81);
            this.txtEditorSample.TabIndex = 4;
            this.txtEditorSample.Text = "یک نوشته نمونه\r\nA sample text";
            // 
            // btnBackColor
            // 
            this.btnBackColor.Location = new System.Drawing.Point(84, 56);
            this.btnBackColor.Name = "btnBackColor";
            this.btnBackColor.Size = new System.Drawing.Size(114, 28);
            this.btnBackColor.TabIndex = 3;
            this.btnBackColor.Text = "رنگ پس‌زمینه";
            this.btnBackColor.UseVisualStyleBackColor = true;
            this.btnBackColor.Click += new System.EventHandler(this.btnBackColor_Click);
            // 
            // btnForeColor
            // 
            this.btnForeColor.Location = new System.Drawing.Point(341, 55);
            this.btnForeColor.Name = "btnForeColor";
            this.btnForeColor.Size = new System.Drawing.Size(114, 28);
            this.btnForeColor.TabIndex = 3;
            this.btnForeColor.Text = "رنگ قلم";
            this.btnForeColor.UseVisualStyleBackColor = true;
            this.btnForeColor.Click += new System.EventHandler(this.btnForeColor_Click);
            // 
            // numFontSize
            // 
            this.numFontSize.Location = new System.Drawing.Point(19, 24);
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(91, 25);
            this.numFontSize.TabIndex = 2;
            this.numFontSize.ValueChanged += new System.EventHandler(this.numFontSize_ValueChanged);
            // 
            // ddlFonts
            // 
            this.ddlFonts.FormattingEnabled = true;
            this.ddlFonts.Location = new System.Drawing.Point(180, 24);
            this.ddlFonts.Name = "ddlFonts";
            this.ddlFonts.Size = new System.Drawing.Size(275, 25);
            this.ddlFonts.TabIndex = 1;
            this.ddlFonts.SelectedIndexChanged += new System.EventHandler(this.ddlFonts_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "اندازه :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "انتخاب رنگ پس‌زمینه:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(471, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "انتخاب رنگ قلم :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(471, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "نوع قلم (فونت) پیش فرض :";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(441, 468);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 32);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "ذخیره";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(549, 468);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 32);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(662, 512);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.Tabs);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تنظیمات فارسین";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.Tabs.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabEditor.ResumeLayout(false);
            this.tabEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabEditor;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkKeepOpenFiles;
        private System.Windows.Forms.ComboBox ddlFonts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnForeColor;
        private System.Windows.Forms.NumericUpDown numFontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEditorSample;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBackColor;
    }
}