
using DVLD.DAL.Entities;
using System.Collections.Generic;
using System.Data;

namespace DVLD.DAL.Interfaces
{
    public interface ITestType
    {
        bool Update(TestType testType);
        List<TestType> GetAll();
        int GetNumberOfTestTypes();
        TestType GetByID(int id);
    }
}
