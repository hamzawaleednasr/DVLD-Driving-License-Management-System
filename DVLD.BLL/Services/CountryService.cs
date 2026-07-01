using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.DAL.Repositories;
using System.Collections.Generic;

namespace DVLD.BLL.Services
{
    public class CountryService
    {
        private readonly ICountry _countryRepo; 

        public CountryService(string connectionString)
        {
            _countryRepo = new CountryRepository(connectionString);
        }

        public List<CountryDto> GetAllCountries()
        {
            List<Country> countries = _countryRepo.GetAllCountries();
            List<CountryDto> countryDtos = new List<CountryDto>();
            foreach (Country country in countries)
            {
                countryDtos.Add(MapToDTO.MapCountryToCountryDto(country));
            }
            return countryDtos;
        }

        public int GetCountryID(string countryName)
        {
            return _countryRepo.GetCountryID(countryName);
        }
    }
}
