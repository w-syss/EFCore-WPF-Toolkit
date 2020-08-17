using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Toolkit.Extensions;

namespace Toolkit.Behaviour
{
    public sealed class DelegateCommandAsync : ICommandAsync
    {
        #region Fields
        private bool _isExecuting;
        private readonly Func<Task> _functionToExecute;
        private readonly Func<bool> _canExecuteFunction;
        private readonly IExceptionHandler _exceptionHandler;
        #endregion

        #region Properties
        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                if (value != _isExecuting)
                {
                    _isExecuting = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region Constructors
        public DelegateCommandAsync(Func<Task> functionToExecute) : this(functionToExecute, null, null)
        {
        }

        public DelegateCommandAsync(Func<Task> functionToExecute, Func<bool> canExecuteFunction) : this(functionToExecute, canExecuteFunction, null)
        {
        }

        public DelegateCommandAsync(Func<Task> functionToExecute, IExceptionHandler exceptionHandler) : this(functionToExecute, null, exceptionHandler)
        {
        }

        public DelegateCommandAsync(Func<Task> functionToExecute, Func<bool> canExecuteFunction, IExceptionHandler exceptionHandler)
        {
            _functionToExecute = functionToExecute;
            _canExecuteFunction = canExecuteFunction;
            _exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Events
        public event EventHandler CanExecuteChanged;
        #endregion

        #region Implementation : ICommand
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().SafelyIgnoreResultAsync(_exceptionHandler);
        }
        #endregion

        #region Implementation : ICommandAsync
        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    IsExecuting = true;
                    await _functionToExecute().ConfigureAwait(true);
                }
                finally
                {
                    IsExecuting = false;
                }
            }
        }
        public bool CanExecute()
        {
            return !_isExecuting && (_canExecuteFunction?.Invoke() ?? true);
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