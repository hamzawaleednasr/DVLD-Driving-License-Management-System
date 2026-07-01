using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Windows.Forms;

namespace DVLD.PL.TestForms
{
    public partial class UpdateTestTypeForm : Form
    {
        private readonly TestTypeService _testTypeService;

        public int SelectedTestTypeID { get; set; }

        public UpdateTestTypeForm()
        {
            InitializeComponent();

            _testTypeService = new TestTypeService(AppConfig.ConnectionString);
        }

        private void UpdateTestTypeForm_Load(object sender, EventArgs e)
        {
            FillForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool result = _testTypeService.Update(
                new TestTypeDto
                {
                    TestTypeID = SelectedTestTypeID,
                    TestTypeTitle = txtTypeTitle.Text,
                    TestTypeDescription = txtTypeDescription.Text,
                    TestTypeFees = Convert.ToDouble(txtTypeFees.Text),
                }
            );

            if (result)
            {
                MessageBox.Show("Test Type Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed To Update Test Type, Try Again Later.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillForm()
        {
            TestTypeDto testType = _testTypeService.GetByID(SelectedTestTypeID);

            if (testType == null) return;

            lblTypeID.Text = testType.TestTypeID.ToString();
            txtTypeTitle.Text = testType.TestTypeTitle;
            txtTypeDescription.Text = testType.TestTypeDescription;
            txtTypeFees.Text = testType.TestTypeFees.ToString();
        }
    }
}
