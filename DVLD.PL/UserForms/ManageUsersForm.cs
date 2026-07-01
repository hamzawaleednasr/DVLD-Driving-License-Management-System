using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.UserForms
{
    public partial class ManageUsersForm : Form
    {
        private readonly UserService _userService;
        private int _selectedUserID = -1;

        public ManageUsersForm()
        {
            InitializeComponent();
            _userService = new UserService(AppConfig.ConnectionString);
        }

        private void ManageUsersForm_Load(object sender, EventArgs e)
        {
            dtgUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cmbFilters.SelectedIndex = 0;
            dtgUsers.ReadOnly = true;
            GetAllUsers();
            SetUsersNumber();
        }

        private void LoadUsers(List<UserViewModel> users)
        {
            dtgUsers.DataSource = users == null ? null : new BindingList<UserViewModel>(users);
        }

        private void GetAllUsers()
        {
            LoadUsers(_userService.GetAllToView());
        }

        private void SetUsersNumber()
        {
            lblUsersNumber.Text = _userService.GetNumberOfUsers().ToString();
        }

        private void GetUserByID(int id)
        {
            UserViewModel user = _userService.GetByIDToView(id);
            LoadUsers(user == null ? null : new List<UserViewModel> { user });
        }

        private void GetUserByPersonID(int personId)
        {
            UserViewModel user = _userService.GetByPersonIDToView(personId);
            LoadUsers(user == null ? null : new List<UserViewModel> { user });
        }

        private void GetUserByUsername(string username)
        {
            UserViewModel user = _userService.GetByUsernameToView(username);
            LoadUsers(user == null ? null : new List<UserViewModel> { user });
        }

        private void GetUsersByFullName(string fullName)
        {
            LoadUsers(_userService.GetByFullNameToView(fullName));
        }

        private void GetUsersByActivity(bool isActive)
        {
            LoadUsers(_userService.GetByActivityToView(isActive));
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltering.Clear();
            cmbFiltering.Items.Clear();

            switch (cmbFilters.SelectedIndex)
            {
                case 1: // User ID
                case 2: // Person ID
                case 3: // Full Name
                case 4: // Username
                    cmbFiltering.Visible = false;
                    txtFiltering.Visible = true;
                    txtFiltering.Focus();
                    break;
                case 5: // Is Active
                    cmbFiltering.Visible = true;
                    txtFiltering.Visible = false;
                    cmbFiltering.Items.Add("All");
                    cmbFiltering.Items.Add("Yes");
                    cmbFiltering.Items.Add("No");
                    cmbFiltering.SelectedIndex = 0;
                    break;
                case 0: // None
                default:
                    cmbFiltering.Visible = false;
                    txtFiltering.Visible = false;
                    GetAllUsers();
                    break;
            }
        }

        private void txtFiltering_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFiltering.Text))
            {
                GetAllUsers();
                return;
            }

            switch (cmbFilters.SelectedIndex)
            {
                case 1: // User ID
                    if (int.TryParse(txtFiltering.Text, out int userId))
                    {
                        GetUserByID(userId);
                    }
                    else
                    {
                        LoadUsers(null);
                    }
                    break;
                case 2: // Person ID
                    if (int.TryParse(txtFiltering.Text, out int personId))
                    {
                        GetUserByPersonID(personId);
                    }
                    else
                    {
                        LoadUsers(null);
                    }
                    break;
                case 3: // Full Name
                    GetUsersByFullName(txtFiltering.Text);
                    break;
                case 4: // Username
                    GetUserByUsername(txtFiltering.Text);
                    break;
            }
        }

        private void cmbFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFiltering.SelectedItem.ToString())
            {
                case "Yes":
                    GetUsersByActivity(true);
                    break;
                case "No":
                    GetUsersByActivity(false);
                    break;
                case "All":
                default:
                    GetAllUsers();
                    break;
            }
        }

        private void txtFiltering_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits for User ID and Person ID
            if (cmbFilters.SelectedIndex == 1 || cmbFilters.SelectedIndex == 2)
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddEditUserForm frm = new AddEditUserForm();
            frm.ShowDialog();
            ManageUsersForm_Load(sender, e);
        }

        // ==== Context Menu Strip Event Handlers ====================
        private void dtgUsers_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dtgUsers.CurrentCell = dtgUsers.Rows[e.RowIndex].Cells[e.ColumnIndex];
                _selectedUserID = Convert.ToInt32(dtgUsers.Rows[e.RowIndex].Cells["UserID"].Value);
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUserDetailsForm frm = new ShowUserDetailsForm();
            frm.SelectedUserID = _selectedUserID;
            frm.ShowDialog();
            ManageUsersForm_Load(sender, e);
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditUserForm frm = new AddEditUserForm();
            frm.UserID = _selectedUserID;
            frm.IsCreateMode = false;
            frm.ShowDialog();
            ManageUsersForm_Load(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool isDeleted = _userService.Delete(_selectedUserID);

                if (isDeleted)
                {
                    MessageBox.Show("The user deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ManageUsersForm_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Could not delete this user!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
