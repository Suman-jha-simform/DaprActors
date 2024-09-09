using System;
using System.Text;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using DemoActors.Interface;

namespace DemoActors.Classes
{
    [Actor(TypeName = "TimerActor")]
    public class TimerActor : Actor, ITimerActor
    {
        private const string TimerName = "MyTimer";
        private const string StateKey = "TimerState";
        private const string LastOutputKey = "LastTimerOutput";

        public TimerActor(ActorHost host) : base(host)
        {
        }

        public async Task StartTimerAsync()
        {
            // Register the timer
            await RegisterTimerAsync(
                TimerName,
                callback: nameof(TimerCallback),
                Encoding.UTF8.GetBytes(""),
                dueTime: TimeSpan.FromSeconds(5),  
                period: TimeSpan.FromSeconds(5)   
            );

        }

        public async Task TimerCallback()
        {
            var currentState = await StateManager.GetStateAsync<int>(StateKey);
            currentState++;
            await StateManager.SetStateAsync(StateKey, currentState);
            var output = $"Timer triggered at {DateTime.Now}, state updated to: {currentState}";
            await StateManager.SetStateAsync(LastOutputKey, output);
        }

        public async Task<string> GetLastTimerOutputAsync()
        {
            var lastOutput = await StateManager.GetStateAsync<string>(LastOutputKey);
            return lastOutput ?? "No timer output available.";
        }

        public async Task StopTimerAsync()
        {
            await StateManager.SetStateAsync(StateKey, 0);
            await UnregisterTimerAsync(TimerName);
        }

        protected override async Task OnActivateAsync()
        {
            // Initialize the state if it doesn't exist
            if (!await StateManager.ContainsStateAsync(StateKey))
            {
                await StateManager.SetStateAsync(StateKey, 0);
            }
        }

        protected override async Task OnDeactivateAsync()
        {
            if (!await StateManager.ContainsStateAsync(StateKey))
            {
                await StateManager.SetStateAsync(StateKey, 0);
            }
            await UnregisterTimerAsync(TimerName);
            await base.OnDeactivateAsync();
        }
    }
}
