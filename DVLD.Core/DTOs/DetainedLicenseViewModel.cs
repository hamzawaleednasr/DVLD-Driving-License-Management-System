
using System;

namespace DVLD.Core.DTOs
{
    public class DetainedLicenseViewModel
    {
        public int DetainedLicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public double FineFees { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool IsReleased { get; set; }
        public int LicenseID { get; set; }
        public string FullName { get; set; }
        public string NationalNumber { get; set; }
        public int? ReleaseApplicationID { get; set; }
    }
}
