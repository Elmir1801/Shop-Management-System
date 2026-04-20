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
    public partial class VendorDelete : Form
    {
        MySqlConnection con;  // ← changed

        public VendorDelete()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            try
            {
                Connect connectObj = new Connect();
                using (con = connectObj.connect())
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT VID FROM VENDOR WHERE VNAME = @vname"))  // ← changed
                    {
                        cmd.Parameters.AddWithValue("@vname", VendorName.Text);
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        using (MySqlDataReader sdr = cmd.ExecuteReader())  // ← changed
                        {
                            sdr.Read();
                            VendorID.Text = sdr["VID"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vendor not found");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Connect connectObj = new Connect();
                con = connectObj.connect();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM VENDOR WHERE VID = @vid", con);  // ← changed
                cmd.Parameters.AddWithValue("@vid", VendorID.Text);
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Vendor Deletion Successful!");
                }
                else
                {
                    MessageBox.Show("Vendor Deletion Failed");
                }
                con.Close();
                VendorID.Clear();
                VendorName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vendor Not found");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private void VendorDelete_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VendorDelete_Load(object sender, EventArgs e)
        {
        }
    }
}