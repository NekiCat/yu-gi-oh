using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TigeR.YuGiOh.UI.Helper
{
    class TitleStretchPanel : Panel
    {
        private double scale;

        protected override Size MeasureOverride(Size availableSize)
        {
            /*if (DesignerProperties.GetIsInDesignMode(this))
            {
                availableSize = new Size(200, 200);
            }*/

            double width = 0;
            Size unlimited = new Size(double.PositiveInfinity, double.PositiveInfinity);
            foreach (UIElement child in Children)
            {
                child.Measure(unlimited);
                width += child.DesiredSize.Width;
            }
            scale = availableSize.Width / width;

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var usedScale = Math.Min(scale, 1);
            Transform scaleTransform = new ScaleTransform(usedScale, 1);
            double width = 0;
            double rwidth = 0;
            foreach (UIElement child in Children)
            {
                child.RenderTransform = scaleTransform;
                if (child is FrameworkElement)
                {
                    var feChild = (FrameworkElement)child;
                    if (feChild.HorizontalAlignment == HorizontalAlignment.Right)
                    {
                        child.Arrange(new Rect(new Point(finalSize.Width - usedScale * (rwidth + child.DesiredSize.Width), 0), new Size(child.DesiredSize.Width, finalSize.Height)));
                        rwidth += child.DesiredSize.Width;
                        continue;
                    }
                }

                child.Arrange(new Rect(new Point(usedScale * width, 0), new Size(child.DesiredSize.Width, finalSize.Height)));
                width += child.DesiredSize.Width;
            }
            return finalSize;
        }
    }
}
