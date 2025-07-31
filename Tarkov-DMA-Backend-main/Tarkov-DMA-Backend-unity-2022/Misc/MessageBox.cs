using Tarkov_DMA_Backend.LowLevel;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;

namespace Tarkov_DMA_Backend.Misc
{
    public static unsafe class MessageBox
    {
        public static int ShowError(string error, string title)
        {
            using PinnedObject<string> pText = new(error);
            using PinnedObject<string> pCaption = new(title);
            return MessageBoxW(HWND.NULL, (char*)(nint)pText, (char*)(nint)pCaption, MB.MB_OK | MB.MB_ICONERROR | MB.MB_DEFBUTTON1 | MB.MB_SYSTEMMODAL);
        }
    }
}
