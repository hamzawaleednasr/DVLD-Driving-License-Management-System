using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;

namespace DVLD.DAL.Interfaces
{
    public interface ILocalLicenseApplication : IRepository<LocalLicenseApplication>
    {
        List<LocalLicenseApplicationViewModel> GetAllToView();
        LocalLicenseApplicationViewModel GetByIDToView(int id);
        List<LocalLicenseApplicationViewModel> GetByNationalNumberToView(string nationalNumber);
        List<LocalLicenseApplicationViewModel> GetByFullNameToView(string fullName);
        List<LocalLicenseApplicationViewModel> GetByStatusToView(string status);
        int GetNumberOfLocalLicenseApplication();
    }
}

