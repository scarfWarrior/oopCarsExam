using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace oopCarsExam
{
    public partial class Form1 : Form
    {
        //database connection
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\twigp\Desktop\TaylorOOPExamA - 5th Dec\C-GOOPWFAExam-main\CarsProject\CarsDatabase.accdb"";Jet OLEDB:Database Password=password");
        OleDbDataAdapter adapter;
        DataTable dt = new DataTable();
        OleDbCommand cmd;
        int pos = 0;


        private void ExecuteCommand()
        {
            try
            {
                //Open Connection to Database
                conn.Open();
                //Run the Query
                cmd.ExecuteNonQuery();
                //Close the Database connection
                conn.Close();
                //Update the Program 

                dt.Reset();
                adapter.Fill(dt);
                showData(pos);
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show($"Something went really wrong!!!{Environment.NewLine}{ex.Message}");
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Fill Form with Database Entries on load
            string sql = "SELECT * FROM tblCars";
            adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(dt);
            showData(pos);
        }


        public void showData(int index)
        {
            //Set textboxs from Data Table
            tbReg.Text = dt.Rows[index]["VehicleRegNo"].ToString();
            tbMake.Text = dt.Rows[index]["Make"].ToString();
            tbEngine.Text = dt.Rows[index]["EngineSize"].ToString();
            tbDate.Text = dt.Rows[index]["DateRegistered"].ToString();
            //Concert Number to Currency
            tbRent.Text = "€" + Convert.ToDouble(dt.Rows[index]["RentalPerDay"]).ToString("#,##,0.00");


            //Check Available set checkbox to True
            if (dt.Rows[index]["Available"].ToString() == "True")
            {
                checkBox1.Checked = true;
            }
            //Set Check box to False 
            else
            {
                checkBox1.Checked = false;
            }

            //Set the Bottom textbox to total number of entries
            tbRecord.Text = pos + 1 + " of " + dt.Rows.Count;
        }

        private void btnfirst_Click(object sender, EventArgs e)
        {
            pos = 0;
            showData(pos);

        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pos--;
            if (pos >= 0)
            {
                showData(pos);
            }
            else
            {
                MessageBox.Show("END");
            }
        }

        private void tbRecord_TextChanged(object sender, EventArgs e)
        {

        }



        private void btnNext_Click(object sender, EventArgs e)
        {
            pos++;
            if (pos < dt.Rows.Count)
            {
                showData(pos);
            }
            else
            {
                MessageBox.Show("END");
                pos = dt.Rows.Count - 1;
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            pos = dt.Rows.Count - 1;
            showData(pos);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Setup SQL Query
            string sql = "UPDATE tblCars SET [Make] = ?, [EngineSize] = ?, [DateRegistered] = ?, [RentalPerDay] = ?, [Available] = ? WHERE [VehicleRegNo] = ? ";
            //Start command
            cmd = new OleDbCommand(sql, conn);

            //Sets paramters in [] to the variable next to it IN ORDER,
            //e.g [EngineSize] = @EngineSize and tbEngine.Text = 2nd ?
            //Must have parameters in the same order you put them in Query
            cmd.Parameters.AddWithValue("@Make", tbMake.Text);
            cmd.Parameters.AddWithValue("@EngineSize", tbEngine.Text);
            cmd.Parameters.AddWithValue("@DateRegistered", tbDate.Text);
            cmd.Parameters.AddWithValue("@RentalPerDay", tbRent.Text.Remove(0, 1));
            cmd.Parameters.AddWithValue("@Available", checkBox1.Checked);
            cmd.Parameters.AddWithValue("@VehicleRegNo", tbReg.Text);


            ExecuteCommand();
            //showData(pos);
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Set up SQL Query
            string sql = "INSERT INTO tblCars ([VehicleRegNo],[Make],[EngineSize],[DateRegistered],[RentalPerDay]) VALUES (?,?,?,?,?)";
            //Start Command
            cmd = new OleDbCommand(sql, conn);

            //Set parameters for Query, See Above for Explination
            cmd.Parameters.AddWithValue("@VehicleRegNo", tbReg.Text);
            cmd.Parameters.AddWithValue("@Make", tbMake.Text);
            cmd.Parameters.AddWithValue("@EngineSize", tbEngine.Text);
            cmd.Parameters.AddWithValue("@DateRegistered", tbDate.Text);
            cmd.Parameters.AddWithValue("@RentalPerDay", tbRent.Text.Trim('€'));
            cmd.Parameters.AddWithValue("@Available", checkBox1.Checked);

            //Run the Query
            ExecuteCommand();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Set up SQL Query
            string sql = "DELETE FROM tblCars WHERE [VehicleRegNo] = ?";
            //Start Command
            cmd = new OleDbCommand(sql, conn);

            //Set Parameter
            cmd.Parameters.AddWithValue("@VehicleRegNo", tbReg.Text);

            //Run Query
            ExecuteCommand();
            //Goes back one - Not Needed!!
            /*btnDelete.PerformClick();*/
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Create search form
            frmSearch frmSearch = new frmSearch();
            //Show search Form
            frmSearch.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
