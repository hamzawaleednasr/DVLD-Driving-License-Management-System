using System;
using System.Collections.Generic;

namespace DVLD.Core.DTOs
{
    public class DriverViewModel
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public string NationalNumber { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DriverLicenses { get; set; }
    }
}
