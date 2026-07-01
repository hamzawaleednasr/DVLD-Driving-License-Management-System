using DVLD.DAL.Entities;
using DVLD.Core.DTOs;
using System.Collections.Generic;
using System.Data;

namespace DVLD.DAL.Interfaces
{
    public interface IDetainedLicense : IRepository<DetainedLicense>
    {
        List<DetainedLicenseViewModel> GetAllToView();
        DetainedLicenseViewModel GetByIDToView(int id);
        DetainedLicenseViewModel GetByNationalNumberToView(string nationalNumber);
        List<DetainedLicenseViewModel> GetByFullNameToView(string fullName);
        List<DetainedLicenseViewModel> GetByReleaseStatusToView(bool isRelease);
        int GetNumberOfDetainedLicenses();
    }
}

