using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class TestService
    {
        private readonly ITest _testRepo;
        private readonly ITestAppointment _testAppointmentRepo;

        public TestService(string connectionString)
        {
            _testRepo = new TestRepository(connectionString);
            _testAppointmentRepo = new TestAppointmentRepository(connectionString);
        }

        public (int? newTestID, bool isSuccess) Add(TestDto dto)
        {
            if (dto.TestAppointmentID <= 0 || dto.CreatedByUserID <= 0)
            {
                return (null, false);
            }

            TestAppointment appt = _testAppointmentRepo.GetByID(dto.TestAppointmentID);
            if (appt == null)
            {
                return (null, false);
            }

            Test test = MapToDTO.MapTestDtoToTest(dto);
            int newID = _testRepo.Add(test);

            if (newID > 0)
            {
                appt.IsLocked = true;
                _testAppointmentRepo.Update(appt);
                return (newID, true);
            }

            return (null, false);
        }

        public bool Update(TestDto dto)
        {
            Test test = MapToDTO.MapTestDtoToTest(dto);
            return _testRepo.Update(test);
        }

        public bool Delete(int id)
        {
            return _testRepo.Delete(id);
        }

        public TestDto GetByID(int id)
        {
            Test test = _testRepo.GetByID(id);
            return MapToDTO.MapTestToTestDto(test);
        }

        public List<TestDto> GetAll()
        {
            List<Test> list = _testRepo.GetAll();
            List<TestDto> dtos = new List<TestDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapTestToTestDto(item));
                }
            }
            return dtos;
        }
    }
}
