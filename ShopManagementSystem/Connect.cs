using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ShopManagementSystem
{
    public class Connect
    {
        MySqlConnection con;
        public static string cs = "Server=localhost;Port=3306;Database=spms1;Uid=root;Pwd=;";

        public MySqlConnection connect()
        {
            try
            {
                con = new MySqlConnection(cs);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message);
            }
            return null;
        }

        public void closeConnection()
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}