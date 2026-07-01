using System.Collections.Generic;
using System.Data;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.Core.Enums;

namespace DVLD.DAL.Interfaces
{
    public interface IPerson : IRepository<Person>
    {
        List<PersonViewModel> GetAllWithCountry();
        PersonViewModel GetByIDWithCountry(int id);
        PersonViewModel GetByNationalNumberWithCountry(string nationalNumber);
        List<PersonViewModel> GetByFirstNameWithCountry(string firstName);
        List<PersonViewModel> GetBySecondNameWithCountry(string secondName);
        List<PersonViewModel> GetByThirdNameWithCountry(string thirdName);
        List<PersonViewModel> GetByLastNameWithCountry(string lastName);
        List<PersonViewModel> GetByNationalityWithCountry(string nationality);
        List<PersonViewModel> GetByGenderWithCountry(bool gender);
        List<PersonViewModel> GetByPhoneWithCountry(string phone);
        List<PersonViewModel> GetByEmailWithCountry(string email);
        int GetNumberOfPeople();
    }
}

