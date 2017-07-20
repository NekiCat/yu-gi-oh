using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TigeR.YuGiOh.Core.Cards;
using TigeR.YuGiOh.UI.Rendering;

namespace TigeR.YuGiOh.UI
{
    /// <summary>
    /// Interaktionslogik für Card.xaml
    /// </summary>
    public partial class CardView : UserControl, INotifyPropertyChanged
    {
        private static readonly LinearGradientBrush silverGradient;

        private static readonly LinearGradientBrush goldGradient;

        private static readonly LinearGradientBrush rainbowGradient;

        static CardView()
        {
            silverGradient = new LinearGradientBrush(new GradientStopCollection(3)
            {
                new GradientStop(Colors.Silver, 0),
                new GradientStop(Colors.White, .5),
                new GradientStop(Colors.Silver, 1)
            });

            goldGradient = new LinearGradientBrush(new GradientStopCollection(3)
            {
                new GradientStop(Colors.Gold, 0),
                new GradientStop(Colors.White, .5),
                new GradientStop(Colors.Gold, 1)
            });

            /*rainbowGradient = new LinearGradientBrush(new GradientStopCollection(5)
            {
                new GradientStop(Color.FromArgb(255, 184, 64, 77), 0),
                new GradientStop(Color.FromArgb(255, 206, 208, 113), .2),
                new GradientStop(Color.FromArgb(255, 168, 223, 121), .6),
                new GradientStop(Color.FromArgb(255, 161, 229, 206), .8),
                new GradientStop(Color.FromArgb(255, 180, 60, 161), 1)
            }, 25);*/
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CardGenerator renderer = new CardGenerator();

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public CardView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register(nameof(Card), typeof(Card), typeof(CardView), new PropertyMetadata(new Card(), OnCardChanged));

        private static void OnCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (d as CardView);
            if (obj == null) return;

            if (e.OldValue != null)
            {
                ((Card)e.OldValue).PropertyChanged -= obj.OnCardPropertyChanged;
                ((Card)e.OldValue).Type.CollectionChanged -= obj.OnCardTypeChanged;
            }

            if (e.NewValue != null)
            {
                ((Card)e.NewValue).PropertyChanged += obj.OnCardPropertyChanged;
                ((Card)e.NewValue).Type.CollectionChanged += obj.OnCardTypeChanged;
                obj.Redraw();
            }
        }

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        private void OnCardPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Redraw();
        }

        private void OnCardTypeChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Redraw();
        }

        public Bitmap Image { get; private set; }

        public BitmapSource ImageSource { get; private set; }

        public Visibility HolofoilVisibility => Card?.Rarity?.Equals("SuperRare", StringComparison.OrdinalIgnoreCase) ?? false ? Visibility.Visible : Visibility.Collapsed;

        public Visibility HolofoilParallelVisibility => Card?.Rarity?.Equals("SecretRare", StringComparison.OrdinalIgnoreCase) ?? false ? Visibility.Visible : Visibility.Collapsed;

        public double HoloGradientStep { get; set; }

        public double HoloGradientStepLeft => HoloGradientStep - 0.25;

        public double HoloGradientStepRight => HoloGradientStep + 0.25;

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            HoloGradientStep = e.GetPosition(this).X / ActualWidth;
        }

        public void Redraw()
        {
            if (Card == null) return;
            Image = renderer.Render(Card);
            var hBitmap = Image.GetHbitmap();
            try
            {
                ImageSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }
    }
}
