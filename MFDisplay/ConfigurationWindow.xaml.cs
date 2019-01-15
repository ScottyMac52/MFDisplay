using log4net;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Mappers;
using MFDSettingsManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;

namespace MFDisplay
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
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

        /// <summary>
        /// Is the configuration Valid?
        /// </summary>
        public bool IsValidConfig => !string.IsNullOrEmpty(Config?.FilePath) ? Directory.Exists(Config.FilePath) : false;

        /// <summary>
        /// Ctor
        /// </summary>
        public ConfigurationWindow()
        {
            InitializeComponent();
        }


        private void TxtFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDataDirty = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtFilePath.TextChanged += TxtFilePath_TextChanged;
            LoadConfig();
        }

        private void LoadConfig()
        {
            var configSection = MFDConfigurationSection.GetConfig(Logger);
            Config = ConfigSectionModelMapper.MapFromConfigurationSection(configSection, Logger);

            cbModules.ItemsSource = Config.Modules;
            cbModules.DisplayMemberPath = "DisplayName";
            cbModules.SelectedValuePath = "ModuleName";

            cbDefaultModule.ItemsSource = Config.Modules;
            cbDefaultModule.DisplayMemberPath = "DisplayName";
            cbDefaultModule.SelectedValuePath = "ModuleName";

            txtFilePath.Text = Config.FilePath;
            chkIsValidPath.IsChecked = IsValidConfig;
            chkSaveClips.IsChecked = Config.SaveClips;

            if (!string.IsNullOrEmpty(Config.DefaultConfig))
            {
                cbModules.SelectedValue = Config.DefaultConfig;
                cbDefaultModule.SelectedValue = Config.DefaultConfig;
                ModuleChanged();
            }

            IsDataDirty = false;
        }


        private void CbDefaultModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsDataDirty = true;
        }

        private void Save()
        {
            var configSection = MFDConfigurationSection.GetConfig(Logger);
            configSection.DefaultConfig = cbDefaultModule.SelectedValue?.ToString();
            configSection.FilePath = txtFilePath.Text;
            configSection.SaveClips = chkSaveClips.IsChecked;
            Logger.Info($"Saving the configuration file {configSection.CurrentConfiguration.FilePath}...");
            configSection.CurrentConfiguration.Save();
        }

        private void ConfigurationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch ((string)((MenuItem)sender).CommandParameter)
            {
                case "Revert":
                    CheckDataDirty(() => { LoadConfig(); }, "reloads the configuration and editing continues.");
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

        private void CheckDataDirty(Action doAction, string yesText = "exits and loses changes.")
        {
            if (IsDataDirty)
            {
                if (MessageBox.Show($"There are changes pending, are you sure you want to lose those changes? Pressing Yes: {yesText} Pressing No: resumes editing.", "Lose Changes?", MessageBoxButton.YesNo, MessageBoxImage.Hand) != MessageBoxResult.Yes)
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
            switch (dlgResult)
            {
                case System.Windows.Forms.DialogResult.OK:
                    txtFilePath.Text = dlgChooseFolder.SelectedPath;
                    break;
                default:
                    break;
            }
        }

        private void ChkSaveClips_Click(object sender, RoutedEventArgs e)
        {
            IsDataDirty = true;
        }

        private void CbModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModuleChanged();
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


        private void ModuleChanged()
        {
            var selectedMod = CurrentModule;
            if (selectedMod != null)
            {
                dgConfigurations.ItemsSource = CurrentModule.Configurations;
            }
        }

        /// <summary>
        /// Provides for the ability to preview a window configuration and change it on the fly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewClick(object sender, RoutedEventArgs e)
        {
            if(!IsValidConfig)
            {
                MessageBox.Show("The configuration is not valid for preview");
                e.Handled = true;
            }

            var btn = sender as System.Windows.Controls.Button;
            var config = ((List<ConfigurationDefinition>)dgConfigurations.ItemsSource).SingleOrDefault(currentConfig => currentConfig.Name == (string)btn.CommandParameter);

            var mfdWindow = new MFDWindow()
            {
                Logger = Logger,
                Configuration = config,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.None,
                Owner = this,
                FilePath = Path.Combine(Config.FilePath, config.ModuleName),
                BorderThickness = new Thickness(5.0F),
                AllowsTransparency = false
            };

            mfdWindow.imgMain.MouseDown += ImgMain_MouseDown;
            mfdWindow.ToggleCloseCheckBox(true);
            mfdWindow.ShowDialog();
            mfdWindow.ToggleCloseCheckBox(false);

            config.Left = (int)mfdWindow.Left;
            config.Top = (int)mfdWindow.Top;
            config.Width = (int)mfdWindow.Width;
            config.Height = (int)mfdWindow.Height;

            ModuleConfigurationDefintion selectedModule = null;
            MFDDefintion selectedConfig = null;

            var configSection = MFDConfigurationSection.GetConfig(Logger);
            var moduleIterator = configSection.Modules.GetEnumerator();
            while (moduleIterator.MoveNext())
            {
                var currentModule = (ModuleConfigurationDefintion)moduleIterator.Current;
                if (currentModule.ModuleName == config.ModuleName)
                {
                    selectedModule = currentModule;
                    break;
                }
            }

            if (selectedModule != null)
            {
                var configIterator = selectedModule.MFDConfigurations.GetEnumerator();
                while (configIterator.MoveNext())
                {
                    var currentConfig = (MFDDefintion)configIterator.Current;
                    if (currentConfig.Name == config.Name)
                    {
                        selectedConfig = currentConfig;
                        selectedConfig.Top = config.Top;
                        selectedConfig.Width = config.Width;
                        selectedConfig.Height = config.Height;
                        selectedConfig.Left = config.Left;
                        break;
                    }
                }
                configSection.CurrentConfiguration.Save();
                LoadConfig();
            }
        }

        private void ImgMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var targetImage = sender as Image;
            var targetWindow = FindParent<Window>(targetImage, "mfdWindow");

            if (targetWindow != null)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    targetWindow.DragMove();
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        /// <summary>
        /// Recursively finds the specified named parent in a control hierarchy
        /// </summary>
        /// <typeparam name="T">The type of the targeted Find</typeparam>
        /// <param name="child">The child control to start with</param>
        /// <param name="parentName">The name of the parent to find</param>
        /// <returns></returns>
        private static T FindParent<T>(DependencyObject child, string parentName)
            where T : DependencyObject
        {
            if (child == null) return null;

            T foundParent = null;
            var currentParent = VisualTreeHelper.GetParent(child);

            do
            {
                var frameworkElement = currentParent as FrameworkElement;
                if(frameworkElement.Name == parentName && frameworkElement is T)
                {
                    foundParent = (T) currentParent;
                    break;
                }

                currentParent = VisualTreeHelper.GetParent(currentParent);

            } while (currentParent != null);

            return foundParent;
        }


        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        private static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
