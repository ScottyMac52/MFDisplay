using log4net;
using MFDSettingsManager.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace MFDisplay
{
    /// <summary>
    /// Each MFD is encapsulated in this class
    /// </summary>
    public partial class MFDWindow : Window
    {
        /// <summary>
        /// Is the image specified loaded?
        /// </summary>
        public bool IsMFDLoaded { get; protected set; }

        /// <summary>
        /// MFD Configuration
        /// </summary>
        public ConfigurationDefinition Configuration { get; set; }

        /// <summary>
        /// Logger
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// The filepath to look for the image files
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Ctor 
        /// </summary>
        public MFDWindow()
        {
            InitializeComponent();
        }

        private bool IsValid => CheckForValidConfig();

        private bool CheckForValidConfig()
        {
            return Directory.Exists(FilePath) 
                    && !string.IsNullOrEmpty(Configuration?.FileName) 
                    && File.Exists(System.IO.Path.Combine(FilePath, Configuration.FileName)
            );
        }

        /// <summary>
        ///  Uses the Configuration to set the properties for this MFD
        /// </summary>
        /// <returns></returns>
        public bool InitializeMFD(ConfigurationDefinition definition)
        {
            Logger.Debug($"Loading the configuration for {definition.Name}");
            Configuration = definition;
            Title = definition?.Name;
            ResizeMode = ResizeMode.NoResize;
            Width = Configuration?.Width ?? 0;
            Height = Configuration?.Height ?? 0;
            Left = Configuration?.Left ?? 0;
            Top = Configuration?.Top ?? 0;
            Opacity = Configuration?.Opacity ?? 1.0;
            return true;
        }

        /// <summary>
        /// Loads the assigned image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeMFD(Configuration);
            LoadImage();
        }

        /// <summary>
        /// Loads the specified image, logs failure and closes self if it fails to load
        /// </summary>
        public void LoadImage()
        {
            IsMFDLoaded = false;
            if (!IsValid)
            {
                Logger.Error($"The configuration {Configuration.ToReadableString()} cannot be loaded using the FilePath: {FilePath}.");
                Close();
            }
            else
            {
                var filePath = System.IO.Path.Combine(FilePath, Configuration.FileName);
                imgMain.Width = Width;
                imgMain.Height = Height;
                imgMain.Source = Configuration.CropImage(filePath);
                IsMFDLoaded = true;
                Logger.Debug($"Image {filePath} is loaded at ({Left}, {Top}) for ({Width}, {Height}) Opacity: {Opacity}");
            }
        }

        /// <summary>
        /// Toggles the visibility of the Close check box on the window
        /// </summary>
        /// <param name="showCheckbox"></param>
        public void ToggleCloseCheckBox(bool showCheckbox = false)
        {
            chkCloseAndSave.Visibility = showCheckbox ? Visibility.Visible : Visibility.Hidden;
            chkCloseAndSave.UpdateLayout();
        }

        #region ResizeWindows
        bool ResizeInProcess = false;
        private void Resize_Init(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = true;
                senderRect.CaptureMouse();
            }
        }

        private void Resize_End(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = false; ;
                senderRect.ReleaseMouseCapture();
            }
        }

        private void Resizeing_Form(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess)
            {
                Rectangle senderRect = sender as Rectangle;
                Window mainWindow = senderRect.Tag as Window;
                if (senderRect != null)
                {
                    double width = e.GetPosition(mainWindow).X;
                    double height = e.GetPosition(mainWindow).Y;
                    senderRect.CaptureMouse();
                    if (senderRect.Name.ToLower().Contains("right"))
                    {
                        width += 5;
                        if (width > 0)
                            mainWindow.Width = width;
                    }
                    if (senderRect.Name.ToLower().Contains("left"))
                    {
                        width -= 5;
                        mainWindow.Left += width;
                        width = mainWindow.Width - width;
                        if (width > 0)
                        {
                            mainWindow.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("bottom"))
                    {
                        height += 5;
                        if (height > 0)
                            mainWindow.Height = height;
                    }
                    if (senderRect.Name.ToLower().Contains("top"))
                    {
                        height -= 5;
                        mainWindow.Top += height;
                        height = mainWindow.Height - height;
                        if (height > 0)
                        {
                            mainWindow.Height = height;
                        }
                    }
                }
            }
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            Logger.Info($"MFD Is Closed -> {Configuration.ToReadableString()}");
        }

        private void ChkCloseAndSave_Click(object sender, RoutedEventArgs e)
        {
            Logger.Debug($"MFD Is Closing -> {Configuration.ToReadableString()}");
            Close();
        }
    }
}
