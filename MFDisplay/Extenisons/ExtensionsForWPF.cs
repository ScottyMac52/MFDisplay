using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace MFDisplay.Extenisons
{
    static class ExtensionsForWPF
    {
        public static Screen GetScreen(this Window window)
        {
            return Screen.FromHandle(new WindowInteropHelper(window).Handle);
        }
    }
}
