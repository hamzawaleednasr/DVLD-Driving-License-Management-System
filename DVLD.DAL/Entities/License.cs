using System;

namespace DVLD.DAL.Entities
{
    public class License
    {
        public int LicenseID { get; set; }
        public DateTime LicenseIssueDate { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public double LicensePaidFees { get; set; }
        public byte LicenseIssueReason { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public int DriverID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
    }
}
