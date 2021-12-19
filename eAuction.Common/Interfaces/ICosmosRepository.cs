using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface ICosmosRepository
    {
        Task<ItemResponse<T>> AddItem<T>(T item, string id, string partitionKey);

        Task<List<T>> QueryItems<T>(string query);

        Task<ItemResponse<T>> UpdateItem<T>(T item, string id, string partitionKey);

        Task<ItemResponse<T>> DeleteItem<T>(string id, string partitionKey);
    }
}
