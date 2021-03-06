﻿using log4net;
using log4net.Config;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Extensions;
using MFDSettingsManager.Models;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MFDisplay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class MFDisplayApp : Application, ISingleInstanceApp
    {
        private const string Unique = "Vyper_MFD4CTS_Application";
        private const string Company = "Vyper Industries";
        private const string Years = "2018, 2019"; 

		/// <summary>
		/// Currently selected module
		/// </summary>
		public string Module { get; private set; }
		
		/// <summary>
		/// Currently selected sub module
		/// </summary>
		public string SubModule { get; private set; }

		static string ConfigurationPath { get; set; }

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
            Module = args.GetSafeArgumentFrom("-mod");
            SubModule = args.GetSafeArgumentFrom("-submod");
			LoadSelectedModule();
            return true;
        }

        #endregion ISingleInstanceApp Members

        /// <summary>
        /// Gets the Name and version of the application
        /// </summary>
        public string VersionString => GetVersionString();

        private string GetVersionString()
        {
            char copyChar = (char) 169;
            var exeAssem = Assembly.GetExecutingAssembly();
            return $"{exeAssem.GetName().Name} {copyChar} {Years} {Company}, Version: {exeAssem.GetName().Version.Major}.{exeAssem.GetName().Version.Minor}.{exeAssem.GetName().Version.Build}.{exeAssem.GetName().Version.Revision}";
        }


        private ILog Logger;
        private static readonly bool configPresent = true;

        static MFDisplayApp()
        {
            ConfigurationPath = $"{Assembly.GetExecutingAssembly().Location}.config";
            if (!File.Exists(ConfigurationPath))
            {
                MessageBox.Show($"The configuration file could not be found! Please re-install or repair the application or restore the configuration file {ConfigurationPath}.", "Invalid Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                configPresent = false;
                return;
            }

            XmlConfigurator.Configure();
        }

		/// <summary>
		/// Loads the current configuration 
		/// </summary>
		public ModulesConfiguration LoadConfiguration()
		{
			Logger = LogManager.GetLogger("MFD4CTS");
			var sectionConfig = MFDConfigurationSection.GetConfig(Logger, ConfigurationPath);
			if (sectionConfig == null)
			{
				ShowConfigurationError();
				Shutdown(5);
				return null;
			}

			return sectionConfig.ToModel(Logger);
		}

		/// <summary>
		/// Loads the selected Module
		/// </summary>
		public void LoadSelectedModule()
		{
			if (!string.IsNullOrEmpty(Module))
			{
				Logger.Info($"External request to change module to {Module}.");
				((MainWindow)MainWindow).ChangeSelectedModule(Module);
			}

			if (!string.IsNullOrEmpty(SubModule))
			{
				Logger.Info($"External request to change sub-module to {SubModule}.");
				((MainWindow)MainWindow).ChangeSelectedSubModule(SubModule);
			}

		}

		/// <summary>
		/// Reports on Configuration errors
		/// </summary>
		/// <param name="parent">Parent Window</param>
		internal void ShowConfigurationError(Window parent = null)
        {
            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Vyper Industries\MFD4CTS\status.log");
            if(parent != null)
            {
                MessageBox.Show(parent, $"Unable to load the configuration from {ConfigurationPath}. Please check the log at {logPath} for details.", "Error in Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"Unable to load the configuration from {ConfigurationPath}. Please check the log at {logPath} for details.", "Error in Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Executed when the application starts
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Module = e.Args.GetSafeArgumentFrom("-mod");
            SubModule = e.Args.GetSafeArgumentFrom("-submod");

            if (!configPresent)
            {
                Shutdown(2);
                return;
            }

			var config = LoadConfiguration();

            Logger.Info($"Startup");
            var mainWindow = new MainWindow()
            {
                Config = config,
                Logger = Logger,
                WindowState = WindowState.Minimized,
                PassedModule = string.IsNullOrEmpty(Module) ? null : Module,
                PassedSubModule = string.IsNullOrEmpty(SubModule) ? null : SubModule,
            };

            mainWindow.ShowDialog();
            Shutdown(0);
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
