using System;

namespace DVLD.DAL.Entities
{
    public class TestAppointment
    {
        public int TestAppointmentID { get; set; }
        public DateTime TestAppointmentDate { get; set; }
        public double TestAppointmentPaidFees { get; set; }
        public bool IsLocked { get; set; }
        public int LocalLicenseApplicationID { get; set; }
        public int TestTypeID { get; set; }
        public int CreatedByUserID { get; set; }
        public int? RetakeTestApplicationID { get; set; }
    }
}
