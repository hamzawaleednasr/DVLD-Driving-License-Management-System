using System;

namespace DVLD.Core.DTOs
{
    public class LicenseClassDto
    {
        public int LicenseClassID { get; set; }
        public string LicenseClassTitle { get; set; } = string.Empty;
        public string LicenseClassDescription { get; set; }
        public double LicenseClassFees { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte ValidityLength { get; set; }
    }
}
