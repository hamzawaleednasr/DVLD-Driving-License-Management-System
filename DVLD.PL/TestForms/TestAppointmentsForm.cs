using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.TestForms
{
    public partial class TestAppointmentsForm : Form
    {
        private readonly TestAppointmentService _testAppointmentService;

        public int LocalLicenseApplicationID { get; set; }
        public int SelectedTestAppointmentID { get; set; }
        public int TestTypeID { get; set; } = 1; // 1 = Vision, 2 = Written, 3 = Practical

        public TestAppointmentsForm()
        {
            InitializeComponent();

            _testAppointmentService = new TestAppointmentService(AppConfig.ConnectionString);
        }

        private void TestAppointmentsForm_Load(object sender, EventArgs e)
        {
            dtgVisionTestAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            applicationDetailsControl1.LocalLicenseApplicationID = LocalLicenseApplicationID;
            
            // Set dynamic titles
            string testName = "Vision";
            if (TestTypeID == 2) testName = "Written";
            else if (TestTypeID == 3) testName = "Practical";

            label1.Text = $"{testName} Test Appointments";
            this.Text = $"{testName} Test Appointments";

            GetAllAppointments();
            SetRecordsNumber();
        }

        private void GetAllAppointments()
        {
            List<TestAppointmentDto> testAppointments = _testAppointmentService.GetBy_TestTypeID_LocalLicenseApplicationID(TestTypeID, LocalLicenseApplicationID);

            dtgVisionTestAppointments.DataSource = new BindingList<TestAppointmentDto>(testAppointments);

            dtgVisionTestAppointments.Columns["TestTypeID"].Visible = false;
            dtgVisionTestAppointments.Columns["CreatedByUserID"].Visible = false;
            dtgVisionTestAppointments.Columns["LocalLicenseApplicationID"].Visible = false;
            dtgVisionTestAppointments.Columns["RetakeTestApplicationID"].Visible = false;
        }

        private void SetRecordsNumber()
        {
            lblRecordNumbers.Text = dtgVisionTestAppointments.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditTestAppointmentForm frm = new AddEditTestAppointmentForm();
            frm.LocalLicenseApplicationID = LocalLicenseApplicationID;
            frm.TestTypeID = TestTypeID;
            frm.IsCreateMode = true;
            frm.ShowDialog();
            GetAllAppointments();
            SetRecordsNumber();
        }

        private void dtgVisionTestAppointments_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dtgVisionTestAppointments.CurrentCell = dtgVisionTestAppointments.Rows[e.RowIndex].Cells[e.ColumnIndex];
                SelectedTestAppointmentID = Convert.ToInt32(dtgVisionTestAppointments.Rows[e.RowIndex].Cells["TestAppointmentID"].Value);
            }
        }

        private void editAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditTestAppointmentForm frm = new AddEditTestAppointmentForm();
            frm.LocalLicenseApplicationID = LocalLicenseApplicationID;
            frm.TestTypeID = TestTypeID;
            frm.TestAppointmentID = SelectedTestAppointmentID;
            frm.IsCreateMode = false;
            frm.ShowDialog();
            GetAllAppointments();
            SetRecordsNumber();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dtgVisionTestAppointments.CurrentRow != null)
            {
                bool isLocked = Convert.ToBoolean(dtgVisionTestAppointments.CurrentRow.Cells["IsLocked"].Value);
                if (isLocked)
                {
                    MessageBox.Show("This test appointment has already been locked (taken). You cannot take it again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            TakeTestForm frm = new TakeTestForm();
            frm.TestTypeID = TestTypeID;
            frm.TestAppointmentID = SelectedTestAppointmentID;
            frm.ShowDialog();
            GetAllAppointments();
            SetRecordsNumber();
        }
    }
}
