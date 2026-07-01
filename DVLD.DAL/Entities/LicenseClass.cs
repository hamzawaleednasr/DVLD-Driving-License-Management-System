using System;

namespace DVLD.DAL.Entities
{
    public class LicenseClass
    {
        public int LicenseClassID { get; set; }
        public string LicenseClassTitle { get; set; } = string.Empty;
        public string LicenseClassDescription { get; set; }
        public double LicenseClassFees { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte ValidityLength { get; set; }
    }
}
