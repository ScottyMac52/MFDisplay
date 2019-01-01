using log4net;
using MFDisplay.Configuration;
using System;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MFDisplay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Configuration section for the MFDs
        /// </summary>
        public MFDConfigurationSection Configuration { get; protected set; }

        /// <summary>
        /// Logger for the application
        /// </summary>
        private ILog logger = null;

		public static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		private void Application_Startup(object sender, StartupEventArgs e)
        {
            logger = LogManager.GetLogger("MFDisplay");
            Configuration = MFDConfigurationSection.GetConfig();

			if(string.IsNullOrEmpty(Configuration.FilePath) || !Directory.Exists(Configuration.FilePath))
			{
				MessageBox.Show($"The path {Configuration.FilePath} is not valid. Please edit {Path.Combine(AssemblyDirectory, "MFDisplay.exe.config")} and change the FilePath.");
				Shutdown(2);
				return;
			}
            logger.Info($"Startup");
			 
            var maindWindow = new MainWindow(Configuration)
            {
                Logger = logger
            };

            MainWindow.Show();
        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
            logger.Info($"Shutdown");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}
