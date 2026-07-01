using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DVLD.PL.LicenseForms
{
    public partial class ManageInternationalLicenseForm : Form
    {
        private readonly InternationalLicenseService _internationalLicenseService;
        private List<InternationalLicenseDto> _allInternationalLicenses;

        public ManageInternationalLicenseForm()
        {
            InitializeComponent();
            _internationalLicenseService = new InternationalLicenseService(AppConfig.ConnectionString);
        }

        private void ManageInternationalLicenseForm_Load(object sender, EventArgs e)
        {
            dtgInternationalLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgInternationalLicenses.ReadOnly = true;
            cmbFilters.SelectedIndex = 0;
            RefreshLicensesList();
        }

        private void RefreshLicensesList()
        {
            _allInternationalLicenses = _internationalLicenseService.GetAll();
            LoadLicensesList(_allInternationalLicenses);
            SetRecordsNumber();
        }

        private void LoadLicensesList(List<InternationalLicenseDto> list)
        {
            dtgInternationalLicenses.DataSource = list == null ? null : new BindingList<InternationalLicenseDto>(list);
        }

        private void SetRecordsNumber()
        {
            lblRecordsNumber.Text = _allInternationalLicenses == null ? "0" : _allInternationalLicenses.Count.ToString();
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltering.Clear();
            cmbFiltering.Items.Clear();

            switch (cmbFilters.SelectedIndex)
            {
                case 1: // International License ID
                case 2: // Driver ID
                case 3: // Local License ID
                    cmbFiltering.Visible = false;
                    txtFiltering.Visible = true;
                    txtFiltering.Focus();
                    break;

                case 4: // Is Active
                    cmbFiltering.Visible = true;
                    txtFiltering.Visible = false;
                    cmbFiltering.Items.Add("All");
                    cmbFiltering.Items.Add("Yes");
                    cmbFiltering.Items.Add("No");
                    cmbFiltering.SelectedIndex = 0;
                    break;

                case 0: // None
                default:
                    cmbFiltering.Visible = false;
                    txtFiltering.Visible = false;
                    LoadLicensesList(_allInternationalLicenses);
                    break;
            }
        }

        private void txtFiltering_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFiltering.Text))
            {
                LoadLicensesList(_allInternationalLicenses);
                return;
            }

            List<InternationalLicenseDto> filteredList = new List<InternationalLicenseDto>();

            switch (cmbFilters.SelectedIndex)
            {
                case 1: // International License ID
                    if (int.TryParse(txtFiltering.Text.Trim(), out int ilID))
                    {
                        filteredList = _allInternationalLicenses.Where(x => x.InternationalLicenseID == ilID).ToList();
                    }
                    break;

                case 2: // Driver ID
                    if (int.TryParse(txtFiltering.Text.Trim(), out int driverID))
                    {
                        filteredList = _allInternationalLicenses.Where(x => x.DriverID == driverID).ToList();
                    }
                    break;

                case 3: // Local License ID
                    if (int.TryParse(txtFiltering.Text.Trim(), out int localLicenseID))
                    {
                        filteredList = _allInternationalLicenses.Where(x => x.IssueUsingLocalLicenseID == localLicenseID).ToList();
                    }
                    break;
            }

            LoadLicensesList(filteredList);
        }

        private void cmbFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_allInternationalLicenses == null) return;

            string selectedValue = cmbFiltering.SelectedItem.ToString();
            List<InternationalLicenseDto> filteredList;

            if (selectedValue == "Yes")
            {
                filteredList = _allInternationalLicenses.Where(x => x.IsActive).ToList();
            }
            else if (selectedValue == "No")
            {
                filteredList = _allInternationalLicenses.Where(x => !x.IsActive).ToList();
            }
            else
            {
                filteredList = _allInternationalLicenses;
            }

            LoadLicensesList(filteredList);
        }

        private void txtFiltering_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits for ID inputs
            if (cmbFilters.SelectedIndex >= 1 && cmbFilters.SelectedIndex <= 3)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddInternationalLicenseForm frm = new AddInternationalLicenseForm();
            frm.ShowDialog();
            RefreshLicensesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
