using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.ApplicationForms
{
    public partial class ManageApplicationTypesForm : Form
    {
        private readonly ApplicationTypeService _applicationTypeService;
        private int _selectedApplicationTypeID = -1;

        public ManageApplicationTypesForm()
        {
            InitializeComponent();

            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
        }

        private void ManageApplicationTypesForm_Load(object sender, EventArgs e)
        {
            GetAllApplicationTypes();
            SetRecordsNumber();
            dtgApplicationTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void GetAllApplicationTypes()
        {
            List<ApplicationTypeDto> applicationTypes = _applicationTypeService.GetAll();

            if (applicationTypes == null) return;

            dtgApplicationTypes.DataSource = new BindingList<ApplicationTypeDto>(applicationTypes);
        }
    
        private void SetRecordsNumber()
        {
            int recordsNumber = _applicationTypeService.GetNumberOfApplicationTypes();
            lblRecordsNumber.Text = recordsNumber.ToString();
        }

        private void dtgApplicationTypes_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dtgApplicationTypes.CurrentCell = dtgApplicationTypes.Rows[e.RowIndex].Cells[e.ColumnIndex];
                _selectedApplicationTypeID = Convert.ToInt32(dtgApplicationTypes.Rows[e.RowIndex].Cells["ApplicationTypeID"].Value);
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditApplicationTypeForm frm = new EditApplicationTypeForm();
            frm.SelectedApplicationTypeID = _selectedApplicationTypeID;
            frm.ShowDialog();
            ManageApplicationTypesForm_Load(sender, e);
        }
    }
}
