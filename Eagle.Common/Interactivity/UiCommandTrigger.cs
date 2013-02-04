using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Eagle.Interactivity
{
    public class UiCommandTrigger : TriggerBase<UIElement>
    {
        private CommandBinding _commandBinding;
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(UiCommandTrigger), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            _commandBinding = new CommandBinding(this.Command);
            _commandBinding.Executed += this.OnCommandExecuted;

            this.AssociatedObject.CommandBindings.Add(_commandBinding);
        }

        private void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.InvokeActions(e.Parameter);
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.CommandBindings.Remove(_commandBinding);
            _commandBinding = null;
            base.OnDetaching();
        }

    }
}
