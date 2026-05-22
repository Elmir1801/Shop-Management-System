using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagementSystem
{
    public class ProductApiResponse
    {
        public bool success { get; set; }
        public List<ProductApiModel> data { get; set; }
    }

    public class ProductApiModel
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
    }
}