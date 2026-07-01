using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Windows.Forms;

namespace DVLD.PL.TestForms
{
    public partial class TakeTestForm : Form
    {
        private readonly TestAppointmentService _testAppointmentService;
        private readonly LocalLicenseApplicationService _localLicenseApplicationService;
        private readonly TestTypeService _testTypeService;
        private readonly TestService _testService;

        public int TestAppointmentID { get; set; } = -1;
        public int TestTypeID { get; set; } = 1; // 1 = Vision, 2 = Written, 3 = Practical

        public TakeTestForm()
        {
            InitializeComponent();

            _testAppointmentService = new TestAppointmentService(AppConfig.ConnectionString);
            _localLicenseApplicationService = new LocalLicenseApplicationService(AppConfig.ConnectionString);
            _testTypeService = new TestTypeService(AppConfig.ConnectionString);
            _testService = new TestService(AppConfig.ConnectionString);
        }

        private void TakeTestForm_Load(object sender, EventArgs e)
        {
            LoadAppointmentData();
        }

        private void LoadAppointmentData()
        {
            var appointment = _testAppointmentService.GetByID(TestAppointmentID);
            if (appointment == null)
            {
                MessageBox.Show($"Could not find test appointment with ID = {TestAppointmentID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            var llaView = _localLicenseApplicationService.GetByIDToView(appointment.LocalLicenseApplicationID);
            if (llaView == null)
            {
                MessageBox.Show("Could not find local driving license application details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblLocalDrivingLicenseAppID.Text = appointment.LocalLicenseApplicationID.ToString();
            lblDrivingClass.Text = llaView.DrivingClass;
            lblFullName.Text = llaView.FullName;
            lblDate.Text = appointment.TestAppointmentDate.ToShortDateString();
            lblFees.Text = appointment.TestAppointmentPaidFees.ToString();

            // Set Form Title and Title Label based on TestTypeID
            string testName = "Vision";
            if (TestTypeID == 2) testName = "Written";
            else if (TestTypeID == 3) testName = "Practical";

            label1.Text = $"Take {testName} Test";
            this.Text = $"Take {testName} Test";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save this test result? You cannot edit it later.", "Confirm Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            {
                return;
            }

            var testDto = new TestDto
            {
                TestAppointmentID = TestAppointmentID,
                TestResult = rbPass.Checked,
                TestNotes = txtNotes.Text.Trim(),
                CreatedByUserID = AppConfig.LoggedInUser.UserID
            };

            var result = _testService.Add(testDto);

            if (result.isSuccess && result.newTestID.HasValue)
            {
                lblTestID.Text = result.newTestID.Value.ToString();
                MessageBox.Show($"Test Result saved successfully with Test ID = {result.newTestID.Value}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to save test result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
