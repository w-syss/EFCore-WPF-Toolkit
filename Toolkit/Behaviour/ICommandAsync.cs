using System.Threading.Tasks;

namespace Toolkit.Behaviour
{
    public interface ICommandAsync : IToolkitCommand
    {
        bool IsExecuting { get; }
        Task ExecuteAsync();
        bool CanExecute();
    }
}