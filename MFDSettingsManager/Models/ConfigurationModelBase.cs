using log4net;
using MFDSettingsManager.Extensions;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MFDSettingsManager.Models
{
    /// <summary>
    /// Properties common to Configuration and SubConfiguration
    /// </summary>
    public abstract class ConfigurationModelBase
    {
        #region Ctor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigurationModelBase()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="cb"></param>
        public ConfigurationModelBase(ConfigurationModelBase cb)
        {
            Enabled = cb.Enabled;
            Logger = cb.Logger;
            FileName = cb.FileName;
            ModuleName = cb.ModuleName;
            Name = cb.Name;
            XOffsetStart = cb.XOffsetStart;
            XOffsetFinish = cb.XOffsetFinish;
            YOffsetStart = cb.YOffsetStart;
            YOffsetFinish = cb.YOffsetFinish;
            Opacity = cb.Opacity;
        }

        #endregion Ctor

        #region Utility properties

        /// <summary>
        /// Logger
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// Folder used to cache the generated images
        /// </summary>
        public string CacheFolder => GetCacheFolderForModule();

        #endregion Utility properties

        #region Identifying properties

        /// <summary>
        /// Module Name
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Name of the Configuration
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// FileName for the image cropping that this configuration uses
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The starting file path for the configuration
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Determines if the configuration is rendered
        /// </summary>
        public bool Enabled { get; internal set; }

        /// <summary>
        /// Default readable string
        /// </summary>
        /// <returns></returns>
        protected virtual string GetReadableString()
        {
            return "";
        }

        #endregion Identifying properties

        #region Image cropping properties

        /// <summary>
        /// Translucency of the image expressed as percentage of solidness 
        /// </summary>
        public float Opacity { get; set; }
        /// <summary>
        /// Starting X position of the Crop
        /// </summary>
        public int XOffsetStart { get; set; }
        /// <summary>
        /// Starting Y position of the Crop
        /// </summary>
        public int YOffsetStart { get; set; }
        /// <summary>
        /// Ending X position of the Crop
        /// </summary>
        public int XOffsetFinish { get; set; }
        /// <summary>
        /// Ending Y position of the Crop
        /// </summary>
        public int YOffsetFinish { get; set; }

        #endregion Image cropping properties

        #region Image cropping 

        /// <summary>
        /// Crop the specified image
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public virtual BitmapSource CropImage(string imagePath)
        {
            if (Enabled)
            {
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(imagePath, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();

                Int32Rect offSet = new Int32Rect(XOffsetStart, YOffsetStart, XOffsetFinish - XOffsetStart, YOffsetFinish - YOffsetStart);
                CroppedBitmap croppedBitmap = new CroppedBitmap(src, offSet);
                var noAlphaSource = new FormatConvertedBitmap();
                noAlphaSource.BeginInit();
                noAlphaSource.Source = croppedBitmap;
                noAlphaSource.DestinationFormat = PixelFormats.Bgr24;
                noAlphaSource.AlphaThreshold = 0;
                noAlphaSource.EndInit();
                SaveImage(noAlphaSource, CacheFolder, $"X_{XOffsetStart}To{XOffsetFinish}Y_{YOffsetStart}To{YOffsetFinish}_{Opacity}");
                return noAlphaSource;
            }
            else
            {
                return null;
            }
        }

        #endregion Image cropping

        #region Image saving

        /// <summary>
        /// Saves the Image in the specified format, default format is bmp
        /// </summary>
        /// <param name="retResult"><seealso cref="BitmapSource"/></param>
        /// <param name="filePath">Directory to save the file to</param>
        /// <param name="imagePrefix">Prefix to add to the filename</param>
        protected virtual void SaveImage(BitmapSource retResult, string filePath, string imagePrefix)
        {
            Directory.CreateDirectory(filePath);
            var fileName = Path.Combine(filePath, $"{imagePrefix}_{Name}_{GetReadableString()}.png");
            retResult?.SaveToPng(fileName);
            Logger.Info($"Cropped image saved as {fileName}.");
        }


        #endregion Image saving

        #region Public overrides

        /// <summary>
        /// Describes the Configuration
        /// </summary>
        /// <returns></returns>
        public string ToReadableString()
        {
            string completePath = null;
            string fileStatus = null;
            var pathExists = false;
            if (!string.IsNullOrEmpty(FilePath) && Directory.Exists(FilePath) && !string.IsNullOrEmpty(FileName))
            {
                completePath = Path.Combine(FilePath, FileName);
                pathExists = File.Exists(completePath);
                fileStatus = pathExists ? "found " : "not found ";
            }

            return $"Config: {Name} for: {ModuleName} at: {GetReadableString()} with Opacity: {Opacity} Enabled: {Enabled} from: {completePath ?? "Unknown Image"} was: {fileStatus} at Offset: ({XOffsetStart}, {YOffsetStart}) for: ({XOffsetFinish - XOffsetStart}, {YOffsetFinish - YOffsetStart}).";
        }

        /// <summary>
        /// Short form
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToReadableString();
        }

        #endregion Public overrides

        #region Private helpers

        /// <summary>
        /// Returns the location of the Cache for the current Module
        /// </summary>
        /// <returns></returns>
        private string GetCacheFolderForModule()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"Vyper Industries\\MFD4CTS\\cache\\{ModuleName}");
        }

        #endregion Private helpers
    }
}
