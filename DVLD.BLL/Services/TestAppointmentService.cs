using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class TestAppointmentService
    {
        private readonly ITestAppointment _testAppointmentRepo;
        private readonly ITest _testRepo;
        private readonly ILocalLicenseApplication _localLicenseApplicationRepo;

        public TestAppointmentService(string connectionString)
        {
            _testAppointmentRepo = new TestAppointmentRepository(connectionString);
            _testRepo = new TestRepository(connectionString);
            _localLicenseApplicationRepo = new LocalLicenseApplicationRepository(connectionString);
        }

        public (int? newAppointmentID, enTestAppointmentSaveStatus status) Add(TestAppointmentDto dto)
        {
            if (dto.LocalLicenseApplicationID <= 0 || dto.TestTypeID <= 0 || dto.CreatedByUserID <= 0)
            {
                return (null, enTestAppointmentSaveStatus.RequiredDataMissing);
            }

            if (_localLicenseApplicationRepo.GetByID(dto.LocalLicenseApplicationID) == null)
            {
                return (null, enTestAppointmentSaveStatus.LocalLicenseApplicationDoesNotExist);
            }

            List<TestAppointment> allAppts = _testAppointmentRepo.GetAll();
            if (allAppts != null)
            {
                foreach (var appt in allAppts)
                {
                    if (appt.LocalLicenseApplicationID == dto.LocalLicenseApplicationID && 
                        appt.TestTypeID == dto.TestTypeID && 
                        !appt.IsLocked)
                    {
                        return (null, enTestAppointmentSaveStatus.PersonAlreadyHasActiveAppointmentForThisTest);
                    }
                }
            }

            if (HasPassedAppointment(dto.LocalLicenseApplicationID, dto.TestTypeID))
            {
                return (null, enTestAppointmentSaveStatus.PersonAlreadyPassedThisTest);
            }

            if (dto.TestTypeID == 2) 
            {
                if (!HasPassedAppointment(dto.LocalLicenseApplicationID, 1))
                {
                    return (null, enTestAppointmentSaveStatus.PreviousTestNotPassed);
                }
            }
            else if (dto.TestTypeID == 3)
            {
                if (!HasPassedAppointment(dto.LocalLicenseApplicationID, 2))
                {
                    return (null, enTestAppointmentSaveStatus.PreviousTestNotPassed);
                }
            }

            if (dto.TestAppointmentDate == default)
            {
                dto.TestAppointmentDate = DateTime.Now;
            }
            dto.IsLocked = false;

            TestAppointment appointment = MapToDTO.MapTestAppointmentDtoToTestAppointment(dto);
            int newID = _testAppointmentRepo.Add(appointment);

            return (newID, enTestAppointmentSaveStatus.Success);
        }

        public bool Update(TestAppointmentDto dto)
        {
            TestAppointment appointment = MapToDTO.MapTestAppointmentDtoToTestAppointment(dto);
            return _testAppointmentRepo.Update(appointment);
        }

        public bool Delete(int id)
        {
            return _testAppointmentRepo.Delete(id);
        }

        public TestAppointmentDto GetByID(int id)
        {
            TestAppointment appt = _testAppointmentRepo.GetByID(id);
            return MapToDTO.MapTestAppointmentToTestAppointmentDto(appt);
        }

        public List<TestAppointmentDto> GetAll()
        {
            List<TestAppointment> list = _testAppointmentRepo.GetAll();
            List<TestAppointmentDto> dtos = new List<TestAppointmentDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapTestAppointmentToTestAppointmentDto(item));
                }
            }
            return dtos;
        }

        public TestAppointmentViewModel GetByIDToView(int id)
        {
            return _testAppointmentRepo.GetByIDToView(id);
        }

        public List<TestAppointmentDto> GetBy_TestTypeID_LocalLicenseApplicationID(int testTypeID, int localLicenseApplicationID)
        {
            return _testAppointmentRepo.GetBy_TestTypeID_LocalLicenseApplicationID(testTypeID, localLicenseApplicationID);
        }

        public int GetNumberOfTestAppointments()
        {
            return _testAppointmentRepo.GetNumberOfTestAppointments();
        }

        private bool HasPassedAppointment(int localLicenseApplicationID, int testTypeID)
        {
            List<TestAppointment> allAppts = _testAppointmentRepo.GetAll();
            if (allAppts != null)
            {
                foreach (var appt in allAppts)
                {
                    if (appt.LocalLicenseApplicationID == localLicenseApplicationID && appt.TestTypeID == testTypeID)
                    {
                        List<Test> allTests = _testRepo.GetAll();
                        if (allTests != null)
                        {
                            foreach (var test in allTests)
                            {
                                if (test.TestAppointmentID == appt.TestAppointmentID && test.TestResult)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
