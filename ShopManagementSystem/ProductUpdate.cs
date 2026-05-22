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
    public partial class ProductUpdate : Form
    {
        private string selectedImagePath = "";

        public ProductUpdate()
        {
            InitializeComponent();
        }

        public class ComboItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        private async void search_Click(object sender, EventArgs e)
        {
            await SearchProductById();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await SearchProductById();
        }

        private async Task SearchProductById()
        {
            if (string.IsNullOrWhiteSpace(productID.Text))
            {
                MessageBox.Show("Please enter Product ID", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"http://localhost:3000/api/products/{productID.Text}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        ProductSearchResponse result = JsonConvert.DeserializeObject<ProductSearchResponse>(json);

                        if (result != null && result.success && result.data != null)
                        {
                            ProductName.Text = result.data.product_name ?? "";
                            txtDescription.Text = result.data.description ?? "";
                            Amount.Text = result.data.price.ToString("0.##", CultureInfo.InvariantCulture);
                            txtImage.Text = result.data.image ?? "";

                            if (!string.IsNullOrWhiteSpace(result.data.image))
                            {
                                string possibleImagePath = Path.Combine(
                                    @"C:\xampp\htdocs\myprojects\Github_Project\online-shopping-system\admin\product_images\",
                                    result.data.image
                                );

                                if (File.Exists(possibleImagePath))
                                {
                                    pictureBox1.ImageLocation = possibleImagePath;
                                    selectedImagePath = possibleImagePath;
                                }
                                else
                                {
                                    pictureBox1.Image = null;
                                    pictureBox1.ImageLocation = null;
                                    selectedImagePath = "";
                                }
                            }
                            else
                            {
                                pictureBox1.Image = null;
                                pictureBox1.ImageLocation = null;
                                selectedImagePath = "";
                            }

                            SelectComboValueByText(cmbCategory, result.data.category);
                            SelectComboValueByText(cmbBrand, result.data.brand);
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

        private void SelectComboValueByText(ComboBox comboBox, string textToFind)
        {
            if (string.IsNullOrWhiteSpace(textToFind))
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                ComboItem item = comboBox.Items[i] as ComboItem;
                if (item != null && string.Equals(item.Text, textToFind, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            comboBox.SelectedIndex = -1;
        }

        private async void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(productID.Text) ||
                string.IsNullOrWhiteSpace(Amount.Text) ||
                cmbCategory.SelectedIndex == -1 ||
                cmbBrand.SelectedIndex == -1)
            {
                MessageBox.Show("Please provide all the details", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (!int.TryParse(productID.Text, out int productId))
                {
                    MessageBox.Show("Invalid Product ID", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                    string apiUrl = $"http://localhost:3000/api/products/{productId}";

                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Product Updation Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ProductName.Clear();
                        txtDescription.Clear();
                        productID.Clear();
                        Amount.Clear();
                        txtImage.Clear();
                        cmbCategory.SelectedIndex = -1;
                        cmbBrand.SelectedIndex = -1;
                        pictureBox1.Image = null;
                        pictureBox1.ImageLocation = null;
                        selectedImagePath = "";
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Product Updation Failed\n" + error, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            ProductName.Clear();
            txtDescription.Clear();
            productID.Clear();
            Amount.Clear();
            txtImage.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbBrand.SelectedIndex = -1;
            pictureBox1.Image = null;
            pictureBox1.ImageLocation = null;
            selectedImagePath = "";
        }

        private void ProductUpdate_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductUpdate_Load(object sender, EventArgs e)
        {
            txtImage.ReadOnly = false;
            txtImage.BackColor = System.Drawing.Color.White;

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmbBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
        }
    }

    public class ProductSearchResponse
    {
        public bool success { get; set; }
        public ProductSearchData data { get; set; }
    }

    public class ProductSearchData
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
}