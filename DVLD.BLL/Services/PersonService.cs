using System.Collections.Generic;
using System.Text.RegularExpressions;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Repositories;
using DVLD.Core.Enums;
using DVLD.DAL.Interfaces;
using System;

namespace DVLD.BLL.Services
{
    public class PersonService
    {
        private readonly IPerson _personRepo;

        public PersonService(string connectionString)
        {
            _personRepo = new PersonRepository(connectionString);
        }

        private bool _IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return true;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public (int? newPersonID, enPersonSaveStatus status) Add(PersonDto personDTO)
        {
            if (string.IsNullOrWhiteSpace(personDTO.NationalNumber) || string.IsNullOrWhiteSpace(personDTO.FirstName) || string.IsNullOrWhiteSpace(personDTO.LastName) || string.IsNullOrEmpty(personDTO.Phone))
            {
                return (null, enPersonSaveStatus.RequiredDataMissing);
            }

            if (personDTO.BirthDate > DateTime.Today.AddYears(-18))
            {
                return (null, enPersonSaveStatus.AgeLessThan18);
            }

            if (!_IsValidEmail(personDTO.Email))
            {
                return (null, enPersonSaveStatus.InvalidEmail);
            }

            if (_personRepo.GetByNationalNumberWithCountry(personDTO.NationalNumber) != null)
            {
                return (null, enPersonSaveStatus.NationalNumberExists);
            }

            if (!string.IsNullOrEmpty(personDTO.Email))
            {
                var existingPeopleWithEmail = _personRepo.GetByEmailWithCountry(personDTO.Email);
                if (existingPeopleWithEmail != null && existingPeopleWithEmail.Count > 0)
                {
                    return (null, enPersonSaveStatus.EmailExists);
                }
            }

            Person person = MapToDTO.MapPersonDtoToPerson(personDTO);

            int newId = _personRepo.Add(person);
            
            return (newId, enPersonSaveStatus.Success);
        }

        public (bool isSuccess, enPersonSaveStatus status) Update(PersonDto personDTO)
        {
            if (string.IsNullOrWhiteSpace(personDTO.NationalNumber) || string.IsNullOrWhiteSpace(personDTO.FirstName) || string.IsNullOrWhiteSpace(personDTO.LastName) || string.IsNullOrEmpty(personDTO.Phone) || string.IsNullOrWhiteSpace(personDTO.Address))
            {
                return (false, enPersonSaveStatus.RequiredDataMissing);
            }

            if (personDTO.BirthDate > DateTime.Today.AddYears(-18))
            {
                return (false, enPersonSaveStatus.AgeLessThan18);
            }

            if (!_IsValidEmail(personDTO.Email))
            {
                return (false, enPersonSaveStatus.InvalidEmail);
            }

            PersonViewModel personWithNationalNumber = _personRepo.GetByNationalNumberWithCountry(personDTO.NationalNumber);
            if (personWithNationalNumber != null && personWithNationalNumber.PersonID != personDTO.PersonID)
            {
                return (false, enPersonSaveStatus.NationalNumberExists);
            }

            if (!string.IsNullOrEmpty(personDTO.Email))
            {
                var existingPeopleWithEmail = _personRepo.GetByEmailWithCountry(personDTO.Email);
                if (existingPeopleWithEmail != null)
                {
                    foreach (var p in existingPeopleWithEmail)
                    {
                        if (p.PersonID != personDTO.PersonID)
                        {
                            return (false, enPersonSaveStatus.EmailExists);
                        }
                    }
                }
            }

            Person person = MapToDTO.MapPersonDtoToPerson(personDTO);
            bool isSuccess = _personRepo.Update(person);

            return (isSuccess, isSuccess ? enPersonSaveStatus.Success : enPersonSaveStatus.RequiredDataMissing);
        }

        public bool Delete(int id)
        {
            bool isSuccess = _personRepo.Delete(id);

            return isSuccess;
        }

        public List<PersonDto> GetAll()
        {
            List<Person> people = _personRepo.GetAll();
            List<PersonDto> peopleDtoList = new List<PersonDto>();
            if (people != null)
            {
                foreach (var person in people)
                {
                    peopleDtoList.Add(MapToDTO.MapPersonToPersonDto(person));
                }
            }
            return peopleDtoList;
        }

        public List<PersonViewModel> GetAllWithCountry()
        {
            List<PersonViewModel> people = _personRepo.GetAllWithCountry();

            return people;
        }

        public PersonDto GetByID(int id)
        {
            Person person = _personRepo.GetByID(id);

            return MapToDTO.MapPersonToPersonDto(person);
        }

        public PersonViewModel GetByIDWithCountry(int id)
        {
            PersonViewModel person = _personRepo.GetByIDWithCountry(id);

            return person;
        }

        public PersonViewModel GetByNationalNumberWithCountry(string nationalNumber)
        {
            PersonViewModel person = _personRepo.GetByNationalNumberWithCountry(nationalNumber);

            return person;
        }

        public List<PersonViewModel> GetByNationalityWithCountry(string nationality)
        {
            List<PersonViewModel> people = _personRepo.GetByNationalityWithCountry(nationality);

            return people;
        }

        public List<PersonViewModel> GetByFirstNameWithCountry(string firstName)
        {
            List<PersonViewModel> people = _personRepo.GetByFirstNameWithCountry(firstName);

            return people;
        }

        public List<PersonViewModel> GetBySecondNameWithCountry(string secondName)
        {
            List<PersonViewModel> people = _personRepo.GetBySecondNameWithCountry(secondName);

            return people;
        }

        public List<PersonViewModel> GetByThirdNameWithCountry(string thirdName)
        {
            List<PersonViewModel> people = _personRepo.GetByThirdNameWithCountry(thirdName);

            return people;
        }

        public List<PersonViewModel> GetByLastNameWithCountry(string lastName)
        {
            List<PersonViewModel> people = _personRepo.GetByLastNameWithCountry(lastName);

            return people;
        }

        public List<PersonViewModel> GetByGenderWithCountry(bool gender)
        {
            List<PersonViewModel> people = _personRepo.GetByGenderWithCountry(gender);

            return people;
        }

        public List<PersonViewModel> GetByEmailWithCountry(string email)
        {
            List<PersonViewModel> people = _personRepo.GetByEmailWithCountry(email);

            return people;
        }

        public List<PersonViewModel> GetByPhoneWithCountry(string phone)
        {
            List<PersonViewModel> people = _personRepo.GetByPhoneWithCountry(phone);

            return people;
        }

        public int GetNumberOfPeople()
        {
            int totalNumber = _personRepo.GetNumberOfPeople();

            return totalNumber;
        }
    }
}

