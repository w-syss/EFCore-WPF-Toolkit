using System;

namespace Toolkit.Syncable
{
    public class DefaultResetAction<T> : IResetAction
    {
        private readonly Action _undoAction;

        public DefaultResetAction(object target, string propertyName, T oldValue)
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
        public void ResetProperty()
        {
            _undoAction.Invoke();
        }
        #endregion
    }
}