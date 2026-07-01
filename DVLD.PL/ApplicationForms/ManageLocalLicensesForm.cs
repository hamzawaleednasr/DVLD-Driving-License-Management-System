using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.PL.LicenseForms;
using DVLD.PL.TestForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.ApplicationForms
{
    public partial class ManageLocalLicensesForm : Form
    {
        private readonly LocalLicenseApplicationService _localLicenseApplicationService;
        private readonly ApplicationService _applicationService;
        private readonly LicenseService _licenseService;
        private readonly DriverService _driverService;
        private readonly LicenseClassService _licenseClassService;
        private int _selectedLocalDrivingLicneseApplicationID = -1;

        public ManageLocalLicensesForm()
        {
            InitializeComponent();

            _localLicenseApplicationService = new LocalLicenseApplicationService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _licenseService = new LicenseService(AppConfig.ConnectionString);
            _driverService = new DriverService(AppConfig.ConnectionString);
            _licenseClassService = new LicenseClassService(AppConfig.ConnectionString);
        }

        private void ManageLocalLicensesForm_Load(object sender, EventArgs e)
        {
            cmbFilters.SelectedIndex = 0;
            dtgLLDApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            GetAllLocalDrivingLicenseApplication();
            SetRecordsNumber();
        }

        private void GetAllLocalDrivingLicenseApplication()
        {
            List<LocalLicenseApplicationViewModel> localLicenseApplications = _localLicenseApplicationService.GetAllToView();

            dtgLLDApplications.DataSource = new BindingList<LocalLicenseApplicationViewModel>(localLicenseApplications);
        }

        private void GetAllByLDLApplicationID(int id)
        {
            LocalLicenseApplicationViewModel localLicenseApplication = _localLicenseApplicationService.GetByIDToView(id);
            dtgLLDApplications.DataSource = localLicenseApplication == null ? null : new List<LocalLicenseApplicationViewModel> { localLicenseApplication };
        }

        private void GetAllByNationalNumber(string nationalNumber)
        {
            List<LocalLicenseApplicationViewModel> localLicenseApplication = _localLicenseApplicationService.GetByNationalNumberToView(nationalNumber);
            dtgLLDApplications.DataSource = new BindingList<LocalLicenseApplicationViewModel>(localLicenseApplication);
        }

        private void GetAllByFullName(string fullName)
        {
            List<LocalLicenseApplicationViewModel> localLicenseApplications = _localLicenseApplicationService.GetByFullNameToView(fullName);
            dtgLLDApplications.DataSource = localLicenseApplications == null ? null : new BindingList<LocalLicenseApplicationViewModel>(localLicenseApplications);
        }

        private void GetAllByStatus(string status)
        {
            List<LocalLicenseApplicationViewModel> localLicenseApplications = _localLicenseApplicationService.GetByStatusToView(status);
            dtgLLDApplications.DataSource = localLicenseApplications == null ? null : new BindingList<LocalLicenseApplicationViewModel>(localLicenseApplications);
        }

        private void SetRecordsNumber()
        {
            int recordsNumber = _localLicenseApplicationService.GetNumberOfLocalLicenseApplication();
            lblRecordsNumber.Text = recordsNumber.ToString();
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltering.Clear();
            cmbFiltering.SelectedIndex = 0;

            switch (cmbFilters.SelectedIndex)
            {
                case 0:
                    txtFiltering.Visible = false;
                    cmbFiltering.Visible = false;
                    GetAllLocalDrivingLicenseApplication();
                    break;
                case 1:
                case 2:
                case 3:
                    txtFiltering.Visible = true;
                    cmbFiltering.Visible = false;
                    txtFiltering.Focus();
                    break;
                case 4:
                    txtFiltering.Visible = false;
                    cmbFiltering.Visible = true;
                    break;
            }
        }

        private void txtFiltering_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFiltering.Text))
            {
                GetAllLocalDrivingLicenseApplication();
                return;
            }

            switch (cmbFilters.SelectedIndex)
            {
                case 1:
                    if (int.TryParse(txtFiltering.Text, out int id))
                    {
                        GetAllByLDLApplicationID(id);
                    }
                    else
                    {
                        dtgLLDApplications.DataSource = null;
                    }
                    break;
                case 2:
                    GetAllByNationalNumber(txtFiltering.Text);
                    break;
                case 3:
                    GetAllByFullName(txtFiltering.Text);
                    break;
            }
        }

        private void cmbFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilters.SelectedIndex == 4)
            {
                string selectedStatus = cmbFiltering.SelectedItem.ToString();
                if (selectedStatus == "All")
                {
                    GetAllLocalDrivingLicenseApplication();
                }
                else
                {
                    GetAllByStatus(selectedStatus);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditLocalDrivingLicenseApplication frm = new AddEditLocalDrivingLicenseApplication();
            frm.ShowDialog();
            ManageLocalLicensesForm_Load(sender, e);
        }

        private void dtgLLDApplications_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right & e.RowIndex >= 0)
            {
                dtgLLDApplications.CurrentCell = dtgLLDApplications.Rows[e.RowIndex].Cells[e.ColumnIndex];
                _selectedLocalDrivingLicneseApplicationID = Convert.ToInt32(dtgLLDApplications.Rows[e.RowIndex].Cells["LocalLicenseApplicationID"].Value);
            }
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedLocalDrivingLicneseApplicationID <= 0) return;

            ShowApplicationDetailsForm frm = new ShowApplicationDetailsForm(_selectedLocalDrivingLicneseApplicationID);
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedLocalDrivingLicneseApplicationID <= 0) return;

            AddEditLocalDrivingLicenseApplication frm = new AddEditLocalDrivingLicenseApplication();
            frm.IsCreateMode = false;
            frm.LocalLicenseApplicationID = _selectedLocalDrivingLicneseApplicationID;
            frm.ShowDialog();
            GetAllLocalDrivingLicenseApplication();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedLocalDrivingLicneseApplicationID <= 0) return;

            if (MessageBox.Show("Are you sure you want to delete this application?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                LocalLicenseApplicationDto llaDto = _localLicenseApplicationService.GetByID(_selectedLocalDrivingLicneseApplicationID);
                if (llaDto != null)
                {
                    if (_localLicenseApplicationService.Delete(_selectedLocalDrivingLicneseApplicationID))
                    {
                        if (_applicationService.Delete(llaDto.ApplicationID))
                        {
                            MessageBox.Show("Application deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetAllLocalDrivingLicenseApplication();
                            SetRecordsNumber();
                        }
                        else
                        {
                            MessageBox.Show("Local driving license application deleted, but failed to delete the parent application.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            GetAllLocalDrivingLicenseApplication();
                            SetRecordsNumber();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete local license application. It might have test appointments or licenses linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedLocalDrivingLicneseApplicationID <= 0) return;

            if (MessageBox.Show("Are you sure you want to cancel this application?", "Confirm Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                LocalLicenseApplicationDto llaDto = _localLicenseApplicationService.GetByID(_selectedLocalDrivingLicneseApplicationID);
                if (llaDto != null)
                {
                    ApplicationDto appDto = _applicationService.GetByID(llaDto.ApplicationID);
                    if (appDto != null)
                    {
                        appDto.ApplicationStatus = "Canceled";
                        appDto.ApplicationLastStatusDate = DateTime.Now;

                        if (_applicationService.Update(appDto))
                        {
                            MessageBox.Show("Application canceled successfully.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetAllLocalDrivingLicenseApplication();
                        }
                        else
                        {
                            MessageBox.Show("Failed to cancel application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dtgLLDApplications.CurrentRow == null)
            {
                e.Cancel = true;
                return;
            }

            string status = dtgLLDApplications.CurrentRow.Cells["Status"].Value.ToString();
            int passedTests = Convert.ToInt32(dtgLLDApplications.CurrentRow.Cells["PassedTests"].Value);

            if (status == "New")
            {
                editApplicationToolStripMenuItem.Enabled = true;
                cancelApplicationToolStripMenuItem.Enabled = true;
                deleteApplicationToolStripMenuItem.Enabled = true;
                scheduleTestsToolStripMenuItem.Enabled = true;

                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = false;
            }
            else
            {
                editApplicationToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = (status == "Canceled");
                scheduleTestsToolStripMenuItem.Enabled = false;
            }

            switch (passedTests)
            {
                case 0:
                    visionToolStripMenuItem.Enabled = true;
                    writtenToolStripMenuItem.Enabled = false;
                    practicalTestToolStripMenuItem.Enabled = false;
                    showLicenseToolStripMenuItem.Enabled = false;
                    break;
                case 1:
                    visionToolStripMenuItem.Enabled = false;
                    writtenToolStripMenuItem.Enabled = true;
                    practicalTestToolStripMenuItem.Enabled = false;
                    showLicenseToolStripMenuItem.Enabled = false;
                    break;
                case 2:
                    visionToolStripMenuItem.Enabled = false;
                    writtenToolStripMenuItem.Enabled = false;
                    practicalTestToolStripMenuItem.Enabled = true;
                    showLicenseToolStripMenuItem.Enabled = false;
                    break;
                case 3:
                    visionToolStripMenuItem.Enabled = false;
                    writtenToolStripMenuItem.Enabled = false;
                    practicalTestToolStripMenuItem.Enabled = false;
                    scheduleTestsToolStripMenuItem.Enabled = false;
                    if (status == "Completed")
                    {
                        issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                        showLicenseToolStripMenuItem.Enabled = true;
                    }
                    else if (status == "New")
                    {
                        issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                        showLicenseToolStripMenuItem.Enabled = false;
                    }
                    break;

            }
        }

        private void visionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAppointmentsForm frm = new TestAppointmentsForm();
            frm.LocalLicenseApplicationID = _selectedLocalDrivingLicneseApplicationID;
            frm.TestTypeID = 1; // Vision
            frm.ShowDialog();
            ManageLocalLicensesForm_Load(sender, e);
        }

        private void writtenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAppointmentsForm frm = new TestAppointmentsForm();
            frm.LocalLicenseApplicationID = _selectedLocalDrivingLicneseApplicationID;
            frm.TestTypeID = 2; // Written
            frm.ShowDialog();
            ManageLocalLicensesForm_Load(sender, e);
        }

        private void practicalTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAppointmentsForm frm = new TestAppointmentsForm();
            frm.LocalLicenseApplicationID = _selectedLocalDrivingLicneseApplicationID;
            frm.TestTypeID = 3; // Practical
            frm.ShowDialog();
            ManageLocalLicensesForm_Load(sender, e);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var localLicenseApplication = _localLicenseApplicationService.GetByID(_selectedLocalDrivingLicneseApplicationID);
            var application = _applicationService.GetByID(localLicenseApplication.ApplicationID);
            var licenseClass = _licenseClassService.GetByID(localLicenseApplication.LicenseClassID);
            int driverId = -1;

            var driver = _driverService.GetByPersonID(application.PersonID);
            if (driver != null)
            {
                driverId = driver.DriverID;
            }
            else 
            { 
                var driverResult = _driverService.Add(
                    new DriverDto
                    {
                        PersonID = application.PersonID,
                        CreatedByUserID = AppConfig.LoggedInUser.UserID,
                    }
                );

                driverId = (int)driverResult.newDriverID;
            }

            var licenseResult = _licenseService.Add(
                new LicenseDto
                {
                    LicensePaidFees = licenseClass.LicenseClassFees,
                    LicenseIssueReason = 1,
                    ApplicationID = application.ApplicationID,
                    DriverID = driverId,
                    LicenseClassID = localLicenseApplication.LicenseClassID,
                }
            );

            if (licenseResult.isSuccess && licenseResult.newLicenseID.HasValue)
            {
                MessageBox.Show($"License Issued Successfully with ID = {licenseResult.newLicenseID.Value}",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = true;

                application.ApplicationStatus = "Completed";
                application.ApplicationLastStatusDate = DateTime.Now;
                _applicationService.Update(application);
                ManageLocalLicensesForm_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Failed to issue license. Please try again.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedLocalDrivingLicneseApplicationID <= 0) return;

            var localLicenseApplication = _localLicenseApplicationService.GetByID(_selectedLocalDrivingLicneseApplicationID);
            if (localLicenseApplication == null)
            {
                MessageBox.Show("Local license application details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int licenseID = _licenseService.GetActiveLicenseIDByApplicationID(localLicenseApplication.ApplicationID);

            if (licenseID != -1)
            {
                ShowLicenseForm frm = new ShowLicenseForm();
                frm.LicenseID = licenseID;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No active license was found for this application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showPersonLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = _applicationService.GetByID(_localLicenseApplicationService.GetByID(_selectedLocalDrivingLicneseApplicationID).ApplicationID).PersonID;

            ShowLicensesHistoryForm frm = new ShowLicensesHistoryForm();
            frm.PersonID = personID;
            frm.ShowDialog();
        }
    }
}
