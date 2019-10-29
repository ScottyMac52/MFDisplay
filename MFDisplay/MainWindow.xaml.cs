using log4net;
using MFDSettingsManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MFDisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Logger for the window
        /// </summary>
        public ILog Logger { get; set;}

        /// <summary>
        /// The configuration
        /// </summary>
        public ModulesConfiguration Config { get; set; }
       
        /// <summary>
        /// Sorted list of the active Windows
        /// </summary>
        protected SortedList<string, AuxWindow> WindowList { get; set; }

        /// <summary>
        /// The list of available modules
        /// </summary>
        protected List<ModuleDefinition> AvailableModules { get; set; }

        /// <summary>
        /// Currently selected Module
        /// </summary>
        protected ModuleDefinition SelectedModule { get; set; }

        /// <summary>
        /// The name of the module that was passed in
        /// </summary>
        public string PassedModule { get; internal set; }

        /// <summary>
        /// The name of the sub-module that was passed
        /// </summary>
        public string PassedSubModule { get; internal set; }

        /// <summary>
        /// Ctor, initializes component, logging, sorted list and loads the configuration  
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When we load we create the configured Windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupWindow();
        }

        /// <summary>
        /// When we are closing we close the windows and clear the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logger.Info("Closing the Windows...");
            DestroyWindows();
        }

        /// <summary>
        /// Sets up the main window
        /// </summary>
        public void SetupWindow()
        {
            WindowList = new SortedList<string, AuxWindow>();
            var moduleList = Config?.Modules;
            moduleList?.Sort(new ModuleDefinitionComparer());
            AvailableModules = moduleList;

            cbModules.ItemsSource = AvailableModules;
            cbModules.DisplayMemberPath = "DisplayName";
            cbModules.SelectedValuePath = "ModuleName";

        }

        /// <summary>
        /// Creates the Windows using the specified Module Definition
        /// </summary>
        public void CreateWindows()
        {
            Logger.Debug($"Creating configuration {SelectedModule?.DisplayName}");
            SelectedModule?.Configurations?.ForEach(config =>
            {
                Logger.Info($"Creating {config.ToReadableString()}");
                var newAuxWindow = new AuxWindow()
                {
                    Logger = Logger,
                    Configuration = config,
                    FilePath = Config.FilePath,
                    SubConfigurationName = PassedSubModule
                };
                newAuxWindow.Show();
                if (newAuxWindow.IsWindowLoaded)
                {
                    WindowList.Add(config.Name, newAuxWindow);
                    newAuxWindow.Visibility = Visibility.Visible;
                }
                else
                {
                    newAuxWindow?.Close();
                }
            });
        }

        /// <summary>
        /// Closes all the open Windows
        /// </summary>
        public void DestroyWindows()
        {
            WindowList.ToList().ForEach(mfd =>
            {
                if (mfd.Value.IsLoaded)
                {
                    mfd.Value.Hide();
                    mfd.Value.Close();
                }
            });

            WindowList.Clear();
            Logger.Info($"Window list cleared.");
        }

        /// <summary>
        /// Used to change the selected module 
        /// </summary>
        /// <param name="moduleName"></param>
        public void ChangeSelectedModule(string moduleName)
        {
            if (moduleName != (string)cbModules?.SelectedValue)
            {
                cbModules.SelectedValue = moduleName;
            }
        }

        /// <summary>
        /// Changes the selected sub-module
        /// </summary>
        /// <param name="subModeSpecified"></param>
        public void ChangeSelectedSubModule(string subModeSpecified)
        {
            PassedSubModule = subModeSpecified;
            DestroyWindows();
            CreateWindows();
        }


        private void ProcessChangedModule(string moduleName)
        {
            if (GetSelectedDefinition(moduleName))
            {
                DestroyWindows();
                try
                {
                    CreateWindows();
                    Logger.Info($"Module loaded {moduleName}.");
                }
                catch (IndexOutOfRangeException ioorx)
                {
                    Logger.Error($"Not able to determine selected module", ioorx);
                }
            }
            else
            {
                Logger.Error($"{moduleName} does not exist as a module in the current configuration.");
            }

        }

        /// <summary>
        /// Gets the specified Definition
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public bool GetSelectedDefinition(string moduleName)
        {
            if(string.IsNullOrEmpty(moduleName))
            {
                return false;
            }
            Logger.Info($"Configuration requested for {moduleName}");
            SelectedModule = AvailableModules.Where(am => am.ModuleName == moduleName).FirstOrDefault();
            return SelectedModule != null;
        }


        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedModule = e.AddedItems.Count > 0 ? (ModuleDefinition)e.AddedItems[0] : e.RemovedItems.Count > 0 ? (ModuleDefinition)e.RemovedItems[0] : null;
                Logger.Info($"Module selected {selectedModule?.DisplayName}");
                ProcessChangedModule(selectedModule.ModuleName);
            }
            catch(IndexOutOfRangeException ioorx)
            {
                Logger.Error($"Not able to determine selected module", ioorx);
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Logger.Info($"MainWindow Is Closed.");
        }

        private void CbModules_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Config.DefaultConfig) || !string.IsNullOrEmpty(PassedModule))
            {
                if (string.IsNullOrEmpty(PassedModule))
                {
                    Logger.Info($"Loading the default configuration {Config.DefaultConfig}...");
                }
                else
                {
                    Logger.Info($"Loading the requested configuration {PassedModule}...");
                }
                var selectedModule = PassedModule ?? Config.DefaultConfig;
                cbModules.SelectedValue = selectedModule;
            }
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((MFDisplayApp)Application.Current).VersionString);
        }

        private void FileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Clears the cache of all PNG files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearCache_Click(object sender, RoutedEventArgs e)
        {
            DestroyWindows();
            var cacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"Vyper Industries\\MFD4CTS\\cache\\");
            var cacheParent = new DirectoryInfo(cacheFolder);
            var fileList = cacheParent?.EnumerateFiles("*.png", SearchOption.AllDirectories).ToList();
            fileList?.ForEach((file) =>
            {
                try
                {
                    file?.Delete();
                }
                catch (Exception ex)
                {
                    Logger.Error($"Unable to delete cache file: {file.FullName}", ex);
                }
            });
            try
            {
                cacheParent?.Delete(true);

            }
            catch (Exception dex)
            {

                Logger.Error($"Unable to delete cache directory: {cacheFolder}", dex);
            }
            CreateWindows();
        }

		/// <summary>
		/// Menu item to relaod the current configuration
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReloadConfiguration_Click(object sender, RoutedEventArgs e)
		{
			var currentapp = ((MFDisplayApp)Application.Current);
			var module = (string)cbModules.SelectedValue;
			DestroyWindows();
			Config = currentapp.LoadConfiguration();
			currentapp.LoadSelectedModule();
			SetupWindow();
			ChangeSelectedModule(module);
		}
	}
}
