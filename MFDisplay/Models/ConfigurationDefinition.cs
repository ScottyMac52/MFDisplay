using MFDisplay.Extensions;
using MFDisplay.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MFDisplay.Models
{
    public class ConfigurationDefinition : IMFDDefintion
    {
        public string ModuleName { get; set; }
        public string FileName { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public string Name { get; set; }
        public float Opacity { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int XOffsetStart { get; set; }
        public int YOffsetStart { get; set; }
        public int XOffsetFinish { get; set; }
        public int YOffsetFinish { get; set; }
        public bool? UseOffsets { get; set; }
        public bool? SaveResults { get; set; }

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
        
        public string ToReadableString()
        {
            return $"{Name} at ({Left}, {Top}) for ({Width}, {Height}) with Opacity {Opacity} from {FileName} at ({XOffsetStart}, {YOffsetStart}) for ({XOffsetFinish - XOffsetStart}, {YOffsetFinish - YOffsetStart}).";
        }
    }
}