using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class StockDelete : Form
    {
        public StockDelete()
        {
            InitializeComponent();
        }

        private async void Search_Click(object sender, EventArgs e)
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
                        SingleStockDeleteResponse result =
                            JsonConvert.DeserializeObject<SingleStockDeleteResponse>(json);

                        if (result != null && result.success && result.data != null)
                        {
                            ProductName.Text = result.data.product_name;

                            if (result.data.quantity <= 0)
                            {
                                MessageBox.Show("Product has no stock", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Product not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Product not found\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductID.Text))
            {
                MessageBox.Show("Please enter Product ID", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // first check stock before delete
                    string getUrl = $"http://localhost:3000/api/stocks/{ProductID.Text}";
                    HttpResponseMessage getResponse = await client.GetAsync(getUrl);

                    if (!getResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Product not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string json = await getResponse.Content.ReadAsStringAsync();
                    SingleStockDeleteResponse result =
                        JsonConvert.DeserializeObject<SingleStockDeleteResponse>(json);

                    if (result == null || !result.success || result.data == null)
                    {
                        MessageBox.Show("Product not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ProductName.Text = result.data.product_name;

                    if (result.data.quantity <= 0)
                    {
                        MessageBox.Show("Product has no stock", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult confirm = MessageBox.Show(
                        "Are you sure you want to delete this stock?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirm != DialogResult.Yes)
                        return;

                    string deleteUrl = $"http://localhost:3000/api/stocks/{ProductID.Text}";
                    HttpResponseMessage deleteResponse = await client.DeleteAsync(deleteUrl);

                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Stock Deletion Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        RefreshOpenReportStocks();

                        ProductID.Clear();
                        ProductName.Clear();
                    }
                    else
                    {
                        string error = await deleteResponse.Content.ReadAsStringAsync();
                        MessageBox.Show("Stock Deletion Failed\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshOpenReportStocks()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is ReportStocks reportStocks)
                {
                    reportStocks.RefreshStocksFromOutside();
                    break;
                }
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ProductID.Clear();
            ProductName.Clear();
        }

        private void StockDelete_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StockDelete_Load(object sender, EventArgs e)
        {
            ProductName.ReadOnly = true;
            ProductName.BackColor = System.Drawing.Color.LightGray;
        }
    }

    public class SingleStockDeleteResponse
    {
        public bool success { get; set; }
        public SingleStockDeleteData data { get; set; }
    }

    public class SingleStockDeleteData
    {
        public int stock_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public DateTime last_updated { get; set; }
    }
}