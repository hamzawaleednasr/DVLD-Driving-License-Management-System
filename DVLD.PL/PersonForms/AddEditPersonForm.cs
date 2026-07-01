using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DVLD.PL.PersonForms
{
    public partial class AddEditPersonForm : Form
    {
        private string personalPhotoPath;
        private readonly PersonService _personService;
        private readonly CountryService _countryService;

        public bool IsCreateMode { get; set; } = true;
        public int PersonID { get; set; }

        public AddEditPersonForm()
        {
            InitializeComponent();

            _personService = new PersonService(AppConfig.ConnectionString);
            _countryService = new CountryService(AppConfig.ConnectionString);
        }

        private void AddEditPersonForm_Load(object sender, EventArgs e)
        {
            fillCountryComboBox();
            framingBirthDatePicker();
            if (!IsCreateMode)
            {
                PersonDto person = _personService.GetByID(PersonID);

                if (person == null) return;

                lblAddEditPerson.Text = "Edit Person Details";
                btnSave.Visible = false;
                btnUpdate.Visible = true;

                lblPersonID.Text = person.PersonID.ToString();
                txtFirstName.Text = person.FirstName.ToString();
                txtSecondName.Text = person.SecondName.ToString();
                txtThirdName.Text = person.ThirdName.ToString();
                txtLastName.Text = person.LastName.ToString();
                txtNationalNumber.Text = person.NationalNumber.ToString();
                txtEmail.Text = person.Email.ToString();
                txtAddress.Text = person.Address.ToString();
                txtPhone.Text = person.Phone.ToString();
                rbMale.Checked = !person.Gender;
                rbFemale.Checked = person.Gender;
                dtpBirthDate.Value = Convert.ToDateTime(person.BirthDate);
                cmbCountries.SelectedValue = Convert.ToInt32(person.CountryID); 
                if (!string.IsNullOrEmpty(person.PersonalPhoto) && File.Exists(person.PersonalPhoto))
                {
                    pbPersonalImage.Image = Image.FromFile(person.PersonalPhoto);
                    personalPhotoPath = person.PersonalPhoto;
                }
            }
        }

        // ==== Validate Form Fields ===========================================
        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errorProvider1.SetError(txtFirstName, "First Name Required!");
                e.Cancel = true; 
            }
            else
            {
                errorProvider1.SetError(txtFirstName, ""); 
            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                errorProvider1.SetError(txtLastName, "Last Name Required!");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtLastName, "");
            }
        }

        private void txtNationalNumber_Validating(object sender, CancelEventArgs e)
        {
            if (!txtNationalNumber.Text.StartsWith("N"))
            {
                errorProvider1.SetError(txtNationalNumber, "National Number must start with 'N' capital!");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtNationalNumber, "");
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtPhone.Text, out int test) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Phone must be only numbers");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtPhone, "");
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text)) return;

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorProvider1.SetError(txtEmail, "Invalid email!");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void framingBirthDatePicker()
        {
            dtpBirthDate.MaxDate = DateTime.Today.AddYears(-18);
        }

        private void fillCountryComboBox()
        {
            List<CountryDto> countryDtos = _countryService.GetAllCountries();
            cmbCountries.DataSource = countryDtos;
            cmbCountries.DisplayMember = "CountryName";
            cmbCountries.ValueMember = "CountryID";
        }

        // ===== Save Photo In Our Blob Storage ==================================
        private void CopyPhotoInBlob(string phPath)
        {
            if (!Directory.Exists(AppConfig.StoragePath))
            {
                Directory.CreateDirectory(AppConfig.StoragePath);
            }

            string extestion = Path.GetExtension(phPath);
            string photoName = Guid.NewGuid().ToString();
            string newPhotoPath = AppConfig.StoragePath + photoName + extestion;
            personalPhotoPath = newPhotoPath;

            File.Copy(phPath, newPhotoPath, true);
        }

        private void lnklblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Title = "Choose a photo";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.webp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                CopyPhotoInBlob(filePath);

                pbPersonalImage.Image = Image.FromFile(personalPhotoPath);

                lnklblSetImage.Enabled = false;
                lnklblDeleteImage.Visible = true;
            }
        }

        private void lnklblDeleteImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonalImage.Image = Properties.Resources.DefaultPhoto;

            lnklblSetImage.Enabled = true;
            lnklblDeleteImage.Visible = false;

            personalPhotoPath = string.Empty;
        }

        // ===== Save/Update Person ====================================
        private void btnSave_Click(object sender, EventArgs e)
        {
            var result = _personService.Add(
                new PersonDto
                {
                    CountryID = Convert.ToInt32(cmbCountries.SelectedValue),
                    Gender = rbMale.Checked ? false : true,
                    NationalNumber = txtNationalNumber.Text,
                    FirstName = txtFirstName.Text,
                    SecondName = txtSecondName.Text,
                    ThirdName = txtThirdName.Text,
                    LastName = txtLastName.Text,
                    Address = txtAddress.Text,
                    Phone = txtPhone.Text,
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text,
                    PersonalPhoto = personalPhotoPath,
                    BirthDate = dtpBirthDate.Value,
                }
            );

            if (result.newPersonID != null && result.status == enPersonSaveStatus.Success)
            {
                lblPersonID.Text = Convert.ToString(result.newPersonID);
                MessageBox.Show($"The new person added successfully with {result.newPersonID} id number.", "Person Added Successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PersonID = (int)result.newPersonID;
                this.Close();
            }
            else
            {
                switch (result.status)
                {
                    case enPersonSaveStatus.RequiredDataMissing:
                        MessageBox.Show("You must fill all required fields!", "Required data missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enPersonSaveStatus.InvalidEmail:
                        MessageBox.Show("The Email is invalid, please try again.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtEmail.Focus();
                        break;
                    case enPersonSaveStatus.EmailExists:
                        MessageBox.Show("The Email is already exists, try another one.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtEmail.Focus();
                        break;
                    case enPersonSaveStatus.NationalNumberExists:
                        MessageBox.Show("The National Number is already exists, try another one.", "Naitonal Number Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtNationalNumber.Focus();
                        break;
                    case enPersonSaveStatus.AgeLessThan18:
                        MessageBox.Show("The age must be grater than 18 years, you're not eligible.", "NotApplicable Age", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dtpBirthDate.Focus();
                        break;
                }
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var result = _personService.Update(
                new PersonDto
                {
                    PersonID = Convert.ToInt32(lblPersonID.Text),
                    CountryID = Convert.ToInt32(cmbCountries.SelectedValue),
                    Gender = rbMale.Checked ? false : true,
                    NationalNumber = txtNationalNumber.Text,
                    FirstName = txtFirstName.Text,
                    SecondName = txtSecondName.Text,
                    ThirdName = txtThirdName.Text,
                    LastName = txtLastName.Text,
                    Address = txtAddress.Text,
                    Phone = txtPhone.Text,
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text,
                    PersonalPhoto = personalPhotoPath,
                    BirthDate = dtpBirthDate.Value,
                }
            );

            if (result.isSuccess && result.status == enPersonSaveStatus.Success)
            {
                MessageBox.Show($"The new person updated successfully!", "Person Updated Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                switch (result.status)
                {
                    case enPersonSaveStatus.RequiredDataMissing:
                        MessageBox.Show("You must fill all required fields!", "Required data missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enPersonSaveStatus.InvalidEmail:
                        MessageBox.Show("The Email is invalid, please try again.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtEmail.Focus();
                        break;
                    case enPersonSaveStatus.EmailExists:
                        MessageBox.Show("The Email is already exists, try another one.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtEmail.Focus();
                        break;
                    case enPersonSaveStatus.NationalNumberExists:
                        MessageBox.Show("The National Number is already exists, try another one.", "Naitonal Number Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtNationalNumber.Focus();
                        break;
                    case enPersonSaveStatus.AgeLessThan18:
                        MessageBox.Show("The age must be grater than 18 years, you're not eligible.", "NotApplicable Age", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dtpBirthDate.Focus();
                        break;
                }
            }
        }
    }
}
