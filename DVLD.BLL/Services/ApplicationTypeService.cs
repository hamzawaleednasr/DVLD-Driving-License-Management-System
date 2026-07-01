using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;

namespace DVLD.BLL.Services
{
    public class ApplicationTypeService
    {
        private readonly IApplicationType _applicationTypeRepo;

        public ApplicationTypeService(string connectionString)
        {
            _applicationTypeRepo = new ApplicationTypeRepository(connectionString);
        }

        public bool Update(ApplicationTypeDto applicationTypeDto)
        {
            if (applicationTypeDto == null || string.IsNullOrWhiteSpace(applicationTypeDto.ApplicationTypeTitle) || applicationTypeDto.ApplicationTypeFees < 0)
            {
                return false;
            }

            ApplicationType applicationType = MapToDTO.MapApplicationTypeDtoToApplicationType(applicationTypeDto);
            return _applicationTypeRepo.Update(applicationType);
        }

        public List<ApplicationTypeDto> GetAll()
        {
            List<ApplicationType> types = _applicationTypeRepo.GetAll();
            List<ApplicationTypeDto> typeDtos = new List<ApplicationTypeDto>();
            if (types != null)
            {
                foreach (var t in types)
                {
                    typeDtos.Add(MapToDTO.MapApplicationTypeToApplicationTypeDto(t));
                }
            }
            return typeDtos;
        }

        public ApplicationTypeDto GetByID(int id)
        {
            return MapToDTO.MapApplicationTypeToApplicationTypeDto(_applicationTypeRepo.GetByID(id));
        }

        public int GetNumberOfApplicationTypes()
        {
            return _applicationTypeRepo.GetNumberOfApplicationTypes();
        }
    }
}
