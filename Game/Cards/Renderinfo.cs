using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Fields;

namespace TigeR.YuGiOh.Core.Cards
{
    public class RenderInfo
    {
        #region TitleColor

        /// <summary>
        /// Backing field for <see cref="TitleColor"/>.
        /// </summary>
        private Independent<String> titleColor = new Independent<String>();

        /// <summary>
        /// The color name of the color the title should be written with. May
        /// contain a CSS color code instead. May contain special colors,
        /// "silver-shiny", "gold-shiny" and "rainbow" for special card effects.
        /// </summary>
        public string TitleColor
        {
            get
            {
                return titleColor;
            }
            set
            {
                titleColor.Value = value;
            }
        }

        #endregion

        #region TitleEmbossed

        /// <summary>
        /// Backing field for <see cref="TitleEmbossed"/>.
        /// </summary>
        private Independent<bool> titleEmbossed = new Independent<bool>();

        /// <summary>
        /// Whether the title should be rendered with an embossed effect.
        /// </summary>
        public bool TitleEmbossed
        {
            get
            {
                return titleEmbossed;
            }
            set
            {
                titleEmbossed.Value = value;
            }
        }

        #endregion

        #region Holofoil

        /// <summary>
        /// Backing field for <see cref="Holofoil"/>.
        /// </summary>
        private Independent<String> holofoil = new Independent<String>();

        /// <summary>
        /// Contains whether a specific holographic effect should be applied to the
        /// card image. Values contain "shiny", "parallel" and "hieroglyph".
        /// </summary>
        public string Holofoil
        {
            get
            {
                return holofoil;
            }
            set
            {
                holofoil.Value = value;
            }
        }

        #endregion
    }
}
