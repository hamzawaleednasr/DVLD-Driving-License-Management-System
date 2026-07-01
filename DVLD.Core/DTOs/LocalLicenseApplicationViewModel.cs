using System;

namespace DVLD.Core.DTOs
{
    public class LocalLicenseApplicationViewModel
    {
        public int LocalLicenseApplicationID { get; set; }
        public string DrivingClass { get; set; }
        public string NationalNumber { get; set; }
        public string FullName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int PassedTests { get; set; }
        public string Status { get; set; }
    }
}
