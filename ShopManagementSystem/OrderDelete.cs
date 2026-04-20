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
    public partial class OrderDelete : Form
    {
        MySqlConnection con;  // ← changed

        public OrderDelete()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Connect connectObj = new Connect();
                con = connectObj.connect();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM MYORDER WHERE ORD_ID = @oid", con);  // ← changed
                cmd.Parameters.AddWithValue("@oid", OrderID.Text);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i != 0)
                {
                    MessageBox.Show("Order Deletion Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Order Deletion Failed", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                OrderID.Clear();
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
            OrderID.Clear();
        }

        private void OrderDelete_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OrderDelete_Load(object sender, EventArgs e)
        {
        }
    }
}