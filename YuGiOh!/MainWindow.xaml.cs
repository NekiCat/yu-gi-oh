using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TigeR.YuGiOh.Core.Data;
using TigeR.YuGiOh.UI;

namespace TigeR.YuGiOh
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            fullCardView.IsFaceDown = !fullCardView.IsFaceDown;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            fullCardView.IsDefending = !fullCardView.IsDefending;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var loader = new CardLoader();
            fullCardView.FrontSide = new CardView() { Card = loader.LoadFromFile("card.card") };
        }
    }
}
