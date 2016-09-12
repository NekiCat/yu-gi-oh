using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Effects;
using UpdateControls.Collections;
using UpdateControls.Fields;

namespace TigeR.YuGiOh.Core.Cards
{
    public class Card
    {
        /// <summary>
        /// Backing field for <see cref="Author"/>.
        /// </summary>
        private Independent<String> author = new Independent<string>(String.Empty);

        /// <summary>
        /// The author of the card.
        /// </summary>
        public string Author
        {
            get
            {
                Debug.Assert(author != null);
                return author;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                author.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="CardType"/>.
        /// </summary>
        private Independent<String> cardType = new Independent<String>(String.Empty);

        /// <summary>
        /// Not to be confused with <see cref="Type"/>, a property of monsters. This specifies the
        /// type of card, ie. "Monster", "Fusion", "Spell", "Trap", "Synchro" and others.
        /// </summary>
        public string CardType
        {
            get
            {
                Debug.Assert(cardType != null);
                return cardType;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                cardType.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="CardSubType"/>.
        /// </summary>
        private Independent<String> cardSubType = new Independent<String>(String.Empty);

        /// <summary>
        /// An extension to the <see cref="CardType"/> property. This specifies a subtype for
        /// monster cards, ie. "Normal", "Effect", "Toon" and others.
        /// </summary>
        public string CardSubType
        {
            get
            {
                Debug.Assert(cardSubType != null);
                return cardSubType;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                cardSubType.Value = value;
            }
        }

        /// <summary>
        /// Checks whether the card is a type of monster card. This is true if both <see cref="IsSpell"/> and <see cref="IsTrap"/> are false.
        /// </summary>
        public bool IsMonster => !IsSpell && !IsTrap;

        /// <summary>
        /// Checks whether the card is a type of spell card. This is true if the <see cref="CardType"/> contains the word "Spell".
        /// </summary>
        public bool IsSpell => CardType.Contains("Spell");

        /// <summary>
        /// Checks whether the card is a type of trap card. This is true if the <see cref="CardType"/> contains the word "Trap".
        /// </summary>
        public bool IsTrap => CardType.Contains("Trap");

        /// <summary>
        /// Backing field for <see cref="Name"/>.
        /// </summary>
        private Independent<String> name = new Independent<String>(String.Empty);

        /// <summary>
        /// The name of the card. Displayed at the top.
        /// </summary>
        public string Name
        {
            get
            {
                Debug.Assert(name != null);
                return name;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                name.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Rarity"/>.
        /// </summary>
        private Independent<String> rarity = new Independent<String>(String.Empty);

        /// <summary>
        /// The rarity of the card. Influences the color of the name of the card.
        /// </summary>
        public string Rarity
        {
            get
            {
                Debug.Assert(rarity != null);
                return rarity;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                rarity.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Attribute"/>.
        /// </summary>
        private Independent<String> attribute = new Independent<String>(String.Empty);

        /// <summary>
        /// The attribute of the card. Displayed right of the <see cref="Name"/>, as an icon.
        /// </summary>
        public string Attribute
        {
            get
            {
                Debug.Assert(attribute != null);
                return attribute;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                attribute.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Level"/>.
        /// </summary>
        private Independent<int> level = new Independent<int>();

        /// <summary>
        /// The level of the card. Displayed below the <see cref="Name"/>, as a number of stars.
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="LevelReversed"/>.
        /// </summary>
        private Independent<bool> levelReversed = new Independent<bool>();

        /// <summary>
        /// If the level stars should be reversed and come from the left instead from the right. This inverts the colors as well.
        /// </summary>
        public bool LevelReversed
        {
            get
            {
                return levelReversed;
            }
            set
            {
                levelReversed.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Cover"/>.
        /// </summary>
        private Independent<String> cover = new Independent<String>(String.Empty);

        /// <summary>
        /// The path to the cover image in the zip file.
        /// </summary>
        public string Cover
        {
            get
            {
                Debug.Assert(cover != null);
                return cover;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                cover.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Edition"/>.
        /// </summary>
        private Independent<String> edition = new Independent<String>(String.Empty);

        /// <summary>
        /// The card edition, usually "1st Edition", "Limited Edition" or the like. Displayed below the picture on the left.
        /// </summary>
        public string Edition
        {
            get
            {
                Debug.Assert(edition != null);
                return edition;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                edition.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Set"/>.
        /// </summary>
        private Independent<String> set = new Independent<String>(String.Empty);

        /// <summary>
        /// The set of the card. Displayed below the picture on the right.
        /// </summary>
        public string Set
        {
            get
            {
                Debug.Assert(set != null);
                return set;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                set.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Type"/>.
        /// </summary>
        private Independent<String> type = new Independent<String>(String.Empty);

        /// <summary>
        /// The type of the card, for monsters. Displayed above the lore text.
        /// </summary>
        public string Type
        {
            get
            {
                Debug.Assert(type != null);
                return type;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                type.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Description"/>.
        /// </summary>
        private Independent<String> description = new Independent<String>(String.Empty);

        /// <summary>
        /// Either the lore or the effect of the card, depending on the <see cref="Type"/>.
        /// </summary>
        public string Description
        {
            get
            {
                Debug.Assert(description != null);
                return description;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                description.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Attack"/>.
        /// </summary>
        private Independent<String> attack = new Independent<String>();

        /// <summary>
        /// The attack strength of the card, in case it is a monster.
        /// </summary>
        public string Attack
        {
            get
            {
                Debug.Assert(attack != null);
                return attack;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                attack.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Defense"/>.
        /// </summary>
        private Independent<String> defense = new Independent<String>();

        /// <summary>
        /// The defense of the card, in case it is a monster.
        /// </summary>
        public string Defense
        {
            get
            {
                Debug.Assert(defense != null);
                return defense;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                defense.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Number"/>.
        /// </summary>
        private Independent<String> number = new Independent<String>(String.Empty);

        /// <summary>
        /// The card number. Usually numeric. Displayed in the bottom left corner.
        /// </summary>
        public string Number
        {
            get
            {
                Debug.Assert(number != null);
                return number;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                number.Value = value;
            }
        }

        /// <summary>
        /// Backing field for <see cref="Copyright"/>.
        /// </summary>
        private Independent<String> copyright = new Independent<String>(String.Empty);

        /// <summary>
        /// The copyright holder of the card. Displayed in the bottom right corner.
        /// </summary>
        public string Copyright
        {
            get
            {
                Debug.Assert(copyright != null);
                return copyright;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                copyright.Value = value;
            }
        }

        /// <summary>
        /// Effects of the card. May be empty.
        /// </summary>
        public ICollection<Effect> Effects { get; } = new IndependentList<Effect>();

        /// <summary>
        /// Determines if this card has any effects.
        /// </summary>
        public bool HasEffects => Effects.Count > 0;

        /// <summary>
        /// Resources the card has stored, such as the cover image.
        /// </summary>
        public IDictionary<String, Resource> Resources { get; } = new IndependentDictionary<String, Resource>();
    }
}
