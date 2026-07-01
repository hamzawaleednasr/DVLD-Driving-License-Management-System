using DVLD.DAL.Entities;
using System.Collections.Generic;

namespace DVLD.DAL.Interfaces
{
    public interface ILicenseClass
    {
        List<LicenseClass> GetAll();
        LicenseClass GetByID(int id);
        bool Update(LicenseClass licenseClass);
    }
}
