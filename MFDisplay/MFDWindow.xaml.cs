using log4net;
using MFDSettingsManager.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

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
            Visibility = Visibility.Hidden;
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

            if(Opacity == 0)
            {
                Logger?.Info($"Skipped {Configuration.ToReadableString()}...");
                Close();
                return;
            }

            LoadImage();
            Logger?.Debug($"Loading the configuration for {Configuration.Name} from Module {Configuration.ModuleName}");
        }

        /// <summary>
        /// Loads the specified image, logs failure and closes self if it fails to load
        /// </summary>
        public void LoadImage()
        {
            BitmapSource bitmapSource = null;
            IsMFDLoaded = false;
            if (!IsValid)
            {
                Logger?.Error($"The configuration {Configuration.ToReadableString()} cannot be loaded using the full path: {Path.Combine(FilePath, Configuration.FileName)}.");
                Close();
            }
            else
            {
                var filePath = Path.Combine(FilePath, Configuration.FileName);
                imgMain.Width = Width;
                imgMain.Height = Height;
                try
                {
                    bitmapSource = Configuration.CropImage(filePath);
                }
                catch (Exception ex)
                {
                    Logger?.Error($"Unable to load {Configuration.ToReadableString()}.", ex);
                    Close();
                }
                finally
                {
                    if(bitmapSource != null)
                    {
                        imgMain.Source = bitmapSource;
                        IsMFDLoaded = true;
                        Logger?.Debug($"Image {filePath} is loaded.");
                        Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Logger?.Info($"MFD Is Closed -> {Configuration.Name}.");
        }
    }
}
