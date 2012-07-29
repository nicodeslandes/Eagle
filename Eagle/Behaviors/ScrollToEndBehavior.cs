using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Eagle.Behaviors
{
    public class ScrollToEndBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty ScrollToEndOnNewItemsProperty = DependencyProperty.Register("ScrollToEndOnNewItems", typeof(bool), typeof(ScrollToEndBehavior), new UIPropertyMetadata(false, new PropertyChangedCallback(OnScrollToEndOnNewItemsChanged)));

        private ScrollViewer _scrollViewer;
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
                ScheduleScrollToEnd();
            }
            else
            {
                SetItems(this.AssociatedObject, null);
                CancelNextScrollToEnd();
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
                            var b = Interaction.GetBehaviors(o)
                                .OfType<ScrollToEndBehavior>().Single();
                            b.ScheduleScrollToEnd();
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
            _scrollViewer = GetScrollViewer(this.AssociatedObject);
            if (_scrollViewer == null)
            {
                EventHandler onLayoutUpdated = null;
                onLayoutUpdated = (e, s) =>
                {
                    _scrollViewer = GetScrollViewer(this.AssociatedObject);
                    if (_scrollViewer != null) this.AssociatedObject.LayoutUpdated -= onLayoutUpdated;
                };

                this.AssociatedObject.LayoutUpdated += onLayoutUpdated;
                this.AssociatedObject.UpdateLayout();
            }

            this.EnableOrDisableScrollToEnd(this.ScrollToEndOnNewItems);
        }

        private static ScrollViewer GetScrollViewer(DependencyObject element)
        {
            var count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is ScrollViewer)
                {
                    return (ScrollViewer)child;
                }

                var scrollViewer = GetScrollViewer(child);
                if (scrollViewer != null)
                {
                    return scrollViewer;
                }
            }

            return null;
        }

        Task _nextScrollTask = null;
        DateTime _lastScrollTime = DateTime.MinValue;
        object _scrollSync = new object();
        static private readonly TimeSpan MinScrollDelay = TimeSpan.FromMilliseconds(200);

        private void ScheduleScrollToEnd()
        {
            lock (_scrollSync)
            {
                if (_nextScrollTask != null)
                {
                    Debug.WriteLine("Scrolling pending; skipping");
                    return;
                }

                if (DateTime.Now - _lastScrollTime < MinScrollDelay)
                {
                    Debug.WriteLine("Only {0} ms since last scroll; scheduling next scroll in {1} ms", (DateTime.Now - _lastScrollTime).TotalMilliseconds, MinScrollDelay.TotalMilliseconds);
                    var context = SynchronizationContext.Current;
                    _nextScrollTask = Task.Delay(MinScrollDelay).ContinueWith(_ => context.Post(a => DoScrollToEnd(), null));
                    return;
                }
            }

            Debug.WriteLine("Scrolling now");
            DoScrollToEnd();
        }

        private void DoScrollToEnd(bool checkScrollViewer = true)
        {
            if (checkScrollViewer && _scrollViewer == null)
            {
                EventHandler handler = null;
                handler = (s, e) =>
                {
                    if (_scrollViewer == null)
                    {
                        _scrollViewer = GetScrollViewer(this.AssociatedObject);
                    }

                    DoScrollToEnd(false);
                    this.AssociatedObject.LayoutUpdated -= handler;
                };

                this.AssociatedObject.LayoutUpdated += handler;
                this.AssociatedObject.UpdateLayout();
                return;
            }

            if (_scrollViewer != null)
            {
                Debug.WriteLine("Scrolling using scroll viewer");
                _scrollViewer.ScrollToEnd();
            }
            else
            {
                var items = this.AssociatedObject.Items;
                if (items != null && items.Count > 0)
                {
                    Debug.WriteLine("Scrolling to end");
                    this.AssociatedObject.ScrollIntoView(items[items.Count - 1]);
                }
            }

            lock (_scrollSync)
            {
                _lastScrollTime = DateTime.Now;
                _nextScrollTask = null;
            }
        }

        private void CancelNextScrollToEnd()
        {
        }
    }
}
