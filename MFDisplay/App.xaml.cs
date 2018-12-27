using log4net;
using MFDisplay.Configuration;
using System.Windows;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

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

        public App()
        {
            AppCenter.Start("d94b01d4-7eda-4715-b7f9-50ea785733e4", typeof(Analytics));
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
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
            logger.Info($"Shutdown");
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}
