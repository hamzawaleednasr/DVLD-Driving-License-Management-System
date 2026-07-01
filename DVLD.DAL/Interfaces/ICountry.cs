using DVLD.DAL.Entities;
using System.Collections.Generic;

namespace DVLD.DAL.Interfaces
{
    public interface ICountry
    {
        List<Country> GetAllCountries();
        int GetCountryID(string countryName);
    }
}
