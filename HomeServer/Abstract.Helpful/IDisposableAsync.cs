using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    [A_InfrastructureIndependentHelper]
    public interface IDisposableAsync
    {
        Task DisposeAsync();
    }
}