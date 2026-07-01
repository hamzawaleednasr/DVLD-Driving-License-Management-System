using DVLD.Core.DTOs;
using DVLD.DAL.Entities;

namespace DVLD.BLL
{
    public static class MapToDTO
    {
        // === Person Mapping ===
        public static PersonDto MapPersonToPersonDto(Person person)
        {
            if (person == null) return null;
            return new PersonDto
            {
                PersonID = person.PersonID,
                CountryID = person.CountryID,
                NationalNumber = person.NationalNumber,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName,
                LastName = person.LastName,
                Gender = person.Gender,
                Phone = person.Phone,
                Email = person.Email,
                PersonalPhoto = person.PersonalPhoto,
                BirthDate = person.BirthDate,
                Address = person.Address,
            };
        }

        public static Person MapPersonDtoToPerson(PersonDto personDto)
        {
            if (personDto == null) return null;
            return new Person
            {
                PersonID = personDto.PersonID,
                CountryID = personDto.CountryID,
                NationalNumber = personDto.NationalNumber,
                FirstName = personDto.FirstName,
                SecondName = personDto.SecondName,
                ThirdName = personDto.ThirdName,
                LastName = personDto.LastName,
                Gender = personDto.Gender,
                Phone = personDto.Phone,
                Email = personDto.Email,
                PersonalPhoto = personDto.PersonalPhoto,
                BirthDate = personDto.BirthDate,
                Address = personDto.Address,
            };
        }

        // === User Mapping ===
        public static UserDto MapUserToUserDto(User user)
        {
            if (user == null) return null;
            return new UserDto
            {
                UserID = user.UserID,
                PersonID = user.PersonID,
                IsActive = user.IsActive,
                Username = user.Username,
                Password = user.Password,
                CreatedAt = user.CreatedAt
            };
        }

        public static User MapUserDtoToUser(UserDto userDto)
        {
            if (userDto == null) return null;
            return new User
            {
                UserID = userDto.UserID,
                PersonID = userDto.PersonID,
                IsActive = userDto.IsActive,
                Username = userDto.Username,
                Password = userDto.Password,
                CreatedAt = userDto.CreatedAt
            };
        }

        // === Driver Mapping ===
        public static DriverDto MapDriverToDriverDto(Driver driver)
        {
            if (driver == null) return null;
            return new DriverDto
            {
                DriverID = driver.DriverID,
                PersonID = driver.PersonID,
                CreatedByUserID = driver.CreatedByUserID,
                CreatedAt = driver.CreatedAt
            };
        }

        public static Driver MapDriverDtoToDriver(DriverDto driverDto)
        {
            if (driverDto == null) return null;
            return new Driver
            {
                DriverID = driverDto.DriverID,
                PersonID = driverDto.PersonID,
                CreatedByUserID = driverDto.CreatedByUserID,
                CreatedAt = driverDto.CreatedAt
            };
        }

        // === License Mapping ===
        public static LicenseDto MapLicenseToLicenseDto(License license)
        {
            if (license == null) return null;
            return new LicenseDto
            {
                LicenseID = license.LicenseID,
                LicenseIssueDate = license.LicenseIssueDate,
                LicenseExpirationDate = license.LicenseExpirationDate,
                LicensePaidFees = license.LicensePaidFees,
                LicenseIssueReason = license.LicenseIssueReason,
                IsActive = license.IsActive,
                Notes = license.Notes,
                DriverID = license.DriverID,
                ApplicationID = license.ApplicationID,
                LicenseClassID = license.LicenseClassID
            };
        }

        public static License MapLicenseDtoToLicense(LicenseDto licenseDto)
        {
            if (licenseDto == null) return null;
            return new License
            {
                LicenseID = licenseDto.LicenseID,
                LicenseIssueDate = licenseDto.LicenseIssueDate,
                LicenseExpirationDate = licenseDto.LicenseExpirationDate,
                LicensePaidFees = licenseDto.LicensePaidFees,
                LicenseIssueReason = licenseDto.LicenseIssueReason,
                IsActive = licenseDto.IsActive,
                Notes = licenseDto.Notes,
                DriverID = licenseDto.DriverID,
                ApplicationID = licenseDto.ApplicationID,
                LicenseClassID = licenseDto.LicenseClassID
            };
        }

        // === Application Mapping ===
        public static ApplicationDto MapApplicationToApplicationDto(Application application)
        {
            if (application == null) return null;
            return new ApplicationDto
            {
                ApplicationID = application.ApplicationID,
                ApplicationStatus = application.ApplicationStatus,
                ApplicationLastStatusDate = application.ApplicationLastStatusDate,
                ApplicationPaidFees = application.ApplicationPaidFees,
                CreatedAt = application.CreatedAt,
                PersonID = application.PersonID,
                ApplicationTypeID = application.ApplicationTypeID,
                CreatedByUserID = application.CreatedByUserID
            };
        }

