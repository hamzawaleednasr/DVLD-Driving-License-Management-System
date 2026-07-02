using System;
using System.Windows.Forms;

namespace DVLD.PL
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppConfig.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm frm = new LoginForm();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new HomePageForm());
            }
        }
    }
}
