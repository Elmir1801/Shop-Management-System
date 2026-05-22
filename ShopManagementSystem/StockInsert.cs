using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class StockInsert : Form
    {
        public StockInsert()
        {
            InitializeComponent();
        }

        private async void search_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductID.Text))
            {
                MessageBox.Show("Please enter Product ID", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await SearchProduct();
        }

        private async Task SearchProduct()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"http://localhost:3000/api/products/{ProductID.Text}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        ProductResponse result = JsonConvert.DeserializeObject<ProductResponse>(json);

                        if (result != null && result.success && result.data != null)
                        {
                            ProductName.Text = result.data.product_name;
                        }
                        else
                        {
                            MessageBox.Show("Product not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Product not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Add_Click(object sender, EventArgs e)
        {
            if (ProductID.Text == "" || Quantity.Text == "" || ProductName.Text == "")
            {
                MessageBox.Show("Please provide all the details", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int productId = Convert.ToInt32(ProductID.Text);
                int quantity = Convert.ToInt32(Quantity.Text);

                var stockData = new
                {
                    product_id = productId,
                    quantity = quantity
                };

                string json = JsonConvert.SerializeObject(stockData);

                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    string apiUrl = "http://localhost:3000/api/stocks";

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Stock Insertion Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ProductID.Clear();
                        Quantity.Clear();
                        ProductName.Clear();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Stock Insertion Failed\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            ProductID.Clear();
            Quantity.Clear();
            ProductName.Clear();
        }

        private void StockInsert_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StockInsert_Load(object sender, EventArgs e)
        {
            ProductName.ReadOnly = true;
            ProductName.BackColor = System.Drawing.Color.LightGray;
        }
    }
}