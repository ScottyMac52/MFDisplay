﻿using log4net;
using MFDisplay.Extenisons;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Extensions;
using MFDSettingsManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using Image = System.Windows.Controls.Image;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace MFDisplay
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        /// <summary>
        /// If true then editing is allowed
        /// </summary>
        public static bool EDIT_ACTIVE = false;

        /// <summary>
        /// Logger for the window
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// The configuration
        /// </summary>
        public ModulesConfiguration Config { get; private set; }

        /// <summary>
        /// Currently selected Module
        /// </summary>
        public ModuleDefinition CurrentModule => GetCurrentModule();

        /// <summary>
        /// DataDirty variable
        /// </summary>
        public bool IsDataDirty { get; private set; }

        /// <summary>
        /// The current list of preview windows
        /// </summary>
        public List<MFDWindow> PreviewWindows { get; private set; }

        /// <summary>
        /// Is the configuration Valid?
        /// </summary>
        public bool IsValidConfig => !string.IsNullOrEmpty(Config?.FilePath) ? Directory.Exists(Config.FilePath) : false;

        /// <summary>
        /// Gets the currently selected definition
        /// </summary>
        public ConfigurationDefinition CurrentConfiguration => GetCurrentDefintion();

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
            if(!LoadConfig())
            {
                ((MFDisplayApp)Application.Current).ShowConfigurationError(this);
                Close();
            }

            PreviewWindows = new List<MFDWindow>();
        }

        private void LoadConfigurationSectionAsModel()
        {
            var configSection = MFDConfigurationSection.GetConfig(Logger);
            Config = configSection?.ToModel(Logger);
        }

        private bool LoadConfig()
        {
            LoadConfigurationSectionAsModel();
            if(Config == null)
            {
                return false;
            }
            cbModules.ItemsSource = Config?.Modules;
            cbModules.DisplayMemberPath = "DisplayName";
            cbModules.SelectedValuePath = "ModuleName";

            cbDefaultModule.ItemsSource = Config?.Modules;
            cbDefaultModule.DisplayMemberPath = "DisplayName";
            cbDefaultModule.SelectedValuePath = "ModuleName";

            txtFilePath.Text = Config?.FilePath;
            chkIsValidPath.IsChecked = IsValidConfig;
            chkSaveClips.IsChecked = Config?.SaveClips;

            if (!string.IsNullOrEmpty(Config?.DefaultConfig))
            {
                cbModules.SelectedValue = Config?.DefaultConfig;
                cbDefaultModule.SelectedValue = Config?.DefaultConfig;
                ModuleChanged();
            }

            IsDataDirty = false;
            return true;
        }


        private void CbDefaultModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsDataDirty = true;
        }

        private void Save()
        {
            var savedConfig = MFDConfigurationSection.GetConfig(Logger);
            savedConfig.FilePath = txtFilePath.Text;
            savedConfig.SaveClips = chkSaveClips.IsChecked;
            savedConfig.DefaultConfig = cbDefaultModule.SelectedValue?.ToString();
            Logger.Info($"Saving the configuration file {savedConfig.CurrentConfiguration.FilePath}...");
            // TODO: Update the Configuration Section from the Model
            try
            {
                savedConfig.CurrentConfiguration.Save(ConfigurationSaveMode.Modified, true);
            }
            catch (System.Exception ex)
            {
                Logger.Error($"Unable to save the configuration to {savedConfig.CurrentConfiguration.FilePath}", ex);
            }
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
                    IsDataDirty = true;
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

        private ConfigurationDefinition GetCurrentDefintion()
        {
            return dgConfigurations.CurrentItem as ConfigurationDefinition;
        }

        private void ModuleChanged()
        {
            var selectedMod = CurrentModule;
            if (selectedMod != null)
            {
                PreviewWindows?.ForEach(pw =>
                {
                    if(pw.IsLoaded)
                    {
                        pw.Close();
                    }
                });
                PreviewWindows?.Clear();
                dgConfigurations.ItemsSource = CurrentModule.Configurations;
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

        private void DgConfigurations_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var currentDef = CurrentConfiguration;
            var txtControl = e.EditingElement as System.Windows.Controls.TextBox;
            var bindingExpression = (BindingExpression)txtControl.BindingGroup.BindingExpressions.FirstOrDefault(be => be.Target == txtControl);
            var propertyName = bindingExpression.ResolvedSourcePropertyName;
            var propertyInfo = currentDef.GetType().GetProperties(System.Reflection.BindingFlags.Public).SingleOrDefault(prop => prop.Name == propertyName);

            if (!EDIT_ACTIVE)
            {
                var oldValue = propertyInfo.GetValue(currentDef);
                ((System.Windows.Controls.TextBox)e.EditingElement).Text = oldValue?.ToString();
                e.Cancel = true;
                return;
            }

            var screens = Screen.AllScreens;
            var screen = this.GetScreen();
            var mfdConfiguration = e.Row.DataContext as ConfigurationDefinition;

            if(int.TryParse(txtControl.Text, out int newValue))
            {
                var checkParameter = propertyName.ToLower();
                switch (checkParameter)
                {
                    case "left":
                        {
                            var bounds = (screen.Bounds.X + screen.Bounds.Width);
                            var validStart = screen.Bounds.X;

                            if (newValue > bounds || newValue <= screen.Bounds.X)
                            {
                                MessageBox.Show($"The coordinate {newValue} is not valid, has to be between {validStart} and {bounds}", $"Invalid Coordinate for {checkParameter}", MessageBoxButton.OK);
                                e.Cancel = true;
                                return;
                            }
                        }
                        break;

                    case "top":
                        {
                            var bounds = (screen.Bounds.Y + screen.Bounds.Height);
                            var validStart = screen.Bounds.Y;
                            if (newValue > bounds || newValue <= screen.Bounds.Y)
                            {
                                MessageBox.Show($"The coordinate {newValue} is not valid, has to be between {validStart} and {bounds}", $"Invalid Coordinate {checkParameter}", MessageBoxButton.OK);
                                e.Cancel = true;
                                return;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            IsDataDirty = true;
        }

        /// <summary>
        /// Processes a request to preview a configuration
        /// </summary>
        /// <param name="button"></param>
        public void ProcessPreviewRequest(Button button)
        {
            var currentPreview = PreviewWindows.FirstOrDefault(pw => pw.Tag.Equals(button.Tag));
            if (currentPreview == null)
            {
                var currentDef = CurrentConfiguration;
                var newPreview = new MFDWindow()
                {
                    Logger = Logger,
                    Owner = this,
                    Configuration = currentDef,
                    FilePath = Config.FilePath,
                    Tag = button.Tag,
                    AllowsTransparency = false,
                    WindowState = WindowState.Normal,
                    WindowStyle = WindowStyle.ToolWindow,
                    ResizeMode = ResizeMode.CanResizeWithGrip
                };
                newPreview.Show();
                var screens = Screen.AllScreens;
                var rectPosition = new Rectangle(new Point((int)newPreview.Left, (int)newPreview.Top), new Size((int)newPreview.Width, (int)newPreview.Height));
                var matchingScreen = screens.FirstOrDefault(screen => rectPosition.IntersectsWith(screen.WorkingArea));

                if (matchingScreen == null)
                {
                    MessageBox.Show($"The preview cannot be displayed using the current configuration of {currentDef.ToReadableString()}", "Preview error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    Logger.Error($"Unable to preview, configuration {currentDef.ToReadableString()}.");
                    newPreview.Close();
                }
                else
                {
                    Logger.Info($"Preview of {currentDef.Name} for Module {currentDef.ModuleName} is started.");
                    button.Content = "Close";
                    newPreview.Closed += NewPreview_Closed;
                    PreviewWindows.Add(newPreview);
                }
            }
            else
            {
                currentPreview.Closed -= NewPreview_Closed;
                if (currentPreview.IsLoaded)
                {
                    currentPreview.Close();
                }
                button.Content = "Preview";
                Logger.Info($"Preview of {currentPreview.Tag} is finished.");
                PreviewWindows.Remove(currentPreview);
            }
        }

        private void NewPreview_Closed(object sender, EventArgs e)
        {
            var windowClosed = sender as MFDWindow;
            Logger.Debug($"User closed {windowClosed.Tag}");
        }

        /// <summary>
        /// Allows any of the configurations to be previewed and checks the coordinates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ProcessPreviewRequest(button);
        }
    }
}
