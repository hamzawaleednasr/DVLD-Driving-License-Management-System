using System;

namespace DVLD.Core.DTOs
{
    public class InternationalLicenseHistory
    {
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int LocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
