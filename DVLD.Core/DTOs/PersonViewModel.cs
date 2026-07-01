using System;

namespace DVLD.Core.DTOs
{
    public class PersonViewModel
    {
        public int PersonID { get; set; }
        public bool Gender { get; set; }
        public string GenderText => Gender ? "Female" : "Male";
        public string Nationality { get; set; }
        public string NationalNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PersonalPhoto { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
