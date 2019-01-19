﻿using log4net;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Extensions;
using MFDSettingsManager.Mappers;
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
            var sectionConfig = MFDConfigurationSection.GetConfig(Logger);
            Config = sectionConfig.ToModel(Logger);

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
                    FilePath = Path.Combine(Config.FilePath, SelectedModule.ModuleName)
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
        public void GetSelectedDefinition(string moduleName)
        {
            if(string.IsNullOrEmpty(moduleName))
            {
                return;
            }
            Logger.Info($"Configuration requested for {moduleName}");
            SelectedModule = AvailableModules.Where(am => am.ModuleName == moduleName).FirstOrDefault();
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Logger.Info($"Selected module changing...");
            DestroyMFDs();
            try
            {
                var selectedModule = e.AddedItems.Count > 0 ? (ModuleDefinition)e.AddedItems[0] : e.RemovedItems.Count > 0 ? (ModuleDefinition)e.RemovedItems[0] : null;
                Logger.Info($"Module selected {selectedModule?.DisplayName}");
                GetSelectedDefinition(selectedModule?.ModuleName);
                CreateMFDs();
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
            if (!string.IsNullOrEmpty(Config.DefaultConfig))
            {
                Logger.Info($"Loading the default configuration {Config.DefaultConfig}...");
                cbModules.SelectedValue = Config.DefaultConfig;
            }
        }

        private void ConfigurationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DestroyMFDs();
            var sectionConfig = MFDConfigurationSection.GetConfig(Logger);
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

        }

        private void FileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
