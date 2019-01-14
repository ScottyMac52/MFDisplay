using log4net;
using log4net.Config;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Mappers;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MFDisplay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class MFDisplayApp : Application
    {
        /// <summary>
        /// Configuration section for the MFDs
        /// </summary>
        public MFDConfigurationSection Configuration { get; protected set; }

        private static readonly ILog Logger;

        private static readonly bool configPresent = true;
        private static readonly string configurationFile = "";
        private static readonly string logPath = "";

        static MFDisplayApp()
        {
            var exeAssem = Assembly.GetExecutingAssembly();
            configurationFile = $"{exeAssem.Location}.config";
            if (!File.Exists(configurationFile))
            {
                MessageBox.Show($"The configuration file could not be found! Please re-install or repair the application or restore the configuration file {configurationFile}.", "Invalid Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                configPresent = false;
                return;
            }

            logPath = Path.Combine($"{(new FileInfo(exeAssem.Location)).Directory.FullName}", "mfdisplay.log");
            if (!File.Exists(logPath))
            {
                File.Create(logPath);
            }

            XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(MFDisplayApp));
        }

        /// <summary>
        /// Executed when the application starts
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            if(!configPresent)
            {
                Shutdown(2);
                return;
            }

            var assmLocation = Assembly.GetExecutingAssembly().Location;
            Configuration = MFDConfigurationSection.GetConfig(Logger);
            while (!Directory.Exists(Configuration.FilePath))
            {
                var configWindow = new ConfigurationWindow()
                {
                    Logger = Logger,
                    Config = ConfigSectionModelMapper.MapFromConfigurationSection(Configuration, Logger)
                };
                configWindow.ShowInTaskbar = true;
                configWindow.ShowDialog();
                Configuration = MFDConfigurationSection.GetConfig(Logger);
                if (!Directory.Exists(Configuration.FilePath))
                {
                    var msgResult = MessageBox.Show($"{Configuration.FilePath} is not a valid path. Do you wish to try another path?", "Invalid Path Configuration", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                    if(msgResult != MessageBoxResult.Yes)
                    {
                        break;
                    }
                }
            }

            if (!Directory.Exists(Configuration.FilePath))
            {
                Shutdown(2);
            }
            else
            {
                Logger.Info($"Startup");
                var maindWindow = new MainWindow()
                {
                    Config = Configuration,
                    Logger = Logger
                };
                MainWindow.ShowDialog();
                Shutdown(0);
            }
            base.OnStartup(e);
        }

        /// <summary>
        /// Executed when the application exits.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            Logger.Info($"Shutdown with exit code {e.ApplicationExitCode}");
            base.OnExit(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger?.Error($"{sender?.GetType()?.Name} threw an exception", e?.Exception);
            Shutdown(-1);
        }


    }
}
