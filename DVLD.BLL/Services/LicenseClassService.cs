using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;
using System.Collections.Generic;

namespace DVLD.BLL.Services
{
    public class LicenseClassService
    {
        private readonly ILicenseClass _licenseClassRepo;

        public LicenseClassService(string connectionString)
        {
            _licenseClassRepo = new LicenseClassRepository(connectionString);
        }

        public List<LicenseClassDto> GetAll()
        {
            List<LicenseClass> list = _licenseClassRepo.GetAll();
            List<LicenseClassDto> dtos = new List<LicenseClassDto>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    dtos.Add(MapToDTO.MapLicenseClassToLicenseClassDto(item));
                }
            }
            return dtos;
        }

        public LicenseClassDto GetByID(int id)
        {
            LicenseClass licenseClass = _licenseClassRepo.GetByID(id);
            return licenseClass == null ? null : MapToDTO.MapLicenseClassToLicenseClassDto(licenseClass);
        }

        public bool Update(LicenseClassDto licenseClassDto)
        {
            if (licenseClassDto == null || string.IsNullOrWhiteSpace(licenseClassDto.LicenseClassTitle) || licenseClassDto.LicenseClassFees < 0)
            {
                return false;
            }

            LicenseClass licenseClass = MapToDTO.MapLicenseClassDtoToLicenseClass(licenseClassDto);
            return _licenseClassRepo.Update(licenseClass);
        }
    }
}
