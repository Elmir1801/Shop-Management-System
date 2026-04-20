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
    public partial class CustomerUpdate : Form
    {
        private MySqlConnection con;  // ← changed

        public CustomerUpdate()
        {
            InitializeComponent();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (customerName.Text == "" || CustPhno.Text == "" || customerAddress.Text == "" || Email.Text == "" || customerID.Text == "")
            {
                MessageBox.Show("Please provide all the details", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Connect connectObj = new Connect();
                con = connectObj.connect();
                MySqlCommand cmd = new MySqlCommand("UPDATE CUSTOMER SET cname = @cname,address = @address,phone_number = @phno,email = @email WHERE c_id = @id;", con);  // ← changed

                cmd.Parameters.AddWithValue("@id", customerID.Text);
                cmd.Parameters.AddWithValue("@cname", customerName.Text);
                cmd.Parameters.AddWithValue("@phno", Convert.ToInt64(CustPhno.Text));
                cmd.Parameters.AddWithValue("@address", customerAddress.Text);
                cmd.Parameters.AddWithValue("@email", Email.Text);
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Customer Updation Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Customer Updation Failed", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            customerName.Clear();
            CustPhno.Clear();
            customerAddress.Clear();
            Email.Clear();
            customerID.Clear();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            customerName.Clear();
            CustPhno.Clear();
            customerAddress.Clear();
            Email.Clear();
            customerID.Clear();
        }

        private void CustomerUpdate_Load(object sender, EventArgs e)
        {
        }
    }
}