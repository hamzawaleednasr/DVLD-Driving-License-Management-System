using System;
using System.Windows.Forms;
using DVLD.PL.ApplicationForms;
using DVLD.PL.DriverForms;
using DVLD.PL.LicenseForms;
using DVLD.PL.PersonForms;
using DVLD.PL.TestForms;
using DVLD.PL.UserForms;

namespace DVLD.PL
{
    public partial class HomePageForm : Form
    {
        public HomePageForm()
        {
            InitializeComponent();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppConfig.LoggedInUser = null;

            Application.Restart();

            Environment.Exit(0);
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManagePeopleForm frm = new ManagePeopleForm();
            frm.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageUsersForm frm = new ManageUsersForm();
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageApplicationTypesForm frm = new ManageApplicationTypesForm();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageTestTypesForm frm = new ManageTestTypesForm();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditLocalDrivingLicenseApplication frm = new AddEditLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocalLicensesForm frm = new ManageLocalLicensesForm();
            frm.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUserDetailsForm frm = new ShowUserDetailsForm();
            frm.SelectedUserID = AppConfig.LoggedInUser.UserID;
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageDriversForm frm = new ManageDriversForm();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddInternationalLicenseForm frm = new AddInternationalLicenseForm();
            frm.ShowDialog();
        }

        private void internationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageInternationalLicenseForm frm = new ManageInternationalLicenseForm();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenewLocalLicenseApplicationForm frm = new RenewLocalLicenseApplicationForm();
            frm.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacementLicenseApplication frm = new ReplacementLicenseApplication();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DetainLicenseForm frm = new DetainLicenseForm();
            frm.ShowDialog();
        }

        private void relToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicenseForm frm = new ReleaseDetainedLicenseForm();
            frm.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageDetainedLicensesForm frm = new ManageDetainedLicensesForm();
            frm.ShowDialog();
        }
    }
}
