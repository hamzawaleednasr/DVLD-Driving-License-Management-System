using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class LicenseService
    {
        private readonly ILicense _licenseRepo;
        private readonly IDriver _driverRepo;
        private readonly IApplication _applicationRepo;

        public LicenseService(string connectionString)
        {
            _licenseRepo = new LicenseRepository(connectionString);
            _driverRepo = new DriverRepository(connectionString);
            _applicationRepo = new ApplicationRepository(connectionString);
        }

        public (int? newLicenseID, bool isSuccess) Add(LicenseDto licenseDto)
        {
            if (licenseDto.DriverID <= 0 || licenseDto.ApplicationID <= 0 || licenseDto.LicenseClassID <= 0)
            {
                return (null, false);
            }

            if (_driverRepo.GetByID(licenseDto.DriverID) == null)
            {
                return (null, false);
            }

            if (_applicationRepo.GetByID(licenseDto.ApplicationID) == null)
            {
                return (null, false);
            }

            if (licenseDto.LicenseIssueDate == default)
            {
                licenseDto.LicenseIssueDate = DateTime.Now;
            }
            licenseDto.IsActive = true;

            License license = MapToDTO.MapLicenseDtoToLicense(licenseDto);
            int newID = _licenseRepo.Add(license);

            return (newID, newID > 0);
        }

        public bool Update(LicenseDto licenseDto)
        {
            License license = MapToDTO.MapLicenseDtoToLicense(licenseDto);
            return _licenseRepo.Update(license);
        }

        public bool Delete(int id)
        {
            return _licenseRepo.Delete(id);
        }

        public LicenseDto GetByID(int id)
        {
            License license = _licenseRepo.GetByID(id);
            return MapToDTO.MapLicenseToLicenseDto(license);
        }

        public LicenseInfoViewModel GetLicenseInfoByID(int id)
        {
            return _licenseRepo.GetLicenseInfoByID(id);
        }

        public List<LicenseDto> GetAll()
        {
            List<License> list = _licenseRepo.GetAll();
            List<LicenseDto> dtos = new List<LicenseDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapLicenseToLicenseDto(item));
                }
            }
            return dtos;
        }

        public int GetActiveLicenseIDByApplicationID(int applicationID)
        {
            return _licenseRepo.GetActiveLicenseIDByApplicationID(applicationID);
        }

        public List<LicenseHistoryViewModel> GetLicenseHistoryByDriverID(int driverID)
        {
            return _licenseRepo.GetLicenseHistoryByDriverID(driverID);
        }
    }
}
