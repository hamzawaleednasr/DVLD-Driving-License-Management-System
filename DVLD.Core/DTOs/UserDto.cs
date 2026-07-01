using System;

namespace DVLD.Core.DTOs
{
    public class UserDto
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public bool IsActive { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
