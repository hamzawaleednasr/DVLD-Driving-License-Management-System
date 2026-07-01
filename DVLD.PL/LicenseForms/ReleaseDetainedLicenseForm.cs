using System;
using System.Linq;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;

namespace DVLD.PL.LicenseForms
{
    public partial class ReleaseDetainedLicenseForm : Form
    {
        private readonly LicenseService _licenseService;
        private readonly DetainedLicenseService _detainedLicenseService;
        private readonly ApplicationService _applicationService;
        private readonly ApplicationTypeService _applicationTypeService;

        private LicenseDto _license;
        private DetainedLicenseDto _detainedLicense;
        private double _releaseAppFees = 15;

        public ReleaseDetainedLicenseForm()
        {
            InitializeComponent();
            _licenseService = new LicenseService(AppConfig.ConnectionString);
            _detainedLicenseService = new DetainedLicenseService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
        }

        private void ReleaseDetainedLicenseForm_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Today.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = AppConfig.LoggedInUser.Username;

            var appType = _applicationTypeService.GetByID(5); // 5 = Release Detained Driving Licsense
            if (appType != null)
            {
                _releaseAppFees = appType.ApplicationTypeFees;
            }
            lblApplicationFees.Text = _releaseAppFees.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseID.Text))
            {
                MessageBox.Show("License ID must not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int licenseID = Convert.ToInt32(txtLicenseID.Text);
            _license = _licenseService.GetByID(licenseID);

            if (_license == null)
            {
                MessageBox.Show($"Could not find license with ID = {licenseID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetForm();
                return;
            }

            driverLicenseInfoControl1.LicenseID = _license.LicenseID;
            lblLicenseID.Text = _license.LicenseID.ToString();

            var detainedList = _detainedLicenseService.GetAll();
            _detainedLicense = detainedList?.FirstOrDefault(d => d.LicenseID == licenseID && !d.IsReleased);

            if (_detainedLicense == null)
            {
                MessageBox.Show("This license is not detained, or has already been released.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRelease.Enabled = false;
                ResetForm();
                return;
            }

            lblDetainID.Text = _detainedLicense.DetainedLicenseID.ToString();
            lblDetainDate.Text = _detainedLicense.DetainDate.ToString("dd/MMM/yyyy");
            lblFineFees.Text = _detainedLicense.FineFees.ToString();
            lblTotalFees.Text = (_releaseAppFees + _detainedLicense.FineFees).ToString();

            btnRelease.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (_license == null || _detainedLicense == null) return;

            if (MessageBox.Show("Are you sure you want to release this detained license?", "Confirm Release", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            var oldApplication = _applicationService.GetByID(_license.ApplicationID);
            if (oldApplication == null)
            {
                MessageBox.Show("Failed to find details of the original application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var appResult = _applicationService.Add(new ApplicationDto
            {
                PersonID = oldApplication.PersonID,
                CreatedAt = DateTime.Now,
                ApplicationTypeID = 5,
                ApplicationStatus = "Completed",
                ApplicationLastStatusDate = DateTime.Now,
                ApplicationPaidFees = _releaseAppFees,
                CreatedByUserID = AppConfig.LoggedInUser.UserID
            });

            if (!appResult.newApplicationID.HasValue || appResult.status != enApplicationSaveStatus.Success)
            {
                MessageBox.Show("Failed to create the release application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int releaseAppID = appResult.newApplicationID.Value;

            _detainedLicense.IsReleased = true;
            _detainedLicense.ReleaseDate = DateTime.Now;
            _detainedLicense.ReleasedByUserID = AppConfig.LoggedInUser.UserID;
            _detainedLicense.ReleaseApplicationID = releaseAppID;

            if (_detainedLicenseService.Update(_detainedLicense))
            {
                lblReleaseAppID.Text = releaseAppID.ToString();
                MessageBox.Show($"License released successfully with Release Application ID = {releaseAppID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRelease.Enabled = false;
                btnSearch.Enabled = false;
                txtLicenseID.Enabled = false;

                driverLicenseInfoControl1.LicenseID = _license.LicenseID;
            }
            else
            {
                MessageBox.Show("Failed to release the detained license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _applicationService.Delete(releaseAppID);
            }
        }

        private void ResetForm()
        {
            _license = null;
            _detainedLicense = null;
            driverLicenseInfoControl1.LicenseID = -1;
            lblLicenseID.Text = "[???]";
            lblDetainID.Text = "[???]";
            lblFineFees.Text = "[???]";
            lblTotalFees.Text = "[???]";
            lblReleaseAppID.Text = "[???]";
            btnRelease.Enabled = false;
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
