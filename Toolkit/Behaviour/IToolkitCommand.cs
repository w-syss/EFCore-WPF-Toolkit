using System.Windows.Input;

namespace Toolkit.Behaviour
{
    public interface IToolkitCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
