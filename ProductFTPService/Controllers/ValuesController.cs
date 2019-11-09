using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductFTPService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string ProductFilePath = "./Data/Products.json";

        // GET api/Products
        [HttpGet, ResponseCache(Duration = 10 * 60)]
        public ActionResult<IEnumerable<string>> Get()
        {

            return this.GetProducts().Select(p => p.Name).ToArray();                    
        }

        private Product[] GetProducts()
        {
            using (StreamReader r = new StreamReader(this.ProductFilePath))
            {
                string json = r.ReadToEnd();
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(json);
                return products.ToArray();
            }
        }

        private class Product
        {
            public string Name { get; set; }
        }
                
    }
}
