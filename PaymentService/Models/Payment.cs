using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models
{
    public class Payment
    {
        public int Id;
        public string Subject;
        public double Total;
        public List<Product> Products;
    }

    public class Product
    {
        public int Id;
        public string Name;
        public double Price;
    }
}
