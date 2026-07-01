using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PL.ApplicationForms
{
    public partial class EditApplicationTypeForm : Form
    {
        private readonly ApplicationTypeService _applicationType;

        public int SelectedApplicationTypeID { get; set; }

        public EditApplicationTypeForm()
        {
            InitializeComponent();

            _applicationType = new ApplicationTypeService(AppConfig.ConnectionString);
        }

        private void EditApplicationTypeForm_Load(object sender, EventArgs e)
        {
            FillForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool result = _applicationType.Update(
                new ApplicationTypeDto
                {
                    ApplicationTypeID = SelectedApplicationTypeID,
                    ApplicationTypeTitle = txtTypeTitle.Text,
                    ApplicationTypeFees = Convert.ToInt32(txtTypeFees.Text),
                }
            );

            if (result)
            {
                MessageBox.Show("Application Type Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed To Updated Application Type, Try Again Later.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
        private void FillForm()
        {
            ApplicationTypeDto applicationType = _applicationType.GetByID(SelectedApplicationTypeID);

            if (applicationType == null) return;

            lblTypeID.Text = applicationType.ApplicationTypeID.ToString();
            txtTypeTitle.Text = applicationType.ApplicationTypeTitle.ToString();
            txtTypeFees.Text = applicationType.ApplicationTypeFees.ToString();
        }
    }
}
