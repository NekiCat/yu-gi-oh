using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Cards;

namespace TigeR.YuGiOh.UI.Rendering
{
    class BackgroundProvider : AbstractAssetProvider
    {
        public override Bitmap GetAsset(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            
            if (card.IsSpell)
            {
                return LoadImage("card_spell.png");
            }
            else if (card.IsTrap)
            {
                return LoadImage("card_trap.png");
            }
            
            switch (card.CardType)
            {
                default:
                case "Monster":
                    break;
                case "Ritual":
                    return LoadImage("card_ritual.png");
                case "Fusion":
                    return LoadImage("card_fusion.png");
                case "Synchro":
                    return LoadImage("card_synchro.png");
                case "DarkSynchro":
                    return LoadImage("card_dark_synchro.png");
                case "Xyz":
                    return LoadImage("card_xyz.png");
            }

            switch (card.CardSubType)
            {
                case "Effect":
                case "Gemini":
                case "Spirit":
                case "Toon":
                case "Tuner":
                case "Union":
                    return LoadImage("card_effect.png");
                default:
                case "Normal":
                case "Divine-Beast":
                    return LoadImage("card_monster.png");
            }
        }
    }
}
