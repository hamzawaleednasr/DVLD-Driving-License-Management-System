using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class InternationalLicenseService
    {
        private readonly IInternationalLicense _internationalLicenseRepo;
        private readonly IDriver _driverRepo;
        private readonly ILicense _licenseRepo;
        private readonly IApplication _applicationRepo;

        public InternationalLicenseService(string connectionString)
        {
            _internationalLicenseRepo = new InternationalLicenseRepository(connectionString);
            _driverRepo = new DriverRepository(connectionString);
            _licenseRepo = new LicenseRepository(connectionString);
            _applicationRepo = new ApplicationRepository(connectionString);
        }

        public (int? newInternationalLicenseID, enInternationalLicenseSaveStatus status) Add(InternationalLicenseDto dto)
        {
            if (dto.DriverID <= 0 || dto.ApplicationID <= 0 || dto.IssueUsingLocalLicenseID <= 0 || dto.CreatedByUserID <= 0)
            {
                return (null, enInternationalLicenseSaveStatus.RequiredDataMissing);
            }

            if (_driverRepo.GetByID(dto.DriverID) == null)
            {
                return (null, enInternationalLicenseSaveStatus.DriverDoesNotExist);
            }

            if (_applicationRepo.GetByID(dto.ApplicationID) == null)
            {
                return (null, enInternationalLicenseSaveStatus.ApplicationDoesNotExist);
            }

            License localLicense = _licenseRepo.GetByID(dto.IssueUsingLocalLicenseID);
            if (localLicense == null)
            {
                return (null, enInternationalLicenseSaveStatus.LocalLicenseDoesNotExist);
            }
            if (localLicense.LicenseClassID != 7) 
            {
                return (null, enInternationalLicenseSaveStatus.LocalLicenseNotClass3);
            }
            if (!localLicense.IsActive)
            {
                return (null, enInternationalLicenseSaveStatus.LocalLicenseNotActive);
            }
            if (localLicense.LicenseExpirationDate < DateTime.Now)
            {
                return (null, enInternationalLicenseSaveStatus.LocalLicenseExpired);
            }

            List<InternationalLicense> activeLicenses = _internationalLicenseRepo.GetAll();
            if (activeLicenses != null)
            {
                foreach (var il in activeLicenses)
                {
                    if (il.DriverID == dto.DriverID && il.IsActive)
                    {
                        return (null, enInternationalLicenseSaveStatus.DriverAlreadyHasActiveInternationalLicense);
                    }
                }
            }

            if (dto.IssueDate == default)
            {
                dto.IssueDate = DateTime.Now;
            }
            if (dto.ExpirationDate == default)
            {
                dto.ExpirationDate = DateTime.Now.AddYears(1); 
            }
            dto.IsActive = true;

            InternationalLicense internationalLicense = MapToDTO.MapInternationalLicenseDtoToInternationalLicense(dto);
            int newID = _internationalLicenseRepo.Add(internationalLicense);

            return (newID, enInternationalLicenseSaveStatus.Success);
        }

        public bool Update(InternationalLicenseDto dto)
        {
            InternationalLicense internationalLicense = MapToDTO.MapInternationalLicenseDtoToInternationalLicense(dto);
            return _internationalLicenseRepo.Update(internationalLicense);
        }

        public bool Delete(int id)
        {
            return _internationalLicenseRepo.Delete(id);
        }

        public InternationalLicenseDto GetByID(int id)
        {
            InternationalLicense il = _internationalLicenseRepo.GetByID(id);
            return MapToDTO.MapInternationalLicenseToInternationalLicenseDto(il);
        }

        public List<InternationalLicenseDto> GetAll()
        {
            List<InternationalLicense> list = _internationalLicenseRepo.GetAll();
            List<InternationalLicenseDto> dtos = new List<InternationalLicenseDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapInternationalLicenseToInternationalLicenseDto(item));
                }
            }
            return dtos;
        }

        public List<InternationalLicenseHistory> GetByLicenseIDToHistory(int id)
        {
            return _internationalLicenseRepo.GetByLicenseIDToHistory(id);
        }

        public InternationalLicenseInfo GetByIDToShowInfo(int id)
        {
            return _internationalLicenseRepo.GetByIDToShowInfo(id);
        }

        public int GetNumberOfLocalInternationalLicenses()
        {
            return _internationalLicenseRepo.GetNumberOfLocalInternationalLicenses();
        }

        public List<InternationalLicenseDto> GetByDriverID(int driverID)
        {
            List<InternationalLicense> list = _internationalLicenseRepo.GetByDriverID(driverID);
            List<InternationalLicenseDto> dtos = new List<InternationalLicenseDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapInternationalLicenseToInternationalLicenseDto(item));
                }
            }
            return dtos;
        }
    }
}
