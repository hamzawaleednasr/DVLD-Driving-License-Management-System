using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PL.LicenseForms
{
    public partial class ShowLicensesHistoryForm : Form
    {
        private readonly DriverService _driverService;
        private readonly LicenseService _licenseService;
        private readonly InternationalLicenseService _internationalLicenseService;

        private int _driverID = -1;

        public int PersonID { get; set; }

        public ShowLicensesHistoryForm()
        {
            InitializeComponent();

            _driverService = new DriverService(AppConfig.ConnectionString);
            _licenseService = new LicenseService(AppConfig.ConnectionString);
            _internationalLicenseService = new InternationalLicenseService(AppConfig.ConnectionString);
        }

        private void ShowLicensesHistoryForm_Load(object sender, EventArgs e)
        {
            personDetailsControl1.PersonID = PersonID;
            _driverID = _driverService.GetByPersonID(PersonID).DriverID;

            LoadLocalLicenses();
            LoadInternationalLicenses();
        }

        private void LoadLocalLicenses()
        {
            List<LicenseHistoryViewModel> licenses = _licenseService.GetLicenseHistoryByDriverID(_driverID);
            dtgLocal.DataSource = licenses == null ? null : new BindingList<LicenseHistoryViewModel>(licenses);
        }

        private void LoadInternationalLicenses()
        {
            List<InternationalLicenseDto> internationalLicenses = _internationalLicenseService.GetByDriverID(_driverID);
            dtgInternational.DataSource = internationalLicenses == null ? null : new BindingList<InternationalLicenseDto>(internationalLicenses);
        }
    }
}
