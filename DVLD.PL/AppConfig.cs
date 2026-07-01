using DVLD.Core.DTOs;
using System.Configuration;

namespace DVLD.PL
{
    public static class AppConfig
    {
        public static string ConnectionString { get; private set; }
        public static string StoragePath;
        public static UserDto LoggedInUser { get; set; }

        public static void Initialize()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            StoragePath = @"D:\Hamza\Programming\.NET stack\back-end\C#\DVLD\people-photos\";
        }
    }
}
