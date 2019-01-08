using MFDSettingsManager.Extensions;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MFDSettingsManager.Models
{
    public class ConfigurationDefinition 
    {
        public string ModuleName { get; internal set; }
        public string FileName { get; internal set; }
        public int Height { get; internal set; }
        public int Left { get; internal set; }
        public string Name { get; internal set; }
        public float Opacity { get; internal set; }
        public int Top { get; internal set; }
        public int Width { get; internal set; }
        public int XOffsetStart { get; internal set; }
        public int YOffsetStart { get; internal set; }
        public int XOffsetFinish { get; internal set; }
        public int YOffsetFinish { get; internal set; }
        public bool? UseOffsets { get; internal set; }
        public bool? SaveResults { get; internal set; }

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

                    string fileName = Path.Combine(fi.Directory.FullName, $"Before_{ModuleName}_{Name}_{Width}_{Height}.jpg");
                    ((BitmapSource)croppedBitmap).SaveToJpeg(fileName);
                }
                var bitmap = croppedBitmap.BitmapImage2Bitmap();
                bitmap = bitmap.Crop();
                retResult = bitmap.ToBitmapSource();
                if ((SaveResults ?? false) == true)
                {
                    var fi = new FileInfo(imagePath);

                    string fileName = Path.Combine(fi.Directory.FullName, $"After_{ModuleName}_{Name}_{Width}_{Height}.jpg");
                    retResult.SaveToJpeg(fileName);
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
    }
}