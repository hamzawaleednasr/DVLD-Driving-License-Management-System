using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.PL.PersonForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DVLD.PL.ApplicationForms
{
    public partial class AddEditLocalDrivingLicenseApplication : Form
    {
        private readonly PersonService _personService;
        private readonly LocalLicenseApplicationService _localLicenseApplicationService;
        private readonly LicenseClassService _licenseClassService;
        private readonly ApplicationTypeService _applicationTypeService;
        private readonly ApplicationService _applicationService;
        private readonly UserService _userService;

        public bool IsCreateMode { get; set; } = true;
        public int LocalLicenseApplicationID { get; set; } = -1;

        public AddEditLocalDrivingLicenseApplication()
        {
            InitializeComponent();

            _personService = new PersonService(AppConfig.ConnectionString);
            _localLicenseApplicationService = new LocalLicenseApplicationService(AppConfig.ConnectionString);
            _licenseClassService = new LicenseClassService(AppConfig.ConnectionString);
            _applicationTypeService = new ApplicationTypeService(AppConfig.ConnectionString);
            _applicationService = new ApplicationService(AppConfig.ConnectionString);
            _userService = new UserService(AppConfig.ConnectionString);
        }

        private void AddLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            cmbFilters.SelectedIndex = 0;
            personDetailsControl1.lnklblEditPerson.Enabled = false;
            FillApplicationInfo();

            if (!IsCreateMode)
            {
                LoadLocalLicenseApplicationData();
            }
        }

        private void LoadLocalLicenseApplicationData()
        {
            LocalLicenseApplicationDto llaDto = _localLicenseApplicationService.GetByID(LocalLicenseApplicationID);
            if (llaDto == null)
            {
                MessageBox.Show($"Could not find Local License Application with ID = {LocalLicenseApplicationID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ApplicationDto appDto = _applicationService.GetByID(llaDto.ApplicationID);
            if (appDto == null)
            {
                MessageBox.Show("Could not find parent application data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            label1.Text = "Update Local Driving License Application";
            this.Text = "Update Local Driving License Application";

            personDetailsControl1.PersonID = appDto.PersonID;
            personDetailsControl1.lnklblEditPerson.Enabled = true;
            groupBox1.Enabled = false;

            lblDLApplicationID.Text = LocalLicenseApplicationID.ToString();
            lblApplicationDate.Text = appDto.CreatedAt.ToShortDateString();
            cmbLicenseClass.SelectedValue = llaDto.LicenseClassID;
            lblApplicationFees.Text = appDto.ApplicationPaidFees.ToString();

            UserDto user = _userService.GetByID(appDto.CreatedByUserID);
            lblCreatedBy.Text = (user != null) ? user.Username : "N/A";
        }

        private void FillApplicationInfo()
        {
            DateTime applicationDate = DateTime.Today;

            lblApplicationDate.Text = applicationDate.ToShortDateString();
            FillLicenseClassComboBox();
            lblApplicationFees.Text = _applicationTypeService.GetByID(2).ApplicationTypeFees.ToString();
            lblCreatedBy.Text = AppConfig.LoggedInUser.Username.ToString();
        }

        private void FillLicenseClassComboBox()
        {
            List<LicenseClassDto> licenseClasses = _licenseClassService.GetAll();

            cmbLicenseClass.DataSource = licenseClasses;
            cmbLicenseClass.DisplayMember = "LicenseClassTitle";
            cmbLicenseClass.ValueMember = "LicenseClassID";
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterString.Text))
            {
                MessageBox.Show("Filter string cannot be an empty string!", "Cannot be empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (cmbFilters.SelectedIndex)
            {
                case 0: // National Number
                    PersonViewModel personByNational = _personService.GetByNationalNumberWithCountry(txtFilterString.Text.ToString());
                    if (personByNational == null)
                    {
                        MessageBox.Show($"Not person found with national number '{txtFilterString.Text}', try again", "Person not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    personDetailsControl1.PersonID = personByNational.PersonID;
                    personDetailsControl1.lnklblEditPerson.Enabled = true;
                    break;
                case 1: // Person ID
                    PersonViewModel personByID = _personService.GetByIDWithCountry(Convert.ToInt32(txtFilterString.Text));
                    if (personByID == null)
                    {
                        MessageBox.Show($"Not person found with id '{txtFilterString.Text}', try again", "Person not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    personDetailsControl1.PersonID = personByID.PersonID;
                    personDetailsControl1.lnklblEditPerson.Enabled = true;
                    break;
            }

        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            AddEditPersonForm frm = new AddEditPersonForm();
            frm.IsCreateMode = true;
            frm.ShowDialog();
            personDetailsControl1.PersonID = frm.PersonID;
            personDetailsControl1.lnklblEditPerson.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsCreateMode)
            {
                LocalLicenseApplicationDto llaDto = _localLicenseApplicationService.GetByID(LocalLicenseApplicationID);
                if (llaDto == null) return;

                int newLicenseClassID = Convert.ToInt32(cmbLicenseClass.SelectedValue);

                if (llaDto.LicenseClassID != newLicenseClassID)
                {
                    ApplicationDto appDto = _applicationService.GetByID(llaDto.ApplicationID);
                    if (appDto == null) return;

                    // Validation 1: Check active application for new class
                    List<LocalLicenseApplicationDto> allLlas = _localLicenseApplicationService.GetAll();
                    if (allLlas != null)
                    {
                        foreach (var lla in allLlas)
                        {
                            if (lla.LocalLicenseApplicationID != LocalLicenseApplicationID && lla.LicenseClassID == newLicenseClassID)
                            {
                                ApplicationDto appCheck = _applicationService.GetByID(lla.ApplicationID);
                                if (appCheck != null && appCheck.PersonID == appDto.PersonID)
                                {
                                    if (appCheck.ApplicationStatus == "New" || appCheck.ApplicationStatus == "In Progress")
                                    {
                                        MessageBox.Show("Choose another license class, this person already has an active application for the selected class!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    // Validation 2: Check active license for new class
                    LicenseService licenseService = new LicenseService(AppConfig.ConnectionString);
                    DriverService driverService = new DriverService(AppConfig.ConnectionString);
                    List<LicenseDto> allLicenses = licenseService.GetAll();
                    if (allLicenses != null)
                    {
                        foreach (var lic in allLicenses)
                        {
                            if (lic.LicenseClassID == newLicenseClassID && lic.IsActive)
                            {
                                DriverDto driver = driverService.GetByID(lic.DriverID);
                                if (driver != null && driver.PersonID == appDto.PersonID)
                                {
                                    MessageBox.Show("This person already has an active license of the same class!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }

                    // Validation 3: Age check
                    LicenseClassDto selectedClass = _licenseClassService.GetByID(newLicenseClassID);
                    PersonViewModel person = _personService.GetByIDWithCountry(appDto.PersonID);
                    int age = DateTime.Today.Year - person.BirthDate.Year;
                    if (person.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;

                    if (age < selectedClass.MinimumAllowedAge)
                    {
                        MessageBox.Show($"The selected person's age is less than the minimum allowed age for this class ({selectedClass.MinimumAllowedAge})!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                llaDto.LicenseClassID = newLicenseClassID;
                if (_localLicenseApplicationService.Update(llaDto))
                {
                    MessageBox.Show("Local Driving License Application Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update Local Driving License Application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            var applicationResult = _applicationService.Add(
                new ApplicationDto
                {
                    ApplicationPaidFees = Convert.ToInt32(lblApplicationFees.Text),
                    CreatedAt = Convert.ToDateTime(lblApplicationDate.Text),
                    PersonID = personDetailsControl1.PersonID,
                    ApplicationTypeID = 2,
                    CreatedByUserID = AppConfig.LoggedInUser.UserID,
                }
            );

            if (applicationResult.newApplicationID != null && applicationResult.status == enApplicationSaveStatus.Success)
            {
                var localLicenseResult = _localLicenseApplicationService.Add(
                    new LocalLicenseApplicationDto
                    {
                        ApplicationID = (int)applicationResult.newApplicationID,
                        LicenseClassID = Convert.ToInt32(cmbLicenseClass.SelectedValue),
                    }
                );

                if (localLicenseResult.newLocalLicenseApplicationID != null && localLicenseResult.status == enLocalLicenseApplicationSaveStatus.Success)
                {
                    lblDLApplicationID.Text = localLicenseResult.newLocalLicenseApplicationID.ToString();

                    MessageBox.Show($"Local Driving License Application Added Successfully with id: '{localLicenseResult.newLocalLicenseApplicationID}'", "Added Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    _applicationService.Delete((int)applicationResult.newApplicationID);

                    switch (localLicenseResult.status)
                    {
                        case enLocalLicenseApplicationSaveStatus.ApplicationDoesNotExist:
                            MessageBox.Show("The parent application does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enLocalLicenseApplicationSaveStatus.LicenseClassDoesNotExist:
                            MessageBox.Show("The selected license class does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enLocalLicenseApplicationSaveStatus.PersonAlreadyHasActiveApplication:
                            MessageBox.Show("Choose another license class, this person already has an active application for the selected class!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enLocalLicenseApplicationSaveStatus.PersonAlreadyHasLicenseOfSameClass:
                            MessageBox.Show("This person already has an active license of the same class!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enLocalLicenseApplicationSaveStatus.AgeLessThanMinimum:
                            MessageBox.Show("The selected person's age is less than the minimum allowed age for this class!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        case enLocalLicenseApplicationSaveStatus.RequiredDataMissing:
                            MessageBox.Show("Required local license application data is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            else
            {
                switch (applicationResult.status)
                {
                    case enApplicationSaveStatus.PersonDoesNotExist:
                        MessageBox.Show("The selected person does not exist in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enApplicationSaveStatus.ApplicationTypeDoesNotExist:
                        MessageBox.Show("The application type does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enApplicationSaveStatus.CreatedByUserDoesNotExist:
                        MessageBox.Show("The user who created this application does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case enApplicationSaveStatus.RequiredDataMissing:
                        MessageBox.Show("Required data is missing. Please fill all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