        public static Application MapApplicationDtoToApplication(ApplicationDto applicationDto)
        {
            if (applicationDto == null) return null;
            return new Application
            {
                ApplicationID = applicationDto.ApplicationID,
                ApplicationStatus = applicationDto.ApplicationStatus,
                ApplicationLastStatusDate = applicationDto.ApplicationLastStatusDate,
                ApplicationPaidFees = applicationDto.ApplicationPaidFees,
                CreatedAt = applicationDto.CreatedAt,
                PersonID = applicationDto.PersonID,
                ApplicationTypeID = applicationDto.ApplicationTypeID,
                CreatedByUserID = applicationDto.CreatedByUserID
            };
        }

        // === LocalLicenseApplication Mapping ===
        public static LocalLicenseApplicationDto MapLocalLicenseApplicationToLocalLicenseApplicationDto(LocalLicenseApplication localApp)
        {
            if (localApp == null) return null;
            return new LocalLicenseApplicationDto
            {
                LocalLicenseApplicationID = localApp.LocalLicenseApplicationID,
                ApplicationID = localApp.ApplicationID,
                LicenseClassID = localApp.LicenseClassID
            };
        }

        public static LocalLicenseApplication MapLocalLicenseApplicationDtoToLocalLicenseApplication(LocalLicenseApplicationDto localAppDto)
        {
            if (localAppDto == null) return null;
            return new LocalLicenseApplication
            {
                LocalLicenseApplicationID = localAppDto.LocalLicenseApplicationID,
                ApplicationID = localAppDto.ApplicationID,
                LicenseClassID = localAppDto.LicenseClassID
            };
        }

        // === DetainedLicense Mapping ===
        public static DetainedLicenseDto MapDetainedLicenseToDetainedLicenseDto(DetainedLicense detainedLicense)
        {
            if (detainedLicense == null) return null;
            return new DetainedLicenseDto
            {
                DetainedLicenseID = detainedLicense.DetainedLicenseID,
                DetainDate = detainedLicense.DetainDate,
                FineFees = detainedLicense.FineFees,
                ReleaseDate = detainedLicense.ReleaseDate,
                IsReleased = detainedLicense.IsReleased,
                LicenseID = detainedLicense.LicenseID,
                CreatedByUserID = detainedLicense.CreatedByUserID,
                ReleasedByUserID = detainedLicense.ReleasedByUserID,
                ReleaseApplicationID = detainedLicense.ReleaseApplicationID
            };
        }

        public static DetainedLicense MapDetainedLicenseDtoToDetainedLicense(DetainedLicenseDto detainedLicenseDto)
        {
            if (detainedLicenseDto == null) return null;
            return new DetainedLicense
            {
                DetainedLicenseID = detainedLicenseDto.DetainedLicenseID,
                DetainDate = detainedLicenseDto.DetainDate,
                FineFees = detainedLicenseDto.FineFees,
                ReleaseDate = detainedLicenseDto.ReleaseDate,
                IsReleased = detainedLicenseDto.IsReleased,
                LicenseID = detainedLicenseDto.LicenseID,
                CreatedByUserID = detainedLicenseDto.CreatedByUserID,
                ReleasedByUserID = detainedLicenseDto.ReleasedByUserID,
                ReleaseApplicationID = detainedLicenseDto.ReleaseApplicationID
            };
        }

        // === InternationalLicense Mapping ===
        public static InternationalLicenseDto MapInternationalLicenseToInternationalLicenseDto(InternationalLicense intLicense)
        {
            if (intLicense == null) return null;
            return new InternationalLicenseDto
            {
                InternationalLicenseID = intLicense.InternationalLicenseID,
                IssueDate = intLicense.IssueDate,
                ExpirationDate = intLicense.ExpirationDate,
                IsActive = intLicense.IsActive,
                DriverID = intLicense.DriverID,
                ApplicationID = intLicense.ApplicationID,
                IssueUsingLocalLicenseID = intLicense.IssueUsingLocalLicenseID,
                CreatedByUserID = intLicense.CreatedByUserID
            };
        }

        public static InternationalLicense MapInternationalLicenseDtoToInternationalLicense(InternationalLicenseDto intLicenseDto)
        {
            if (intLicenseDto == null) return null;
            return new InternationalLicense
            {
                InternationalLicenseID = intLicenseDto.InternationalLicenseID,
                IssueDate = intLicenseDto.IssueDate,
                ExpirationDate = intLicenseDto.ExpirationDate,
                IsActive = intLicenseDto.IsActive,
                DriverID = intLicenseDto.DriverID,
                ApplicationID = intLicenseDto.ApplicationID,
                IssueUsingLocalLicenseID = intLicenseDto.IssueUsingLocalLicenseID,
                CreatedByUserID = intLicenseDto.CreatedByUserID
            };
        }

        // === TestAppointment Mapping ===
        public static TestAppointmentDto MapTestAppointmentToTestAppointmentDto(TestAppointment testAppt)
        {
            if (testAppt == null) return null;
            return new TestAppointmentDto
            {
                TestAppointmentID = testAppt.TestAppointmentID,
                TestAppointmentDate = testAppt.TestAppointmentDate,
                TestAppointmentPaidFees = testAppt.TestAppointmentPaidFees,
                IsLocked = testAppt.IsLocked,
                LocalLicenseApplicationID = testAppt.LocalLicenseApplicationID,
                TestTypeID = testAppt.TestTypeID,
                CreatedByUserID = testAppt.CreatedByUserID,
                RetakeTestApplicationID = testAppt.RetakeTestApplicationID
            };
        }

