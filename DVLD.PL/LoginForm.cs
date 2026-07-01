using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.Enums;

namespace DVLD.PL
{
    public partial class LoginForm : Form
    {
        private UserService _userService;    

        public LoginForm()
        {
            InitializeComponent();

            _userService = new UserService(AppConfig.ConnectionString);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var result = _userService.Login(txtUsername.Text, txtPassword.Text);

            if (result.isValid)
            {
                AppConfig.LoggedInUser = result.LoggedInUser;

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                switch (result.status)
                {
                    case enLoginStatus.InActive:
                        MessageBox.Show("This user is inactive, contact with your admin.", "User InActive", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enLoginStatus.UsernameNotExists:
                        MessageBox.Show("This user isn't exist, try again", "User not exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enLoginStatus.PasswordWrong:
                        MessageBox.Show("Password wrong, try again.", "Wrong Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enLoginStatus.RequiredDataMissing:
                        MessageBox.Show("You must write username and password, fill all fields", "Required data missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }
    }
}
