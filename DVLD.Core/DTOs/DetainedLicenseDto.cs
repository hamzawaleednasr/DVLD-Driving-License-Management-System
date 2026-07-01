using System;

namespace DVLD.Core.DTOs
{
    public class DetainedLicenseDto
    {
        public int DetainedLicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public double FineFees { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool IsReleased { get; set; }
        public int LicenseID { get; set; }
        public int CreatedByUserID { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }
    }
}
