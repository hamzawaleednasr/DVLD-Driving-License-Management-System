using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.DTOs;

namespace DVLD.PL.LicenseForms
{
    public partial class ManageDetainedLicensesForm : Form
    {
        private readonly DetainedLicenseService _detainedLicenseService;
        private List<DetainedLicenseViewModel> _allDetainedLicenses;

        public ManageDetainedLicensesForm()
        {
            InitializeComponent();
            _detainedLicenseService = new DetainedLicenseService(AppConfig.ConnectionString);
        }

        private void ManageDetainedLicensesForm_Load(object sender, EventArgs e)
        {
            dtgDetainedLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgDetainedLicenses.ReadOnly = true;
            cmbFilters.SelectedIndex = 0;
            RefreshDetainedLicenses();
        }

        private void RefreshDetainedLicenses()
        {
            _allDetainedLicenses = _detainedLicenseService.GetAllToView();
            LoadDetainedLicenses(_allDetainedLicenses);
            SetRecordsCount();
        }

        private void LoadDetainedLicenses(List<DetainedLicenseViewModel> list)
        {
            dtgDetainedLicenses.DataSource = list == null ? null : new BindingList<DetainedLicenseViewModel>(list);
        }

        private void SetRecordsCount()
        {
            lblRecordsNumber.Text = _allDetainedLicenses == null ? "0" : _allDetainedLicenses.Count.ToString();
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltering.Clear();
            cmbFiltering.Items.Clear();

            switch (cmbFilters.SelectedIndex)
            {
                case 1: // Detain ID
                case 3: // National No
                case 4: // Full Name
                case 5: // Release App. ID
                    cmbFiltering.Visible = false;
                    txtFiltering.Visible = true;
                    txtFiltering.Focus();
                    break;

                case 2: // Is Released
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
                    LoadDetainedLicenses(_allDetainedLicenses);
                    break;
            }
        }

        private void txtFiltering_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFiltering.Text))
            {
                LoadDetainedLicenses(_allDetainedLicenses);
                return;
            }

            string searchText = txtFiltering.Text.Trim();
            List<DetainedLicenseViewModel> filteredList = new List<DetainedLicenseViewModel>();

            switch (cmbFilters.SelectedIndex)
            {
                case 1: // Detain ID
                    if (int.TryParse(searchText, out int detainID))
                    {
                        filteredList = _allDetainedLicenses.Where(x => x.DetainedLicenseID == detainID).ToList();
                    }
                    break;

                case 3: // National No
                    filteredList = _allDetainedLicenses.Where(x => x.NationalNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;

                case 4: // Full Name
                    filteredList = _allDetainedLicenses.Where(x => x.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;

                case 5: // Release App. ID
                    if (int.TryParse(searchText, out int releaseAppID))
                    {
                        filteredList = _allDetainedLicenses.Where(x => x.ReleaseApplicationID == releaseAppID).ToList();
                    }
                    break;
            }

            LoadDetainedLicenses(filteredList);
        }

        private void cmbFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_allDetainedLicenses == null) return;

            string selectedValue = cmbFiltering.SelectedItem.ToString();
            List<DetainedLicenseViewModel> filteredList;

            if (selectedValue == "Yes")
            {
                filteredList = _allDetainedLicenses.Where(x => x.IsReleased).ToList();
            }
            else if (selectedValue == "No")
            {
                filteredList = _allDetainedLicenses.Where(x => !x.IsReleased).ToList();
            }
            else
            {
                filteredList = _allDetainedLicenses;
            }

            LoadDetainedLicenses(filteredList);
        }

        private void txtFiltering_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits for ID filters
            if (cmbFilters.SelectedIndex == 1 || cmbFilters.SelectedIndex == 5)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            DetainLicenseForm frm = new DetainLicenseForm();
            frm.ShowDialog();
            RefreshDetainedLicenses();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicenseForm frm = new ReleaseDetainedLicenseForm();
            frm.ShowDialog();
            RefreshDetainedLicenses();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
