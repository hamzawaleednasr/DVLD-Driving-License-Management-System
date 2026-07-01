using System;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.Core.Enums;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class ApplicationService
    {
        private readonly IApplication _applicationRepo;
        private readonly IPerson _personRepo;
        private readonly IApplicationType _applicationTypeRepo;
        private readonly IUser _userRepo;

        public ApplicationService(string connectionString)
        {
            _applicationRepo = new ApplicationRepository(connectionString);
            _personRepo = new PersonRepository(connectionString);
            _applicationTypeRepo = new ApplicationTypeRepository(connectionString);
            _userRepo = new UserRepository(connectionString);
        }

        public (int? newApplicationID, enApplicationSaveStatus status) Add(ApplicationDto applicationDto)
        {
            if (applicationDto.PersonID <= 0 || applicationDto.ApplicationTypeID <= 0 || applicationDto.CreatedByUserID <= 0)
            {
                return (null, enApplicationSaveStatus.RequiredDataMissing);
            }

            if (_personRepo.GetByID(applicationDto.PersonID) == null)
            {
                return (null, enApplicationSaveStatus.PersonDoesNotExist);
            }

            List<ApplicationType> appTypes = _applicationTypeRepo.GetAll();
            bool typeExists = false;
            if (appTypes != null)
            {
                foreach (var type in appTypes)
                {
                    if (type.ApplicationTypeID == applicationDto.ApplicationTypeID)
                    {
                        typeExists = true;
                        if (applicationDto.ApplicationPaidFees == 0)
                        {
                            applicationDto.ApplicationPaidFees = type.ApplicationTypeFees;
                        }
                        break;
                    }
                }
            }

            if (!typeExists)
            {
                return (null, enApplicationSaveStatus.ApplicationTypeDoesNotExist);
            }

            if (_userRepo.GetByID(applicationDto.CreatedByUserID) == null)
            {
                return (null, enApplicationSaveStatus.CreatedByUserDoesNotExist);
            }

            if (string.IsNullOrEmpty(applicationDto.ApplicationStatus))
            {
                applicationDto.ApplicationStatus = "New";
            }
            if (applicationDto.CreatedAt == default)
            {
                applicationDto.CreatedAt = DateTime.Now;
            }
            if (applicationDto.ApplicationLastStatusDate == default)
            {
                applicationDto.ApplicationLastStatusDate = DateTime.Now;
            }

            Application application = MapToDTO.MapApplicationDtoToApplication(applicationDto);
            int newID = _applicationRepo.Add(application);

            return (newID, enApplicationSaveStatus.Success);
        }

        public bool Update(ApplicationDto applicationDto)
        {
            Application application = MapToDTO.MapApplicationDtoToApplication(applicationDto);
            return _applicationRepo.Update(application);
        }

        public bool Delete(int id)
        {
            return _applicationRepo.Delete(id);
        }

        public ApplicationDto GetByID(int id)
        {
            Application application = _applicationRepo.GetByID(id);
            return MapToDTO.MapApplicationToApplicationDto(application);
        }

        public List<ApplicationDto> GetAll()
        {
            List<Application> applications = _applicationRepo.GetAll();
            List<ApplicationDto> applicationDtos = new List<ApplicationDto>();
            if (applications != null)
            {
                foreach (var app in applications)
                {
                    applicationDtos.Add(MapToDTO.MapApplicationToApplicationDto(app));
                }
            }
            return applicationDtos;
        }
    }
}
