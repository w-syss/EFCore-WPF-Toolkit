using System;
using System.Windows.Input;

namespace Toolkit.Syncable
{
    public class ResetCommand<T> : ICommand
    {
        private readonly Action _undoAction;

        public ResetCommand(object target, string propertyName, T oldValue)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var setter = target
                .GetType()
                .GetProperty(propertyName)?
                .GetSetMethod() ?? throw new ArgumentException($"Public property with name {propertyName} doesn't exist.");

            _undoAction = () => setter.Invoke(target, new object[] { oldValue });
        }

        #region Implementation : ICommand
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _undoAction.Invoke();
        }
        #endregion
    }
}