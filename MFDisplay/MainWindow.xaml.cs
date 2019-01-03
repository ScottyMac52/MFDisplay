using log4net;
using MFDisplay.Interfaces;
using MFDisplay.Mappers;
using MFDisplay.Models;
using MFDSettingsManager;
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
        public MFDConfigurationSection Config { get; set; }
       
        /// <summary>
        /// Sorted list of the active MFDs
        /// </summary>
        protected SortedList<string, MFDWindow> MFDList { get; set; }

        /// <summary>
        /// The list of available modules
        /// </summary>
        protected List<IModuleDefinition> AvailableModules { get; set; }

        /// <summary>
        /// Ctor, initializes component, logging, sorted list and loads the configuration  
        /// </summary>
        public MainWindow(MFDConfigurationSection config)
        {
            MFDList = new SortedList<string, MFDWindow>();
            AvailableModules = new List<IModuleDefinition>();
            Config = config;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            SetupWindow();
        }

        /// <summary>
        /// When we load we create the configured MFDs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Info("is Loaded");
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
            var model = ConfigSectionModelMapper.MapFromConfigurationSection(Config);
            AvailableModules = model.Modules;

            cbModules.ItemsSource = AvailableModules;
            cbModules.DisplayMemberPath = "DisplayName";
            cbModules.SelectedValuePath = "ModuleName";

        }

        /// <summary>
        /// Creates the MFDs using the specified Module Definition
        /// </summary>
        /// <param name="moduleDef"><seealso cref="IModuleDefinition"/></param>
        public void CreateMFDs(IModuleDefinition moduleDef)
        {
            Logger.Debug($"Creating configuration {moduleDef.DisplayName}");
            moduleDef.Configurations.ForEach(config =>
            {
                Logger.Info($"Creatiing {config.ToReadableString()}");
                var newmfdWindow = new MFDWindow()
                {
                    Logger = Logger,
                    Configuration = config,
                    FilePath = Path.Combine(Config.FilePath, moduleDef.ModuleName)
                };
                newmfdWindow.Show();
                if (newmfdWindow.IsMFDLoaded)
                {
                    MFDList.Add(config.Name, newmfdWindow);
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
        /// Gets the specified Definition
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public IModuleDefinition GetSelectedDefinition(string moduleName)
        {
            Logger.Info($"Configuration requested for {moduleName}");
            return AvailableModules.Where(am => am.ModuleName == moduleName).FirstOrDefault();
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Logger.Info($"Selected module changing...");
            DestroyMFDs();
            var selectedModule = (ModuleDefinition)e.AddedItems[0];
            Logger.Info($"Module selected {selectedModule.DisplayName}");
            var moduleDef = GetSelectedDefinition(selectedModule.ModuleName);
            CreateMFDs(moduleDef);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Logger.Info($"Is Closed.");
        }

        private void CbModules_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Config.DefaultConfig))
            {
                Logger.Info($"Loading the default configuration {Config.DefaultConfig}...");
                cbModules.SelectedValue = Config.DefaultConfig;
            }
        }
    }
}
