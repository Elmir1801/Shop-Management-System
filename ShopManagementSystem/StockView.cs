using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class StockView : Form
    {
        public StockView()
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

            await ViewStock();
        }

        private async Task ViewStock()
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
                        StockSingleResponse result = JsonConvert.DeserializeObject<StockSingleResponse>(json);

                        if (result != null && result.success && result.data != null)
                        {
                            Productname.Text = result.data.product_name;
                            Quantity.Text = result.data.quantity.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Stock not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Stock not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            ProductID.Clear();
            Productname.Clear();
            Quantity.Clear();
        }

        private void StockView_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StockView_Load(object sender, EventArgs e)
        {
            Productname.ReadOnly = true;
            Productname.BackColor = System.Drawing.Color.LightGray;
            Quantity.ReadOnly = true;
            Quantity.BackColor = System.Drawing.Color.LightGray;
        }
    }

    public class StockSingleResponse
    {
        public bool success { get; set; }
        public StockSingleData data { get; set; }
    }

    public class StockSingleData
    {
        public int stock_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public DateTime last_updated { get; set; }
    }
}