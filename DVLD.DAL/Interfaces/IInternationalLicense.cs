using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;

namespace DVLD.DAL.Interfaces
{
    public interface IInternationalLicense : IRepository<InternationalLicense>
    {
        List<InternationalLicenseHistory> GetByLicenseIDToHistory(int id);
        InternationalLicenseInfo GetByIDToShowInfo(int id);
        int GetNumberOfLocalInternationalLicenses();
        List<InternationalLicense> GetByDriverID(int driverID);
    }
}

