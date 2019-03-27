﻿using log4net;
using MFDSettingsManager.Enum;
using MFDSettingsManager.Extensions;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
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
        /// If true then the results of the cropping are saved
        /// </summary>
        public bool? SaveResults { get; set; }
        #endregion Image cropping properties

        #region Public methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigurationDefinition()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="dc"></param>
        public ConfigurationDefinition(ConfigurationDefinition dc)
        {
            Logger = dc.Logger;
            FileName = dc.FileName;
            ModuleName = dc.ModuleName;
            Left = dc.Left;
            Top = dc.Top;
            Width = dc.Width;
            Height = dc.Height;
            Name = dc.Name;
            XOffsetStart = dc.XOffsetStart;
            XOffsetFinish = dc.XOffsetFinish;
            YOffsetStart = dc.YOffsetStart;
            YOffsetFinish = dc.YOffsetFinish;
            SaveResults = dc.SaveResults;
            Opacity = dc.Opacity;
        }

        /// <summary>
        /// Crop the specified image
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public BitmapSource CropImage(string imagePath)
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
                       
            if ((SaveResults ?? false) == true)
            {
                var fi = new FileInfo(imagePath);

                SaveImage(noAlphaSource, fi.Directory.FullName, $"X_{XOffsetStart}To{XOffsetFinish}Y_{YOffsetStart}To{YOffsetFinish}_{Opacity}");
            }
            return noAlphaSource;
        }

        /// <summary>
        /// Describes the Configuration
        /// </summary>
        /// <returns></returns>
        public string ToReadableString()
        {
            string completePath = null;
            string fileStatus = null;
            var pathExists = false;
            if(!string.IsNullOrEmpty(FilePath) && Directory.Exists(FilePath) && !string.IsNullOrEmpty(FileName))
            {
                completePath = Path.Combine(FilePath, FileName);
                pathExists = File.Exists(completePath);
                fileStatus = pathExists ? "found " : "not found ";
            }
            return $"Config {Name} for {ModuleName} at ({Left}, {Top}) for ({Width}, {Height}) with Opacity {Opacity} from {completePath ?? "Unknown Image"} was {fileStatus} at Offset ({XOffsetStart}, {YOffsetStart}) for ({XOffsetFinish - XOffsetStart}, {YOffsetFinish - YOffsetStart}).";
        }

        /// <summary>
        /// Short form
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToReadableString();
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
            var fileName = Path.Combine(filePath, $"{imagePrefix}_{ModuleName}_{Name}_{Width}_{Height}.png");
            retResult?.SaveToPng(fileName);
            Logger.Debug($"Cropped image saved as {fileName}.");
        }

        #endregion Private helpers
    }
}