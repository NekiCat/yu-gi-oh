using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Cards;

namespace TigeR.YuGiOh.UI.Rendering
{
    abstract class AbstractAssetProvider
    {
        public abstract Bitmap GetAsset(Card card);

        protected Stream Load(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream("TigeR.YuGiOh.UI.Assets." + filename.Replace('/', '.'));
        }

        protected Bitmap LoadImage(string filename)
        {
            return new Bitmap(Load(filename));
        }

        protected Bitmap LoadSvg(string filename, Size size)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = Load(filename)) {
                var doc = SvgDocument.Open<SvgDocument>(stream);
                return doc.Draw(size.Width, size.Height);
            }
        }
    }
}
