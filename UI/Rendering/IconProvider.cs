using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Cards;
using TigeR.YuGiOh.UI.Resources;

namespace TigeR.YuGiOh.UI.Rendering
{
    class IconProvider : AbstractAssetProvider
    {
        public override Bitmap GetAsset(Card card)
        {
            if (card.IsSpell)
            {
                return LoadSvg("spell.svg", CardRendering.Default.IconSize);
            }
            else if (card.IsTrap)
            {
                return LoadSvg("trap.svg", CardRendering.Default.IconSize);
            }

            return LoadSvg($"attribute_{card.Attribute.ToLower()}.svg", CardRendering.Default.IconSize);
        }
    }
}
