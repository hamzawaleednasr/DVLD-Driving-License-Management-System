using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.DriverForms
{
    public partial class ManageDriversForm : Form
    {
        private readonly DriverService _driverService;

        public ManageDriversForm()
        {
            InitializeComponent();
            _driverService = new DriverService(AppConfig.ConnectionString);
        }

        private void ManageDriversForm_Load(object sender, EventArgs e)
        {
            dtgDrivers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgDrivers.ReadOnly = true;
            cmbFilters.SelectedIndex = 0;
            GetAllDrivers();
            SetDriversCount();
        }

        private void LoadDrivers(List<DriverViewModel> drivers)
        {
            dtgDrivers.DataSource = drivers == null ? null : new BindingList<DriverViewModel>(drivers);
        }

        private void GetAllDrivers()
        {
            LoadDrivers(_driverService.GetAllToView());
        }

        private void SetDriversCount()
        {
            lblDriversNumber.Text = _driverService.GetNumberOfDrivers().ToString();
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltering.Clear();

            if (cmbFilters.SelectedIndex == 0) // None
            {
                txtFiltering.Visible = false;
                GetAllDrivers();
            }
            else
            {
                txtFiltering.Visible = true;
                txtFiltering.Focus();
            }
        }

        private void txtFiltering_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFiltering.Text))
            {
                GetAllDrivers();
                return;
            }

            switch (cmbFilters.SelectedIndex)
            {
                case 1: // Driver ID
                    if (int.TryParse(txtFiltering.Text.Trim(), out int driverId))
                    {
                        var driver = _driverService.GetByID(driverId); 
                        if (driver != null)
                        {
                            var view = _driverService.GetByPersonIDToView(driver.PersonID);
                            LoadDrivers(view == null ? null : new List<DriverViewModel> { view });
                        }
                        else
                        {
                            dtgDrivers.DataSource = null;
                        }
                    }
                    else
                    {
                        dtgDrivers.DataSource = null;
                    }
                    break;

                case 2: // Person ID
                    if (int.TryParse(txtFiltering.Text.Trim(), out int personId))
                    {
                        var view = _driverService.GetByPersonIDToView(personId);
                        LoadDrivers(view == null ? null : new List<DriverViewModel> { view });
                    }
                    else
                    {
                        dtgDrivers.DataSource = null;
                    }
                    break;

                case 3: // National No
                    var driverByNationalNo = _driverService.GetByNationalNumberToView(txtFiltering.Text.Trim());
                    LoadDrivers(driverByNationalNo == null ? null : new List<DriverViewModel> { driverByNationalNo });
                    break;

                case 4: // Full Name
                    var driversByName = _driverService.GetByFullNameToView(txtFiltering.Text.Trim());
                    LoadDrivers(driversByName);
                    break;

                default:
                    GetAllDrivers();
                    break;
            }
        }

        private void txtFiltering_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilters.SelectedIndex == 1 || cmbFilters.SelectedIndex == 2)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
