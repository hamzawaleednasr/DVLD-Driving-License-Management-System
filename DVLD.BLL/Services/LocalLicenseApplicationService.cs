using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class LocalLicenseApplicationService
    {
        private readonly ILocalLicenseApplication _localLicenseApplicationRepo;
        private readonly IApplication _applicationRepo;
        private readonly ILicense _licenseRepo;
        private readonly IDriver _driverRepo;

        public LocalLicenseApplicationService(string connectionString)
        {
            _localLicenseApplicationRepo = new LocalLicenseApplicationRepository(connectionString);
            _applicationRepo = new ApplicationRepository(connectionString);
            _licenseRepo = new LicenseRepository(connectionString);
            _driverRepo = new DriverRepository(connectionString);
        }

        public (int? newLocalLicenseApplicationID, enLocalLicenseApplicationSaveStatus status) Add(LocalLicenseApplicationDto dto)
        {
            if (dto.ApplicationID <= 0 || dto.LicenseClassID <= 0)
            {
                return (null, enLocalLicenseApplicationSaveStatus.RequiredDataMissing);
            }

            Application app = _applicationRepo.GetByID(dto.ApplicationID);
            if (app == null)
            {
                return (null, enLocalLicenseApplicationSaveStatus.ApplicationDoesNotExist);
            }

            List<LocalLicenseApplication> allLlas = _localLicenseApplicationRepo.GetAll();
            if (allLlas != null)
            {
                foreach (var lla in allLlas)
                {
                    if (lla.LicenseClassID == dto.LicenseClassID)
                    {
                        Application appCheck = _applicationRepo.GetByID(lla.ApplicationID);
                        if (appCheck != null && appCheck.PersonID == app.PersonID)
                        {
                            if (appCheck.ApplicationStatus == "New" || appCheck.ApplicationStatus == "In Progress")
                            {
                                return (null, enLocalLicenseApplicationSaveStatus.PersonAlreadyHasActiveApplication);
                            }
                        }
                    }
                }
            }

            List<License> allLicenses = _licenseRepo.GetAll();
            if (allLicenses != null)
            {
                foreach (var lic in allLicenses)
                {
                    if (lic.LicenseClassID == dto.LicenseClassID && lic.IsActive)
                    {
                        Driver driver = _driverRepo.GetByID(lic.DriverID);
                        if (driver != null && driver.PersonID == app.PersonID)
                        {
                            return (null, enLocalLicenseApplicationSaveStatus.PersonAlreadyHasLicenseOfSameClass);
                        }
                    }
                }
            }

            LocalLicenseApplication localLicenseApplication = MapToDTO.MapLocalLicenseApplicationDtoToLocalLicenseApplication(dto);
            int newID = _localLicenseApplicationRepo.Add(localLicenseApplication);

            return (newID, enLocalLicenseApplicationSaveStatus.Success);
        }

        public bool Update(LocalLicenseApplicationDto dto)
        {
            LocalLicenseApplication localLicenseApplication = MapToDTO.MapLocalLicenseApplicationDtoToLocalLicenseApplication(dto);
            return _localLicenseApplicationRepo.Update(localLicenseApplication);
        }

        public bool Delete(int id)
        {
            return _localLicenseApplicationRepo.Delete(id);
        }

        public LocalLicenseApplicationDto GetByID(int id)
        {
            LocalLicenseApplication lla = _localLicenseApplicationRepo.GetByID(id);
            return MapToDTO.MapLocalLicenseApplicationToLocalLicenseApplicationDto(lla);
        }

        public List<LocalLicenseApplicationDto> GetAll()
        {
            List<LocalLicenseApplication> list = _localLicenseApplicationRepo.GetAll();
            List<LocalLicenseApplicationDto> dtos = new List<LocalLicenseApplicationDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapLocalLicenseApplicationToLocalLicenseApplicationDto(item));
                }
            }
            return dtos;
        }

        public List<LocalLicenseApplicationViewModel> GetAllToView()
        {
            return _localLicenseApplicationRepo.GetAllToView();
        }

        public LocalLicenseApplicationViewModel GetByIDToView(int id)
        {
            return _localLicenseApplicationRepo.GetByIDToView(id);
        }

        public List<LocalLicenseApplicationViewModel> GetByNationalNumberToView(string nationalNumber)
        {
            return _localLicenseApplicationRepo.GetByNationalNumberToView(nationalNumber);
        }

        public List<LocalLicenseApplicationViewModel> GetByFullNameToView(string fullName)
        {
            return _localLicenseApplicationRepo.GetByFullNameToView(fullName);
        }

        public List<LocalLicenseApplicationViewModel> GetByStatusToView(string status)
        {
            return _localLicenseApplicationRepo.GetByStatusToView(status);
        }

        public int GetNumberOfLocalLicenseApplication()
        {
            return _localLicenseApplicationRepo.GetNumberOfLocalLicenseApplication();
        }
    }
}
