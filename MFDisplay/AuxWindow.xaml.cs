using log4net;
using MFDSettingsManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace MFDisplay
{
    /// <summary>
    /// Each MFD is encapsulated in this class
    /// </summary>
    public partial class AuxWindow : Window
    {
        /// <summary>
        /// Is the image specified loaded?
        /// </summary>
        public bool IsWindowLoaded { get; protected set; }

        /// <summary>
        /// The name of the sub configuration to load
        /// </summary>
        public string SubConfigurationName { get; set; }

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
        public AuxWindow()
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
        public bool InitializeWindow(ConfigurationDefinition definition)
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
            InitializeWindow(Configuration);

            if(Opacity == 0)
            {
                Logger?.Info($"Skipped {Configuration.ToReadableString()}...");
                Close();
                return;
            }
            if (Configuration.Enabled)
            {
                LoadImage();
                Logger?.Debug($"Loading the configuration for {Configuration.Name} from Module {Configuration.ModuleName}");
            }
            else
            {
                Logger?.Info($"Configuration for {Configuration.Name} for Module {Configuration.ModuleName} is currently disabled in configuration.");
            }
        }

        /// <summary>
        /// Loads the specified image, logs failure and closes self if it fails to load
        /// </summary>
        public void LoadImage()
        {
            SubConfigurationDefinition subConfig = null;
            IsWindowLoaded = false;
            if (!IsValid)
            {
                Logger?.Error($"The configuration {Configuration.ToReadableString()} cannot be loaded using the full path: {Path.Combine(FilePath, Configuration.FileName)}.");
                Close();
            }
            else
            {
                var cacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"Vyper Industries\\MFD4CTS\\cache\\{Configuration.ModuleName}");

                // Previous renders are cached so if the cache file is available then it will be used
                var imagePrefix = $"X_{Configuration.XOffsetStart}To{Configuration.XOffsetFinish}Y_{Configuration.YOffsetStart}To{Configuration.YOffsetFinish}_{Configuration.Opacity}";
                var cacheFile = Path.Combine(cacheFolder, $"{imagePrefix}_{Configuration.Name}_{Configuration.Width}_{Configuration.Height}.png");
                imgMain.Source = GetBitMapSource(Configuration, cacheFolder, cacheFile);
                imgMain.Width = Width;
                imgMain.Height = Height;
                imgMain.Visibility = Visibility.Visible;

                try
                {
                    // First check to see if we are using a SubConfiguration
                    if (!string.IsNullOrEmpty(SubConfigurationName))
                    {
                        subConfig = Configuration?.SubConfigurations?.FirstOrDefault(sc => sc.Name.Equals(SubConfigurationName, StringComparison.InvariantCultureIgnoreCase));
                        if (subConfig != null)
                        {
                            Logger?.Info($"Processing sub-configuration: {subConfig.ToString()} for Module: {Configuration.ModuleName}!");
                            var insetImagePrefix = $"X_{subConfig.XOffsetStart}To{subConfig.XOffsetFinish}Y_{subConfig.YOffsetStart}To{subConfig.YOffsetFinish}_{subConfig.Opacity}";
                            var width = subConfig.EndX - subConfig.StartX;
                            var height = subConfig.EndY - subConfig.StartY;
                            var insetCacheFile = Path.Combine(cacheFolder, $"{insetImagePrefix}_{subConfig.Name}_{width}_{height}.png");
                            imgInsert.Source = GetBitMapSource(subConfig, cacheFolder, insetCacheFile);
                            imgInsert.Width = width;
                            imgInsert.Height = height;
                            imgInsert.Opacity = subConfig.Opacity;
                            imgInsert.Visibility = Visibility.Visible;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger?.Error($"Unable to load {subConfig?.ToReadableString()}.", ex);
                    Close();
                }
                IsWindowLoaded = true;
            }
        }

        /// <summary>
        /// Loads the image from the cache or crops it from the source image
        /// </summary>
        /// <typeparam name="T">Must be derived from <seealso cref="ConfigurationModelBase"/></typeparam>
        /// <param name="configSource">The configuration source, derived from <seealso cref="ConfigurationModelBase"/> </param>
        /// <param name="cacheFolder">Location of the cache of images</param>
        /// <param name="cacheFile">Full path to the requested file in the cache</param>
        /// <returns><seealso cref="BitmapSource"/></returns>
        private BitmapSource GetBitMapSource<T>(T configSource, string cacheFolder, string cacheFile)
            where T: ConfigurationModelBase
        {
            BitmapSource bitmapSource = null;
            string filePath = string.Empty;

            try
            {
                if (File.Exists(cacheFile))
                {
                    Logger.Info($"Cache file found: {cacheFile}");
                    Stream imageStreamSource = new FileStream(cacheFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                    PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    bitmapSource = decoder.Frames[0];
                }
                else
                {
                    if (!Directory.Exists(cacheFolder))
                    {
                        Directory.CreateDirectory(cacheFolder);
                    }
                    filePath = Path.Combine(FilePath, configSource.FileName);
                    bitmapSource = configSource.CropImage(filePath, false);
                }
            }
            catch (Exception ex)
            {
                Logger?.Error($"Unable to load {configSource.ToReadableString()}.", ex);
            }
            finally
            {
                if (bitmapSource != null)
                {
                    Logger?.Info($"Configuration {configSource.ToReadableString()} is loaded.");
                }
                else
                {
                    Logger?.Warn($"Configuration was not loaded -> {configSource.ToReadableString()}.");
                    Close();
                }
            }

            return bitmapSource;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Logger?.Info($"Window Is Closed -> {Configuration.Name}.");
        }

        private void AuxWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            imgMain = null;
        }
    }
}
