using DGVPrinterHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class ReportProduct : Form
    {
        string apiUrl = "http://localhost:3000/api/products";
        string imageFolder = @"C:\xampp\htdocs\myprojects\Github_Project\online-shopping-system\admin\product_images\";

        public ReportProduct()
        {
            InitializeComponent();
        }

        private async void ReportProduct_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            textBoxtxtProductName.ReadOnly = true;
            textBoxtxtProductName.BackColor = Color.White;
            textBoxtxtProductName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            await LoadProductsFromApi();
        }

        private async Task LoadProductsFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        ProductApiResponse result =
                            JsonConvert.DeserializeObject<ProductApiResponse>(json);

                        if (result != null && result.success && result.data != null)
                        {
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = result.data;
                        }
                        else
                        {
                            MessageBox.Show("No products found");
                        }
                    }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                    return;

                // DISPLAY PRODUCT NAME
                if (dataGridView1.Columns.Contains("product_name"))
                {
                    textBoxtxtProductName.Text =
                        dataGridView1.CurrentRow.Cells["product_name"].Value?.ToString();
                }

                // DISPLAY IMAGE
                if (!dataGridView1.Columns.Contains("image"))
                    return;

                string imageName =
                    dataGridView1.CurrentRow.Cells["image"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(imageName))
                {
                    pictureBox1.Image = null;
                    return;
                }

                string imagePath = Path.Combine(imageFolder, imageName);

                // CHECK NORMAL IMAGE
                if (File.Exists(imagePath))
                {
                    pictureBox1.ImageLocation = imagePath;
                }
                else
                {
                    // TRY JPG
                    string jpgPath = Path.Combine(
                        imageFolder,
                        Path.GetFileNameWithoutExtension(imageName) + ".jpg"
                    );

                    // TRY PNG
                    string pngPath = Path.Combine(
                        imageFolder,
                        Path.GetFileNameWithoutExtension(imageName) + ".png"
                    );

                    // TRY JPEG
                    string jpegPath = Path.Combine(
                        imageFolder,
                        Path.GetFileNameWithoutExtension(imageName) + ".jpeg"
                    );

                    if (File.Exists(jpgPath))
                    {
                        pictureBox1.ImageLocation = jpgPath;
                    }
                    else if (File.Exists(pngPath))
                    {
                        pictureBox1.ImageLocation = pngPath;
                    }
                    else if (File.Exists(jpegPath))
                    {
                        pictureBox1.ImageLocation = jpegPath;
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Image Error: " + ex.Message);
            }
        }
        private async void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                Product product = new Product
                {
                    product_id = Convert.ToInt32(row.Cells["product_id"].Value),
                    product_name = row.Cells["product_name"].Value?.ToString(),
                    description = row.Cells["description"].Value?.ToString(),
                    price = Convert.ToDecimal(row.Cells["price"].Value),
                    image = row.Cells["image"].Value?.ToString(),
                    category = row.Cells["category"].Value?.ToString(),
                    brand = row.Cells["brand"].Value?.ToString()
                };

                string json = JsonConvert.SerializeObject(product);

                using (HttpClient client = new HttpClient())
                {
                    StringContent content =
                        new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response =
                        await client.PutAsync(apiUrl + "/" + product.product_id, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Update failed:\n" + error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Edit error: " + ex.Message);
            }
        }
        private void Print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();

            printer.Title = "Products Report";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date.ToString("MM/dd/yyyy"));
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Shop Management System";
            printer.FooterSpacing = 15;
            printer.printDocument.DefaultPageSettings.Landscape = false;

            printer.PrintDataGridView(dataGridView1);
        }

        private void ReportProduct_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await LoadProductsFromApi();
        }
    }

    public class Product
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