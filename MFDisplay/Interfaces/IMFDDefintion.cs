using System.Windows.Media.Imaging;

namespace MFDisplay.Interfaces
{

    public interface IMFDDefintion
    {
        string ModuleName { get; set; }
        string FileName { get; set; }
        int Height { get; set; }
        int Left { get; set; }
        string Name { get; set; }
        float Opacity { get; set; }
        int Top { get; set; }
        int Width { get; set; }
        int XOffsetStart { get; set; }
        int YOffsetStart { get; set; }
        int XOffsetFinish { get; set; }
        int YOffsetFinish { get; set; }
        bool? UseOffsets { get; set; }
        bool? SaveResults { get; set; }
        BitmapSource CropImage(string imagePath);
        string ToReadableString();
      
    }
}