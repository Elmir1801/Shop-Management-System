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
    public partial class MyLogin : Form
    {
        MySqlConnection con;  // ← changed

        public MyLogin()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (userName.Text == "" || this.password.Text == "")
            {
                MessageBox.Show("Please provide UserName and Password", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string username = userName.Text;
            string password = this.password.Text;
            try
            {
                Connect connectObj = new Connect();
                con = connectObj.connect();
                MySqlCommand cmd = new MySqlCommand("Select * from LOGIN where UserName=@username and Password=@password", con);  // ← changed
                cmd.Parameters.AddWithValue("@username", userName.Text);
                cmd.Parameters.AddWithValue("@password", this.password.Text);
                MySqlDataAdapter adapt = new MySqlDataAdapter(cmd);  // ← changed
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                int count = ds.Tables[0].Rows.Count;
                if (count == 1)
                {
                    MessageBox.Show("Login Successful!", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FormMenu menu = new FormMenu();
                    this.Hide();
                    menu.Show();
                }
                else
                {
                    MessageBox.Show("Invalid UserName or Password", "Captions", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                {
                    con.Close();
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            userName.Clear();
            password.Clear();
        }

        private void MyLogin_Load(object sender, EventArgs e)
        {
        }
    }
}