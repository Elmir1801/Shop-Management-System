using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;  // ← changed
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class ProductUpdate : Form
    {
        MySqlConnection con;  // ← changed

        public ProductUpdate()
        {
            InitializeComponent();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (ProductName.Text == "" || productID.Text == "" || VendorID.Text == "" || Amount.Text == "")
            {
                MessageBox.Show("Please provide all the details", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Connect conObj = new Connect();
                con = conObj.connect();
                MySqlCommand cmd = new MySqlCommand("UPDATE PRODUCT SET pname = @pname,amount = @amount,vid = @vid WHERE pid = @id;", con);  // ← changed

                cmd.Parameters.AddWithValue("@id", productID.Text);
                cmd.Parameters.AddWithValue("@pname", ProductName.Text);
                cmd.Parameters.AddWithValue("@vid", VendorID.Text);
                cmd.Parameters.AddWithValue("@amount", Amount.Text);
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Product Updation Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Product Updation Failed", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            ProductName.Clear();
            productID.Clear();
            VendorID.Clear();
            Amount.Clear();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ProductName.Clear();
            productID.Clear();
            VendorID.Clear();
            Amount.Clear();
        }

        private void ProductUpdate_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductUpdate_Load(object sender, EventArgs e)
        {
        }
    }
}