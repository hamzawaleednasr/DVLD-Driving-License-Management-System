using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class UserService
    {
        private readonly IUser _userRepo;
        private readonly IPerson _personRepo;

        public UserService(string connectionString)
        {
            _userRepo = new UserRepository(connectionString);
            _personRepo = new PersonRepository(connectionString);
        }


        private string _HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); 

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool _VerifyPassword(string password, string savedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(savedHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }
                return true;
            }
        }

        public (int? newUserID, enUserSaveStatus status) Add(UserDto userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Username) || string.IsNullOrWhiteSpace(userDTO.Password))
            {
                return (null, enUserSaveStatus.RequiredDataMissing);
            }

            if (_personRepo.GetByID(userDTO.PersonID) == null)
            {
                return (null, enUserSaveStatus.PersonNotExists);
            }

            if (_userRepo.GetByPersonIDToView(userDTO.PersonID) != null)
            {
                return (null, enUserSaveStatus.PersonAlreadyUser);
            }

            if (_userRepo.GetByUsernameToView(userDTO.Username) != null)
            {
                return (null, enUserSaveStatus.UsernameAlreadyExists);
            }

            string hashedPassword = _HashPassword(userDTO.Password);

            userDTO.Password = hashedPassword;
            User user = MapToDTO.MapUserDtoToUser(userDTO);
            int newUserID = _userRepo.Add(user);
            return (newUserID, enUserSaveStatus.Success);
        }
    
        public (bool isSuccess, enUserSaveStatus status) Update(UserDto userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Username)) return (false, enUserSaveStatus.RequiredDataMissing);

            UserViewModel foundUser = _userRepo.GetByUsernameToView(userDTO.Username);
            if (foundUser != null && foundUser.UserID != userDTO.UserID)
            {
                return (false, enUserSaveStatus.UsernameAlreadyExists);
            }

            User user = MapToDTO.MapUserDtoToUser(userDTO);
            bool isSuccess = _userRepo.Update(user);
            return (isSuccess, enUserSaveStatus.Success);
        }
    
        public bool Delete(int id)
        {
            return _userRepo.Delete(id);
        }

        public List<UserDto> GetAll()
        {
            List<UserDto> userDTOs = new List<UserDto>();
            List<User> users = _userRepo.GetAll();
            if (users.Count > 0)
            {
                foreach (User user in users)
                {
                    userDTOs.Add(MapToDTO.MapUserToUserDto(user));
                }
            }
            return userDTOs;
        }

        public List<UserViewModel> GetAllToView()
        {
            return _userRepo.GetAllToView();
        }

        public UserDto GetByID(int id)
        {
            User user = _userRepo.GetByID(id);
            return MapToDTO.MapUserToUserDto(user);
        }

        public UserViewModel GetByIDToView(int id)
        {
            return _userRepo.GetByIDToView(id);
        }

        public UserViewModel GetByPersonIDToView(int personID)
        {
            return _userRepo.GetByPersonIDToView(personID);
        }

        public UserViewModel GetByUsernameToView(string username)
        {
            return _userRepo.GetByUsernameToView(username);
        }

        public List<UserViewModel> GetByFullNameToView(string fullName)
        {
            return _userRepo.GetByFullNameToView(fullName);
        }

        public List<UserViewModel> GetByActivityToView(bool isActive)
        {
            return _userRepo.GetByActivityToView(isActive);
        }

        public (bool isValid, enLoginStatus status, UserDto LoggedInUser) Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return (false, enLoginStatus.RequiredDataMissing, null);

            User foundUser = _userRepo.GetByUsername(username);
            if (foundUser == null)
            {
                return (false, enLoginStatus.UsernameNotExists, null);
            }

            if (!foundUser.IsActive)
            {
                return (false, enLoginStatus.InActive, null);
            }

            if (!_VerifyPassword(password, foundUser.Password))
            {
                return (false, enLoginStatus.PasswordWrong, null);
            }

            UserDto foundUserDTO = MapToDTO.MapUserToUserDto(foundUser);
            return (true, enLoginStatus.Success, foundUserDTO);
        }

        public bool ChangePassword(int userID, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword)) return false;

            newPassword = _HashPassword(newPassword);
            return _userRepo.ChangePassword(userID, newPassword);
        }

        public int GetNumberOfUsers()
        {
            return _userRepo.GetNumberOfUsers();
        }
    }
}
