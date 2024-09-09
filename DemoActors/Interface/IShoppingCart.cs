using Dapr.Actors;
using Dapr.Actors.Runtime;
using System.Threading.Tasks;

namespace DemoActors.Interface
{
    public interface IShoppingCartActor : IActor
    {
        Task AddItemAsync(string item);
        Task RemoveItemAsync(string item);
        Task<List <string>> GetItemAsync();
    }
}
