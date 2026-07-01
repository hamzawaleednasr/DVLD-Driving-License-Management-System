using System;
using System.Windows.Forms;

namespace DVLD.PL.LicenseForms
{
    public partial class ShowLicenseForm : Form
    {
        public int LicenseID { get; set; }

        public ShowLicenseForm()
        {
            InitializeComponent();
        }

        private void DriverLicenseInfo_Load(object sender, EventArgs e)
        {
            driverLicenseInfoControl1.LicenseID = LicenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
