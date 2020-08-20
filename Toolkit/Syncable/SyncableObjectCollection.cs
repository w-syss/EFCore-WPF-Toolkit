using System.Threading.Tasks;
using Toolkit.DataStore;
using Toolkit.Observable;

namespace Toolkit.Syncable
{
    public class SyncableObjectCollection<T> : ObservableObjectCollection<T> where T : class, ISyncableObject
    {

        public async Task<bool> RemoveFromDataStore(T item)
        {
            using var context = DataContextHandler.CreateContext();
            context.Remove(item);

            var result = await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return result > 0;
        }
    }
}