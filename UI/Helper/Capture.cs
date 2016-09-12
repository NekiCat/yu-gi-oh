using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TigeR.YuGiOh.UI.Helper
{
    public static class Capture
    {
        /// <summary>
        /// Renders a WPF visual to a BitmapSource.
        /// </summary>
        /// <seealso cref="https://blogs.msdn.microsoft.com/jaimer/2009/07/03/rendertargetbitmap-tips/"/>
        /// <param name="target"></param>
        /// <returns></returns>
        public static BitmapSource CaptureVisual(Visual target)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            var renderer = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);
            var drawingVisual = new DrawingVisual();
            using (var context = drawingVisual.RenderOpen())
            {
                var vb = new VisualBrush(target);
                context.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }

            renderer.Render(drawingVisual);
            return renderer;
        }

        /// <summary>
        /// Renders a WPF visual to a BitmapSource with the specified size.
        /// </summary>
        /// <seealso cref="https://blogs.msdn.microsoft.com/jaimer/2009/07/03/rendertargetbitmap-tips/"/>
        /// <param name="target"></param>
        /// <returns></returns>
        public static BitmapSource CaptureVisual(Visual target, int width, int height)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            var renderer = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            var drawingVisual = new DrawingVisual();
            using (var context = drawingVisual.RenderOpen())
            {
                var vb = new VisualBrush(target);
                context.DrawRectangle(vb, null, new Rect(0, 0, width, height));
            }

            renderer.Render(drawingVisual);
            return renderer;
        }

        /// <summary>
        /// Renders a WPF visual to an image and resizes it to 256x256.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Stream CreatePngThumbnail(Visual target)
        {
            var source = CaptureVisual(target);
            return CreatePngThumbnail(source);
        }

        /// <summary>
        /// Creates an image from a BitmapSource and resizes it to 256x256.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Stream CreatePngThumbnail(BitmapSource source)
        {
            // todo: transparent areas are probably not necessary (they disturb a proper icon shadow)

            Rect rect;
            if (source.Width > source.Height)
            {
                rect = new Rect(0, 128 - (source.Height / source.Width * 128), 256, source.Height / source.Width * 256);
            }
            else
            {
                rect = new Rect(128 - (source.Width / source.Height * 128), 0, source.Width / source.Height * 256, 256);
            }
            
            var group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, rect));

            var drawingVisual = new DrawingVisual();
            using (var context = drawingVisual.RenderOpen())
            {
                context.DrawDrawing(group);
            }

            var renderer = new RenderTargetBitmap(256, 256, 96, 96, PixelFormats.Default);
            renderer.Render(drawingVisual);

            var stream = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderer));
            encoder.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
