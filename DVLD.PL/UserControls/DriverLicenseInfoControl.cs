using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD.PL.UserControls
{
    public partial class DriverLicenseInfoControl : UserControl
    {
        private readonly LicenseService _licenseService;
        private int _licenseId = -1;

        public int LicenseID
        {
            get { return _licenseId; }
            set
            {
                _licenseId = value;
                if (_licenseId > 0)
                {
                    LoadLicenseData();
                }
                else
                {
                    ResetLicenseData();
                }
            }
        }

        public DriverLicenseInfoControl()
        {
            InitializeComponent();
            _licenseService = new LicenseService(AppConfig.ConnectionString);
        }

        public void LoadLicenseData()
        {
            LicenseInfoViewModel licenseInfo = _licenseService.GetLicenseInfoByID(_licenseId);

            if (licenseInfo == null)
            {
                MessageBox.Show($"Could not find license with ID = {_licenseId}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetLicenseData();
                return;
            }

            lblLicenseID.Text = licenseInfo.LicenseID.ToString();
            lblClass.Text = licenseInfo.ClassName;
            lblName.Text = licenseInfo.FullName;
            lblNationalNo.Text = licenseInfo.NationalNo;
            lblGendor.Text = licenseInfo.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = licenseInfo.LicenseIssueDate.ToString("dd/MMM/yyyy");
            lblExpirationDate.Text = licenseInfo.LicenseExpirationDate.ToString("dd/MMM/yyyy");
            lblDateOfBirth.Text = licenseInfo.DateOfBirth.ToString("dd/MMM/yyyy");
            lblDriverID.Text = licenseInfo.DriverID.ToString();
            lblIsActive.Text = licenseInfo.IsActive ? "Yes" : "No";
            lblIsDetained.Text = licenseInfo.IsDetained ? "Yes" : "No";
            lblNotes.Text = string.IsNullOrWhiteSpace(licenseInfo.Notes) ? "No Notes" : licenseInfo.Notes;

            string issueReason = "First Time";
            if (licenseInfo.LicenseIssueReason == 2) issueReason = "Renew";
            else if (licenseInfo.LicenseIssueReason == 3) issueReason = "Replacement for Damaged";
            else if (licenseInfo.LicenseIssueReason == 4) issueReason = "Replacement for Lost";
            lblIssueReason.Text = issueReason;

            pbDriverImage.Image = Properties.Resources.DefaultPhoto;

            if (!string.IsNullOrWhiteSpace(licenseInfo.ImagePath) && File.Exists(licenseInfo.ImagePath))
            {
                try
                {
                    pbDriverImage.Image = Image.FromFile(licenseInfo.ImagePath);
                }
                catch
                {
                }
            }
        }

        public void ResetLicenseData()
        {
            _licenseId = -1;
            lblLicenseID.Text = "N/A";
            lblClass.Text = "N/A";
            lblName.Text = "N/A";
            lblNationalNo.Text = "N/A";
            lblGendor.Text = "N/A";
            lblIssueDate.Text = "N/A";
            lblExpirationDate.Text = "N/A";
            lblDateOfBirth.Text = "N/A";
            lblDriverID.Text = "N/A";
            lblIsActive.Text = "N/A";
            lblIsDetained.Text = "N/A";
            lblNotes.Text = "N/A";
            lblIssueReason.Text = "N/A";
            pbDriverImage.Image = Properties.Resources.DefaultPhoto;
        }
    }
}
