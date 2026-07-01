using System;
using System.Windows.Forms;

namespace DVLD.PL.ApplicationForms
{
    public partial class ShowApplicationDetailsForm : Form
    {
        public int LocalLicenseApplicationID { get; set; } = -1;

        public ShowApplicationDetailsForm()
        {
            InitializeComponent();
        }

        public ShowApplicationDetailsForm(int localLicenseApplicationID)
        {
            InitializeComponent();
            LocalLicenseApplicationID = localLicenseApplicationID;
        }

        private void ShowApplicationDetailsForm_Load(object sender, EventArgs e)
        {
            if (LocalLicenseApplicationID > 0)
            {
                applicationDetailsControl1.LocalLicenseApplicationID = LocalLicenseApplicationID;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
