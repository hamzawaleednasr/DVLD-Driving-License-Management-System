using System;
using System.Linq;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;

namespace DVLD.PL.LicenseForms
{
    public partial class DetainLicenseForm : Form
    {
        private readonly LicenseService _licenseService;
        private readonly DetainedLicenseService _detainedLicenseService;
        private LicenseDto _license;

        public DetainLicenseForm()
        {
            InitializeComponent();
            _licenseService = new LicenseService(AppConfig.ConnectionString);
            _detainedLicenseService = new DetainedLicenseService(AppConfig.ConnectionString);
        }

        private void DetainLicenseForm_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Today.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = AppConfig.LoggedInUser.Username;
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

            if (IsLicenseDetained(licenseID))
            {
                MessageBox.Show("This license is already detained!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
            }
            else
            {
                btnDetain.Enabled = true;
            }

        }

        private bool IsLicenseDetained(int licenseID)
        {
            var detainedList = _detainedLicenseService.GetAll();
            return detainedList != null && detainedList.Any(d => d.LicenseID == licenseID && !d.IsReleased);
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (_license == null) return;

            if (string.IsNullOrWhiteSpace(txtFineFees.Text))
            {
                MessageBox.Show("Please enter the fine fees amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtFineFees.Text.Trim(), out double fineFees) || fineFees < 0)
            {
                MessageBox.Show("Please enter a valid fine fee amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm Detain", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            var detainResult = _detainedLicenseService.Add(new DetainedLicenseDto
            {
                LicenseID = _license.LicenseID,
                DetainDate = DateTime.Now,
                FineFees = fineFees,
                CreatedByUserID = AppConfig.LoggedInUser.UserID,
                IsReleased = false
            });

            if (detainResult.status == enDetainedLicenseSaveStatus.Success && detainResult.newDetainedLicenseID.HasValue)
            {
                int newDetainID = detainResult.newDetainedLicenseID.Value;
                lblDetainID.Text = newDetainID.ToString();
                MessageBox.Show($"License detained successfully with Detain ID = {newDetainID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                btnDetain.Enabled = false;
                btnSearch.Enabled = false;
                txtLicenseID.Enabled = false;
                txtFineFees.Enabled = false;

                driverLicenseInfoControl1.LicenseID = _license.LicenseID;
            }
            else
            {
                MessageBox.Show("Failed to detain this license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            _license = null;
            driverLicenseInfoControl1.LicenseID = -1;
            lblLicenseID.Text = "[???]";
            lblDetainID.Text = "[???]";
            btnDetain.Enabled = false;
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && txtFineFees.Text.Contains("."))
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
