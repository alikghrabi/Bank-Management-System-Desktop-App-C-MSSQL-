using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace BankManagmentSystem
{
    public partial class Transactions : Form
    {
        public Transactions()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\UserPc\OneDrive\Documents\BankDb.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=false;");

        int Balance;
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void CheckBalance()
        {
            try
            {

                Con.Open();
                string Query = "Select * from AccountTbl where ACNum=" + CheckBalTb.Text + "";

                SqlCommand cmd = new SqlCommand(Query, Con);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    BalanceLbl.Text = dr["AcBal"] + "$".ToString();
                    Balance = Convert.ToInt32(dr["AcBal"].ToString());
                }

                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error in CheckBalance function" + Ex.Message);
            }
        }

        private void CheckAvailableBal()
        {
            string Query = "Select * from AccountTbl where ACNum=" + FromTb.Text + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                BalanceLabel.Text = dr["AcBal"] + "$".ToString();
                Balance = Convert.ToInt32(dr["AcBal"].ToString());
            }

        }

        private void GetOldBalance(String account)
        {
            Con.Open();
            string Query = "Select * from AccountTbl where ACNum=" + account + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Balance = Convert.ToInt32(dr["AcBal"].ToString());
            }

            Con.Close();
        }

        private void CheckBalBtn_Click(object sender, EventArgs e)
        {
            if (CheckBalTb.Text == "")
            {
                MessageBox.Show("Enter Bank Account Number");
                
            }
            else
            {
                CheckBalance();
                if (BalanceLbl.Text == "Your Balance")
                {
                    MessageBox.Show("Account Not Found");
                    CheckBalTb.Text = "";
                }
            }
        }

        


        //using Deposit Procedure 
        private void Deposit()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Deposit", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@accountNumber", DepAccountTb.Text);
                cmd.Parameters.AddWithValue("@amount", DepAmtTb.Text);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error in Deposit function" + Ex.Message);
            }
        }


        //using Procedure Withdraw
        private void Withdraw()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Withdraw", Con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@accountNumber", WdAccountTb.Text);
                cmd.Parameters.AddWithValue("@amount", WdAmtTb.Text);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error in Withdraw function" + Ex.Message);
            }
        }

        private void SubstractBal()
        {
            GetOldBalance(FromTb.Text);
            int Newbal = Balance - Convert.ToInt32(TransAmtTb.Text);
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update AccountTbl set AcBal=@AB where ACNum=@AcKey", Con);
                cmd.Parameters.AddWithValue("@AB", Newbal);
                cmd.Parameters.AddWithValue("@AcKey", FromTb.Text);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("error in SubstractBal function" + Ex.Message);
            }
        }

        private void AddBal()
        {
            GetOldBalance(ToTb.Text);
            int Newbal = Balance + Convert.ToInt32(TransAmtTb.Text);
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update AccountTbl set AcBal=@AB where ACNum=@AcKey", Con);
                cmd.Parameters.AddWithValue("@AB", Newbal);
                cmd.Parameters.AddWithValue("@AcKey", ToTb.Text);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error in AddBal function" + Ex.Message);
            }
        }



        private void DepositbBtn_Click(object sender, EventArgs e)
        {
            if (DepAccountTb.Text == "" || DepAmtTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Deposit", Con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@accountNumber", DepAccountTb.Text);
                    cmd.Parameters.AddWithValue("@amount", DepAmtTb.Text);
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Money Deposit!!!");
                    Con.Close();
                    DepAmtTb.Text = "";
                    DepAccountTb.Text = "";
               

                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error in DepositBtn_click function" + Ex.Message);
                }
            }
        }



        private void WdBtn_Click(object sender, EventArgs e)
        {
            if (WdAccountTb.Text == "" || WdAmtTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
               
                try
                {
                    Con.Open();
                   

                    SqlCommand cmd = new SqlCommand("Withdraw", Con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@accountNumber", WdAccountTb.Text);
                    cmd.Parameters.AddWithValue("@amount", WdAmtTb.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Money Withdrawn!!!");
                    Con.Close();
                    WdAmtTb.Text = "";
                    WdAccountTb.Text = "";
                    

                }
                catch (Exception Ex)
                {
                    MessageBox.Show("error in WdBtn_Click" + Ex.Message);
                }
               


            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (FromTb.Text == "")
            {
                MessageBox.Show("Enter Source Account");
            }
            else
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(*) from AccountTbl where ACNum='" + FromTb.Text + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    CheckAvailableBal();
                    Con.Close();
                }
                else
                {
                    MessageBox.Show("Account Doesn't Exist");
                    FromTb.Text = "";
                }
                Con.Close();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (ToTb.Text == "")
            {
                MessageBox.Show("Enter Destination Account");
            }
            else
            {
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(*) from AccountTbl where ACNum='" + ToTb.Text + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Account Found");
                    Con.Close();
                    if (ToTb.Text == FromTb.Text)
                    {
                        MessageBox.Show("Source And Destination Accounts are Same!!");
                        ToTb.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Account Doesn't Exist");
                    ToTb.Text = "";
                    Con.Close();
                }
                Con.Close();
            }
        }

        private void Transfer()
        {
            try
            {

                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into TransferTbl(TrSrc,Trdes,TrAmt,TrDate)values(@TS,@TD,@TA,@TDa)", Con);

                cmd.Parameters.AddWithValue("@TS", FromTb.Text);
                cmd.Parameters.AddWithValue("@TD", ToTb.Text);
                cmd.Parameters.AddWithValue("@TA", TransAmtTb.Text);
                cmd.Parameters.AddWithValue("@TDa", DateTime.Now.Date);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Money Transfered");

                Con.Close();


            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error in Transfer function" + Ex.Message);
            }
        }

        private void TransBtn_Click(object sender, EventArgs e)
        {
            if (ToTb.Text == "" || FromTb.Text == "" || TransAmtTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else if (Convert.ToUInt16(TransAmtTb.Text) > Balance)
            {
                MessageBox.Show("Insufficient balance");
            }
            else
            {


                Transfer();
                SubstractBal();
                AddBal();
                FromTb.Text = "";
                ToTb.Text = "";
                TransAmtTb.Text = "";
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update AgentTb set AStatus=@AS where AStatus=@AA", Con);
                cmd.Parameters.AddWithValue("@AS", "Inactive");
                cmd.Parameters.AddWithValue("@AA", "Active");
                cmd.ExecuteNonQuery();
                Con.Close();
                Application.Exit();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("error in exit click" + Ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            log Obj = new log();
            Obj.Show();

        }
    }
}