using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using System.Collections.Generic;

namespace DVLD.DAL.Interfaces
{
    public interface ITestAppointment : IRepository<TestAppointment>
    {
        TestAppointmentViewModel GetByIDToView(int id);
        int GetNumberOfTestAppointments();
        List<TestAppointmentDto> GetBy_TestTypeID_LocalLicenseApplicationID(int testTypeID, int localLicenseApplicationID);
    }
}

