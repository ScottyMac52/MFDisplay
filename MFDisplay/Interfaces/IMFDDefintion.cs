using System.Windows.Media.Imaging;

namespace MFDisplay.Interfaces
{

    public interface IMFDDefintion
    {
        string ModuleName { get; }
        string FileName { get; }
        int Height { get; }
        int Left { get; }
        string Name { get; }
        float Opacity { get; }
        int Top { get; }
        int Width { get; }
        int XOffsetStart { get; }
        int YOffsetStart { get; }
        int XOffsetFinish { get; }
        int YOffsetFinish { get; }
        bool? UseOffsets { get; }
        bool? SaveResults { get; }
        BitmapSource CropImage(string imagePath);
        string ToReadableString();
      
    }
}