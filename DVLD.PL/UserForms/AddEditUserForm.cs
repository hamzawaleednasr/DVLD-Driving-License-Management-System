using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.PL.PersonForms;
using System;
using System.Windows.Forms;

namespace DVLD.PL.UserForms
{
    public partial class AddEditUserForm : Form
    {
        private readonly PersonService _personService;
        private readonly UserService _userService;

        public bool IsCreateMode { get; set; } = true;
        public int UserID { get; set; }

        public AddEditUserForm()
        {
            InitializeComponent();

            _personService = new PersonService(AppConfig.ConnectionString);
            _userService = new UserService(AppConfig.ConnectionString);
        }

        private void AddEditUserForm_Load(object sender, EventArgs e)
        {
            cmbFilters.SelectedIndex = 0;
            personDetailsControl1.lnklblEditPerson.Enabled = false;

            if (!IsCreateMode)
            {
                lblAddEditUser.Text = "Update User";
                groupBox1.Enabled = false;
                btnSave.Text = "Update";

                UserDto user = _userService.GetByID(UserID);
                if (user != null)
                {
                    lblUserID.Text = user.UserID.ToString();
                    txtUsername.Text = user.Username;

                    txtPassword.Enabled = false;
                    txtConfirmPassword.Enabled = false;

                    personDetailsControl1.PersonID = user.PersonID;
                    personDetailsControl1.lnklblEditPerson.Enabled = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tcAddEditUser.SelectedIndex++;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tcAddEditUser.SelectedIndex--;
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterString.Text))
            {
                MessageBox.Show("Filter string cannot be an empty string!", "Cannot be empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (cmbFilters.SelectedIndex) 
            {
                case 0: // National Number
                    PersonViewModel personByNational = _personService.GetByNationalNumberWithCountry(txtFilterString.Text.ToString());
                    if (personByNational == null)
                    {
                        MessageBox.Show($"Not person found with national number '{txtFilterString.Text}', try again", "Person not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (_userService.GetByPersonIDToView(personByNational.PersonID) != null)
                    {
                        MessageBox.Show($"Person with national number '{txtFilterString.Text} already related with another user', try anohter one.", "Cannot link with this person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    personDetailsControl1.PersonID = personByNational.PersonID;
                    personDetailsControl1.lnklblEditPerson.Enabled = true;
                    break;
                case 1: // Person ID
                    PersonViewModel personByID = _personService.GetByIDWithCountry(Convert.ToInt32(txtFilterString.Text));
                    if (personByID == null)
                    {
                        MessageBox.Show($"Not person found with id '{txtFilterString.Text}', try again", "Person not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (_userService.GetByPersonIDToView(personByID.PersonID) != null)
                    {

                        MessageBox.Show($"Person with id '{txtFilterString.Text} already related with another user', try anohter one.", "Cannot link with this person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    personDetailsControl1.PersonID = personByID.PersonID;
                    personDetailsControl1.lnklblEditPerson.Enabled = true;
                    break;
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            AddEditPersonForm frm = new AddEditPersonForm();
            frm.IsCreateMode = true;
            frm.ShowDialog();
            personDetailsControl1.PersonID = frm.PersonID;
            personDetailsControl1.lnklblEditPerson.Enabled = true;
        }

        private void txtUsername_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "username required!");
            }
            else
            {
                UserViewModel foundUser = _userService.GetByUsernameToView(txtUsername.Text.ToString());
                if (foundUser != null && (IsCreateMode || foundUser.UserID != UserID))
                {
                    errorProvider1.SetError(txtUsername, $"'{txtUsername.Text}' already taken, try another one.");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(txtUsername, "");
                }
            }
        }

        private void txtConfirmPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsCreateMode) return;

            if (string.IsNullOrEmpty(txtConfirmPassword.Text) || txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Confirmation password not match the password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsCreateMode)
            {
                if (txtConfirmPassword.Text != txtPassword.Text)
                {
                    MessageBox.Show("Confirmation password does not match the password.", "Must matching", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = _userService.Add(
                    new UserDto {
                        PersonID = personDetailsControl1.PersonID,
                        Username = txtUsername.Text,
                        Password = txtPassword.Text,
                    }
                );

                if (result.newUserID != null && result.status == enUserSaveStatus.Success)
                {
                    MessageBox.Show($"User with added successfully, with id {result.newUserID}", "User added successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblUserID.Text = result.newUserID.ToString();
                    this.Close();
                }
                else
                {
                    switch (result.status)
                    {
                        case enUserSaveStatus.RequiredDataMissing:
                            MessageBox.Show($"Required data missing, you must fill all required fields!", "Required data missing!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enUserSaveStatus.PersonAlreadyUser:
                            MessageBox.Show($"This person already user, choose another one.", "Person already user!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enUserSaveStatus.UsernameAlreadyExists:
                            MessageBox.Show($"Username already exists, use another one.", "Username already exists!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enUserSaveStatus.PersonNotExists:
                            MessageBox.Show($"This person not exists, choose another one.", "Person not exists!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            else
            {
                UserDto currentUser = _userService.GetByID(UserID);
                bool isActive = currentUser != null ? currentUser.IsActive : true;

                var result = _userService.Update(
                    new UserDto {
                        UserID = UserID,
                        PersonID = personDetailsControl1.PersonID,
                        Username = txtUsername.Text,
                        IsActive = isActive
                    }
                );

                if (result.isSuccess && result.status == enUserSaveStatus.Success)
                {
                    MessageBox.Show($"User updated successfully!", "User Updated Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    switch (result.status)
                    {
                        case enUserSaveStatus.RequiredDataMissing:
                            MessageBox.Show($"Required data missing, you must fill all required fields!", "Required data missing!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enUserSaveStatus.UsernameAlreadyExists:
                            MessageBox.Show($"Username already exists, use another one.", "Username already exists!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
        }
    }
}
