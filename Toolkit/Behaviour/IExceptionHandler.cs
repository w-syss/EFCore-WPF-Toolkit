using System;

namespace Toolkit.Behaviour
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }
}