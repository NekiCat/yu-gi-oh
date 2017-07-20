using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

namespace DeckBuilder
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new String(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        private static Bitmap MakeThumbnail(Bitmap source)
        {
            var target = new Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(target))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float scale = Math.Min(256f / source.Width, 256f / source.Height);
                var scaleWidth = (int)(source.Width * scale);
                var scaleHeight = (int)(source.Height * scale);
                graphics.DrawImage(source, new System.Drawing.Rectangle(((int)256 - scaleWidth) / 2, ((int)256 - scaleHeight) / 2, scaleWidth, scaleHeight));
            }

            return target;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            DataContext = this;
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
            var bitmap = MakeThumbnail(cardView.Image);

            using (var thumb = new MemoryStream())
            {
                bitmap.Save(thumb, System.Drawing.Imaging.ImageFormat.Png);
                loader.SaveToFile("last.card", Card, thumb);
            }
        }

        private void SelectCoverImage(object sender, RoutedEventArgs e)
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

        private void ShowEffectEditor(object sender, RoutedEventArgs e)
        {
            var window = new EffectEditorWindow();
            window.ShowDialog();
        }

        private void ExportCardAsImage(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonSaveFileDialog())
            {
                dialog.DefaultExtension = "png";
                dialog.DefaultFileName = MakeValidFileName(Card.Set + " " + Card.Name);
                dialog.OverwritePrompt = true;
                dialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var bitmap = (BitmapSource)cardView.ImageSource;
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    
                    using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        encoder.Save(fs);
                    }
                }
            }
        }

        private void LoadCardFromFile(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Card", "*.card"));
                dialog.DefaultExtension = "card";

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var loader = new CardLoader();
                    try
                    {
                        Card = loader.LoadFromFile(dialog.FileName);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void SaveCardToFile(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonSaveFileDialog())
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Card", "*.card"));
                dialog.DefaultExtension = "card";
                dialog.DefaultFileName = MakeValidFileName(Card.Set + " " + Card.Name);
                dialog.OverwritePrompt = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var loader = new CardLoader();
                    var bitmap = MakeThumbnail(cardView.Image);

                    using (var thumb = new MemoryStream())
                    {
                        bitmap.Save(thumb, System.Drawing.Imaging.ImageFormat.Png);
                        loader.SaveToFile(dialog.FileName, Card, thumb);
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewCard(object sender, RoutedEventArgs e)
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

        private void RemoveTypeItem(object sender, RoutedEventArgs e)
        {
            var item = (string)((Button)e.Source).Tag;
            if (item == null) return;
            Card.Type.Remove(item);
        }

        private void AddTypeItem(object sender, RoutedEventArgs e)
        {
            var text = typeTextBox.Text.Trim();
            if (text.Length == 0) return;
            if (Card.Type.Contains(text))
            {
                MessageBox.Show("Type " + text + " is already set.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                Card.Type.Add(text);
            }
        }
    }
}
