using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace FileViewer
{
    public class VisualPresenter : UIElement
    {
        private readonly List<Visual> _visuals = new List<Visual>();

        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        public void AddVisual(Visual visual)
        {
            base.AddVisualChild(visual);
            _visuals.Add(visual);
        }
    }
}
