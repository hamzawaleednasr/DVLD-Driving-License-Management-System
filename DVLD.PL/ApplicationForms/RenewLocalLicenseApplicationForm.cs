using System;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;

namespace DVLD.PL.ApplicationForms
{
    public partial class RenewLocalLicenseApplicationForm : Form
    {
        private readonly LicenseService _licenseService;
        private readonly ApplicationService _applicationService;
        private readonly ApplicationTypeService _applicationTypeService;
        private readonly LicenseClassService _licenseClassService;

        private LicenseDto _license;

        public int LicenseID { get; set; }


        public RenewLocalLicenseApplicationForm()
        {
            InitializeComponent();

            _licenseService = new LicenseService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
            _licenseClassService = new LicenseClassService(AppConfig.ConnectionString);
        }

        private void RenewLocalLicenseApplicationForm_Load(object sender, EventArgs e)
        {
            driverLicenseInfoControl1.LicenseID = LicenseID;
            LoadApplicationData();
        }

        private void LoadApplicationData()
        {
            lblApplicationDate.Text = DateTime.Today.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = DateTime.Today.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = AppConfig.LoggedInUser.Username;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseID.Text))
            {
                MessageBox.Show("License id must not be an empty string!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _license = _licenseService.GetByID(Convert.ToInt32(txtLicenseID.Text));

            if (_license == null)
            {
                MessageBox.Show($"Couldn't to found the license with id '{txtLicenseID.Text}', try another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_license.LicenseExpirationDate > DateTime.Now)
            {
                MessageBox.Show($"The license with id '{_license.LicenseID}' not expired yet, it will expire in '{_license.LicenseExpirationDate.ToString("dd/MMM/yyyy")}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_license.IsActive)
            {
                MessageBox.Show($"The license with id '{_license.LicenseID}' is not active. Only active licenses can be renewed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            driverLicenseInfoControl1.LicenseID = _license.LicenseID;

            // Load and display fees
            var appType = _applicationTypeService.GetByID(2); // 2 = Renew Driving License Service
            var licenseClass = _licenseClassService.GetByID(_license.LicenseClassID);

            double appFees = appType != null ? appType.ApplicationTypeFees : 7;
            double licenseFees = licenseClass != null ? licenseClass.LicenseClassFees : 20;

            lblApplicationFees.Text = appFees.ToString();
            lblLicenseFees.Text = licenseFees.ToString();
            lblTotalFees.Text = (appFees + licenseFees).ToString();

            lblOldLicenseID.Text = _license.LicenseID.ToString();
            btnRenew.Enabled = true;
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (_license == null) return;

            if (MessageBox.Show("Are you sure you want to renew this license?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            var oldApplication = _applicationService.GetByID(_license.ApplicationID);
            if (oldApplication == null)
            {
                MessageBox.Show("Failed to find details of the old application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var appType = _applicationTypeService.GetByID(2); // 2 = Renew Driving License Service
            double appFees = appType != null ? appType.ApplicationTypeFees : 7;

            var appResult = _applicationService.Add(new ApplicationDto
            {
                PersonID = oldApplication.PersonID,
                CreatedAt = DateTime.Now,
                ApplicationTypeID = 2, // Renew Driving License Service
                ApplicationStatus = "Completed",
                ApplicationLastStatusDate = DateTime.Now,
                ApplicationPaidFees = appFees,
                CreatedByUserID = AppConfig.LoggedInUser.UserID
            });

            if (!appResult.newApplicationID.HasValue || appResult.status != enApplicationSaveStatus.Success)
            {
                MessageBox.Show("Failed to create the renewal application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int newApplicationID = appResult.newApplicationID.Value;

            _license.IsActive = false;
            if (!_licenseService.Update(_license))
            {
                MessageBox.Show("Failed to deactivate the old license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _applicationService.Delete(newApplicationID);
                return;
            }

            var licenseClass = _licenseClassService.GetByID(_license.LicenseClassID);
            double licenseFees = licenseClass != null ? licenseClass.LicenseClassFees : 20;
            int validityLength = licenseClass != null ? licenseClass.ValidityLength : 10;

            var licenseResult = _licenseService.Add(new LicenseDto
            {
                LicenseIssueDate = DateTime.Now,
                LicenseExpirationDate = DateTime.Now.AddYears(validityLength),
                LicensePaidFees = licenseFees,
                LicenseIssueReason = 2, // 2 = Renew
                IsActive = true,
                Notes = txtNotes.Text.Trim(),
                DriverID = _license.DriverID,
                ApplicationID = newApplicationID,
                LicenseClassID = _license.LicenseClassID
            });

            if (licenseResult.isSuccess && licenseResult.newLicenseID.HasValue)
            {
                int newLicenseID = licenseResult.newLicenseID.Value;
                lblRLApplicationID.Text = newApplicationID.ToString();
                lblRenewedLicenseID.Text = newLicenseID.ToString();
                lblExpirationDate.Text = DateTime.Now.AddYears(validityLength).ToString("dd/MMM/yyyy");

                driverLicenseInfoControl1.LicenseID = newLicenseID;

                MessageBox.Show($"License renewed successfully with new License ID = {newLicenseID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRenew.Enabled = false;
                btnSearch.Enabled = false;
                txtLicenseID.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed to issue the renewed license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _license.IsActive = true;
                _licenseService.Update(_license);
                _applicationService.Delete(newApplicationID);
            }
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
