using log4net;
using MFDSettingsManager.Models;
using System;
using System.Collections.Generic;
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
        /// Sorted list of the active MFDs
        /// </summary>
        protected SortedList<string, MFDWindow> MFDList { get; set; }

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
        /// Was the Cougar argument specified?
        /// </summary>
        public bool UseCougar { get; internal set; }

        /// <summary>
        /// Ctor, initializes component, logging, sorted list and loads the configuration  
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When we load we create the configured MFDs
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
            Logger.Info("Closing the MFD Windows...");
            DestroyMFDs();
        }

        /// <summary>
        /// Sets up the main window
        /// </summary>
        public void SetupWindow()
        {
            MFDList = new SortedList<string, MFDWindow>();
            AvailableModules = Config?.Modules;

            cbModules.ItemsSource = AvailableModules;
            cbModules.DisplayMemberPath = "DisplayName";
            cbModules.SelectedValuePath = "ModuleName";

        }

        /// <summary>
        /// Creates the MFDs using the specified Module Definition
        /// </summary>
        public void CreateMFDs()
        {
            Logger.Debug($"Creating configuration {SelectedModule?.DisplayName}");
            SelectedModule?.Configurations?.ForEach(config =>
            {
                Logger.Info($"Creating {config.ToReadableString()}");
                var newmfdWindow = new MFDWindow()
                {
                    Logger = Logger,
                    Configuration = config,
                    FilePath = Config.FilePath,
                    UseCougar = UseCougar
                };
                newmfdWindow.Show();
                if (newmfdWindow.IsMFDLoaded)
                {
                    MFDList.Add(config.Name, newmfdWindow);
                }
                else
                {
                    newmfdWindow?.Close();
                }
            });
        }

        /// <summary>
        /// Closes all the open Windows
        /// </summary>
        public void DestroyMFDs()
        {
            MFDList.ToList().ForEach(mfd =>
            {
                if (mfd.Value.IsLoaded)
                {
                    mfd.Value.Hide();
                    mfd.Value.Close();
                }
            });

            MFDList.Clear();
            Logger.Info($"MFD list cleared.");
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

        private void ProcessChangedModule(string moduleName)
        {
            if (GetSelectedDefinition(moduleName))
            {
                DestroyMFDs();
                try
                {
                    CreateMFDs();
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
            Logger.Info($"Is Closed.");
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

        private void ConfigurationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DestroyMFDs();
            var configWindow = new ConfigurationWindow()
            {
                Logger = Logger,
                Owner = this
            };
            configWindow.ShowDialog();
            SetupWindow();
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((MFDisplayApp)Application.Current).VersionString);
        }

        private void FileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
