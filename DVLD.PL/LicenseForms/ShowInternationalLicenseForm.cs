using System;
using System.Windows.Forms;

namespace DVLD.PL.LicenseForms
{
    public partial class ShowInternationalLicenseForm : Form
    {
        public int InterantionalLicenseID { get; set; }

        public ShowInternationalLicenseForm()
        {
            InitializeComponent();
        }

        private void ShowInternationalLicenseForm_Load(object sender, EventArgs e)
        {
            internationalLicenseInfoControl1.InternationalLicenseID = InterantionalLicenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
