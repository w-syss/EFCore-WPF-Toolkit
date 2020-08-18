using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Toolkit.Behaviour;
using Toolkit.Observable;

namespace Toolkit.Syncable
{
    /// <summary>
    /// Provides functionality to synchronize data to an external datastore
    /// and notification of changes made to the object.
    /// </summary>
    public interface ISyncableObject : IObservable
    {
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