using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Eagle.Behavior
{
    public class ScrollToEndBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty ScrollToEndOnNewItemsProperty = DependencyProperty.Register("ScrollToEndOnNewItems", typeof(bool), typeof(ScrollToEndBehavior), new UIPropertyMetadata(false, new PropertyChangedCallback(OnScrollToEndOnNewItemsChanged)));

        private static void OnScrollToEndOnNewItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ScrollToEndBehavior scrollToEndBehavior = o as ScrollToEndBehavior;
            if (scrollToEndBehavior != null)
                scrollToEndBehavior.OnScrollToEndOnNewItemsChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnScrollToEndOnNewItemsChanged(bool oldValue, bool newValue)
        {
            if (this.AssociatedObject != null)
            {
                EnableOrDisableScrollToEnd(newValue);
            }
        }

        private void EnableOrDisableScrollToEnd(bool enable)
        {
            if (enable)
            {
                this.AssociatedObject.SetBinding(ScrollToEndBehavior.ItemsProperty, new Binding("Items") { RelativeSource = RelativeSource.Self });
                var items = this.AssociatedObject.Items;
                if (items != null && items.Count > 0)
                {
                    this.AssociatedObject.ScrollIntoView(items[items.Count - 1]);
                }
            }
            else
            {
                SetItems(this.AssociatedObject, null);
            }
        }

        public bool ScrollToEndOnNewItems
        {
            get { return (bool)GetValue(ScrollToEndOnNewItemsProperty); }
            set { SetValue(ScrollToEndOnNewItemsProperty, value); }
        }
        
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached("Items", typeof(ItemCollection), typeof(ScrollToEndBehavior), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemsChanged)));

        public static ItemCollection GetItems(DependencyObject target)
        {
            return (ItemCollection)target.GetValue(ItemsProperty);
        }
        
        public static void SetItems(DependencyObject target, ItemCollection value)
        {
            target.SetValue(ItemsProperty, value);
        }

        private static void OnItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            OnItemsChanged(o, (ItemCollection)e.OldValue, (ItemCollection)e.NewValue);
        }

        private static void OnItemsChanged(DependencyObject o, ItemCollection oldValue, ItemCollection newValue)
        {
            if (oldValue != null)
            {
                var oldEventHandler = GetCollectionChangedEventHandler(o);
                if (oldEventHandler != null)
                {
                    ((INotifyCollectionChanged)oldValue).CollectionChanged -= oldEventHandler;
                }
            }

            var collection = (INotifyCollectionChanged)newValue;
            if (collection != null)
            {
                NotifyCollectionChangedEventHandler collectionChangedEventHandler =
                    (sender, ea) =>
                    {
                        if (ea.Action == NotifyCollectionChangedAction.Add)
                        {
                            ((ListBox)o).ScrollIntoView(ea.NewItems[0]);
                        }
                    };
                collection.CollectionChanged += collectionChangedEventHandler;
                SetCollectionChangedEventHandler(o, collectionChangedEventHandler);
            }
        }

        public static readonly DependencyProperty CollectionChangedEventHandlerProperty = DependencyProperty.RegisterAttached("CollectionChangedEventHandler", typeof(NotifyCollectionChangedEventHandler), typeof(ScrollToEndBehavior), new UIPropertyMetadata(null));

        public static NotifyCollectionChangedEventHandler GetCollectionChangedEventHandler(DependencyObject target)
        {
            return (NotifyCollectionChangedEventHandler)target.GetValue(CollectionChangedEventHandlerProperty);
        }
        public static void SetCollectionChangedEventHandler(DependencyObject target, NotifyCollectionChangedEventHandler value)
        {
            target.SetValue(CollectionChangedEventHandlerProperty, value);
        }

        protected override void OnAttached()
        {
            this.EnableOrDisableScrollToEnd(this.ScrollToEndOnNewItems);
        }
    }
}
