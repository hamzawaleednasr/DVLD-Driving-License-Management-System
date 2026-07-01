using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PL.UserForms
{
    public partial class ShowUserDetailsForm : Form
    {
        public int SelectedUserID;

        public ShowUserDetailsForm()
        {
            InitializeComponent();
        }

        private void ShowUserDetailsForm_Load(object sender, EventArgs e)
        {
            userDetailsControl1.UserID = SelectedUserID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
