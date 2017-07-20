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
    class StarProvider : AbstractAssetProvider
    {
        public override Bitmap GetAsset(Card card)
        {
            return card.LevelReversed ?
                LoadSvg("rank_star.svg", CardRendering.Default.StarSize) :
                LoadSvg("level_star.svg", CardRendering.Default.StarSize);
        }
    }
}
