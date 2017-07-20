using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TigeR.YuGiOh.UI.Rendering
{
    class FontProvider : IDisposable
    {
        private readonly PrivateFontCollection fontCollection = new PrivateFontCollection();

        public FontFamily Title => fontCollection.Families[2];

        public FontFamily Edition => fontCollection.Families[1];

        public FontFamily Set => fontCollection.Families[1];

        public FontFamily Number => fontCollection.Families[1];

        public FontFamily Copyright => fontCollection.Families[1];

        public FontFamily Type => fontCollection.Families[0];

        public FontFamily Description => fontCollection.Families[3];

        public FontFamily AtkDef => fontCollection.Families[0];

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        public FontProvider()
        {
            Load("Yu-Gi-Oh! Matrix Regular Small Caps 2.ttf");
            Load("Stone Serif Semibold.ttf");
            Load("Yu-Gi-Oh! ITC Stone Serif Small Caps Bold.ttf");
            Load("Yu-Gi-Oh! ITC Stone Serif LT Italic.ttf");
        }

        private void Load(string filename)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TigeR.YuGiOh.UI.Assets.Fonts." + filename))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                uint c = 0;

                var pData = Marshal.AllocCoTaskMem(buffer.Length);
                Marshal.Copy(buffer, 0, pData, buffer.Length);
                AddFontMemResourceEx(pData, (uint)buffer.Length, IntPtr.Zero, ref c);
                fontCollection.AddMemoryFont(pData, buffer.Length);
                Marshal.FreeCoTaskMem(pData);
            }
        }

        public void Dispose()
        {
            fontCollection.Dispose();
        }
    }
}
