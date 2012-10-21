using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace Eagle.FilePicker.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action executeAction) : this(executeAction, null)
        {
        }

        public DelegateCommand(Action executeAction, Func<bool> canExecute)
        {
            _canExecute = canExecute;
            _executeAction = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this._canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public void Execute(object parameter)
        {
            _executeAction();
        }
    }
}
