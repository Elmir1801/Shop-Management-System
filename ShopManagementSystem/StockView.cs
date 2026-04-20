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
    public partial class StockView : Form
    {
        MySqlConnection con;  // ← changed

        public StockView()
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
                            Productname.Text = sdr["PNAME"].ToString();
                        }
                    }
                    using (MySqlCommand cmd = new MySqlCommand("SELECT QUANTITY FROM STOCK WHERE PID = @pid"))  // ← changed
                    {
                        cmd.Parameters.AddWithValue("@pid", ProductID.Text);
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        using (MySqlDataReader sdr = cmd.ExecuteReader())  // ← changed
                        {
                            sdr.Read();
                            Quantity.Text = sdr["QUANTITY"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stock not found", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
}