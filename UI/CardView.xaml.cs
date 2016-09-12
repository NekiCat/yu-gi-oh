using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TigeR.YuGiOh.Core.Cards;
using TigeR.YuGiOh.UI.Helper;
using UpdateControls.Fields;
using UpdateControls.XAML;

namespace TigeR.YuGiOh.UI
{
    /// <summary>
    /// Interaktionslogik für Card.xaml
    /// </summary>
    public partial class CardView : UserControl
    {
        private static readonly LinearGradientBrush silverGradient;

        private static readonly LinearGradientBrush goldGradient;

        private static readonly LinearGradientBrush rainbowGradient;

        static CardView()
        {
            var silverGradientStopList = new GradientStopCollection(3);
            silverGradientStopList.Add(new GradientStop(Colors.Silver, 0));
            silverGradientStopList.Add(new GradientStop(Colors.White, .5));
            silverGradientStopList.Add(new GradientStop(Colors.Silver, 1));
            silverGradient = new LinearGradientBrush(silverGradientStopList);

            var goldGradientStopList = new GradientStopCollection(3);
            goldGradientStopList.Add(new GradientStop(Colors.Gold, 0));
            goldGradientStopList.Add(new GradientStop(Colors.White, .5));
            goldGradientStopList.Add(new GradientStop(Colors.Gold, 1));
            goldGradient = new LinearGradientBrush(goldGradientStopList);

            var rainbowGradientStopList = new GradientStopCollection(5);
            rainbowGradientStopList.Add(new GradientStop(Color.FromArgb(255, 184, 64, 77), 0));
            rainbowGradientStopList.Add(new GradientStop(Color.FromArgb(255, 206, 208, 113), .2));
            rainbowGradientStopList.Add(new GradientStop(Color.FromArgb(255, 168, 223, 121), .6));
            rainbowGradientStopList.Add(new GradientStop(Color.FromArgb(255, 161, 229, 206), .8));
            rainbowGradientStopList.Add(new GradientStop(Color.FromArgb(255, 180, 60, 161), 1));
            rainbowGradient = new LinearGradientBrush(rainbowGradientStopList, 25);
        }
        
        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }
        
        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register(nameof(Card), typeof(Card), typeof(CardView), new PropertyMetadata(new Card()));

        public Brush CardBackground
        {
            get
            {
                if (Card?.IsMonster ?? false)
                {
                    switch (Card.CardType)
                    {
                        default:
                        case "Monster":
                            break;
                        case "Ritual":
                            return new ImageBrush(Assets.LoadImage("card_ritual.png"));
                        case "Fusion":
                            return new ImageBrush(Assets.LoadImage("card_fusion.png"));
                        case "Synchro":
                            return new ImageBrush(Assets.LoadImage("card_synchro.png"));
                        case "DarkSynchro":
                            return new ImageBrush(Assets.LoadImage("card_dark_synchro.png"));
                        case "Xyz":
                            return new ImageBrush(Assets.LoadImage("card_xyz.png"));
                    }

                    switch (Card.CardSubType)
                    {
                        case "Effect":
                        case "Gemini":
                        case "Spirit":
                        case "Toon":
                        case "Tuner":
                        case "Union":
                            return new ImageBrush(Assets.LoadImage("card_effect.png"));
                        default:
                        case "Normal":
                        case "Divine-Beast":
                            return new ImageBrush(Assets.LoadImage("card_monster.png"));
                    }
                }
                else if (Card?.IsSpell ?? false)
                {
                    return new ImageBrush(Assets.LoadImage("card_spell.png"));
                }
                else if (Card?.IsTrap ?? false)
                {
                    return new ImageBrush(Assets.LoadImage("card_trap.png"));
                }

                return new ImageBrush(Assets.LoadImage("card_monster.png"));
            }
        }

        public ImageSource Icon
        {
            get
            {
                if (Card?.IsMonster ?? false)
                {
                    return Assets.LoadSvg($"attribute_{Card.Attribute.ToLower()}.svg");
                }
                else if (Card?.IsSpell ?? false)
                {
                    return Assets.LoadSvg("spell.svg");
                }
                else if (Card?.IsTrap ?? false)
                {
                    return Assets.LoadSvg("trap.svg");
                }

                return null;
            }
        }

        public ImageSource SpellTrapIcon
        {
            get
            {
                if ((Card?.IsSpell ?? false) || (Card?.IsTrap ?? false))
                {
                    switch (Card.CardSubType)
                    {
                        case "Continuous":
                            return Assets.LoadSvg("ts_continuous.svg");
                        case "Equip":
                            return Assets.LoadSvg("ts_equip.svg");
                        case "Field":
                            return Assets.LoadSvg("ts_field.svg");
                        case "Quick-Play":
                            return Assets.LoadSvg("ts_quick-play.svg");
                        case "Ritual":
                            return Assets.LoadSvg("ts_ritual.svg");
                    }
                }

                return null;
            }
        }
        
        public ImageSource Cover
        {
            get
            {
                if (!String.IsNullOrEmpty(Card?.Cover))
                {
                    var source = new BitmapImage();
                    source.BeginInit();
                    source.StreamSource = Card.Resources[Card.Cover].Stream;
                    source.EndInit();

                    return source;
                }

                return null;
            }
        }

        public IEnumerable<int> StarLevel
        {
            get
            {
                if (Card == null || !Card.IsMonster) return Enumerable.Empty<int>();
                return Enumerable.Range(1, Card.Level);
            }
        }

        public string SpellTrapText
        {
            get
            {
                if (Card?.IsSpell ?? false)
                {
                    return "Spell Card";
                }
                else if (Card?.IsTrap ?? false)
                {
                    return "Trap Card";
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        
        public Brush NameColor
        {
            get
            {
                switch (Card?.Rarity)
                {
                    case "Rare":
                    case "GhostRare":
                        return silverGradient;
                    case "SecretRare":
                        return rainbowGradient;
                    case "UltraRare":
                    case "UltimateRare":
                    case "ParallelRare":
                    case "StarfoilRare":
                    case "GoldUltraRare":
                        return goldGradient;
                    default:
                        if (Card == null) return Brushes.Black;
                        if (!Card.IsMonster || Card.CardType == "Xyz") return Brushes.White;
                        return Brushes.Black;
                }
            }
        }

        public Visibility HolofoilVisibility
        {
            get
            {
                switch (Card?.Rarity)
                {
                    case "SuperRare":
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
        }

        public Visibility HolofoilParallelVisibility
        {
            get
            {
                switch (Card?.Rarity)
                {
                    case "SecretRare":
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
        }

        private readonly Independent<double> holoGradientStep = new Independent<double>(0);

        public double HoloGradientStep
        {
            get
            {
                return holoGradientStep.Value;
            }
            set
            {
                holoGradientStep.Value = value;
            }
        }

        public double HoloGradientStepLeft => HoloGradientStep - 0.25;

        public double HoloGradientStepRight => HoloGradientStep + 0.25;

        public Brush MetaColor
        {
            get
            {
                if (Card?.CardType == "Xyz") return Brushes.White;
                return Brushes.Black;
            }
        }

        public string Type
        {
            get
            {
                return Card?.Type ?? "Monster";
            }
        }

        public Style DescriptionStyle
        {
            get
            {
                if (Card?.CardType == "Monster") return (Style)Resources["LoreStyle"];
                return (Style)Resources["DescriptionStyle"];
            }
        }

        public CardView()
        {
            InitializeComponent();
            DataContext = ForView.Wrap(this);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            HoloGradientStep = e.GetPosition(this).X / ActualWidth;
        }
    }
}
