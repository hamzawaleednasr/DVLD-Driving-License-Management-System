using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using System.Collections.Generic;

namespace DVLD.DAL.Interfaces
{
    public interface IDriver : IRepository<Driver>
    {
        Driver GetByPersonID(int personID);
        List<DriverViewModel> GetAllToView();
        DriverViewModel GetByPersonIDToView(int personID);
        DriverViewModel GetByNationalNumberToView(string nationalNumber);
        List<DriverViewModel> GetByFullNameToView(string fullName);
        int GetNumberOfDrivers();
    }
}

