using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class DriverService
    {
        private readonly IDriver _driverRepo;
        private readonly IPerson _personRepo;

        public DriverService(string connectionString)
        {
            _driverRepo = new DriverRepository(connectionString);
            _personRepo = new PersonRepository(connectionString);
        }

        public (int? newDriverID, enDriverSaveStatus status) Add(DriverDto driverDto)
        {
            if (driverDto.PersonID <= 0 || driverDto.CreatedByUserID <= 0)
            {
                return (null, enDriverSaveStatus.RequiredDataMissing);
            }

            PersonViewModel person = _personRepo.GetByIDWithCountry(driverDto.PersonID);
            if (person == null)
            {
                return (null, enDriverSaveStatus.PersonDoesNotExist);
            }

            DriverViewModel existingDriver = _driverRepo.GetByPersonIDToView(driverDto.PersonID);
            if (existingDriver != null)
            {
                return (null, enDriverSaveStatus.PersonAlreadyDriver);
            }

            Driver driver = MapToDTO.MapDriverDtoToDriver(driverDto);
            int newDriverID = _driverRepo.Add(driver);

            return (newDriverID, enDriverSaveStatus.Success);
        }

        public bool Update(DriverDto driverDto)
        {
            Driver driver = MapToDTO.MapDriverDtoToDriver(driverDto);
            return _driverRepo.Update(driver);
        }

        public bool Delete(int id)
        {
            return _driverRepo.Delete(id);
        }

        public List<DriverDto> GetAll()
        {
            List<Driver> drivers = _driverRepo.GetAll();
            List<DriverDto> driversDtoList = new List<DriverDto>();
            if (drivers != null)
            {
                foreach (var d in drivers)
                {
                    driversDtoList.Add(MapToDTO.MapDriverToDriverDto(d));
                }
            }
            return driversDtoList;
        }

        public DriverDto GetByID(int id)
        {
            Driver driver = _driverRepo.GetByID(id);
            return MapToDTO.MapDriverToDriverDto(driver);
        }

        public DriverDto GetByPersonID(int personID)
        {
            Driver driver = _driverRepo.GetByPersonID(personID);
            return MapToDTO.MapDriverToDriverDto(driver);
        }

        public List<DriverViewModel> GetAllToView()
        {
            return _driverRepo.GetAllToView();
        }

        public DriverViewModel GetByPersonIDToView(int personID)
        {
            return _driverRepo.GetByPersonIDToView(personID);
        }

        public DriverViewModel GetByNationalNumberToView(string nationalNumber)
        {
            return _driverRepo.GetByNationalNumberToView(nationalNumber);
        }

        public List<DriverViewModel> GetByFullNameToView(string fullName)
        {
            return _driverRepo.GetByFullNameToView(fullName);
        }

        public int GetNumberOfDrivers()
        {
            return _driverRepo.GetNumberOfDrivers();
        }
    }
}
