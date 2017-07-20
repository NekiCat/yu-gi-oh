using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TigeR.YuGiOh.Core.Effects;

namespace TigeR.YuGiOh.Core.Cards
{
    public class Card : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The author of the card.
        /// </summary>
        public string Author { get; set; } = String.Empty;

        /// <summary>
        /// Not to be confused with <see cref="Type"/>, a property of monsters. This specifies the
        /// type of card, ie. "Monster", "Fusion", "Spell", "Trap", "Synchro" and others.
        /// </summary>
        public string CardType { get; set; } = String.Empty;

        /// <summary>
        /// An extension to the <see cref="CardType"/> property. This specifies a subtype for
        /// monster cards, ie. "Normal", "Effect", "Toon" and others.
        /// </summary>
        public string CardSubType { get; set; } = String.Empty;

        /// <summary>
        /// Checks whether the card is a type of monster card. This is true if both <see cref="IsSpell"/> and <see cref="IsTrap"/> are false.
        /// </summary>
        public bool IsMonster => !IsSpell && !IsTrap;

        /// <summary>
        /// Checks whether the card is a type of spell card. This is true if the <see cref="CardType"/> contains the word "Spell".
        /// </summary>
        public bool IsSpell => CardType.IndexOf("Spell", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Checks whether the card is a type of trap card. This is true if the <see cref="CardType"/> contains the word "Trap".
        /// </summary>
        public bool IsTrap => CardType.IndexOf("Trap", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// The name of the card. Displayed at the top.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// The rarity of the card. Influences the color of the name of the card.
        /// </summary>
        public string Rarity { get; set; } = String.Empty;

        /// <summary>
        /// The attribute of the card. Displayed right of the <see cref="Name"/>, as an icon.
        /// </summary>
        public string Attribute { get; set; } = String.Empty;

        /// <summary>
        /// The level of the card. Displayed below the <see cref="Name"/>, as a number of stars.
        /// </summary>
        public int Level { get; set; } = 0;

        /// <summary>
        /// If the level stars should be reversed and come from the left instead from the right. This changes the colors of the stars as well.
        /// </summary>
        public bool LevelReversed { get; set; } = false;

        /// <summary>
        /// The path to the cover image in the zip file.
        /// </summary>
        public string Cover { get; set; } = String.Empty;

        /// <summary>
        /// The card edition, usually "1st Edition", "Limited Edition" or the like. Displayed below the picture on the left.
        /// </summary>
        public string Edition { get; set; } = String.Empty;

        /// <summary>
        /// The set of the card. Displayed below the picture on the right.
        /// </summary>
        public string Set { get; set; } = String.Empty;

        /// <summary>
        /// The type of the card, for monsters. Displayed above the lore text.
        /// </summary>
        public ObservableCollection<String> Type { get; } = new ObservableCollection<String>();

        /// <summary>
        /// Either the lore or the effect of the card, depending on the <see cref="Type"/>.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// The attack strength of the card, in case it is a monster.
        /// </summary>
        public string Attack { get; set; } = String.Empty;

        /// <summary>
        /// The defense of the card, in case it is a monster.
        /// </summary>
        public string Defense { get; set; } = String.Empty;

        /// <summary>
        /// The card number. Usually numeric. Displayed in the bottom left corner.
        /// </summary>
        public string Number { get; set; } = String.Empty;

        /// <summary>
        /// The copyright holder of the card. Displayed in the bottom right corner.
        /// </summary>
        public string Copyright { get; set; } = String.Empty;

        /// <summary>
        /// Effects of the card. May be empty.
        /// </summary>
        public ObservableCollection<Effect> Effects { get; } = new ObservableCollection<Effect>();

        /// <summary>
        /// Determines if this card has any effects.
        /// </summary>
        public bool HasEffects => Effects.Count > 0;

        /// <summary>
        /// Resources the card has stored, such as the cover image.
        /// </summary>
        public Dictionary<String, Resource> Resources { get; } = new Dictionary<String, Resource>();
    }
}
