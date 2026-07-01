using DVLD.DAL.Entities;
using System.Collections.Generic;
using System.Data;

namespace DVLD.DAL.Interfaces
{
    public interface IApplicationType
    {
        bool Update(ApplicationType applicationType);
        List<ApplicationType> GetAll();
        ApplicationType GetByID(int id);
        int GetNumberOfApplicationTypes();
    }
}
