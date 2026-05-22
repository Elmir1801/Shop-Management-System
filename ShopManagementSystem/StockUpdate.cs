using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class StockUpdate : Form
    {
        public StockUpdate()
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

            await SearchStock();
        }

        private async Task SearchStock()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"http://localhost:3000/api/stocks/{ProductID.Text}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        SingleStockApiResponse result = JsonConvert.DeserializeObject<SingleStockApiResponse>(json);

                        if (result != null && result.success && result.data != null)
                        {
                            ProductName.Text = result.data.product_name;
                            Quantity.Text = result.data.quantity.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Stock record not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Stock record not found\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductID.Text) ||
                string.IsNullOrWhiteSpace(Quantity.Text) ||
                string.IsNullOrWhiteSpace(ProductName.Text))
            {
                MessageBox.Show("Please provide all the details", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int productId = Convert.ToInt32(ProductID.Text);
                int quantity = Convert.ToInt32(Quantity.Text);

                if (quantity < 0)
                {
                    MessageBox.Show("Quantity cannot be negative", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var stockData = new
                {
                    quantity = quantity
                };

                string json = JsonConvert.SerializeObject(stockData);

                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    string apiUrl = $"http://localhost:3000/api/stocks/{productId}";

                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Stock Updation Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ProductID.Clear();
                        Quantity.Clear();
                        ProductName.Clear();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Stock Updation Failed\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values.", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            ProductID.Clear();
            ProductName.Clear();
            Quantity.Clear();
        }

        private void StockUpdate_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StockUpdate_Load(object sender, EventArgs e)
        {
            ProductName.ReadOnly = true;
            ProductName.BackColor = System.Drawing.Color.LightGray;
        }

        private void Quantity_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class SingleStockApiResponse
    {
        public bool success { get; set; }
        public SingleStockData data { get; set; }
    }

    public class SingleStockData
    {
        public int stock_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public DateTime last_updated { get; set; }
    }
}