using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using DVLD.PL.PersonForms;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD.PL.UserControls
{
    public partial class PersonDetailsControl : UserControl
    {
        private readonly PersonService _personService;
        private int _personId;

        public int PersonID
        {
            get { return _personId; }
            set
            {
                _personId = value;
                if (_personId > 0)
                {
                    LoadPersonData(_personId);
                }
            }
        }

        public PersonDetailsControl()
        {
            InitializeComponent();

            _personService = new PersonService(AppConfig.ConnectionString);
        }

        private void PersonDetails_Load(object sender, EventArgs e)
        {
        }

        public void LoadPersonData(int id)
        {
            PersonViewModel person = _personService.GetByIDWithCountry(_personId);

            if (person == null) return;

            lblPersonID.Text = Convert.ToString(person.PersonID);
            lblFullName.Text = Convert.ToString(person.FirstName + ' ' + (string.IsNullOrEmpty(person.SecondName) ? null : person.SecondName + ' ') + (string.IsNullOrEmpty(person.ThirdName) ? null : person.ThirdName + ' ') + person.LastName);
            lblNationalNumber.Text = Convert.ToString(person.NationalNumber);
            lblGender.Text = Convert.ToString(person.Gender ? "Female" : "Male");
            lblEmail.Text = Convert.ToString(string.IsNullOrEmpty(person.Email) ? "Has No Email." : person.Email);
            lblAddress.Text = Convert.ToString(string.IsNullOrEmpty(person.Address) ? "Has No Address." : person.Address);
            lblBirthDate.Text = person.BirthDate.ToString("MM-dd-yy");
            lblCountryName.Text = Convert.ToString(person.Nationality);
            lblPhone.Text = Convert.ToString(string.IsNullOrEmpty(person.Phone) ? "Has No Phone." : person.Phone);
            if (!string.IsNullOrWhiteSpace(person.PersonalPhoto) && File.Exists(person.PersonalPhoto))
            {
                pbPersonPicture.Image = Image.FromFile(person.PersonalPhoto);
            }
        }

        private void lnklblEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddEditPersonForm frm = new AddEditPersonForm();
            frm.PersonID = this.PersonID;
            frm.IsCreateMode = false;
            frm.ShowDialog();
            LoadPersonData(PersonID);
        }
    }
}
