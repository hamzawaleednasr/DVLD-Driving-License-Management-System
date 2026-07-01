using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using System.Collections.Generic;

namespace DVLD.DAL.Interfaces
{
    public interface ILicense : IRepository<License>
    {
        LicenseInfoViewModel GetLicenseInfoByID(int licenseID);
        int GetActiveLicenseIDByApplicationID(int applicationID);
        List<LicenseHistoryViewModel> GetLicenseHistoryByDriverID(int driverID);
    }
}
