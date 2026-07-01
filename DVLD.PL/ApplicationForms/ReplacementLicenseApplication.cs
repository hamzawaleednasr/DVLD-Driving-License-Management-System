using System;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;

namespace DVLD.PL.ApplicationForms
{
    public partial class ReplacementLicenseApplication : Form
    {
        private readonly ApplicationTypeService _applicationTypeService;
        private readonly LicenseClassService _licenseClassService;
        private readonly ApplicationService _applicationService;
        private readonly LicenseService _licenseService;

        private LicenseDto _license;

        public ReplacementLicenseApplication()
        {
            InitializeComponent();

            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
            _licenseClassService = new LicenseClassService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _licenseService = new LicenseService(AppConfig.ConnectionString);
        }

        private void ReplacementLicenseApplication_Load(object sender, EventArgs e)
        {
            LoadFormData();
        }

        private void LoadFormData()
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = AppConfig.LoggedInUser.Username;
        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamaged.Checked)
            {
                lblTitle.Text = "Replacement Damage License";
                this.Text = "Replacement Damage License";
                lblApplicationFees.Text = _applicationTypeService.GetByID(5).ApplicationTypeFees.ToString();
                lblApplicationFees.Tag = 5;
            }
            else
            {
                lblTitle.Text = "Replacement Lost License";
                this.Text = "Replacement Lost License";
                lblApplicationFees.Text = _applicationTypeService.GetByID(6).ApplicationTypeFees.ToString();
                lblApplicationFees.Tag = 6;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseID.Text))
            {
                MessageBox.Show("License ID must not be an empty string!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _license = _licenseService.GetByID(Convert.ToInt32(txtLicenseID.Text));

            if (_license == null)
            {
                MessageBox.Show($"Couldn't to found the license with application id '{txtLicenseID.Text}', try anothern one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_license.IsActive)
            {
                MessageBox.Show($"License with id '{txtLicenseID.Text}' isn't active, try anohter active one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            driverLicenseInfoControl1.LicenseID = _license.LicenseID;
            btnIssue.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (_license == null) return;

            if (MessageBox.Show("Are you sure you want to replace this license?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            var oldApplication = _applicationService.GetByID(_license.ApplicationID);
            if (oldApplication == null)
            {
                MessageBox.Show("Failed to found the old application, please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var appType = _applicationTypeService.GetByID(Convert.ToInt32(lblApplicationFees.Tag));
            var appFees = appType != null ? appType.ApplicationTypeFees : 10;

            var appResult = _applicationService.Add(new ApplicationDto
            {
                ApplicationStatus = "Completed",
                ApplicationLastStatusDate = DateTime.Now,
                ApplicationPaidFees = appFees,
                CreatedAt = DateTime.Now,
                PersonID = oldApplication.PersonID,
                ApplicationTypeID = appType.ApplicationTypeID,
                CreatedByUserID = AppConfig.LoggedInUser.UserID,
            });

            if (!appResult.newApplicationID.HasValue || appResult.status != enApplicationSaveStatus.Success)
            {
                MessageBox.Show("Failed to create a new application, try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int newApplicationID = appResult.newApplicationID.Value;

            lblLRApplicationID.Text = newApplicationID.ToString();
            lblOldLicenseID.Text = _license.LicenseID.ToString();

            _license.IsActive = false;
            if (!_licenseService.Update(_license))
            {
                MessageBox.Show("Failed to deactivate the old license, try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                LicenseIssueReason = Convert.ToByte(rbDamaged.Checked ? 3 : 4), // 3, 4 = Replacement
                IsActive = true,
                DriverID = _license.DriverID,
                ApplicationID = newApplicationID,
                LicenseClassID = _license.LicenseClassID
            });

            if (licenseResult.isSuccess && licenseResult.newLicenseID.HasValue)
            {
                int newLicenseID = licenseResult.newLicenseID.Value;
                lblReplacedLicenseID.Text = newLicenseID.ToString();

                driverLicenseInfoControl1.LicenseID = newLicenseID;

                MessageBox.Show($"License replacemented successfully with new License ID = {newLicenseID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnIssue.Enabled = false;
                btnSearch.Enabled = false;
                txtLicenseID.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed to issue the replacement license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _license.IsActive = true;
                _licenseService.Update(_license);
                _applicationService.Delete(newApplicationID);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
