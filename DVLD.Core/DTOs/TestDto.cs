namespace DVLD.Core.DTOs
{
    public class TestDto
    {
        public int TestID { get; set; }
        public bool TestResult { get; set; }
        public string TestNotes { get; set; }
        public int CreatedByUserID { get; set; }
        public int TestAppointmentID { get; set; }
    }
}
