using System;
using System.Windows.Forms;

namespace DVLD.PL.PersonForms
{
    public partial class ShowPersonDetailsForm : Form
    {
        public int SelectedPersonID { get; set; }

        public ShowPersonDetailsForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowPersonDetails_Load(object sender, EventArgs e)
        {
            personDetailsControl1.PersonID = SelectedPersonID;
        }
    }
}
