using Dapr.Actors;
using Dapr.Actors.Runtime;
using DemoActors.Interface;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DemoActors.Classes
{
    [Actor(TypeName = "ReminderActor")]
    public class ReminderActor : Actor, IReminderActor
    {
        private const string ReminderName = "MyReminder";
        private const string StateKey = "Counter";
        private const string LastOutputKey = "LastReminderOutput";

        public ReminderActor(ActorHost host) : base(host) { }

        public async Task StartReminderAsync()
        {
            await StateManager.SetStateAsync(StateKey, 0);

            // Register the reminder
            await RegisterReminderAsync(
                ReminderName,
                Encoding.UTF8.GetBytes("ReminderData"),
                dueTime: TimeSpan.FromSeconds(5),  // Delay before the first reminder
                period: TimeSpan.FromSeconds(5)    // Interval between reminders
            );
        }

        public async Task<string> GetReminderOutputAsync()
        {
            var state = await StateManager.GetStateAsync<string>(LastOutputKey);
            return state ?? "No reminder output";
        }

        public async Task ReminderCallback()
        {

            var currentState = await StateManager.GetStateAsync<int>(StateKey);
            currentState++;
            await StateManager.SetStateAsync(StateKey, currentState);
            var output = $"Reminder triggered at {DateTime.Now}, state updated to: {currentState}";
            await StateManager.SetStateAsync(LastOutputKey, output);
        }

        protected override async Task OnActivateAsync()
        {
            // Initialize the LastOutputKey state to avoid KeyNotFoundException
            if (!await StateManager.ContainsStateAsync(LastOutputKey))
            {
                await StateManager.SetStateAsync(LastOutputKey, "No reminder output");
            }
            await StateManager.SetStateAsync(StateKey, 0);

        }

        protected override async Task OnDeactivateAsync()
        {
            await UnregisterReminderAsync(ReminderName);
            await base.OnDeactivateAsync();
        }

        public async Task StopReminderAsync()
        { 
            await UnregisterReminderAsync("MyReminder");
        }

    }
}