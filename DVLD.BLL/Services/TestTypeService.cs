using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class TestTypeService
    {
        private readonly ITestType _testTypeRepo;

        public TestTypeService(string connectionString)
        {
            _testTypeRepo = new TestTypeRepository(connectionString);
        }

        public bool Update(TestTypeDto testTypeDto)
        {
            if (testTypeDto == null || string.IsNullOrWhiteSpace(testTypeDto.TestTypeTitle) || testTypeDto.TestTypeFees < 0)
            {
                return false;
            }

            TestType testType = MapToDTO.MapTestTypeDtoToTestType(testTypeDto);
            return _testTypeRepo.Update(testType);
        }

        public List<TestTypeDto> GetAll()
        {
            List<TestType> list = _testTypeRepo.GetAll();
            List<TestTypeDto> dtos = new List<TestTypeDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapTestTypeToTestTypeDto(item));
                }
            }
            return dtos;
        }

        public int GetNumberOfTestTypes()
        {
            return _testTypeRepo.GetNumberOfTestTypes();
        }

        public TestTypeDto GetByID(int id)
        {
            TestType testType = _testTypeRepo.GetByID(id);
            return testType == null ? null : MapToDTO.MapTestTypeToTestTypeDto(testType);
        }
    }
}
