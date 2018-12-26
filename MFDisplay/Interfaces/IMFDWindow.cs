namespace MFDisplay.Interfaces
{
    public interface IMFDWindow
    {
        IMFDDefintion Configuration { get; }
        bool IsMFDLoaded { get; }
        void InitializeComponent();
        bool InitializeMFD(IMFDDefintion definition);
        void LoadImage();
    }
}