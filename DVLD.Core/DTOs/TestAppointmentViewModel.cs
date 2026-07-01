using System;

namespace DVLD.Core.DTOs
{
    public class TestAppointmentViewModel
    {
        public int LocalLicenseApplicationID { get; set; }
        public double TestAppointmentPaidFees { get; set; }
        public string LicenseClassTitle { get; set; }
        public string FullName { get; set; }
        public DateTime TestAppointmentDate { get; set; }
    }
}
