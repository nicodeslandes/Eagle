using System.ComponentModel.Composition;
using System.Windows;

namespace Eagle.Services
{
    [Export(typeof(IClipboardService))]
    public class ClipboardService : IClipboardService
    {
        public void CopyText(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
