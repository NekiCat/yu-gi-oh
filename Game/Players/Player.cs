using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Cards;

namespace TigeR.YuGiOh.Core.Players
{
    /// <summary>
    /// Base class for entities controlling one of the players.
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// The cards the player has in his deck.
        /// </summary>
        public IList<Card> Deck { get; protected set; }

        /// <summary>
        /// Determines if the <see cref="Deck"/> still has cards.
        /// </summary>
        /// <returns></returns>
        public bool HasDeckCards() => Deck.Count > 0;

        /// <summary>
        /// The cards the player is holding in his hands.
        /// </summary>
        public IList<Card> Hand { get; protected set; }

        private int lifepoints = 8000;

        /// <summary>
        /// The lifepoints of the player.
        /// </summary>
        public int LifePoints
        {
            get
            {
                return lifepoints;
            }
            set
            {
                lifepoints = Math.Max(0, value);
            }
        }

        // The next method is async for child classes. Not calling await is normal.
        #pragma warning disable 1998

        /// <summary>
        /// Draws a card. Usually gets the first card from the <see cref="Deck"/> and puts it in the <see cref="Hand"/>.
        /// </summary>
        /// <returns></returns>
        public virtual async Task DrawAsync()
        {
            var card = Deck[0];
            Deck.RemoveAt(0);
            Hand.Add(card);
        }

        #pragma warning restore 1998
    }
}
