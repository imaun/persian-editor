using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using PersianEditor.Models;

namespace PersianEditor
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
            var files = new List<PartexFile>();
            int i = 1;
            foreach (var line in startupEventArgs.CommandLine)
            {
                if (i > 1)
                    files.Add(new PartexFile(line));
                i++;
            }
            if(mainForm == null)
                OnCreateMainForm();
            mainForm.OpenArgumentFiles(files);
        }


        void this_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            //Form1 form = MainForm as Form1; //My derived form type
            //form.LoadFile(e.CommandLine[1]);
            var files = new List<PartexFile>();
            int i = 1;
            foreach (var line in e.CommandLine)
            {
                if(i > 1)
                    files.Add(new PartexFile(line));
                i++;
            }
            mainForm.OpenArgumentFiles(files);
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
