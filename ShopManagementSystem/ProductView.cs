using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class ProductView : Form
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private async void Search_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductName.Text))
            {
                MessageBox.Show("Please enter product name", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await SearchProductByName();
        }

        private async Task SearchProductByName()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "http://localhost:3000/api/products";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Failed to connect to API.", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    ProductListResponse result = JsonConvert.DeserializeObject<ProductListResponse>(json);

                    if (result == null || !result.success || result.data == null || result.data.Count == 0)
                    {
                        MessageBox.Show("No product data found from API.", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string searchText = ProductName.Text.Trim().ToLower();

                    // 1. exact match first
                    var product = result.data.FirstOrDefault(p =>
                        !string.IsNullOrWhiteSpace(p.product_name) &&
                        p.product_name.Trim().Equals(searchText, StringComparison.OrdinalIgnoreCase));

                    // 2. partial match if exact match not found
                    if (product == null)
                    {
                        product = result.data.FirstOrDefault(p =>
                            !string.IsNullOrWhiteSpace(p.product_name) &&
                            p.product_name.ToLower().Contains(searchText));
                    }

                    if (product != null)
                    {
                        ProdName.Text = product.product_name;
                        Amount.Text = product.price.ToString();
                        VendorID.Text = product.brand;
                        ProductID.Text = product.product_id.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Product not found!!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ProdName.Clear();
            Amount.Clear();
            VendorID.Clear();
            ProductName.Clear();
            ProductID.Clear();
        }

        private void ProductView_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductView_Load(object sender, EventArgs e)
        {
            ProdName.ReadOnly = true;
            Amount.ReadOnly = true;
            VendorID.ReadOnly = true;
            ProductID.ReadOnly = true;

            ProdName.BackColor = System.Drawing.Color.LightGray;
            Amount.BackColor = System.Drawing.Color.LightGray;
            VendorID.BackColor = System.Drawing.Color.LightGray;
            ProductID.BackColor = System.Drawing.Color.LightGray;
        }
    }

    public class ProductListResponse
    {
        public bool success { get; set; }
        public List<ProductItem> data { get; set; }
    }

    public class ProductItem
    {
        public int product_id { get; set; }
        public int? php_product_id { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public int quantity { get; set; }
    }
}