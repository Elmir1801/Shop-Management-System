using DGVPrinterHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class ReportStocks : Form
    {
        public ReportStocks()
        {
            InitializeComponent();
        }

        private async void ReportStocks_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            await LoadStocksFromApi();
        }

        public async void RefreshStocksFromOutside()
        {
            await LoadStocksFromApi();
        }

        private async Task LoadStocksFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "http://localhost:3000/api/stocks";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        StockApiResponse result = JsonConvert.DeserializeObject<StockApiResponse>(json);

                        if (result != null && result.success && result.data != null)
                        {
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = result.data;

                            if (dataGridView1.Columns["stock_id"] != null)
                                dataGridView1.Columns["stock_id"].HeaderText = "Stock ID";

                            if (dataGridView1.Columns["product_id"] != null)
                                dataGridView1.Columns["product_id"].HeaderText = "Product ID";

                            if (dataGridView1.Columns["product_name"] != null)
                                dataGridView1.Columns["product_name"].HeaderText = "Product Name";

                            if (dataGridView1.Columns["quantity"] != null)
                                dataGridView1.Columns["quantity"].HeaderText = "Quantity";

                            if (dataGridView1.Columns["last_updated"] != null)
                                dataGridView1.Columns["last_updated"].HeaderText = "Last Updated";
                        }
                        else
                        {
                            MessageBox.Show("No stock data found from API.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to load stocks from API.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message);
            }
        }

        private void print_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No records to print.");
                return;
            }

            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Available Stock Report";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date.ToString("MM/dd/yyyy"));
            printer.SubTitleFormatFlags = System.Drawing.StringFormatFlags.LineLimit | System.Drawing.StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = System.Drawing.StringAlignment.Near;
            printer.Footer = "Shop Management System";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = false;
            printer.PrintDataGridView(dataGridView1);
        }

        private void ReportStocks_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class StockApiResponse
    {
        public bool success { get; set; }
        public List<StockApiModel> data { get; set; }
    }

    public class StockApiModel
    {
        public int stock_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public DateTime last_updated { get; set; }
    }
}