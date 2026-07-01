using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Windows.Forms;

namespace DVLD.PL.UserControls
{
    public partial class ApplicationDetailsControl : UserControl
    {
        private readonly LocalLicenseApplicationService _localLicenseApplicationService;
        private readonly ApplicationService _applicationService;
        private readonly UserService _userService;
        private readonly ApplicationTypeService _applicationTypeService;

        private int _localLicenseApplicationID = -1;
        private int _personID = -1;

        public int LocalLicenseApplicationID
        {
            get { return _localLicenseApplicationID; }
            set
            {
                _localLicenseApplicationID = value;
                if (_localLicenseApplicationID > 0)
                {
                    LoadApplicationData(_localLicenseApplicationID);
                }
                else
                {
                    ResetApplicationData();
                }
            }
        }

        public ApplicationDetailsControl()
        {
            InitializeComponent();

            _localLicenseApplicationService = new LocalLicenseApplicationService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _userService = new UserService(AppConfig.ConnectionString);
            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
        }

        private void ResetApplicationData()
        {
            _personID = -1;
            lblLocalLicenseApplicationID.Text = "N/A";
            lblAppliedForLicense.Text = "N/A";
            lblPassedTests.Text = "N/A";

            lblApplicationID.Text = "N/A";
            lblStatus.Text = "N/A";
            lblFees.Text = "N/A";
            lblType.Text = "N/A";
            lblApplicant.Text = "N/A";
            lblDate.Text = "N/A";
            lblStatusDate.Text = "N/A";
            lblCreatedByUser.Text = "N/A";
        }

        private void LoadApplicationData(int id)
        {
            LocalLicenseApplicationViewModel viewDto = _localLicenseApplicationService.GetByIDToView(id);
            if (viewDto == null)
            {
                MessageBox.Show($"Could not find Local License Application with ID = {id}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetApplicationData();
                return;
            }

            LocalLicenseApplicationDto llaDto = _localLicenseApplicationService.GetByID(id);
            if (llaDto == null)
            {
                MessageBox.Show($"Could not load application details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetApplicationData();
                return;
            }

            ApplicationDto appDto = _applicationService.GetByID(llaDto.ApplicationID);
            if (appDto == null)
            {
                MessageBox.Show($"Could not load base application details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetApplicationData();
                return;
            }

            // Save Person ID
            _personID = appDto.PersonID;

            // Load LDL App Details
            lblLocalLicenseApplicationID.Text = viewDto.LocalLicenseApplicationID.ToString();
            lblAppliedForLicense.Text = viewDto.DrivingClass;
            lblPassedTests.Text = viewDto.PassedTests.ToString() + "/3";

            // Load Base App Details
            lblApplicationID.Text = appDto.ApplicationID.ToString();
            lblStatus.Text = appDto.ApplicationStatus;
            lblFees.Text = appDto.ApplicationPaidFees.ToString();

            ApplicationTypeDto appType = _applicationTypeService.GetByID(appDto.ApplicationTypeID);
            lblType.Text = (appType != null) ? appType.ApplicationTypeTitle : "N/A";

            lblApplicant.Text = viewDto.FullName;
            lblDate.Text = appDto.CreatedAt.ToShortDateString();
            lblStatusDate.Text = appDto.ApplicationLastStatusDate.ToShortDateString();

            UserDto user = _userService.GetByID(appDto.CreatedByUserID);
            lblCreatedByUser.Text = (user != null) ? user.Username : "N/A";
        }

        private void lnkShowPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_personID <= 0)
            {
                MessageBox.Show("No applicant associated with this application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PersonForms.ShowPersonDetailsForm frm = new PersonForms.ShowPersonDetailsForm();
            frm.SelectedPersonID = _personID;
            frm.ShowDialog();
        }
    }
}
