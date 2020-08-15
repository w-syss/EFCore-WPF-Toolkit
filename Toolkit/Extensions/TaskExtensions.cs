using System;
using System.Threading.Tasks;
using Toolkit.Behaviour;

namespace Toolkit.Extensions
{
    public static class TaskExtensions
    {
        public static async void SafelyIgnoreResultAsync(this Task task, IExceptionHandler handler = null)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                handler?.HandleException(ex);
            }
        }
    }
}