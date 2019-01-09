﻿using log4net;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Mappers;
using MFDSettingsManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;

namespace MFDisplayConfiguration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Logger for the window
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// The configuration
        /// </summary>
        public ModulesConfiguration Config { get; set; }

        /// <summary>
        /// Currently selected Module
        /// </summary>
        public ModuleDefinition CurrentModule => GetCurrentModule();

        /// <summary>
        /// DataDirty variable
        /// </summary>
        public bool IsDataDirty { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CbModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModuleChanged();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFilePath.TextChanged += TxtFilePath_TextChanged;
            LoadConfig();
        }

        private void TxtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDataDirty = true;
        }

        private ModuleDefinition GetCurrentModule()
        {
            var selectedModuile = (string)cbModules.SelectedValue;
            if (!string.IsNullOrEmpty(selectedModuile))
            {
                return Config.Modules.FirstOrDefault(mod => mod.ModuleName == selectedModuile);
            }
            return null;
        }

        private void LoadConfig()
        {
            var configSection = MFDConfigurationSection.GetConfig();
            Config = ConfigSectionModelMapper.MapFromConfigurationSection(configSection, Logger);
            cbModules.ItemsSource = Config.Modules;
            cbModules.DisplayMemberPath = "DisplayName";
            cbModules.SelectedValuePath = "ModuleName";

            cbDefaultModule.ItemsSource = Config.Modules;
            cbDefaultModule.DisplayMemberPath = "DisplayName";
            cbDefaultModule.SelectedValuePath = "ModuleName";

            txtFilePath.Text = Config.FilePath;
            chkIsValidPath.IsChecked = Directory.Exists(Config.FilePath);
            chkSaveClips.IsChecked = Config.SaveClips;

            if (!string.IsNullOrEmpty(Config.DefaultConfig))
            {
                cbModules.SelectedValue = Config.DefaultConfig;
                cbDefaultModule.SelectedValue = Config.DefaultConfig;
                ModuleChanged();
            }

            IsDataDirty = false;
        }

        private void ModuleChanged()
        {
            var selectedMod = CurrentModule;
            if (selectedMod != null)
            {
                dgConfigurations.ItemsSource = CurrentModule.Configurations;
            }
        }

        private void CbDefaultModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsDataDirty = true;
        }

        private void Save()
        {
            var configSection = MFDConfigurationSection.GetConfig();
            configSection.DefaultConfig = cbDefaultModule.SelectedValue?.ToString();
            configSection.FilePath = txtFilePath.Text;
            configSection.SaveClips = chkSaveClips.IsChecked;
            configSection.CurrentConfiguration.Save();
            LoadConfig();
        }

        private void ConfigurationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch((string) ((MenuItem)sender).CommandParameter)
            {
                case "Revert":
                    CheckDataDirty(() => { LoadConfig(); });
                    break;
                case "Save":
                    Save();
                    LoadConfig();
                    break;
                case "Close":
                    CheckDataDirty(() => { Close(); });
                    break;
            }
        }

        private void CheckDataDirty(Action doAction)
        {
            if (IsDataDirty)
            {
                if (MessageBox.Show("There are change pending, are you sure you want to lose those changes?", "Lose Changes?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            doAction?.Invoke();
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch ((string)((MenuItem)sender).CommandParameter)
            {
                case "About":
                    ShowAbout();
                    break;
            }
        }

        private void ShowAbout()
        {
            MessageBox.Show(this, "Text for about dialog", "About", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void BtnBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            var dlgChooseFolder = new FolderBrowserDialog()
            {
                SelectedPath = Config.FilePath,
                Description = "Select the path where the CTS images are located",
                ShowNewFolderButton = false
            };

            var dlgResult = dlgChooseFolder.ShowDialog();
            switch(dlgResult)
            {
                case System.Windows.Forms.DialogResult.OK:
                    txtFilePath.Text = dlgChooseFolder.SelectedPath;
                    break;
                default:
                    break;
            }
        }
    }
}
