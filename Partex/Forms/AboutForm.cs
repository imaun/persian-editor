using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Farcin.Editor.Utils;

namespace Farcin.Editor.Forms
{
    public partial class AboutForm : Form
    {
        private int typeAboutIndex = 0;
        private int verIndex = 0;
        private int winInfoIndex = 0;
        private bool printUpTime = false;

        private string[] aboutChars = new[] { "ب", "ر", "ن", "ا", "م", "ه", " ", "ن", "و", "ی", "س",
            ":", " ",
            "ا", "ی", "م", "ا", "ن", " ", "ن", "ع", "م", "ت", "ی"
        };

        private string[] verChars = new[] {
            "ن", "س", "خ", "ه", ":", " ", "1", ".", "0"
        };

        private int maxAboutChars;

        private SysWindowsInfo _winInfo = SysInfoHelper.GetWindowsInfo();
        private string _System_UpTime = $"";

        public AboutForm() {
            InitializeComponent();
            timer1.Interval = 100;
            maxAboutChars = aboutChars.Length;
            timer1.Tick += Timer1_Tick;
            timer1.Enabled = true;
            timer2.Interval = 100;
            timer2.Tick += Timer2_Tick;
            timer3.Interval = 300;
            timer3.Tick += Timer3_Tick;
            timer4.Interval = 300;
            timer4.Tick += Timer4_Tick;
        }

        private void Timer4_Tick(object sender, EventArgs e) {
            if(printUpTime) {
                txtAbout.Text += SysInfoHelper.LocalizedUpTime;
                timer4.Enabled = false;
                return;
            }
            txtAbout.Text += "----------------" + Environment.NewLine;
            txtAbout.Text += "آپ تایم سیستم :";
            printUpTime = true;
        }

        private void Timer3_Tick(object sender, EventArgs e) {
            int max = 4;
            if(winInfoIndex == max) {
                timer3.Enabled = false;
                timer4.Enabled = true;
                return;
            }

            if(winInfoIndex == 0) {
                txtAbout.Text += "-----------" + Environment.NewLine;
                winInfoIndex++;
                return;
            }
            if(winInfoIndex == 1) {
                txtAbout.Text += $"{_winInfo.Title}{Environment.NewLine}";
                winInfoIndex++;
                return;
            }
            if(winInfoIndex == 2) {
                txtAbout.Text += $"{_winInfo.Version}{Environment.NewLine}";
                winInfoIndex++;
                return;
            }
            if(winInfoIndex == 3) {
                txtAbout.Text += $"{_winInfo.User}{Environment.NewLine}";
                winInfoIndex++;
            }

        }

        private void Timer2_Tick(object sender, EventArgs e) {
            int max = verChars.Length;
            if(verIndex < max) {
                txtAbout.Text += verChars[verIndex];
                verIndex++;
            }
            else {
                timer2.Enabled = false;
                txtAbout.Text += Environment.NewLine;
                timer3.Enabled = true;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e) {
            if (typeAboutIndex < maxAboutChars) {
                txtAbout.Text += aboutChars[typeAboutIndex];
                typeAboutIndex++;
            }
            else {
                timer1.Enabled = false;
                txtAbout.Text += Environment.NewLine;
                timer2.Enabled = true;
            }
        }



        private void AboutForm_Load(object sender, EventArgs e) {
            
        }

        private void AboutForm_Activated(object sender, EventArgs e) {
            
        }
    }
}
