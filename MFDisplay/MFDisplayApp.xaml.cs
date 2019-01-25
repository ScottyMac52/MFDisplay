using log4net;
using log4net.Config;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Extensions;
using MFDSettingsManager.Models;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace MFDisplay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class MFDisplayApp : Application, ISingleInstanceApp
    {
        private const string Unique = "Vyper_MFD4CTS_Application";

        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<MFDisplayApp>.InitializeAsFirstInstance(Unique))
            {
                var application = new MFDisplayApp();

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<MFDisplayApp>.Cleanup();
            }
        }

        #region ISingleInstanceApp Members

        /// <summary>
        /// Second instance handler
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            var modSpecified = args.GetSafeArgumentFrom("-mod");
            if(!string.IsNullOrEmpty(modSpecified))
            {
                Logger.Info($"External request to change module to {modSpecified}.");
                ((MainWindow)MainWindow).ChangeSelectedModule(modSpecified);
            }
            return true;
        }

        #endregion ISingleInstanceApp Members

        /// <summary>
        /// Configuration section for the MFDs
        /// </summary>
        public ModulesConfiguration Configuration { get; protected set; }

        private ILog Logger;
        private static readonly bool configPresent = true;
        private static readonly string configurationFile = "";

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

            XmlConfigurator.Configure();
        }


        /// <summary>
        /// Executed when the application starts
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var modSpecified = e.Args.GetSafeArgumentFrom("-mod");

            if (!configPresent)
            {
                Shutdown(2);
                return;
            }

            Logger = LogManager.GetLogger("MFD4CTS");
            var assmLocation = Assembly.GetExecutingAssembly().Location;
            var sectionConfig = MFDConfigurationSection.GetConfig(Logger);
            Configuration = sectionConfig.ToModel(Logger);
            while (!Directory.Exists(Configuration.FilePath))
            {
                var configWindow = new ConfigurationWindow()
                {
                    Logger = Logger
                };
                configWindow.ShowInTaskbar = true;
                configWindow.ShowDialog();
                sectionConfig = MFDConfigurationSection.GetConfig(Logger);
                Configuration = sectionConfig.ToModel(Logger);
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
                var mainWindow = new MainWindow()
                {
                    Config = Configuration,
                    Logger = Logger,
                    WindowState = WindowState.Minimized,
                    PassedModule = string.IsNullOrEmpty(modSpecified) ? null : modSpecified
                };
                mainWindow.ShowDialog();
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
