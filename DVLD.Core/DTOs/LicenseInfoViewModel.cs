using System;

namespace DVLD.Core.DTOs
{
    public class LicenseInfoViewModel
    {
        public int LicenseID { get; set; }
        public string ClassName { get; set; }
        public string FullName { get; set; }
        public string NationalNo { get; set; }
        public byte Gender { get; set; } // 0 = Male, 1 = Female
        public DateTime DateOfBirth { get; set; }
        public int DriverID { get; set; }
        public DateTime LicenseIssueDate { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public byte LicenseIssueReason { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string ImagePath { get; set; }
        public bool IsDetained { get; set; }
    }
}
