using System;

namespace Toolkit.Behaviour
{
    public sealed class DelegateCommand : IToolkitCommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _action;

        #region Constructors
        public DelegateCommand(Action action) : this(action, null)
        {
        }

        public DelegateCommand(Action action, Func<bool> canExecute)
        {
            _canExecute = canExecute;
            _action = action;
        }
        #endregion

        #region Events
        public event EventHandler CanExecuteChanged;
        #endregion

        #region Implementation : ICommand
        public bool CanExecute(object parameter = null)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter = null)
        {
            _action();
        }
        #endregion

        #region Implementation : IToolkitCommand
        public void TriggerCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}