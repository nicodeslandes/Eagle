using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Eagle.Behaviors
{
    public static class SelectOnPreviewMouseClickBehavior
    {
        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for Enabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(SelectOnPreviewMouseClickBehavior), new PropertyMetadata(false, OnEnableChanged));


        static void OnPreviewMouseDown(UIElement element, System.Windows.Input.MouseButtonEventArgs e)
        {
            var menuItem = element as MenuItem;
            if (menuItem != null)
            {
                menuItem.IsChecked = true;
                return;
            }

            var treeItem = element as TreeViewItem;
            if (treeItem != null)
            {
                treeItem.IsSelected = true;
            }
        }

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if ((bool)e.NewValue)
                element.PreviewMouseDown += (_, ea) => OnPreviewMouseDown(element, ea);
            
        }
    }
}
