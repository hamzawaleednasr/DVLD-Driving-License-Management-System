using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DVLD.BLL.Services;
using DVLD.Core.DTOs;

namespace DVLD.PL.UserControls
{
    public partial class InternationalLicenseInfoControl : UserControl
    {
        private readonly InternationalLicenseService _internationalLicenseService;
        private int _internationalLicenseID;

        public int InternationalLicenseID 
        { 
            get { return _internationalLicenseID; }
            set
            {
                _internationalLicenseID = value;
                if (_internationalLicenseID > 0)
                {
                    LoadLicenseData();
                }
            }
        }

        public InternationalLicenseInfoControl()
        {
            InitializeComponent();

            _internationalLicenseService = new InternationalLicenseService(AppConfig.ConnectionString);
        }

        public void LoadLicenseData()
        {
             InternationalLicenseInfo iLicense = _internationalLicenseService.GetByIDToShowInfo(_internationalLicenseID);

            lblName.Text = iLicense.FullName;
            lblIntLicenseID.Text = iLicense.InternationalLicenseID.ToString();
            lblGender.Text = iLicense.Gender ? "Female" : "Male";
            lblIssueDate.Text = iLicense.IssueDate.ToString("dd/MMM/yyyy");
            lblLicenseID.Text = iLicense.LocalLicenseID.ToString();
            lblApplicationID.Text = iLicense.ApplicationID.ToString();
            lblIsActive.Text = iLicense.IsActive ? "Yes" : "No";
            lblBirthDate.Text = iLicense.BirthDate.ToString("dd/MMM/yyyy");
            lblDriverID.Text = iLicense.DriverID.ToString();
            lblExpirationDate.Text = iLicense.ExpirationDate.ToString("dd/MMM/yyyy");
            lblNationalNumber.Text = iLicense.NationalNumber;
            if (File.Exists(iLicense.PersonalPhoto)) pictureBox1.Image = Image.FromFile(iLicense.PersonalPhoto);
        }
    }
}
