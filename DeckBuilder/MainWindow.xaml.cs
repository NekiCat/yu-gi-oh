using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using TigeR.YuGiOh.Core.Data;
using TigeR.YuGiOh.Core.Effects;
using TigeR.YuGiOh.UI.Helper;
using UpdateControls.Fields;
using UpdateControls.XAML;

namespace DeckBuilder
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Card Card
        {
            get
            {
                return cardView.Card;
            }
            set
            {
                cardView.Card = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ForView.Wrap(this);
        }

        public Visibility IsMonster => Card.IsMonster ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility IsNotMonster => Card.IsMonster ? Visibility.Collapsed : Visibility.Visible;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Closing += Close;

            if (File.Exists("last.card"))
            {
                var loader = new CardLoader();
                Card = loader.LoadFromFile("last.card");
            }
            else
            {
                Card = new Card();
            }
        }

        private void Close(object sender, CancelEventArgs e)
        {
            var loader = new CardLoader();
            using (var thumb = Capture.CreatePngThumbnail(cardView))
            {
                loader.SaveToFile("last.card", Card, thumb);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    using (var fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        var ms = new MemoryStream((int)fs.Length);
                        fs.CopyTo(ms);
                        ms.Seek(0, SeekOrigin.Begin);

                        Card.Cover = "images/" + System.IO.Path.GetFileName(dialog.FileName);
                        Card.Resources[Card.Cover] = new Resource(Card.Cover, ms);
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new EffectEditorWindow();
            window.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonSaveFileDialog())
            {
                dialog.DefaultExtension = "png";
                dialog.DefaultFileName = Card.Name;
                dialog.OverwritePrompt = true;
                dialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    cardView.HoloGradientStep = -1;
                    var bitmap = Capture.CaptureVisual(cardView, 813, 1185);
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    
                    using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        encoder.Save(fs);
                    }
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.DefaultExtension = "card";

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var loader = new CardLoader();
                    Card = loader.LoadFromFile(dialog.FileName);
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonSaveFileDialog())
            {
                dialog.DefaultExtension = "card";
                dialog.DefaultFileName = Card.Name;
                dialog.OverwritePrompt = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var loader = new CardLoader();
                    using (var thumb = Capture.CreatePngThumbnail(cardView))
                    {
                        loader.SaveToFile(dialog.FileName, Card, thumb);
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var edition = Card.Edition;
            var set = Card.Set;
            var number = Card.Number;
            var copyright = Card.Copyright;

            Card = new Card()
            {
                Edition = edition,
                Set = set,
                Number = number,
                Copyright = copyright,
            };
        }
    }
}
