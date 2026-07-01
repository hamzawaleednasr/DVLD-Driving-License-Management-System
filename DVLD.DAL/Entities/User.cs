using System;

namespace DVLD.DAL.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public bool IsActive { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
