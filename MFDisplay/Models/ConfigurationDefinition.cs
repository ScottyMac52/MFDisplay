using MFDisplay.Interfaces;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MFDisplay.Models
{
    public class ConfigurationDefinition : IMFDDefintion
    {
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

        public BitmapSource CropImage(string imagePath)
        {
            Int32Rect offSet;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(imagePath, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            if ((UseOffsets ?? false) == true)
            {
                offSet = new Int32Rect(XOffsetStart, YOffsetStart, XOffsetFinish - XOffsetStart, YOffsetFinish - YOffsetStart);
            }
            else
            {
                return src;
            }

            return new CroppedBitmap(src, offSet);
        }


        public string ToReadableString()
        {
            return $"{Name} at ({Left}, {Top}) for ({Width}, {Height}) with Opacity {Opacity} from {FileName}.";
        }
    }
}