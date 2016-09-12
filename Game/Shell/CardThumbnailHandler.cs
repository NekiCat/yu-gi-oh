using SharpShell.Attributes;
using SharpShell.SharpThumbnailHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Data;

namespace TigeR.YuGiOh.Core.Shell
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.FileExtension, ".card")]
    public class CardThumbnailHandler : SharpThumbnailHandler
    {
        private readonly CardLoader loader = new CardLoader();

        protected override Bitmap GetThumbnailImage(uint width)
        {
            using (var thumb = loader.GetThumbnail(SelectedItemStream))
            {
                int newWidth, newHeight;
                if (thumb.Width > thumb.Height)
                {
                    if (width > thumb.Width)
                    {
                        newWidth = thumb.Width;
                        newHeight = thumb.Height;
                    }
                    else
                    {
                        newWidth = (int)width;
                        newHeight = (int)(thumb.Height * (float)width / thumb.Width);
                    }
                }
                else
                {
                    if (width > thumb.Height)
                    {
                        newWidth = thumb.Width;
                        newHeight = thumb.Height;
                    }
                    else
                    {
                        newWidth = (int)(thumb.Width * (float)width / thumb.Height);
                        newHeight = (int)width;
                    }
                }

                var result = new Bitmap(newWidth, newHeight);
                using (var g = Graphics.FromImage(result))
                {
                    g.DrawImage(thumb, 0, 0, result.Width, result.Height);
                }

                return result;
            }
        }
    }
}
