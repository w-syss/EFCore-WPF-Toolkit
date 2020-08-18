using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Toolkit.Behaviour;
using Toolkit.DataStore;
using Toolkit.Observable;

namespace Toolkit.Syncable
{
    /// <inheritdoc/>
    public abstract class SyncableObject : ObservableObject, ISyncableObject
    {
        #region Fields
        private readonly HashSet<string> _modifiedProperties;
        private readonly Dictionary<string, IResetAction> _resetCommands;
        #endregion

        #region Constructor
        public SyncableObject()
        {
            _modifiedProperties = new HashSet<string>();
            _resetCommands = new Dictionary<string, IResetAction>();
            IsSynced = true;

            PropertyChanged += OnPropertyChanged;

            SyncCommand = new DelegateCommandAsync(Sync, () => IsSynced);
            RevertAllCommand = new DelegateCommand(RevertAll, () => !IsSynced);
        }
        #endregion

        #region Local methods
        private void SetSyncStatus()
        {
            IsSynced = _modifiedProperties.Count == 0;

            SyncCommand.TriggerCanExecuteChanged();
            RevertAllCommand.TriggerCanExecuteChanged();
        }

        private void MarkPropertyModified(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            _modifiedProperties.Add(propertyName);
            NotifyPropertyChanged(propertyName);
        }

        private void MarkPropertyUnmodified(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            _modifiedProperties.Remove(propertyName);
            NotifyPropertyChanged(propertyName);
        }

        private void MarkAllPropertiesUnmodified()
        {
            _modifiedProperties.Clear();
            SetSyncStatus();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            SetSyncStatus();
        }
        #endregion

        #region Overrides : ObservableObject
        public override void SetAndNotify<T>(ref T property, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(property, newValue))
            {
                return;
            }

            if (IsSynced)
            {
                var currentValue = EqualityComparer<T>.Default.Equals(property, default) ? newValue : property;
                _resetCommands[propertyName] = new DefaultResetAction<T>(this, propertyName, currentValue);
            }

            property = newValue;
            MarkPropertyModified(propertyName);
        }
        #endregion

        #region Implementation : ISyncableObject
        public bool IsSynced { get; private set; }

        public bool IsModified(string propertyName)
        {
            return _modifiedProperties.Contains(propertyName);
        }

        public void RevertAll()
        {
            var modifiedProperties = new List<string>(_modifiedProperties);
            foreach (var propertyName in modifiedProperties)
            {
                Revert(propertyName);
            }
        }

        public void Revert(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            if (_resetCommands.TryGetValue(propertyName, out var resetCommand))
            {
                resetCommand.ResetProperty();
                MarkPropertyUnmodified(propertyName);
            }
        }

        public async Task<bool> Sync()
        {
            using var context = DataContextHandler.CreateContext();

            context.Attach(this);

            foreach (var propertyName in _modifiedProperties)
            {
                context
                    .Entry(this)
                    .Property(propertyName)
                    .IsModified = true;
            }

            var result = await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            if (result > 0)
            {
                MarkAllPropertiesUnmodified();
                return true;
            }
            else
            {
                return false;
            }
        }

        public IToolkitCommand SyncCommand { get; }
        public IToolkitCommand RevertAllCommand { get; }
        #endregion
    }
}