using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

// Taken from http://flipcontrol.codeplex.com/
namespace TigeR.YuGiOh.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class FlipControl : UserControl
    {
        public FlipControl()
        {
            InitializeComponent();
        }

        public bool IsFaceDown
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set
            {
                if (IsFaceDown != value)
                {
                    SetValue(IsFlippedProperty, value);
                    if (value)
                    {
                        ((Storyboard)Resources["FlipFaceDown"]).Begin();
                    }
                    else
                    {
                        ((Storyboard)Resources["FlipFaceUp"]).Begin();
                    }
                }
            }
        }
        
        public static readonly DependencyProperty IsFlippedProperty =
            DependencyProperty.Register(nameof(IsFaceDown), typeof(bool), typeof(FlipControl), new PropertyMetadata(false));
        
        public bool IsDefending
        {
            get { return (bool)GetValue(IsDefendingProperty); }
            set
            {
                if (IsDefending != value)
                {
                    SetValue(IsDefendingProperty, value);
                    if (value)
                    {
                        ((Storyboard)Resources["FlipDefense"]).Begin();
                    }
                    else
                    {
                        ((Storyboard)Resources["FlipOffense"]).Begin();
                    }
                }
            }
        }
        
        public static readonly DependencyProperty IsDefendingProperty =
            DependencyProperty.Register(nameof(IsDefending), typeof(bool), typeof(FlipControl), new PropertyMetadata(false));
        
        public Visual FrontSide
        {
            get { return Side1.Visual; }
            set { Side1.Visual = value; }
        }
    }
}