using log4net;
using MFDSettingsManager.Extensions;
using System;
using System.Drawing;
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
        /// <param name="useNewMethod">If true then the NEW transparency preserving crop and resize are used</param>
        /// <returns></returns>
        public virtual BitmapSource CropImage(string imagePath, bool useNewMethod = false)
        {
            if (Enabled)
            {
                var imgSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                Int32Rect offSet = new Int32Rect(XOffsetStart, YOffsetStart, XOffsetFinish - XOffsetStart, YOffsetFinish - YOffsetStart);
                if (useNewMethod)
                {
                    var targetImage = Image.FromFile(imgSource.LocalPath);
                    var reSizedImage = Resize(targetImage, new System.Drawing.Size(200, 200));
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = imgSource;
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    SaveImage(src, CacheFolder, $"TEST_X_{XOffsetStart}To{XOffsetFinish}Y_{YOffsetStart}To{YOffsetFinish}_{Opacity}");
                    return src;
                }
                else
                {
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = imgSource;
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    var croppedBitmap = new CroppedBitmap(src, offSet);
                    var noAlphaSource = new FormatConvertedBitmap();
                    noAlphaSource.BeginInit();
                    noAlphaSource.Source = croppedBitmap;
                    noAlphaSource.DestinationFormat = PixelFormats.Bgr24;
                    //noAlphaSource.AlphaThreshold = 0;
                    noAlphaSource.EndInit();
                    SaveImage(noAlphaSource, CacheFolder, $"X_{XOffsetStart}To{XOffsetFinish}Y_{YOffsetStart}To{YOffsetFinish}_{Opacity}");
                    return noAlphaSource;
                }
            }
            else
            {
                Logger?.Warn($"The configuration is disabled, {GetReadableString()}");
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

        private Image MakeImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private static Image Resize(Image image,
            System.Drawing.Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                var originalWidth = image.Width;
                var originalHeight = image.Height;
                var percentWidth = (float)size.Width / (float)originalWidth;
                var percentHeight = (float)size.Height / (float)originalHeight;
                var percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphicsHandle.InterpolationMode =
                           System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        #endregion Private helpers
    }
}
