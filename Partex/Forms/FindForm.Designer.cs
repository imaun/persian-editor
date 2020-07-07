namespace PersianEditor.Forms
{
    partial class FindForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Tabs = new System.Windows.Forms.TabControl();
            this.tabFind = new System.Windows.Forms.TabPage();
            this.btnFindCount = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabReplace = new System.Windows.Forms.TabPage();
            this.btnReplaceFind = new System.Windows.Forms.Button();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReplaceValue = new System.Windows.Forms.TextBox();
            this.chkSwitchCap = new System.Windows.Forms.CheckBox();
            this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.chkReplaceWholeWord = new System.Windows.Forms.CheckBox();
            this.chkReplaceSwitchCap = new System.Windows.Forms.CheckBox();
            this.Tabs.SuspendLayout();
            this.tabFind.SuspendLayout();
            this.tabReplace.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.tabFind);
            this.Tabs.Controls.Add(this.tabReplace);
            this.Tabs.Location = new System.Drawing.Point(14, 14);
            this.Tabs.Name = "Tabs";
            this.Tabs.RightToLeftLayout = true;
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(520, 226);
            this.Tabs.TabIndex = 0;
            this.Tabs.SelectedIndexChanged += new System.EventHandler(this.Tabs_SelectedIndexChanged);
            // 
            // tabFind
            // 
            this.tabFind.Controls.Add(this.chkMatchWholeWord);
            this.tabFind.Controls.Add(this.chkSwitchCap);
            this.tabFind.Controls.Add(this.btnFindCount);
            this.tabFind.Controls.Add(this.btnFind);
            this.tabFind.Controls.Add(this.txtFind);
            this.tabFind.Controls.Add(this.label1);
            this.tabFind.Location = new System.Drawing.Point(4, 24);
            this.tabFind.Name = "tabFind";
            this.tabFind.Padding = new System.Windows.Forms.Padding(3);
            this.tabFind.Size = new System.Drawing.Size(512, 198);
            this.tabFind.TabIndex = 0;
            this.tabFind.Text = "یافتن";
            this.tabFind.UseVisualStyleBackColor = true;
            this.tabFind.Click += new System.EventHandler(this.tabFind_Click);
            // 
            // btnFindCount
            // 
            this.btnFindCount.Location = new System.Drawing.Point(33, 61);
            this.btnFindCount.Name = "btnFindCount";
            this.btnFindCount.Size = new System.Drawing.Size(87, 27);
            this.btnFindCount.TabIndex = 2;
            this.btnFindCount.Text = "تعداد";
            this.btnFindCount.UseVisualStyleBackColor = true;
            this.btnFindCount.Click += new System.EventHandler(this.btnFindCount_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(33, 28);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(87, 27);
            this.btnFind.TabIndex = 1;
            this.btnFind.Text = "یافتن";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(145, 30);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(256, 23);
            this.txtFind.TabIndex = 0;
            this.txtFind.TextChanged += new System.EventHandler(this.txtWhatToFind_TextChanged);
            this.txtFind.Enter += new System.EventHandler(this.txtWhatToFind_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(408, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "جستجوی :";
            // 
            // tabReplace
            // 
            this.tabReplace.Controls.Add(this.chkReplaceWholeWord);
            this.tabReplace.Controls.Add(this.chkReplaceSwitchCap);
            this.tabReplace.Controls.Add(this.btnReplaceAll);
            this.tabReplace.Controls.Add(this.btnReplace);
            this.tabReplace.Controls.Add(this.btnReplaceFind);
            this.tabReplace.Controls.Add(this.txtReplaceValue);
            this.tabReplace.Controls.Add(this.txtReplace);
            this.tabReplace.Controls.Add(this.label3);
            this.tabReplace.Controls.Add(this.label2);
            this.tabReplace.Location = new System.Drawing.Point(4, 24);
            this.tabReplace.Name = "tabReplace";
            this.tabReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tabReplace.Size = new System.Drawing.Size(512, 198);
            this.tabReplace.TabIndex = 1;
            this.tabReplace.Text = "جایگزینی";
            this.tabReplace.UseVisualStyleBackColor = true;
            this.tabReplace.Click += new System.EventHandler(this.tabReplace_Click);
            // 
            // btnReplaceFind
            // 
            this.btnReplaceFind.Location = new System.Drawing.Point(33, 28);
            this.btnReplaceFind.Name = "btnReplaceFind";
            this.btnReplaceFind.Size = new System.Drawing.Size(87, 27);
            this.btnReplaceFind.TabIndex = 3;
            this.btnReplaceFind.Text = "یافتن";
            this.btnReplaceFind.UseVisualStyleBackColor = true;
            this.btnReplaceFind.Click += new System.EventHandler(this.btnReplaceFind_Click);
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(145, 30);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(256, 23);
            this.txtReplace.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(408, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "جستجوی :";
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(33, 61);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(87, 27);
            this.btnReplace.TabIndex = 4;
            this.btnReplace.Text = "جایگزینی";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(33, 94);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(87, 27);
            this.btnReplaceAll.TabIndex = 5;
            this.btnReplaceAll.Text = "جایگزینی همه";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(408, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "جایگزینی با :";
            // 
            // txtReplaceValue
            // 
            this.txtReplaceValue.Location = new System.Drawing.Point(145, 64);
            this.txtReplaceValue.Name = "txtReplaceValue";
            this.txtReplaceValue.Size = new System.Drawing.Size(256, 23);
            this.txtReplaceValue.TabIndex = 2;
            // 
            // chkSwitchCap
            // 
            this.chkSwitchCap.AutoSize = true;
            this.chkSwitchCap.Location = new System.Drawing.Point(310, 120);
            this.chkSwitchCap.Name = "chkSwitchCap";
            this.chkSwitchCap.Size = new System.Drawing.Size(178, 19);
            this.chkSwitchCap.TabIndex = 3;
            this.chkSwitchCap.Text = "حساس به کوچک و بزرگی حروف";
            this.chkSwitchCap.UseVisualStyleBackColor = true;
            this.chkSwitchCap.CheckedChanged += new System.EventHandler(this.chkSwitchCap_CheckedChanged);
            // 
            // chkMatchWholeWord
            // 
            this.chkMatchWholeWord.AutoSize = true;
            this.chkMatchWholeWord.Location = new System.Drawing.Point(393, 154);
            this.chkMatchWholeWord.Name = "chkMatchWholeWord";
            this.chkMatchWholeWord.Size = new System.Drawing.Size(95, 19);
            this.chkMatchWholeWord.TabIndex = 3;
            this.chkMatchWholeWord.Text = "مطابق با کلمه";
            this.chkMatchWholeWord.UseVisualStyleBackColor = true;
            this.chkMatchWholeWord.CheckedChanged += new System.EventHandler(this.chkMatchWholeWord_CheckedChanged);
            // 
            // chkReplaceWholeWord
            // 
            this.chkReplaceWholeWord.AutoSize = true;
            this.chkReplaceWholeWord.Location = new System.Drawing.Point(393, 154);
            this.chkReplaceWholeWord.Name = "chkReplaceWholeWord";
            this.chkReplaceWholeWord.Size = new System.Drawing.Size(95, 19);
            this.chkReplaceWholeWord.TabIndex = 6;
            this.chkReplaceWholeWord.Text = "مطابق با کلمه";
            this.chkReplaceWholeWord.UseVisualStyleBackColor = true;
            this.chkReplaceWholeWord.CheckedChanged += new System.EventHandler(this.chkReplaceWholeWord_CheckedChanged);
            // 
            // chkReplaceSwitchCap
            // 
            this.chkReplaceSwitchCap.AutoSize = true;
            this.chkReplaceSwitchCap.Location = new System.Drawing.Point(310, 120);
            this.chkReplaceSwitchCap.Name = "chkReplaceSwitchCap";
            this.chkReplaceSwitchCap.Size = new System.Drawing.Size(178, 19);
            this.chkReplaceSwitchCap.TabIndex = 7;
            this.chkReplaceSwitchCap.Text = "حساس به کوچک و بزرگی حروف";
            this.chkReplaceSwitchCap.UseVisualStyleBackColor = true;
            this.chkReplaceSwitchCap.CheckedChanged += new System.EventHandler(this.chkReplaceSwitchCap_CheckedChanged);
            // 
            // FindForm
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 254);
            this.Controls.Add(this.Tabs);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "یافتن";
            this.Activated += new System.EventHandler(this.FindForm_Activated);
            this.Deactivate += new System.EventHandler(this.FindForm_Deactivate);
            this.Load += new System.EventHandler(this.FindForm_Load);
            this.Shown += new System.EventHandler(this.FindForm_Shown);
            this.Tabs.ResumeLayout(false);
            this.tabFind.ResumeLayout(false);
            this.tabFind.PerformLayout();
            this.tabReplace.ResumeLayout(false);
            this.tabReplace.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tabFind;
        private System.Windows.Forms.TabPage tabReplace;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFindCount;
        private System.Windows.Forms.Button btnReplaceFind;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.TextBox txtReplaceValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkSwitchCap;
        private System.Windows.Forms.CheckBox chkMatchWholeWord;
        private System.Windows.Forms.CheckBox chkReplaceWholeWord;
        private System.Windows.Forms.CheckBox chkReplaceSwitchCap;
    }
}