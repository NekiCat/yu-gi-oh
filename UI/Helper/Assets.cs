using Svg2Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace TigeR.YuGiOh.UI.Helper
{
    static class Assets
    {
        public static StreamResourceInfo Load(string filename)
        {
            try
            {
                return Application.GetResourceStream(new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Assets/{filename}"));
            }
            catch (IOException)
            {
                return null;
            }
        }

        public static ImageSource LoadImage(string filename)
        {
            var info = Load(filename);
            if (info == null) return null;

            using (var stream = info.Stream)
            {
                var result = new BitmapImage();
                result.BeginInit();
                result.StreamSource = stream;
                result.EndInit();
                return result;
            }
        }

        public static ImageSource LoadSvg(string filename)
        {
            var info = Load(filename);
            if (info == null) return null;

            using (var stream = info.Stream)
            {
                return SvgReader.Load(stream);
            }
        }
    }
}
