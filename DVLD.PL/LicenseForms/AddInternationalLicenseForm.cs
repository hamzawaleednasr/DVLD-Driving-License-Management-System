using System;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;

namespace DVLD.PL.LicenseForms
{
    public partial class AddInternationalLicenseForm : Form
    {
        private readonly InternationalLicenseService _internationalLicenseService;
        private readonly ApplicationTypeService _applicationTypeService;
        private readonly ApplicationService _applicationService;
        private readonly LicenseService _licenseService;

        private LicenseDto _license;
        private int _internationalLicenseID = -1;

        public AddInternationalLicenseForm()
        {
            InitializeComponent();

            _internationalLicenseService = new InternationalLicenseService(AppConfig.ConnectionString);
            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _licenseService = new LicenseService(AppConfig.ConnectionString);
        }

        private void AddInternationalLicenseForm_Load(object sender, EventArgs e)
        {
            btnIssue.Enabled = false;
            LoadApplicationData();
        }

        private void LoadApplicationData()
        {
            lblIssueDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblExpirationDate.Text = DateTime.Now.AddYears(10).ToString("dd/MMM/yyyy");
            lblApplicationDate.Text = lblIssueDate.Text;
            lblFees.Text = _applicationTypeService.GetByID(1).ApplicationTypeFees.ToString();
            lblCreatedBy.Text = AppConfig.LoggedInUser.Username;
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text))
            {
                MessageBox.Show("License ID must not be empty, try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _license = _licenseService.GetByID(Convert.ToInt32(txtLicenseID.Text));

            if (_license == null)
            {
                MessageBox.Show($"Couldn't to found license with id '{txtLicenseID.Text}', try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_license.LicenseClassID != 7)
            {
                MessageBox.Show("License must be from class 3, try with another license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_license.IsActive)
            {
                MessageBox.Show("License must be active, try another license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            driverLicenseInfoControl1.LicenseID = _license.LicenseID;
            btnIssue.Enabled = true;
            lnkPersonHistory.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (_license == null) return;

            var applicationResult = _applicationService.Add(
                new ApplicationDto
                {
                    ApplicationStatus = "Completed",
                    PersonID = _applicationService.GetByID(_license.ApplicationID).PersonID,
                    ApplicationTypeID = 1,
                    CreatedByUserID = AppConfig.LoggedInUser.UserID,
                }    
            );

            if (applicationResult.status == enApplicationSaveStatus.Success && applicationResult.newApplicationID.HasValue)
            {
                int newApplicationID = applicationResult.newApplicationID.Value;

                var licenseResult = _internationalLicenseService.Add(
                    new InternationalLicenseDto
                    {
                        IssueDate = Convert.ToDateTime(lblIssueDate.Text),
                        DriverID = _license.DriverID,
                        ApplicationID = newApplicationID,
                        IssueUsingLocalLicenseID = Convert.ToInt32(txtLicenseID.Text),
                        CreatedByUserID = AppConfig.LoggedInUser.UserID,
                    }
                );

                if (licenseResult.status == enInternationalLicenseSaveStatus.Success && licenseResult.newInternationalLicenseID.HasValue)
                {
                    int newInternationalLicenseID = licenseResult.newInternationalLicenseID.Value;
                    _internationalLicenseID = newInternationalLicenseID;

                    lblILApplicationID.Text = newApplicationID.ToString();
                    lblILLicenseID.Text = newInternationalLicenseID.ToString();
                    lblLocalLicenseID.Text = txtLicenseID.Text;
                    lnkShowLicense.Enabled = true;
                    MessageBox.Show($"International License Issued Successfully with ID = {newInternationalLicenseID}",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    switch (licenseResult.status)
                    {
                        case enInternationalLicenseSaveStatus.DriverAlreadyHasActiveInternationalLicense:
                            MessageBox.Show("Failed to issue license: This driver already has an active international license.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case enInternationalLicenseSaveStatus.LocalLicenseNotClass3:
                            MessageBox.Show("Failed to issue license: The local license must be Class 3 (Ordinary driving license) to issue an international license.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case enInternationalLicenseSaveStatus.LocalLicenseNotActive:
                            MessageBox.Show("Failed to issue license: The local license is not active.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case enInternationalLicenseSaveStatus.LocalLicenseExpired:
                            MessageBox.Show("Failed to issue license: The local license is expired.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case enInternationalLicenseSaveStatus.LocalLicenseDoesNotExist:
                            MessageBox.Show("Failed to issue license: The specified local license does not exist.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case enInternationalLicenseSaveStatus.DriverDoesNotExist:
                            MessageBox.Show("Failed to issue license: The specified driver does not exist.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case enInternationalLicenseSaveStatus.ApplicationDoesNotExist:
                            MessageBox.Show("Failed to issue license: The associated application does not exist.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        case enInternationalLicenseSaveStatus.RequiredDataMissing:
                            MessageBox.Show("Failed to issue license: Some required fields are missing.",
                                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        default:
                            MessageBox.Show("Failed to issue international license due to an unknown system error.",
                                            "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            else
            {
                switch (applicationResult.status)
                {
                    case enApplicationSaveStatus.RequiredDataMissing:
                        MessageBox.Show("Failed to create application: Some required fields are missing.",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case enApplicationSaveStatus.PersonDoesNotExist:
                        MessageBox.Show("Failed to create application: The specified person does not exist.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case enApplicationSaveStatus.ApplicationTypeDoesNotExist:
                        MessageBox.Show("Failed to create application: The specified application type is invalid.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case enApplicationSaveStatus.CreatedByUserDoesNotExist:
                        MessageBox.Show("Failed to create application: The system user who created this application does not exist.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    default:
                        MessageBox.Show("Failed to create application due to an unknown system error.",
                                        "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void lnkPersonHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicensesHistoryForm frm = new ShowLicensesHistoryForm();
            frm.PersonID = _applicationService.GetByID(_license.ApplicationID).PersonID;
            frm.ShowDialog();
        }

        private void lnkShowLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowInternationalLicenseForm frm = new ShowInternationalLicenseForm();
            frm.InterantionalLicenseID = _internationalLicenseID;
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
