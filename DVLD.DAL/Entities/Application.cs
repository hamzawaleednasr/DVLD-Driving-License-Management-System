using System;

namespace DVLD.DAL.Entities
{
    public class Application
    {
        public int ApplicationID { get; set; }
        public string ApplicationStatus { get; set; } = string.Empty;
        public DateTime ApplicationLastStatusDate { get; set; }
        public double ApplicationPaidFees { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PersonID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
