using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using System;
using System.Windows.Forms;

namespace DVLD.PL.TestForms
{
    public partial class AddEditTestAppointmentForm : Form
    {
        private readonly LocalLicenseApplicationService _localLicenseApplicationService;
        private readonly ApplicationService _applicationService;
        private readonly TestAppointmentService _testAppointmentService;
        private readonly TestTypeService _testTypeService;
        private readonly ApplicationTypeService _applicationTypeService;

        public int LocalLicenseApplicationID { get; set; } = -1;
        public int TestTypeID { get; set; } = 1; // 1 = Vision, 2 = Written, 3 = Practical
        public int TestAppointmentID { get; set; } = -1;
        public bool IsCreateMode { get; set; } = true;

        public AddEditTestAppointmentForm()
        {
            InitializeComponent();

            _localLicenseApplicationService = new LocalLicenseApplicationService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _testAppointmentService = new TestAppointmentService(AppConfig.ConnectionString);
            _testTypeService = new TestTypeService(AppConfig.ConnectionString);
            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
        }

        private void AddEditTestAppointmentForm_Load(object sender, EventArgs e)
        {
            dtpTestDate.MinDate = DateTime.Now;

            FillFormData();
            ConfigureFormTitle();

            if (IsCreateMode)
            {
                CheckIfRetakeTest();
            }
            else
            {
                LoadExistingAppointmentData();
            }
        }

        private void ConfigureFormTitle()
        {
            string testName = "Vision";
            if (TestTypeID == 2) testName = "Written";
            else if (TestTypeID == 3) testName = "Practical";

            string operation = IsCreateMode ? "Schedule" : "Reschedule";
            label1.Text = $"{operation} {testName} Test";
            this.Text = $"{operation} {testName} Test";
        }

        private void FillFormData()
        {
            var llaView = _localLicenseApplicationService.GetByIDToView(LocalLicenseApplicationID);

            if (llaView != null)
            {
                lblLDLApplicationID.Text = LocalLicenseApplicationID.ToString();
                lblDrivingClass.Text = llaView.DrivingClass;
                lblName.Text = llaView.FullName;
            }

            var testType = _testTypeService.GetByID(TestTypeID);
            if (testType != null)
            {
                lblFees.Text = testType.TestTypeFees.ToString();
            }
        }

        private void CheckIfRetakeTest()
        {
            var appointments = _testAppointmentService.GetBy_TestTypeID_LocalLicenseApplicationID(TestTypeID, LocalLicenseApplicationID);

            bool isRetake = appointments != null && appointments.Count > 0;

            if (isRetake)
            {
                gbRetakeTestApplication.Enabled = true;

                // ApplicationTypeID = 7 is typically Retake Test
                var retakeAppType = _applicationTypeService.GetByID(7);
                double retakeFees = (retakeAppType != null) ? retakeAppType.ApplicationTypeFees : 5;

                lblRetakeFees.Text = retakeFees.ToString();
                lblRetakeApplicationID.Text = "N/A";
                lblTotalFees.Text = (Convert.ToDouble(lblFees.Text) + retakeFees).ToString();
            }
            else
            {
                gbRetakeTestApplication.Enabled = false;
                lblRetakeFees.Text = "0";
                lblRetakeApplicationID.Text = "N/A";
                lblTotalFees.Text = lblFees.Text;
            }
        }

        private void LoadExistingAppointmentData()
        {
            var appointment = _testAppointmentService.GetByID(TestAppointmentID);
            if (appointment == null)
            {
                MessageBox.Show($"Could not find test appointment with ID = {TestAppointmentID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            dtpTestDate.Value = appointment.TestAppointmentDate;

            if (appointment.RetakeTestApplicationID.HasValue && appointment.RetakeTestApplicationID.Value > 0)
            {
                gbRetakeTestApplication.Enabled = true;
                lblRetakeApplicationID.Text = appointment.RetakeTestApplicationID.Value.ToString();

                var retakeAppType = _applicationTypeService.GetByID(7);
                double retakeFees = (retakeAppType != null) ? retakeAppType.ApplicationTypeFees : 5;
                lblRetakeFees.Text = retakeFees.ToString();
                lblTotalFees.Text = (Convert.ToDouble(lblFees.Text) + retakeFees).ToString();
            }
            else
            {
                gbRetakeTestApplication.Enabled = false;
                lblRetakeFees.Text = "0";
                lblRetakeApplicationID.Text = "N/A";
                lblTotalFees.Text = lblFees.Text;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsCreateMode)
            {
                int? retakeAppID = null;

                if (gbRetakeTestApplication.Enabled)
                {
                    var llaDto = _localLicenseApplicationService.GetByID(LocalLicenseApplicationID);
                    if (llaDto == null) return;

                    var parentApp = _applicationService.GetByID(llaDto.ApplicationID);
                    if (parentApp == null) return;

                    var appType = _applicationTypeService.GetByID(7); // 7 = Retake Test
                    double paidFees = (appType != null) ? appType.ApplicationTypeFees : 5;

                    var appResult = _applicationService.Add(new ApplicationDto
                    {
                        PersonID = parentApp.PersonID,
                        CreatedAt = DateTime.Now,
                        ApplicationTypeID = 7, // Retake Test
                        ApplicationStatus = "Completed",
                        ApplicationLastStatusDate = DateTime.Now,
                        ApplicationPaidFees = paidFees,
                        CreatedByUserID = AppConfig.LoggedInUser.UserID
                    });

                    if (appResult.newApplicationID.HasValue && appResult.status == enApplicationSaveStatus.Success)
                    {
                        retakeAppID = appResult.newApplicationID.Value;
                    }
                    else
                    {
                        MessageBox.Show("Failed to create Retake Test application. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                var appointmentDto = new TestAppointmentDto
                {
                    TestAppointmentDate = dtpTestDate.Value,
                    TestAppointmentPaidFees = Convert.ToDouble(lblFees.Text),
                    IsLocked = false,
                    LocalLicenseApplicationID = LocalLicenseApplicationID,
                    TestTypeID = TestTypeID,
                    CreatedByUserID = AppConfig.LoggedInUser.UserID,
                    RetakeTestApplicationID = retakeAppID
                };

                var result = _testAppointmentService.Add(appointmentDto);

                if (result.newAppointmentID.HasValue && result.status == enTestAppointmentSaveStatus.Success)
                {
                    MessageBox.Show($"Test Appointment Scheduled successfully with ID = {result.newAppointmentID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    if (retakeAppID.HasValue)
                    {
                        _applicationService.Delete(retakeAppID.Value);
                    }

                    switch (result.status)
                    {
                        case enTestAppointmentSaveStatus.PersonAlreadyHasActiveAppointmentForThisTest:
                            MessageBox.Show("This person already has an active appointment for this test. Please lock or take the test first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enTestAppointmentSaveStatus.PersonAlreadyPassedThisTest:
                            MessageBox.Show("This person has already passed this test!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enTestAppointmentSaveStatus.PreviousTestNotPassed:
                            MessageBox.Show("Must pass the previous test in the sequence first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        default:
                            MessageBox.Show("Failed to schedule test appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            else
            {
                var appointment = _testAppointmentService.GetByID(TestAppointmentID);
                if (appointment == null) return;

                appointment.TestAppointmentDate = dtpTestDate.Value;

                if (_testAppointmentService.Update(appointment))
                {
                    MessageBox.Show("Test Appointment Rescheduled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to reschedule test appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
