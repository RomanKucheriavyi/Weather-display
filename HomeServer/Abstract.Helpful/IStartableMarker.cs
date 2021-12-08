using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    /// <summary>
    ///     Used by Dependency Injection, to Start all services when app starts
    /// </summary>
    [A_InfrastructureIndependentHelper]
    public interface IStartableMarker
    {
        void Start();
    }

    /// <summary>
    ///     Used by Dependency Injection, to StartAsync all services when app starts
    /// </summary>
    [A_InfrastructureIndependentHelper]
    public interface IStartableAsyncMarker
    {
        Task StartAsync();
    }
}