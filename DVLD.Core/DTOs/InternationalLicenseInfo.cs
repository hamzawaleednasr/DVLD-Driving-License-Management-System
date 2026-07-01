using System;

namespace DVLD.Core.DTOs
{
    public class InternationalLicenseInfo
    {
        public string FullName { get; set; }
        public string NationalNumber { get; set; }
        public string PersonalPhoto { get; set; }
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int LocalLicenseID { get; set; }
        public int DriverID { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public bool Gender { get; set; }
    }
}
