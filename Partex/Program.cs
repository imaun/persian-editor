using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace Farcin.Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.CurrentCulture = new CultureInfo("fa-IR");
            string[] args = Environment.GetCommandLineArgs();
            var controller = new SingleInstanceController();
            controller.Run(args);
        }
    }

    public class SingleInstanceController : WindowsFormsApplicationBase
    {
        public MainForm mainForm;

        public SingleInstanceController()
        {
            IsSingleInstance = true;
            Startup += OnStartup;
            StartupNextInstance += this_StartupNextInstance;
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            var filePaths = new List<string>();
            int i = 1;
            foreach (var line in startupEventArgs.CommandLine)
            {
                if (i > 1)
                    filePaths.Add(line);
                i++;
            }
            if(mainForm == null)
                OnCreateMainForm();
            mainForm.OpenArgumentFiles(filePaths);
        }


        void this_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            //Form1 form = MainForm as Form1; //My derived form type
            //form.LoadFile(e.CommandLine[1]);
            var filePaths = new List<string>();
            int i = 1;
            foreach (var line in e.CommandLine)
            {
                if(i > 1)
                    filePaths.Add(line);
                i++;
            }
            mainForm.OpenArgumentFiles(filePaths);
        }

        protected override void OnCreateMainForm()
        {
            if (mainForm != null) return;
            
            mainForm = new MainForm();
            MainForm = mainForm;
        }

        protected override void OnShutdown()
        {
            mainForm.Dispose();
            base.OnShutdown();
        }
    }
}
