using Dapr.Actors;

namespace DemoActors.Interface
{
    public interface ITimerActor : IActor
    {
        Task StartTimerAsync();
        Task TimerCallback();
        Task StopTimerAsync();
        Task<string> GetLastTimerOutputAsync();
    }
}
