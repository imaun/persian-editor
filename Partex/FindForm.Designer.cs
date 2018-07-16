namespace PersianEditor
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
            this.txtWhatToFind = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabReplace = new System.Windows.Forms.TabPage();
            this.Tabs.SuspendLayout();
            this.tabFind.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.tabFind);
            this.Tabs.Controls.Add(this.tabReplace);
            this.Tabs.Location = new System.Drawing.Point(12, 12);
            this.Tabs.Name = "Tabs";
            this.Tabs.RightToLeftLayout = true;
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(446, 196);
            this.Tabs.TabIndex = 0;
            // 
            // tabFind
            // 
            this.tabFind.Controls.Add(this.btnFindCount);
            this.tabFind.Controls.Add(this.btnFind);
            this.tabFind.Controls.Add(this.txtWhatToFind);
            this.tabFind.Controls.Add(this.label1);
            this.tabFind.Location = new System.Drawing.Point(4, 22);
            this.tabFind.Name = "tabFind";
            this.tabFind.Padding = new System.Windows.Forms.Padding(3);
            this.tabFind.Size = new System.Drawing.Size(438, 170);
            this.tabFind.TabIndex = 0;
            this.tabFind.Text = "یافتن";
            this.tabFind.UseVisualStyleBackColor = true;
            // 
            // btnFindCount
            // 
            this.btnFindCount.Location = new System.Drawing.Point(28, 53);
            this.btnFindCount.Name = "btnFindCount";
            this.btnFindCount.Size = new System.Drawing.Size(75, 23);
            this.btnFindCount.TabIndex = 3;
            this.btnFindCount.Text = "تعداد";
            this.btnFindCount.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(28, 24);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "یافتن";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtWhatToFind
            // 
            this.txtWhatToFind.Location = new System.Drawing.Point(124, 26);
            this.txtWhatToFind.Name = "txtWhatToFind";
            this.txtWhatToFind.Size = new System.Drawing.Size(220, 21);
            this.txtWhatToFind.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(350, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "جستجوی :";
            // 
            // tabReplace
            // 
            this.tabReplace.Location = new System.Drawing.Point(4, 22);
            this.tabReplace.Name = "tabReplace";
            this.tabReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tabReplace.Size = new System.Drawing.Size(438, 170);
            this.tabReplace.TabIndex = 1;
            this.tabReplace.Text = "جایگذاری";
            this.tabReplace.UseVisualStyleBackColor = true;
            // 
            // FindForm
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 220);
            this.Controls.Add(this.Tabs);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "یافتن";
            this.Load += new System.EventHandler(this.FindForm_Load);
            this.Shown += new System.EventHandler(this.FindForm_Shown);
            this.Tabs.ResumeLayout(false);
            this.tabFind.ResumeLayout(false);
            this.tabFind.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tabFind;
        private System.Windows.Forms.TabPage tabReplace;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtWhatToFind;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFindCount;
    }
}