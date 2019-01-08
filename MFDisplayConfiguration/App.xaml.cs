using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.IO;
using System.Windows.Documents;

namespace MFDisplayConfiguration
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            var fileName = GetFileToEdit();
            if(string.IsNullOrEmpty(fileName))
            {
                Shutdown(2);
                return;
            }

            var mainWnd = new MainWindow()
            {
                ConfigurationFile = fileName
            };
            mainWnd.ShowDialog();
            Shutdown(0);
        }

        private string GetFileToEdit()
        {
            string fileName = null;
            // Create OpenFileDialog 
            var dlg = new OpenFileDialog
            {
                // Set filter for file extension and default file extension 
                DefaultExt = ".config",
                Filter = "MFDisplay Configuration (.config)|MFDisplay.exe.config",
                InitialDirectory = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName,
            };
            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();
            // Get the selected file name
            if ((result ?? false) == true)
            {
                // Open document 
                fileName = dlg.FileName;
            }
            return fileName;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
