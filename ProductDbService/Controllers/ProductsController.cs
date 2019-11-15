using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace ProductDbService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static readonly string EndpointUri = "https://iegproductsdb.documents.azure.com:443/";
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private string databaseId = "ProductDatabase";
        private string containerId = "ProductContainer";

        private IConfiguration _configuration;

        public ProductsController(IConfiguration configuration) : base()
        {
            _configuration = configuration;
            this.InitDatabaseConnectionAsync();
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            return await GetProductsFromDB();
        }

        public async Task InitDatabaseConnectionAsync()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, _configuration["PrimaryKey"]);
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/Name");
        }

        private async Task<String[]> GetProductsFromDB()
        {
            if (this.container == null)
                await this.InitDatabaseConnectionAsync();

            var sqlQueryText = "SELECT * FROM c";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Product> queryResultSetIterator = this.container.GetItemQueryIterator<Product>(queryDefinition);

            List<Product> products = new List<Product>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Product> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Product product in currentResultSet)
                {
                    products.Add(product);
                    Console.WriteLine("\tRead {0}\n", product);
                }
            }
            return products.Select(p => p.Name).ToArray();
        }

        private class Product
        {
            public string Name { get; set; }
        }

    }
}
