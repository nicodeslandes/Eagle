using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Eagle.FilePicker.ViewModels
{
    public sealed class MenuItemViewModel
    {
        public MenuItemViewModel(string header, ICommand command)
            : this(header, command, Enumerable.Empty<MenuItemViewModel>())
        {
        }

        public MenuItemViewModel(string header, params MenuItemViewModel[] subItems)
            : this(header, null, subItems)
        {
        }

        private MenuItemViewModel(string header, ICommand command, IEnumerable<MenuItemViewModel> subItems)
        {
            SubItems = new ObservableCollection<MenuItemViewModel>(subItems);
            Command = command;
            Header = header;
        }
        public string Header { get; private set; }

        public ICommand Command { get; private set; }

        public ObservableCollection<MenuItemViewModel> SubItems { get; private set; }
    }
}
