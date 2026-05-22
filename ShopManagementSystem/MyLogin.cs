using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class MyLogin : Form
    {
        MySqlConnection con;

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

            try
            {
                Connect connectObj = new Connect();
                con = connectObj.connect();

                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM LOGIN WHERE UserName=@username AND Password=@password",
                    con
                );

                cmd.Parameters.AddWithValue("@username", userName.Text);
                cmd.Parameters.AddWithValue("@password", this.password.Text);

                MySqlDataAdapter adapt = new MySqlDataAdapter(cmd);
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
            userName.Focus();
        }

        private void MyLogin_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 153, 0);
            
        }
    }
}