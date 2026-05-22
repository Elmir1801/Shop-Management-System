using System.Collections.Generic;

namespace ShopManagementSystem
{
    public class ProductResponse
    {
        public bool success { get; set; }
        public ProductData data { get; set; }
    }

    public class ProductData
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public int quantity { get; set; }
    }

    public class ApiMessageResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}