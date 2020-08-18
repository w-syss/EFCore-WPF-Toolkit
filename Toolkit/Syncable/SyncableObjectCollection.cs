using Toolkit.Observable;

namespace Toolkit.Syncable
{
    public class SyncableObjectCollection<T> : ObservableObjectCollection<T> where T : class, ISyncableObject
    {
    }
}