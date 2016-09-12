using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Cards;

namespace TigeR.YuGiOh.Core.Players
{
    public class DefaultPlayer : Player
    {
        public DefaultPlayer()
        {
            Deck = new List<Card>();
        }
    }
}
