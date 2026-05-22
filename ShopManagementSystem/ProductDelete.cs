using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class ProductDelete : Form
    {
        private int selectedApiProductId = 0;

        public ProductDelete()
        {
            InitializeComponent();
        }

        private async void Searchbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductName.Text))
            {
                MessageBox.Show("Please enter product name", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await SearchProductFromApi();
        }

        private async Task SearchProductFromApi()
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
                    DeleteProductListResponse result = JsonConvert.DeserializeObject<DeleteProductListResponse>(json);

                    if (result == null || !result.success || result.data == null || result.data.Count == 0)
                    {
                        MessageBox.Show("No product data found from API.", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string searchText = ProductName.Text.Trim().ToLower();

                    var product = result.data.FirstOrDefault(p =>
                        !string.IsNullOrWhiteSpace(p.product_name) &&
                        p.product_name.Trim().ToLower() == searchText);

                    if (product == null)
                    {
                        product = result.data.FirstOrDefault(p =>
                            !string.IsNullOrWhiteSpace(p.product_name) &&
                            p.product_name.ToLower().Contains(searchText));
                    }

                    if (product != null)
                    {
                        selectedApiProductId = product.product_id;
                        ProductID.Text = product.product_id.ToString();
                    }
                    else
                    {
                        selectedApiProductId = 0;
                        ProductID.Clear();
                        MessageBox.Show("Product not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Deletebtn_Click(object sender, EventArgs e)
        {
            if (selectedApiProductId <= 0)
            {
                MessageBox.Show("Please search product first.", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this product?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            await DeleteProductFromApi();
        }

        private async Task DeleteProductFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "http://localhost:3000/api/products/" + selectedApiProductId;
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Product Deletion Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ProductName.Clear();
                        ProductID.Clear();
                        selectedApiProductId = 0;
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Product Deletion Failed\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProductDelete_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductDelete_Load(object sender, EventArgs e)
        {
            ProductID.ReadOnly = true;
            ProductID.BackColor = System.Drawing.Color.LightGray;
        }
    }

    public class DeleteProductListResponse
    {
        public bool success { get; set; }
        public List<DeleteProductItem> data { get; set; }
    }

    public class DeleteProductItem
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