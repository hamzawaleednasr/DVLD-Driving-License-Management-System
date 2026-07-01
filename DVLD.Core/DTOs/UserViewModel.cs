using System;

namespace DVLD.Core.DTOs
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }
}
