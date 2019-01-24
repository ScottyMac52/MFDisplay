using MFDSettingsManager.Mappers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MFDSettingsManager.Extensions
{
    /// <summary>
    /// Image Handler Extensions
    /// </summary>
    public static class ImageHandlerExtensions
    {
        /// <summary>
        /// Saves the <seealso cref="BitmapSource"/> as a PNG
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <param name="fileName"></param>
        public static void SaveToPng(this BitmapSource bitmapImage, string fileName)
        {
            var encoder = new PngBitmapEncoder();

            SaveUsingEncoder(bitmapImage, fileName, encoder);
        }

        /// <summary>
        /// Saves a <seealso cref="BitmapSource"/> with Transparency
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <param name="transparentColor"><seealso cref="System.Drawing.Color"/>Color to be made transparent</param>
        /// <returns><seealso cref="BitmapSource"/></returns>
        public static BitmapSource ToTransparentPng(this BitmapSource sourceBitmap, System.Drawing.Color transparentColor)
        {
            // Write Png
            //get a real 32bpp argb image
            //            FormatConvertedBitmap fcb = new FormatConvertedBitmap(sourceBitmap, PixelFormats.Bgra32, null, 0);
            FormatConvertedBitmap fcb = new FormatConvertedBitmap(sourceBitmap, sourceBitmap.Format, null, 0);
            WriteableBitmap writeable = new WriteableBitmap(fcb);

            int pixelWidth = (int)sourceBitmap.PixelWidth;
            int pixelHeight = (int)sourceBitmap.PixelHeight;
            //int Stride = pixelWidth * 4;

            int Stride = (pixelWidth * sourceBitmap.Format.BitsPerPixel + 7) / 8;

            BitmapSource imgSource = (BitmapSource)writeable;
            byte[] pixels = new byte[pixelHeight * Stride];
            imgSource.CopyPixels(pixels, Stride, 0);
            byte TransparentByte = byte.Parse("0");
            byte Byte255 = byte.Parse("255");

            int N = pixelWidth * pixelHeight;
            //Operate the pixels directly
            int index = sourceBitmap.Format.BitsPerPixel / 8;
            for (int i = 0; i < N; i++)
            {
                byte? a,b,c,d = null;

                switch(index)
                {
                    case 4:
                        a = pixels[i * index]; // Blue
                        b = pixels[i * index + 1]; // Green
                        c = pixels[i * index + 2]; // Red
                        d = pixels[i * index + 3]; // A
                        break;
                    default:
                        a = pixels[i * index]; // Blue
                        b = pixels[i * index + 1]; // Green
                        c = pixels[i * index + 2]; // Red
                        break;
                }

                if (
                    ((a.HasValue && a == Byte255) || !a.HasValue)
                    && ((b.HasValue && b == Byte255) || !b.HasValue)
                    && ((c.HasValue && c == Byte255) || !c.HasValue)
                    && ((d.HasValue && d == Byte255) || !d.HasValue)
                   )
                {
                    pixels[i * index] = transparentColor.B;
                    pixels[i * index + 1] = transparentColor.G;
                    pixels[i * index + 2] = transparentColor.R;
                    if (index > 3)
                    {
                        pixels[i * index + 3] = TransparentByte;
                    }
                }
            }
            writeable.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixels, Stride, 0);
            return writeable;
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="source">The source bitmap.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitSrc;
        }

        /// <summary>
        /// Converts a <seealso cref="BitmapSource"/> into a <seealso cref="Bitmap"/>
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <param name="transparentColor"></param>
        /// <returns></returns>
        public static Bitmap BitmapImage2Bitmap(this BitmapSource bitmapImage, System.Drawing.Color transparentColor)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                bitmap.MakeTransparent(transparentColor);
                return new Bitmap(bitmap);
            }
        }



        private static void SaveUsingEncoder(BitmapSource bitmapImage, string fileName, BitmapEncoder encoder)
        {
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
                encoder.Save(stream);
        }
    }
}
