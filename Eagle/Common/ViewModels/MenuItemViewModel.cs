using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Eagle.Common.ViewModels
{
    public sealed class MenuItemViewModel
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;
        
        public MenuItemViewModel(string header, Action action, Func<bool> canExecute = null)
            : this(header, action, canExecute, Enumerable.Empty<MenuItemViewModel>())
        {
        }

        public MenuItemViewModel(string header, params MenuItemViewModel[] subItems)
            : this(header, null, null, subItems)
        {
        }

        private MenuItemViewModel(string header, Action action, Func<bool> canExecute, IEnumerable<MenuItemViewModel> subItems)
        {
            _canExecute = canExecute;
            _action = action;
            SubItems = new ObservableCollection<MenuItemViewModel>(subItems);
            Header = header;
        }

        public string Header { get; private set; }

        public void InvokeAction()
        {
            if (_action != null)
                this._action();
        }

        public bool CanInvokeAction()
        {
            return _canExecute == null || _canExecute();
        }

        public ObservableCollection<MenuItemViewModel> SubItems { get; private set; }
    }
}
