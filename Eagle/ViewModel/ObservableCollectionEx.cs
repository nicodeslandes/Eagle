using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eagle.ViewModel
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
        }

        public void AddRange(IList<T> items)
        {
            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            this.OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, items));
            this.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Items[]"));
        }
    }
}
