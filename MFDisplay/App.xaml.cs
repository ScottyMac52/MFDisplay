using log4net;
using MFDSettingsManager;
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
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Executed when the application starts
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            Configuration = MFDConfigurationSection.GetConfig();
            if(!File.Exists(Configuration.FilePath))
            {
                var assmLocation = Assembly.GetExecutingAssembly().Location;
                var errorMessage = $"Unable to find path {Configuration.FilePath}";
                Logger.Error(errorMessage, new DirectoryNotFoundException(Configuration.FilePath));
                MessageBox.Show($"{errorMessage}. Please edit {assmLocation}.config and change FilePath to the location of the graphics files.", "Path Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(2);
            }
            Logger.Info($"Startup");
            var maindWindow = new MainWindow(Configuration)
            {
                Logger = Logger
            };
            MainWindow.Show();
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
