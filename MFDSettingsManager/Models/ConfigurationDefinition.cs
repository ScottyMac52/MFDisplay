using log4net;
using MFDSettingsManager.Enum;
using MFDSettingsManager.Extensions;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MFDSettingsManager.Models
{
    /// <summary>
    /// Single Configuration for a Module
    /// </summary>
    public class ConfigurationDefinition 
    {
        /// <summary>
        /// Logger
        /// </summary>
        public ILog Logger { get; set; }

        #region Identifying properties
        /// <summary>
        /// The parent Module to this Configuration
        /// </summary>
        public ModuleDefinition Parent { get; set; }
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

        #endregion Identifying properties

        #region Basic Image Properties Left, Top, Width, Height and Opacity
        /// <summary>
        /// Translucency of the image expressed as percentage of solidness 
        /// </summary>
        public float Opacity { get; set; }
        /// <summary>
        /// Width of the displayed image
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The Height of the displayed image
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Left coordinate of the displayed image
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// Top coordinate of the displayed image
        /// </summary>
        public int Top { get; set; }

        #endregion Basic Image Properties Left, Top, Width, Height and Opacity

        #region Image cropping properties
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
        /// <summary>
        /// If true then cropping is used otherwise the entire image is used
        /// </summary>
        public bool? UseOffsets { get; set; }
        /// <summary>
        /// If true then the results of the cropping are saved
        /// </summary>
        public bool? SaveResults { get; set; }
        /// <summary>
        /// The type to use when saving images
        /// </summary>
        public SavedImageType? ImageType => Parent.ImageType ?? SavedImageType.Bmp;
        #endregion Image cropping properties

        #region Public methods

        /// <summary>
        /// Crop the specified image
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public BitmapSource CropImage(string imagePath)
        {
            BitmapSource retResult = null;
            Int32Rect offSet;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(imagePath, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            if ((UseOffsets ?? false) == true)
            {
                offSet = new Int32Rect(XOffsetStart, YOffsetStart, XOffsetFinish - XOffsetStart, YOffsetFinish - YOffsetStart);
                var croppedBitmap = new CroppedBitmap(src, offSet);
                if ((SaveResults ?? false) == true)
                {
                    var fi = new FileInfo(imagePath);

                    SaveImage(croppedBitmap, fi.Directory.FullName, "Before");
                }
                var bitmap = croppedBitmap.BitmapImage2Bitmap();
                bitmap = bitmap.Crop();
                retResult = bitmap.ToBitmapSource();
                if ((SaveResults ?? false) == true)
                {
                    var fi = new FileInfo(imagePath);
                    SaveImage(retResult, fi.Directory.FullName, "After");
                }
            }
            else
            {
                retResult = src;
            }


            return retResult;
        }

        /// <summary>
        /// Describes the Configuration
        /// </summary>
        /// <returns></returns>
        public string ToReadableString()
        {
            return $"{Name} at ({Left}, {Top}) for ({Width}, {Height}) with Opacity {Opacity} from {FileName} at ({XOffsetStart}, {YOffsetStart}) for ({XOffsetFinish - XOffsetStart}, {YOffsetFinish - YOffsetStart}).";
        }

        #endregion Public methods

        #region Private helpers

        /// <summary>
        /// Saves the Image in the specified format, default format is bmp
        /// </summary>
        /// <param name="retResult"><seealso cref="BitmapSource"/></param>
        /// <param name="filePath">Directory to save the file to</param>
        /// <param name="imagePrefix">Prefix to add to the filename</param>
        private void SaveImage(BitmapSource retResult, string filePath, string imagePrefix)
        {
            switch (ImageType)
            {
                case SavedImageType.Jpeg:
                    {
                        var fileName = Path.Combine(filePath, $"{imagePrefix}_{ModuleName}_{Name}_{Width}_{Height}.jpg");
                        retResult?.SaveToJpeg(fileName);
                        Logger.Debug($"Cropped image saved as {fileName}.");
                    }
                    break;
                case SavedImageType.Png:
                    {
                        var fileName = Path.Combine(filePath, $"{imagePrefix}_{ModuleName}_{Name}_{Width}_{Height}.png");
                        retResult?.SaveToPng(fileName);
                        Logger.Debug($"Cropped image saved as {fileName}.");
                    }
                    break;
                default:
                    {
                        var fileName = Path.Combine(filePath, $"{imagePrefix}_{ModuleName}_{Name}_{Width}_{Height}.bmp");
                        retResult?.SaveToBmp(fileName);
                        Logger.Debug($"Cropped image saved as {fileName}.");
                    }
                    break;
            }
        }

        #endregion Private helpers
    }
}