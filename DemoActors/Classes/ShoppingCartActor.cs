using Dapr.Actors.Runtime;
using DemoActors.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoActors.Classes
{
    [Actor(TypeName = "ShoppingCartActor")]
    public class ShoppingCartActor : Actor, IShoppingCartActor
    {
        public const string _stateName = "GetItems";

        public ShoppingCartActor(ActorHost host) : base(host)
        {
        }

        public async Task AddItemAsync(string item)
        {
            var items = await StateManager.GetOrAddStateAsync(_stateName, new List<string>());
            items.Add(item);
            await StateManager.SetStateAsync(_stateName, items);
        }

        public async Task RemoveItemAsync(string item)
        {
            var items = await StateManager.GetOrAddStateAsync(_stateName, new List<string>());
            items.Remove(item);
            await StateManager.SetStateAsync(_stateName, items);
        }

        public Task<List<string>> GetItemAsync()
        {
            return StateManager.GetOrAddStateAsync(_stateName, new List<string>());
        }

    }
}
