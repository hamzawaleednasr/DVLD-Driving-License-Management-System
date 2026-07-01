using DVLD.BLL.Services;
using DVLD.Core.DTOs;
using System.Windows.Forms;

namespace DVLD.PL.UserControls
{
    public partial class UserDetailsControl : UserControl
    {
        private readonly UserService _userService;
        private int _userID;

        public int UserID
        {
            get { return _userID; }
            set
            {
                _userID = value;

                LoadUserData(_userID);
            }
        }

        public UserDetailsControl()
        {
            InitializeComponent();

            _userService = new UserService(AppConfig.ConnectionString);
        }

        private void LoadUserData(int id)
        {
            UserDto user = _userService.GetByID(id);

            if (user == null) return;

            personDetailsControl1.PersonID = user.PersonID;
            lblUserID.Text = user.UserID.ToString();
            lblUsername.Text = user.Username.ToString();
            lblCreatedAt.Text = user.CreatedAt.ToShortDateString();
            lblIsActive.Text = user.IsActive ? "Yes" : "No";
        }
    }
}
