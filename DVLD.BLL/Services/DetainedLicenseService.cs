using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class DetainedLicenseService
    {
        private readonly IDetainedLicense _detainedLicenseRepo;
        private readonly ILicense _licenseRepo;
        private readonly IUser _userRepo;

        public DetainedLicenseService(string connectionString)
        {
            _detainedLicenseRepo = new DetainedLicenseRepository(connectionString);
            _licenseRepo = new LicenseRepository(connectionString);
            _userRepo = new UserRepository(connectionString);
        }

        public (int? newDetainedLicenseID, enDetainedLicenseSaveStatus status) Add(DetainedLicenseDto detainedLicenseDto)
        {
            if (detainedLicenseDto.LicenseID <= 0 || detainedLicenseDto.CreatedByUserID <= 0 || detainedLicenseDto.FineFees < 0)
            {
                return (null, enDetainedLicenseSaveStatus.RequiredDataMissing);
            }

            if (_userRepo.GetByID(detainedLicenseDto.CreatedByUserID) == null)
            {
                return (null, enDetainedLicenseSaveStatus.UserDoesNotExist);
            }

            if (_licenseRepo.GetByID(detainedLicenseDto.LicenseID) == null)
            {
                return (null, enDetainedLicenseSaveStatus.LicenseDoesNotExist);
            }

            List<DetainedLicense> allDetained = _detainedLicenseRepo.GetAll();
            if (allDetained != null)
            {
                foreach (var dl in allDetained)
                {
                    if (dl.LicenseID == detainedLicenseDto.LicenseID && !dl.IsReleased)
                    {
                        return (null, enDetainedLicenseSaveStatus.LicenseAlreadyDetained);
                    }
                }
            }

            if (detainedLicenseDto.DetainDate == default)
            {
                detainedLicenseDto.DetainDate = DateTime.Now;
            }
            detainedLicenseDto.IsReleased = false;

            DetainedLicense detainedLicense = MapToDTO.MapDetainedLicenseDtoToDetainedLicense(detainedLicenseDto);
            int newID = _detainedLicenseRepo.Add(detainedLicense);

            return (newID, enDetainedLicenseSaveStatus.Success);
        }

        public bool Update(DetainedLicenseDto detainedLicenseDto)
        {
            DetainedLicense detainedLicense = MapToDTO.MapDetainedLicenseDtoToDetainedLicense(detainedLicenseDto);
            return _detainedLicenseRepo.Update(detainedLicense);
        }

        public bool Delete(int id)
        {
            return _detainedLicenseRepo.Delete(id);
        }

        public DetainedLicenseDto GetByID(int id)
        {
            DetainedLicense detainedLicense = _detainedLicenseRepo.GetByID(id);
            return MapToDTO.MapDetainedLicenseToDetainedLicenseDto(detainedLicense);
        }

        public List<DetainedLicenseDto> GetAll()
        {
            List<DetainedLicense> list = _detainedLicenseRepo.GetAll();
            List<DetainedLicenseDto> dtos = new List<DetainedLicenseDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapDetainedLicenseToDetainedLicenseDto(item));
                }
            }
            return dtos;
        }

        public List<DetainedLicenseViewModel> GetAllToView()
        {
            return _detainedLicenseRepo.GetAllToView();
        }

        public DetainedLicenseViewModel GetByIDToView(int id)
        {
            return _detainedLicenseRepo.GetByIDToView(id);
        }

        public DetainedLicenseViewModel GetByNationalNumberToView(string nationalNumber)
        {
            return _detainedLicenseRepo.GetByNationalNumberToView(nationalNumber);
        }

        public List<DetainedLicenseViewModel> GetByFullNameToView(string fullName)
        {
            return _detainedLicenseRepo.GetByFullNameToView(fullName);
        }

        public List<DetainedLicenseViewModel> GetByReleaseStatusToView(bool isRelease)
        {
            return _detainedLicenseRepo.GetByReleaseStatusToView(isRelease);
        }

        public int GetNumberOfDetainedLicenses()
        {
            return _detainedLicenseRepo.GetNumberOfDetainedLicenses();
        }
    }
}
