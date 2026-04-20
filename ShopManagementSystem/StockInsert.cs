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
    public partial class StockInsert : Form
    {
        MySqlConnection con;  // ← changed

        public StockInsert()
        {
            InitializeComponent();
        }

        private void search_Click(object sender, EventArgs e)
        {
            try
            {
                Connect connectObj = new Connect();
                using (con = connectObj.connect())
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT PNAME FROM PRODUCT WHERE PID = @pid"))  // ← changed
                    {
                        cmd.Parameters.AddWithValue("@pid", ProductID.Text);
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        using (MySqlDataReader sdr = cmd.ExecuteReader())  // ← changed
                        {
                            sdr.Read();
                            ProductName.Text = sdr["PNAME"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Product not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (ProductID.Text == "" || Quantity.Text == "" || ProductName.Text == "")
            {
                MessageBox.Show("Please provide all the details", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Connect connectObj = new Connect();
                con = connectObj.connect();
                MySqlCommand cmd = new MySqlCommand("Insert into STOCK (PID,QUANTITY) values(@pID,@quantity);", con);  // ← changed
                cmd.Parameters.AddWithValue("@pid", ProductID.Text);
                cmd.Parameters.AddWithValue("@quantity", Quantity.Text);
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Stock Insertion Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Stock Insertion Failed", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
                ProductID.Clear();
                Quantity.Clear();
                ProductName.Clear();
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