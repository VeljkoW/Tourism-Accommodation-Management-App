using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace BookingApp.Validation
{
    public class StarAdorner : Adorner
    {
        private readonly VisualCollection _visuals;
        private readonly TextBlock _textBlock;

        public StarAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _visuals = new VisualCollection(this);
            _textBlock = new TextBlock
            {
                Text = "*",
                Foreground = Brushes.Red,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(-10, 0, 0, 0)
            };
            _visuals.Add(_textBlock);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _textBlock.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override int VisualChildrenCount => _visuals.Count;
    }
}