        public static TestAppointment MapTestAppointmentDtoToTestAppointment(TestAppointmentDto testApptDto)
        {
            if (testApptDto == null) return null;
            return new TestAppointment
            {
                TestAppointmentID = testApptDto.TestAppointmentID,
                TestAppointmentDate = testApptDto.TestAppointmentDate,
                TestAppointmentPaidFees = testApptDto.TestAppointmentPaidFees,
                IsLocked = testApptDto.IsLocked,
                LocalLicenseApplicationID = testApptDto.LocalLicenseApplicationID,
                TestTypeID = testApptDto.TestTypeID,
                CreatedByUserID = testApptDto.CreatedByUserID,
                RetakeTestApplicationID = testApptDto.RetakeTestApplicationID
            };
        }

        // === Test Mapping ===
        public static TestDto MapTestToTestDto(Test test)
        {
            if (test == null) return null;
            return new TestDto
            {
                TestID = test.TestID,
                TestResult = test.TestResult,
                TestNotes = test.TestNotes,
                CreatedByUserID = test.CreatedByUserID,
                TestAppointmentID = test.TestAppointmentID
            };
        }

        public static Test MapTestDtoToTest(TestDto testDto)
        {
            if (testDto == null) return null;
            return new Test
            {
                TestID = testDto.TestID,
                TestResult = testDto.TestResult,
                TestNotes = testDto.TestNotes,
                CreatedByUserID = testDto.CreatedByUserID,
                TestAppointmentID = testDto.TestAppointmentID
            };
        }

        // === ApplicationType Mapping ===
        public static ApplicationTypeDto MapApplicationTypeToApplicationTypeDto(ApplicationType appType)
        {
            if (appType == null) return null;
            return new ApplicationTypeDto
            {
                ApplicationTypeID = appType.ApplicationTypeID,
                ApplicationTypeFees = appType.ApplicationTypeFees,
                ApplicationTypeTitle = appType.ApplicationTypeTitle
            };
        }

        public static ApplicationType MapApplicationTypeDtoToApplicationType(ApplicationTypeDto appTypeDto)
        {
            if (appTypeDto == null) return null;
            return new ApplicationType
            {
                ApplicationTypeID = appTypeDto.ApplicationTypeID,
                ApplicationTypeFees = appTypeDto.ApplicationTypeFees,
                ApplicationTypeTitle = appTypeDto.ApplicationTypeTitle
            };
        }

        // === TestType Mapping ===
        public static TestTypeDto MapTestTypeToTestTypeDto(TestType testType)
        {
            if (testType == null) return null;
            return new TestTypeDto
            {
                TestTypeID = testType.TestTypeID,
                TestTypeFees = testType.TestTypeFees,
                TestTypeTitle = testType.TestTypeTitle,
                TestTypeDescription = testType.TestTypeDescription
            };
        }

        public static TestType MapTestTypeDtoToTestType(TestTypeDto testTypeDto)
        {
            if (testTypeDto == null) return null;
            return new TestType
            {
                TestTypeID = testTypeDto.TestTypeID,
                TestTypeFees = testTypeDto.TestTypeFees,
                TestTypeTitle = testTypeDto.TestTypeTitle,
                TestTypeDescription = testTypeDto.TestTypeDescription
            };
        }

        // === LicenseClass Mapping ===
        public static LicenseClassDto MapLicenseClassToLicenseClassDto(LicenseClass licenseClass)
        {
            if (licenseClass == null) return null;
            return new LicenseClassDto
            {
                LicenseClassID = licenseClass.LicenseClassID,
                LicenseClassTitle = licenseClass.LicenseClassTitle,
                LicenseClassDescription = licenseClass.LicenseClassDescription,
                LicenseClassFees = licenseClass.LicenseClassFees,
                MinimumAllowedAge = licenseClass.MinimumAllowedAge,
                ValidityLength = licenseClass.ValidityLength
            };
        }

        public static LicenseClass MapLicenseClassDtoToLicenseClass(LicenseClassDto licenseClassDto)
        {
            if (licenseClassDto == null) return null;
            return new LicenseClass
            {
                LicenseClassID = licenseClassDto.LicenseClassID,
                LicenseClassTitle = licenseClassDto.LicenseClassTitle,
                LicenseClassDescription = licenseClassDto.LicenseClassDescription,
                LicenseClassFees = licenseClassDto.LicenseClassFees,
                MinimumAllowedAge = licenseClassDto.MinimumAllowedAge,
                ValidityLength = licenseClassDto.ValidityLength
            };
        }

        // === Country Mapping ===
        public static CountryDto MapCountryToCountryDto(Country country)
        {
            if (country == null) return null;
            return new CountryDto
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }

        public static Country MapCountryDtoToCountry(CountryDto countryDto)
        {
            if (countryDto == null) return null;
            return new Country
            {
                CountryID = countryDto.CountryID,
                CountryName = countryDto.CountryName
            };
        }
    }
}

