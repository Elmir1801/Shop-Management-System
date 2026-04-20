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
    public partial class StockDelete : Form
    {
        MySqlConnection con;  // ← changed

        public StockDelete()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Connect connectObj = new Connect();
                con = connectObj.connect();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM STOCK WHERE PID = @pid", con);  // ← changed
                cmd.Parameters.AddWithValue("@pid", ProductID.Text);
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Stock Deletion Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Stock Deletion Failed", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
                ProductID.Clear();
                ProductName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Product Not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private void Search_Click(object sender, EventArgs e)
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
                        using (MySqlDataReader sdr = cmd.ExecuteReader())  // ← changed (also removed con.Open() - already open)
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
        }
    }
}