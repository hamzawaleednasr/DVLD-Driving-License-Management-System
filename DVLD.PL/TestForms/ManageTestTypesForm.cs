using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.TestForms
{
    public partial class ManageTestTypesForm : Form
    {
        private readonly TestTypeService _testTypeService;
        private int _selectedTestTypeID = -1;

        public ManageTestTypesForm()
        {
            InitializeComponent();

            _testTypeService = new TestTypeService(AppConfig.ConnectionString);
        }

        private void ManageTestTypesForm_Load(object sender, EventArgs e)
        {
            GetAllTestTypes();
            SetRecordsNumber();
            dtgTestTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void GetAllTestTypes()
        {
            List<TestTypeDto> testTypes = _testTypeService.GetAll();

            if (testTypes == null) return;

            dtgTestTypes.DataSource = new BindingList<TestTypeDto>(testTypes);
        }

        private void SetRecordsNumber()
        {
            int recordsNumber = _testTypeService.GetNumberOfTestTypes();
            lblRecordsNumber.Text = recordsNumber.ToString();
        }

        private void dtgTestTypes_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dtgTestTypes.CurrentCell = dtgTestTypes.Rows[e.RowIndex].Cells[e.ColumnIndex];
                _selectedTestTypeID = Convert.ToInt32(dtgTestTypes.Rows[e.RowIndex].Cells["TestTypeID"].Value);
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTestTypeForm frm = new UpdateTestTypeForm();
            frm.SelectedTestTypeID = _selectedTestTypeID;
            frm.ShowDialog();
            ManageTestTypesForm_Load(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
