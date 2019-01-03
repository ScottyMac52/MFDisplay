using log4net;
using MFDSettingsManager;
using System;
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
        private ILog logger = LogManager.GetLogger("MFDisplay");

		private void Application_Startup(object sender, StartupEventArgs e)
        {
            Configuration = MFDConfigurationSection.GetConfig();
            logger.Info($"Startup");
            var maindWindow = new MainWindow(Configuration)
            {
                Logger = logger
            };
            MainWindow.Show();
        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
            logger.Info($"Shutdown with exit code {e.ApplicationExitCode}");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger?.Error($"{sender?.GetType()?.Name} threw an exception", e?.Exception);
            Shutdown(-1);
        }
    }
}
