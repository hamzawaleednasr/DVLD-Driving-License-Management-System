using Microsoft.Win32;

namespace DVLD.BLL.Services
{
    public class RegisteryService
    {
        private const string _keyPath = @"HKEY_CURRENT_USER\Software\dvld";

        private const string _usernameValueName = "username";
        private const string _passwordValueName = "password";

        public static bool SetRememberedData(string username, string password)
        {
            try
            {
                Registry.SetValue(_keyPath, _usernameValueName, username);
                Registry.SetValue(_keyPath, _passwordValueName, password);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static (string username, string password) GetRememberedData()
        {
            try
            {
                string username = Registry.GetValue(_keyPath, _usernameValueName, null)?.ToString();
                string password = Registry.GetValue(_keyPath, _passwordValueName, null)?.ToString();

                return (username, password);
            }
            catch
            {
                return (null, null);
            }
        }
    }
}
