using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileViewer
{
    public class AutogeneratingPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }
            return Size.Empty;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var position = new Point();
            foreach (UIElement child in Children)
            {
                var remainingVerticalSpace = finalSize.Height - position.Y;
                var childHeight = Math.Min(child.DesiredSize.Height, remainingVerticalSpace);
                var childRect = new Rect(position, new Size(finalSize.Width, childHeight));
                child.Arrange(childRect);
                position.Y += childHeight;
                if (position.Y >= finalSize.Height)
                    return finalSize;
            }

            // If we have a generator and there's more space for them,
            // generate new items using the generator
            if (ItemGenerator != null)
            {
                while (position.Y < finalSize.Height)
                {
                    var remainingVerticalSpace = finalSize.Height - position.Y;
                    var newChild = ItemGenerator();
                    newChild.Measure(new Size(finalSize.Width, remainingVerticalSpace));
                    var childHeight = Math.Min(newChild.DesiredSize.Height, remainingVerticalSpace);
                    newChild.Arrange(new Rect(position, new Size(finalSize.Width, childHeight)));
                }
            }

            return finalSize;
        }

        public Func<UIElement> ItemGenerator { get; set; }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }
    }
}
