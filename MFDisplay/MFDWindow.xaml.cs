using log4net;
using MFDSettingsManager.Models;
using System;
using System.IO;
using System.Windows;

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
        /// Ctor setups the configuration and loads the MFD
        /// </summary>
        /// <param name="config"></param>
        public MFDWindow()
        {
            InitializeComponent();
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
            Width = Configuration?.Width ?? 1280;
            Height = Configuration?.Height ?? 720;
            Left = Configuration?.Left ?? 0;
            Top = Configuration?.Top ?? 0;
            Opacity = Configuration?.Opacity ?? 1.0;
            Logger.Info($"Creating MFD -> {Configuration.ToReadableString()}");
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
            var filePath = Path.Combine(FilePath, Configuration.FileName);
            var fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                imgMain.Width = Width;
                imgMain.Height = Height;
                imgMain.Source = Configuration.CropImage(filePath);
                IsMFDLoaded = true;
                Logger.Debug($"Image {fi.FullName} is loaded");
            }
            else
            {
                Logger.Error($"File {fi.FullName} was NOT found!");
                Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Logger.Info($"MFD Is Closed -> {Configuration.ToReadableString()}");
        }

    }
}
