using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigeR.YuGiOh.Core.Cards
{
    public class RenderInfo
    {
        /// <summary>
        /// The color name of the color the title should be written with. May
        /// contain a CSS color code instead. May contain special colors,
        /// "silver-shiny", "gold-shiny" and "rainbow" for special card effects.
        /// </summary>
        public string TitleColor { get; set; } = String.Empty;

        /// <summary>
        /// Whether the title should be rendered with an embossed effect.
        /// </summary>
        public bool TitleEmbossed { get; set; } = false;

        /// <summary>
        /// Contains whether a specific holographic effect should be applied to the
        /// card image. Values contain "shiny", "parallel" and "hieroglyph".
        /// </summary>
        public string Holofoil { get; set; } = String.Empty;
    }
}
