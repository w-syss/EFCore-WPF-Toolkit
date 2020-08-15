using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Threading.Tasks;
using Toolkit.Behaviour;

namespace Toolkit.Syncable
{
    public interface ISyncableObject : INotifyPropertyChanged
    {
        ///
        /// <summary>
        /// Set the <paramref name="newValue"/> to the <paramref name="property"/>
        /// and notify others of the change by invoking the <cref name="PropertyChanged"/> event.
        /// </summary>
        ///
        /// <param name="property">Reference to the backing field of the property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">
        /// Name of the changed property. This can be omitted, since the name
        /// will be resolved as the via the compiler name service./>.
        /// </param>
        ///
        void SetAndNotify<T>(ref T property, T newValue, string propertyName);

        ///
        /// <summary>
        /// Attempt to asynchronously write the changes made
        /// to this object since the last sync.
        /// This will only write the last changes made to this object and no
        /// in-between changes. If this operations is successful, this object
        /// will be marked as synced.
        /// </summary>
        ///
        /// <exception cref="DbUpdateException"><see cref="DbContext"/></exception>
        /// <exception cref="DbUpdateConcurrencyException"><see cref="DbContext"/></exception>
        ///
        /// <returns>
        /// Awaitable task which result will indicate
        /// if the operation was successful or not.
        /// </returns>
        ///
        Task<bool> Sync();

        ///
        /// <summary>
        /// Revert all changes made to this property since the last sync,
        /// without a round-trip to the database.
        /// </summary>
        ///
        void RevertAll();

        ///
        /// <summary>
        /// Revert the specified Property with name <paramref name ="propertyName"/>.
        /// If there is no property for the given name, nothing will happen.
        /// </summary>
        ///
        /// <param name="propertyName">Name of the property to revert.</param>
        ///
        void Revert(string propertyName);

        ///
        /// <summary>
        /// Checks if the the property with the given name
        /// was modified since the last sync.
        /// </summary>
        ///
        /// <param name="propertyName">Name of the property to check.</param>
        ///
        /// <returns>
        /// Boolean indicator whether or not the property was modified
        /// since the last sync. If the property with the given name
        /// doesn't exist, false is returned.
        /// </returns>
        bool IsModified(string propertyName);

        /// <summary>
        /// Read-only property which indicates whether this
        /// object is synced or not.
        /// </summary>
        bool IsSynced { get; }

        /// <summary>
        /// Wraps the <see cref="Sync"/> method.
        /// This should be used for binding.
        /// </summary>
        IToolkitCommand SyncCommand { get; }

        /// <summary>
        /// Wraps the <see cref="RevertAll"/> method.
        /// This should be used for binding.
        /// </summary>
        IToolkitCommand RevertAllCommand { get; }
    }
}