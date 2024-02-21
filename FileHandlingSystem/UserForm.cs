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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FileHandlingSystem
{
    public enum Qualification
    {
        TenthGrade,
        TwelfthGrade,
        // Diplomas
        Diploma,
        PGDiploma,

        // Bachelor's Degrees
        BSc,
        BCA,
        BA,
        BTechCSE, // Bachelor of Technology in Computer Science and Engineering
        BTechCivil, // Bachelor of Technology in Civil Engineering
        BTechIT,
        BE,// Bachelor of Technology in Information Technology
        // Master's Degrees
        MSc,
        MCA
    }

    public partial class UserForm : Form
    {
        UserForm userForm;
        MainForm mainForm;
        private int selectedRowIndex;
        private DataGridView dataGridView;
        private string[] userDataLines; 
        public UserForm()
        {
            InitializeComponent();



        }
        
            

        public void SetSelectedRowIndex(int index)
        {
            selectedRowIndex = index;
        }


        private void InitializeElement()
        {
            date_dateofBirth.Format = DateTimePickerFormat.Short;
            date_joiningDate.Format = DateTimePickerFormat.Short;
            date_dateofBirth.Value = DateTime.Today;
            date_joiningDate.Value = DateTime.Today;
            textbox_serialNumber.Enabled = false;

            if (MainForm.UserFormType == "New")
            {
                DisableScrollButtons();
                btn_Edit.Enabled = false;
                textbox_serialNumber.Text = MainForm.serialNumber.ToString();
            }
            else if (MainForm.UserFormType == "Update")
            {
                DisableScrollButtons();
                btn_add.Enabled = false;
            }
            else if (MainForm.UserFormType == "View")
            {
                btn_add.Visible = false;
                btn_clear.Visible = false;
                btn_Edit.Visible = false;
                textbox_serialNumber.Text = MainForm.values[0]?.ToString();
                InsertDataInForm();
                OpenFormInReadOnly();
            }

        }

        private void OpenFormInReadOnly()
        {
            textbox_prefix.Enabled = false;
            textbox_firstname.Enabled = false;
            textbox_lastname.Enabled = false;
            textbox_middlename.Enabled = false;
            textbox_qualification.Enabled = false;
            textbox_currentcompany.Enabled = false;
            textbox_currentaddress.Enabled = false;
            date_dateofBirth.Enabled = false;
            date_joiningDate.Enabled = false;
        }

        private void InsertDataInForm()
        {
            if (MainForm.values.Length >= 11)
            {
                textbox_prefix.Text = MainForm.values[2]?.ToString();
                textbox_firstname.Text = MainForm.values[3]?.ToString();
                textbox_middlename.Text = MainForm.values[4]?.ToString();
                textbox_lastname.Text = MainForm.values[5]?.ToString();
                textbox_qualification.Text = MainForm.values[7]?.ToString();
                textbox_currentcompany.Text = MainForm.values[9]?.ToString();
                textbox_currentaddress.Text = MainForm.values[10]?.ToString();
                try
                {
                    date_dateofBirth.Value = Convert.ToDateTime(MainForm.values[6]);
                    date_joiningDate.Value = Convert.ToDateTime(MainForm.values[8]);
                }
                catch
                {
                    date_dateofBirth.Visible = false;
                    date_joiningDate.Visible = false;

                }
            }
        }
        public void PopulateFormData(object[] rowData)
        {
            if (rowData != null)
            {
                textbox_serialNumber.Text = rowData.ElementAtOrDefault(0)?.ToString();
                textbox_prefix.Text = rowData.ElementAtOrDefault(1)?.ToString();
                textbox_firstname.Text = rowData.ElementAtOrDefault(2)?.ToString();
                textbox_middlename.Text = rowData.ElementAtOrDefault(3)?.ToString();
                textbox_lastname.Text = rowData.ElementAtOrDefault(4)?.ToString();

                // Check if date of birth value is valid
                if (DateTime.TryParse(rowData.ElementAtOrDefault(5)?.ToString(), out DateTime dob))
                {
                    date_dateofBirth.Value = dob;
                }
                else
                {
                    date_dateofBirth.Value = DateTime.Today; // Set to default value if parsing fails
                }

                textbox_qualification.Text = rowData.ElementAtOrDefault(6)?.ToString();

                // Check if joining date value is valid
                if (DateTime.TryParse(rowData.ElementAtOrDefault(7)?.ToString(), out DateTime joiningDate))
                {
                    date_joiningDate.Value = joiningDate;
                }
                else
                {
                    date_joiningDate.Value = DateTime.Today; // Set to default value if parsing fails
                }

                textbox_currentcompany.Text = rowData.ElementAtOrDefault(8)?.ToString();
                textbox_currentaddress.Text = rowData.ElementAtOrDefault(9)?.ToString();
            }
        }



        private void DisableScrollButtons()
        {
            btn_first.Visible = false;
            btn_last.Visible = false;
            btn_previous.Visible = false;
            btn_next.Visible = false;
        }

        private void EnableElements()
        {
            btn_first.Visible = true;
            btn_last.Visible = true;
            btn_previous.Visible = true;
            btn_next.Visible = true;
            btn_add.Visible = true;
            btn_Edit.Visible = true;
            btn_clear.Visible = true;
            btn_add.Enabled = true;
            btn_Edit.Enabled = true;

            textbox_prefix.Enabled = true;
            textbox_firstname.Enabled = true;
            textbox_lastname.Enabled = true;
            textbox_middlename.Enabled = true;
            textbox_qualification.Enabled = true;
            textbox_currentcompany.Enabled = true;
            textbox_currentaddress.Enabled = true;
            date_dateofBirth.Enabled = true;
            date_joiningDate.Enabled = true;
        }

        public void UpdateDataInFile()
        {
            try
            {
                // Add a breakpoint here to check if this method is being called.
                // Place the breakpoint on the first line inside this method.

                // Get the updated data from the form fields
                string updatedData = $"{textbox_serialNumber.Text},{textbox_prefix.Text},{textbox_firstname.Text},{textbox_middlename.Text},{textbox_lastname.Text},{date_dateofBirth.Value.ToShortDateString()},{textbox_qualification.Text},{date_joiningDate.Value.ToShortDateString()},{textbox_currentcompany.Text},{textbox_currentaddress.Text}";

                // Update the data in the file
                string filePath = "UserData.txt"; // Specify the path to your file
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');
                    if (fields.Length >= 11 && fields[0] == textbox_serialNumber.Text)
                    {
                        lines[i] = updatedData; // Update the line with the new data
                        break;
                    }
                }
                File.WriteAllLines(filePath, lines); // Write the updated data back to the file
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EnableElements();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            userForm = new UserForm();
            mainForm = new MainForm();
            InitializeElement();
            LoadUserDataFromFile();
        }

        public static void LoadLastSerialNumber()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "LastSerialNumber.txt");
            if (File.Exists(filePath))
            {
                string lastSerialNumberStr = File.ReadAllText(filePath);
                if (long.TryParse(lastSerialNumberStr, out long lastSerialNumber))
                {
                    // Increment the last serial number
                    MainForm.serialNumber = lastSerialNumber;
                }
            }
            else
            {
                // If the file doesn't exist, start the serial number from 1
                MainForm.serialNumber = 1;
            }
        }



        private void SaveLastSerialNumber()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "LastSerialNumber.txt");
            File.WriteAllText(filePath, MainForm.serialNumber.ToString());
        }
    

        public void EnableEditing(bool enable)
        {
            // Enable or disable editing for all form fields
            textbox_prefix.Enabled = enable;
            textbox_firstname.Enabled = enable;
            textbox_lastname.Enabled = enable;
            textbox_middlename.Enabled = enable;
            textbox_qualification.Enabled = enable;
            textbox_currentcompany.Enabled = enable;
            textbox_currentaddress.Enabled = enable;
            date_dateofBirth.Enabled = enable;
            date_joiningDate.Enabled = enable;
        }




        private void btn_add_Click(object sender, EventArgs e)
        {
            // Validate input fields
            if (textbox_firstname.Text == "")
            {
                textbox_firstname.Focus();
                MessageBox.Show("First Name is Required");
                return;
            }
            if (textbox_qualification.Text == "")
            {
                textbox_qualification.Focus();
                MessageBox.Show("Qualification is Required");
                return;
            }
            if (textbox_currentcompany.Text == "")
            {
                textbox_currentcompany.Focus();
                MessageBox.Show("Current Company is Required");
                return;
            }

            // Create a string representing the new entry with the current serial number
            string newEntry = $"{MainForm.serialNumber},{textbox_prefix.Text},{textbox_firstname.Text},{textbox_middlename.Text},{textbox_lastname.Text},{date_dateofBirth.Value.ToShortDateString()},{textbox_qualification.Text},{date_joiningDate.Value.ToShortDateString()},{textbox_currentcompany.Text},{textbox_currentaddress.Text}";
            // Append the new entry to the file
            string filePath = "UserData.txt"; // Specify the path to your file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(newEntry);
            }
            MainForm.serialNumber++;
            SaveLastSerialNumber();
            this.Close();           // Close the user form after adding data

        }
        public void ClearFieldsExceptSerialNumber()
        {
            textbox_prefix.Text = "";
            textbox_firstname.Text = "";
            textbox_middlename.Text = "";
            textbox_lastname.Text = "";
            textbox_qualification.Text = "";
            textbox_currentcompany.Text = "";
            textbox_currentaddress.Text = "";
            date_dateofBirth.Value = DateTime.Today;
            date_joiningDate.Value = DateTime.Today;
        }


        private void LoadUserDataFromFile()
        {
            try
            {
                string filePath = "UserData.txt"; // Specify the path to your file
                if (File.Exists(filePath))
                {
                    userDataLines = File.ReadAllLines(filePath);
                    //MessageBox.Show($"Loaded {userDataLines.Length} lines of user data.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    userDataLines = new string[0]; // Initialize an empty array if the file doesn't exist
                    //MessageBox.Show("User data file not found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Move to the first record in the file
        private void btn_first_Click(object sender, EventArgs e)
        {
            if (userDataLines != null && userDataLines.Length > 0)
            {
                PopulateFormData(userDataLines[0].Split(','));
                selectedRowIndex = 0;
            }
        }

        // Move to the previous record in the file
        private void btn_previous_Click(object sender, EventArgs e)
        {
            if (userDataLines != null && userDataLines.Length > 0 && selectedRowIndex > 0)
            {
                PopulateFormData(userDataLines[selectedRowIndex - 1].Split(','));
                selectedRowIndex--;
            }
        }

        // Move to the next record in the file
        private void btn_next_Click(object sender, EventArgs e)
        {
            if (userDataLines != null && userDataLines.Length > 0 && selectedRowIndex < userDataLines.Length - 1)
            {
                PopulateFormData(userDataLines[selectedRowIndex + 1].Split(','));
                selectedRowIndex++;
            }
        }

        // Move to the last record in the file
        private void btn_last_Click(object sender, EventArgs e)
        {
            if (userDataLines != null && userDataLines.Length > 0)
            {
                PopulateFormData(userDataLines[userDataLines.Length - 1].Split(','));
                selectedRowIndex = userDataLines.Length - 1;
            }
        }


       

        private void btn_clear_Click(object sender, EventArgs e)
        {
            // Display a confirmation dialog
            DialogResult result = MessageBox.Show("Are you sure you want to clear all fields? This action cannot be undone.", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            // Check the user's response
            if (result == DialogResult.OK)
            {
                // Clear all fields if the user clicks "OK"
                ClearFieldsExceptSerialNumber();
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (textbox_firstname.Text == "")
            {
                textbox_firstname.Focus();
                MessageBox.Show("First Name is Required");
                return;
            }
            if (textbox_qualification.Text == "")
            {
                textbox_qualification.Focus();
                MessageBox.Show("Qualification is Required");
                return;
            }
            if (textbox_currentcompany.Text == "")
            {
                textbox_currentcompany.Focus();
                MessageBox.Show("Current Company is Required");
                return;
            }

            // Create a string representing the updated entry
            string updatedEntry = $"{textbox_serialNumber.Text},{textbox_prefix.Text},{textbox_firstname.Text},{textbox_middlename.Text},{textbox_lastname.Text},{date_dateofBirth.Value.ToShortDateString()},{textbox_qualification.Text},{date_joiningDate.Value.ToShortDateString()},{textbox_currentcompany.Text},{textbox_currentaddress.Text}";

            // Save the updated data to the file
            SaveUpdatedDataToFile(updatedEntry, selectedRowIndex);

            this.Close();
        }

        private void SaveUpdatedDataToFile(string data, int rowIndex)
        {
            try
            {
                string filePath = "UserData.txt"; // Specify the path to your file
                string[] lines = File.ReadAllLines(filePath);

                // Update the selected row with the new data
                lines[rowIndex] = data;

                // Write the updated data back to the file
                File.WriteAllLines(filePath, lines);

                MessageBox.Show("Data updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}