using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.PL.PersonForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.PersonForms
{
    public partial class ManagePeopleForm : Form
    {
        private readonly PersonService _personService;
        private int _selectedPersonID = -1;

        public ManagePeopleForm()
        {
            InitializeComponent();

            _personService = new PersonService(AppConfig.ConnectionString);
        }

        private void ManagePeopleForm_Load(object sender, EventArgs e)
        {
            dtgPeople.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cmbFilters.SelectedIndex = 0;
            dtgPeople.ReadOnly = true;
            GetAllPeople();
            SetPeopleNumber();
        }

        private void LoadPeople(List<PersonViewModel> people)
        {
            dtgPeople.DataSource = people == null ? null : new BindingList<PersonViewModel>(people);

            if (dtgPeople.Columns.Contains("Gender")) dtgPeople.Columns["Gender"].Visible = false;

            if (dtgPeople.Columns.Contains("GenderText")) dtgPeople.Columns["GenderText"].HeaderText = "Gender";
        }

        private void GetAllPeople()
        {
            LoadPeople(_personService.GetAllWithCountry());
        }

        private void SetPeopleNumber()
        {
            lblPeopleNumber.Text = _personService.GetNumberOfPeople().ToString();
        }

        private void GetPeopleByID(int id)
        {
            PersonViewModel person = _personService.GetByIDWithCountry(id);
            LoadPeople(person == null ? null : new List<PersonViewModel> { person });
        }

        private void GetPeopleByNationalNumber(string nationalNumber)
        {
            PersonViewModel person = _personService.GetByNationalNumberWithCountry(nationalNumber);
            LoadPeople(person == null ? null : new List<PersonViewModel> { person });
        }

        private void GetPeopleByFirstName(string firstName)
        {
            LoadPeople(_personService.GetByFirstNameWithCountry(firstName));
        }

        private void GetPeopleBySecondName(string secondName)
        {
            LoadPeople(_personService.GetBySecondNameWithCountry(secondName));
        }

        private void GetPeopleByThirdName(string thirdName)
        {
            LoadPeople(_personService.GetByThirdNameWithCountry(thirdName));
        }

        private void GetPeopleByLastName(string lastName)
        {
            LoadPeople(_personService.GetByLastNameWithCountry(lastName));
        }

        private void GetPeopleByNationality(string nationality)
        {
            LoadPeople(_personService.GetByNationalityWithCountry(nationality));
        }

        private void GetPeopleByGender(string genderText)
        {
            bool gender = genderText == "Male" ? false : true; 
            LoadPeople(_personService.GetByGenderWithCountry(gender));
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFilters.SelectedIndex)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    cmbFiltering.Visible = false;
                    txtFiltering.Visible = true;
                    break;
                case 3:
                    cmbFiltering.Visible = true;
                    txtFiltering.Visible = false;
                    cmbFiltering.Items.Clear();
                    cmbFiltering.Items.Add("All");
                    cmbFiltering.Items.Add("Male");
                    cmbFiltering.Items.Add("Female");
                    cmbFiltering.SelectedIndex = 0;
                    break;
                case 0:
                default:
                    break;
            }
        }

        private void txtFiltering_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFiltering.Text))
            {
                GetAllPeople();
                return;
            }

            switch (cmbFilters.SelectedIndex)
            {
                case 1:
                    GetPeopleByID(Convert.ToInt32(txtFiltering.Text)); // id
                    break;
                case 2:
                    GetPeopleByNationalNumber(txtFiltering.Text); // national no.
                    break;
                case 4:
                    GetPeopleByFirstName(txtFiltering.Text); // first name
                    break;
                case 5:
                    GetPeopleBySecondName(txtFiltering.Text);  // second //
                    break;
                case 6:
                    GetPeopleByThirdName(txtFiltering.Text);  // third //
                    break;
                case 7:
                    GetPeopleByLastName(txtFiltering.Text);  // last //
                    break;
                case 8:
                    GetPeopleByNationality(txtFiltering.Text);  // nationality
                    break;
            }
        }

        private void cmbFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFiltering.SelectedItem.ToString())
            {
                case "Male":
                    GetPeopleByGender("Male");
                    break;
                case "Female":
                    GetPeopleByGender("Female");
                    break;
                case "All":
                    GetAllPeople();
                    break;
            }
        }

        private void txtFiltering_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilters.SelectedIndex != 1) return;

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; 
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            AddEditPersonForm frm = new AddEditPersonForm();
            frm.ShowDialog();
            ManagePeopleForm_Load(sender, e);
        }

        // ==== Context menu strip ================================
        private void dtgPeople_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dtgPeople.CurrentCell = dtgPeople.Rows[e.RowIndex].Cells[e.ColumnIndex];
                _selectedPersonID = Convert.ToInt32(dtgPeople.Rows[e.RowIndex].Cells["PersonID"].Value);
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonDetailsForm frm = new ShowPersonDetailsForm();
            frm.SelectedPersonID = _selectedPersonID;
            frm.ShowDialog();
            ManagePeopleForm_Load(sender, e);
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditPersonForm frm = new AddEditPersonForm();
            frm.PersonID = _selectedPersonID;
            frm.IsCreateMode = false;
            frm.ShowDialog();
            ManagePeopleForm_Load(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool isDeleted = _personService.Delete(_selectedPersonID);

                if (isDeleted)
                {
                    MessageBox.Show("The person deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ManagePeopleForm_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("This person related with user in system, you must delete user first!", "Cannot Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
