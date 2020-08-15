using Microsoft.EntityFrameworkCore;
using System;

namespace Toolkit.DataStore
{
    public static class DataContextHandler
    {
        private static Func<DbContext> _contextCreator;

        public static DbContext CreateContext()
        {
            return _contextCreator?.Invoke() ?? throw new InvalidOperationException("No context set");
        }

        public static void SetContext<T>()
            where T : DbContext, new()
        {
            _contextCreator = () => new T();
        }
    }
}