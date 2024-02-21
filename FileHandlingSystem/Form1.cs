using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHandlingSystem
{
    public partial class MainForm : Form
    {
        public static string UserFormType = "";
        public static object[] values = new object[10];
        public static long serialNumber;
        FileStream UserDataFile;
        string fileData;

        UserForm userForm;
        MainForm mainForm;

        public MainForm()
        {
            InitializeComponent();
            ShowData();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //ShowData();
            userForm = new UserForm();
            mainForm = new MainForm();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // userForm = new UserForm();
            UserForm.LoadLastSerialNumber();
            ShowData(); // Add columns to DataGridView
            CreateTextFile();
            //MessageBox.Show("absg");
            LoadDataFromFile(); // Load data into the DataGridView
        }
        private void ShowData()
        {
            grid_userData.ReadOnly = true;
            grid_userData.Columns.Add("Serial Number", "Serial Number");
            grid_userData.Columns.Add("Prefix", "Prefix");
            grid_userData.Columns.Add("First Name", "First Name");
            grid_userData.Columns.Add("Middle Name", "Middle Name");
            grid_userData.Columns.Add("Last Name", "Last Name");
            grid_userData.Columns.Add("Date of Birth", "Date of Birth");
            grid_userData.Columns.Add("Qualification", "Qualification");
            grid_userData.Columns.Add("Joining Date", "Joining Date");
            grid_userData.Columns.Add("Current Company", "Current Company");
            grid_userData.Columns.Add("Current Address", "Current Address");
            grid_userData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }



        

        private void LoadDataFromFile()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "UserData.txt");
            using(StreamReader reader=new StreamReader(filePath))
            {
                string line;
                while((line= reader.ReadLine())!= null)
                {
                    string[]fields=line.Split(',');
                   // MessageBox.Show(fields.ToString());
                    grid_userData.Rows.Add(fields);
                    
                }

            }
            
        }


        private void CreateTextFile()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "UserData.txt");
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            UserFormType = "New";
           userForm. ClearFieldsExceptSerialNumber();
            userForm.ShowDialog();
        }

        //private void btn_Update_Click(object sender, EventArgs e)
        //{
        //    UserFormType = "Update";
        //    userForm.ShowDialog();
        //}
        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (grid_userData.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = grid_userData.SelectedRows[0];
                if (selectedRow.Cells[0].Value != null)
                {
                    UserFormType = "Update";
                    userForm = new UserForm();
                    userForm.PopulateFormData(selectedRow.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value).ToArray());
                    userForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please select a valid row to update.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }
        //public void RefreshDataGridView()
        //{
        //    // Reload the data into the DataGridView
        //    grid_userData.Rows.Clear();
        //    string filePath = "UserData.txt"; // Specify the path to your file
        //    string[] lines = File.ReadAllLines(filePath);
        //    foreach (string line in lines)
        //    {
        //        string[] fields = line.Split(',');
        //        grid_userData.Rows.Add(fields);
        //    }

        //    // Save changes back to the file
        //    SaveDataToFile(filePath);
        //}
        public void RefreshDataGridView()
        {
            // Reload the data into the DataGridView
            grid_userData.Rows.Clear();
            LoadDataFromFile(); // Load data into the DataGridView
        }

        public void SaveDataToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (DataGridViewRow row in grid_userData.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                        {
                            sb.Append(cell.Value.ToString()).Append(",");
                        }
                    }
                    if (sb.Length > 0)
                    {
                        sb.Length--; // Remove the last comma
                        writer.WriteLine(sb.ToString());
                    }
                }
            }
        }



        //public void ShowUpdateButton(bool visible)
        //{
        //    btn_Update.Visible = visible;
        //}

        //private void btn_Update_Click(object sender, EventArgs e)
        //{
        //    if (grid_userData.SelectedRows.Count > 0)
        //    {
        //        DataGridViewRow selectedRow = grid_userData.SelectedRows[0];
        //        if (selectedRow.Cells[0].Value != null)
        //        {
        //            UserFormType = "Update";
        //            userForm = new UserForm();
        //            userForm.PopulateFormData(selectedRow.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value).ToArray());
        //            userForm.ShowDialog();
        //            // After the form is closed, update data in the file
        //            userForm.UpdateDataInFile();
        //            // Reload data into the DataGridView
        //            grid_userData.Rows.Clear();
        //            LoadDataFromFile();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Please select a valid row to update.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a row to update.");
        //    }
        //}


        private void grid_userData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < grid_userData.Rows.Count)
            {
                // Get the selected row
                DataGridViewRow selectedRow = grid_userData.Rows[e.RowIndex];

                // Check if the selected row is blank (assuming the first column as an identifier)
                if (selectedRow.Cells[0].Value == null || string.IsNullOrWhiteSpace(selectedRow.Cells[0].Value.ToString()))
                {
                    // If the row is blank, open the form as in btn_new_Click
                    btn_new_Click(sender, e);
                }
                else
                {
                    // Copy the data of the selected row to the values array
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = selectedRow.Cells[i].Value;
                    }

                    // Set the UserFormType to "View"
                    UserFormType = "View";

                    // Initialize a new instance of UserForm
                    userForm = new UserForm();
                    userForm.PopulateFormData(values);
                    // Show the UserForm
                    userForm.ShowDialog();
                }
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileData != null)
            {
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData.txt"), fileData);
            }
            if (UserDataFile != null)
            {
                UserDataFile.Close();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (grid_userData.SelectedRows.Count > 0)
            {
                // Display a confirmation dialog
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // If the user confirms deletion
                if (result == DialogResult.Yes)
                {
                    // Get the selected row
                    DataGridViewRow selectedRow = grid_userData.SelectedRows[0];

                    // Remove the row from the DataGridView
                    grid_userData.Rows.Remove(selectedRow);

                    // Save changes back to the file
                    SaveDataToFile("UserData.txt");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

    }
}
