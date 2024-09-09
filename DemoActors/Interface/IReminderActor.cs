using Dapr.Actors;
using System;
using System.Threading.Tasks;

namespace DemoActors.Interface
{
    public interface IReminderActor : IActor
    {
        Task StartReminderAsync();
        Task ReminderCallback();
        Task<string> GetReminderOutputAsync();
        Task StopReminderAsync();

    }
}