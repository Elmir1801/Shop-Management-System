using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class ProductInsert : Form
    {
        private string selectedImagePath = "";

        public ProductInsert()
        {
            InitializeComponent();
        }

        public class ComboItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        private async void Submit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(Amount.Text) ||
                string.IsNullOrWhiteSpace(txtImage.Text) ||
                cmbCategory.SelectedIndex == -1 ||
                cmbBrand.SelectedIndex == -1)
            {
                MessageBox.Show("Please provide all the details", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (!decimal.TryParse(Amount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal priceValue))
                {
                    if (!decimal.TryParse(Amount.Text, out priceValue))
                    {
                        MessageBox.Show("Invalid amount.", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(selectedImagePath) && File.Exists(selectedImagePath))
                {
                    string fileName = Path.GetFileName(selectedImagePath);
                    string destinationFolder = @"C:\xampp\htdocs\myprojects\Github_Project\online-shopping-system\admin\product_images\";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string destinationPath = Path.Combine(destinationFolder, fileName);
                    File.Copy(selectedImagePath, destinationPath, true);

                    txtImage.Text = fileName;
                }

                var selectedCategory = (ComboItem)cmbCategory.SelectedItem;
                var selectedBrand = (ComboItem)cmbBrand.SelectedItem;

                var productData = new
                {
                    php_product_id = (int?)null,
                    product_name = ProductName.Text.Trim(),
                    description = txtDescription.Text.Trim(),
                    price = priceValue,
                    image = txtImage.Text.Trim(),
                    category = selectedCategory.Text,
                    brand = selectedBrand.Text
                };

                string json = JsonConvert.SerializeObject(productData);

                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    string apiUrl = "http://localhost:3000/api/products";

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();
                        ProductInsertResponse apiResult = JsonConvert.DeserializeObject<ProductInsertResponse>(responseJson);

                        ProductID.Text = apiResult.product_id.ToString();

                        MessageBox.Show(
                            "Product Insertion Successful!\nMain Product ID: " + apiResult.product_id,
                            "Captions",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        ProductName.Clear();
                        txtDescription.Clear();
                        Amount.Clear();
                        txtImage.Clear();
                        cmbCategory.SelectedIndex = -1;
                        cmbBrand.SelectedIndex = -1;
                        pictureBox1.Image = null;
                        selectedImagePath = "";
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Product Insertion Failed\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Error: " + ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Product Image";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    txtImage.Text = Path.GetFileName(selectedImagePath);
                    pictureBox1.ImageLocation = selectedImagePath;
                }
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ProductName.Clear();
            txtDescription.Clear();
            Amount.Clear();
            txtImage.Clear();
            ProductID.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbBrand.SelectedIndex = -1;
            pictureBox1.Image = null;
            selectedImagePath = "";
        }

        private void ProductInsert_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductInsert_Load(object sender, EventArgs e)
        {
            txtImage.ReadOnly = true;
            txtImage.BackColor = System.Drawing.Color.LightGray;

            ProductID.ReadOnly = true;
            ProductID.BackColor = System.Drawing.Color.LightGray;

            LoadCategories();
            LoadBrands();
        }

        private void LoadCategories()
        {
            var categories = new List<ComboItem>
            {
                new ComboItem { Text = "Electronics", Value = "1" },
                new ComboItem { Text = "Ladies Wear", Value = "2" },
                new ComboItem { Text = "Mens Wear", Value = "3" },
                new ComboItem { Text = "Kids Wear", Value = "4" },
                new ComboItem { Text = "Furnitures", Value = "5" },
                new ComboItem { Text = "Home Appliances", Value = "6" }
            };

            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Text";
            cmbCategory.ValueMember = "Value";
            cmbCategory.SelectedIndex = -1;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadBrands()
        {
            var brands = new List<ComboItem>
            {
                new ComboItem { Text = "Samsung", Value = "1" },
                new ComboItem { Text = "HP", Value = "2" },
                new ComboItem { Text = "LG", Value = "3" },
                new ComboItem { Text = "motorolla", Value = "4" },
                new ComboItem { Text = "Cloth Brand", Value = "5" },
                new ComboItem { Text = "Other Brand", Value = "6" }
            };

            cmbBrand.DataSource = brands;
            cmbBrand.DisplayMember = "Text";
            cmbBrand.ValueMember = "Value";
            cmbBrand.SelectedIndex = -1;
            cmbBrand.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
        }
    }

    public class ProductInsertResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int product_id { get; set; }
        public int? php_product_id { get; set; }
    }
}