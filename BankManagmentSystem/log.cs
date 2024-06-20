using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankManagmentSystem
{
    public partial class log : Form
    {
        public log()
        {
            InitializeComponent();
            DisplayLog();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\UserPc\OneDrive\Documents\BankDb.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=false;");

        private void DisplayLog()
        {
            Con.Open();
            string Query = "select * from LogTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            LogDGV.DataSource = ds.Tables[0];
            Con.Close();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void LogDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
