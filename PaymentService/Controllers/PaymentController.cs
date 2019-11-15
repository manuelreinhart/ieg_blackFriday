using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        // POST api/values
        [HttpPost]
        public IEnumerable<Payment> Post([FromBody] string value)
        {

            var product1 = new Product()
            {
                Id = 1564,
                Name = "beer",
                Price = 2.70
            };
            var product2 = new Product()
            {
                Id = 1674,
                Name = "wine",
                Price = 3.40
            };

            var products = new List<Product>();
            products.Add(product1);
            products.Add(product2);

            var payment = new Payment()
            {
                Id = 1,
                Subject = "my super mega cool subject",
                Total = 666.42,
                Products = products
            };

            return new Payment[] { payment, payment };

        }             


    }
}
