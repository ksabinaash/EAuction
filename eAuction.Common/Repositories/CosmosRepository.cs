using eAuction.Common.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace eAuction.Common.Repositories
{
    public class CosmosRepository : ICosmosRepository
    {
        // The Azure Cosmos DB endpoint for running this sample.
        //private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];
        private static readonly string EndpointUri = "https://localhost:8081";

        // The primary key for the Azure Cosmos account.
        //private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
        private static readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "EAuction";

        private string containerId = "Product";

        public CosmosRepository()
        {
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "eAuction" });            
        }        

        public async Task<ItemResponse<T>> AddItem<T>(T item, string id, string partitionKey)
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/ProductName", 400);

            ItemResponse<T> response;

            try
            {
                // Read the item to see if it exists.  
                response = await this.container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container
                response = await this.container.CreateItemAsync<T>(item, new PartitionKey(partitionKey));
            }

            return response;
        }
        
        public async Task<List<T>> QueryItems<T>(string sqlQueryText)
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/ProductName", 400);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            FeedIterator<T> queryResultSetIterator = this.container.GetItemQueryIterator<T>(queryDefinition);

            List<T> items = new List<T>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach (T item in currentResultSet)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public async Task<ItemResponse<T>> UpdateItem<T>(T item, string id, string partitionKey)
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/ProductName", 400);

            ItemResponse<T> response = await this.container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));

            var itemBody = response.Resource;

            itemBody = item;

            // replace the item with the updated content
            response = await this.container.ReplaceItemAsync<T>(itemBody, id, new PartitionKey(partitionKey));

            return response;
        }

        public async Task<ItemResponse<T>> DeleteItem<T>(string id, string partitionKey)
        {
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/ProductName", 400);

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<T> response = await this.container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));

            return response;
        }
    }
}
