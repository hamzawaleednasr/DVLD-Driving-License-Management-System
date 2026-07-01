using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.Core.Enums;

namespace DVLD.DAL.Interfaces
{
    public interface IUser : IRepository<User>
    {
        User GetByUsername(string username);
        List<UserViewModel> GetAllToView();
        UserViewModel GetByIDToView(int userID);
        UserViewModel GetByPersonIDToView(int personID);
        UserViewModel GetByUsernameToView(string username);
        List<UserViewModel> GetByFullNameToView(string fullName);
        List<UserViewModel> GetByActivityToView(bool isActive);
        int GetNumberOfUsers();
        bool ChangePassword(int userID, string newPassword);
    }
}

