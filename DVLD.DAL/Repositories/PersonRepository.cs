using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class PersonRepository : IPerson
    {
        private readonly string _connectionString;

        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Person _MapReaderToPerson(SqlDataReader reader)
        {
            return new Person
            {
                PersonID = Convert.ToInt32(reader["PersonID"]),
                NationalNumber = reader["NationalNumber"].ToString(),
                FirstName = reader["FirstName"].ToString(),
                SecondName = reader["SecondName"] == DBNull.Value ? string.Empty : reader["SecondName"].ToString(),
                ThirdName = reader["ThirdName"] == DBNull.Value ? string.Empty : reader["ThirdName"].ToString(),
                LastName = reader["LastName"].ToString(),
                BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                Address = reader["Address"].ToString(),
                Gender = Convert.ToBoolean(reader["Gender"]),
                Phone = reader["Phone"].ToString(),
                Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString(),
                PersonalPhoto = reader["PersonalPhoto"] == DBNull.Value ? string.Empty : reader["PersonalPhoto"].ToString(),
                CountryID = Convert.ToInt32(reader["CountryID"])
            };
        }

        private PersonViewModel _MapReaderToPersonWithCountry(SqlDataReader reader)
        {
            return new PersonViewModel
            {
                PersonID = Convert.ToInt32(reader["PersonID"]),
                NationalNumber = reader["NationalNumber"].ToString(),
                FirstName = reader["FirstName"].ToString(),
                SecondName = reader["SecondName"] == DBNull.Value ? string.Empty : reader["SecondName"].ToString(),
                ThirdName = reader["ThirdName"] == DBNull.Value ? string.Empty : reader["ThirdName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Nationality = reader["Nationality"].ToString(),
                BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                Gender = Convert.ToBoolean(reader["Gender"]),
                PersonalPhoto = reader["PersonalPhoto"] == DBNull.Value ? string.Empty : reader["PersonalPhoto"].ToString(),
                Address = reader["Address"] == DBNull.Value ? string.Empty : reader["Address"].ToString(),
                Phone = reader["Phone"] == DBNull.Value ? string.Empty : reader["Phone"].ToString(),
                Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString(),
            };
        }

        public List<Person> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM People";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        List<Person> People = new List<Person>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Person person = _MapReaderToPerson(reader);

                                People.Add(person);
                            }
                        }

                        return People;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<Person> GetAll()", ex.Message);
                        return null;
                    }
                }
            }
        }

        public List<PersonViewModel> GetAllWithCountry()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        Countries.CountryName as Nationality,
                                        PersonalPhoto,
                                        Address,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID;"
                ;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        List<PersonViewModel> People = new List<PersonViewModel>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PersonViewModel person = _MapReaderToPersonWithCountry(reader);

                                People.Add(person);
                            }
                        }

                        return People;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetAllWithCountry()", ex.Message);
                        return null;
                    }
                }
            }
        }

        public Person GetByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM People WHERE PersonID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return _MapReaderToPerson(reader);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public Person GetByID(int id)", ex.Message);
                        return null;
                    }
                }
            }
        }

        public PersonViewModel GetByIDWithCountry(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE PersonID = @id;"
                ;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    //try
                    //{
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return _MapReaderToPersonWithCountry(reader);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    //}
                    //catch
                    //{
                    //    return null;
                    //}
                }
            }
        }

        public int Add(Person entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO People (NationalNumber, FirstName, SecondName, ThirdName, LastName, BirthDate, Address, Gender, Phone, Email, PersonalPhoto, CountryID)
                                 VALUES             (@NationalNumber, @FirstName, @SecondName, @ThirdName, @LastName, @BirthDate, @Address, @Gender, @Phone, @Email, @PersonalPhoto, @CountryID);
                                 SELECT SCOPE_IDENTITY();"
                ;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", entity.NationalNumber);
                    command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    command.Parameters.AddWithValue("@SecondName", (object)entity.SecondName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ThirdName", (object)entity.ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", entity.LastName);
                    command.Parameters.AddWithValue("@BirthDate", entity.BirthDate);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Gender", entity.Gender);
                    command.Parameters.AddWithValue("@Phone", entity.Phone);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(entity.Email) ? DBNull.Value : (object)entity.Email);
                    command.Parameters.AddWithValue("@PersonalPhoto", (object)entity.PersonalPhoto ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CountryID", entity.CountryID);

                    try
                    {
                        connection.Open();

                        int newID = Convert.ToInt32(command.ExecuteScalar());

                        return newID;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public int Add(Person entity)", ex.Message);
                        return -1;
                    }
                }
            }
        }

        public bool Update(Person entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE People 
                         SET NationalNumber = @NationalNumber, 
                             FirstName      = @FirstName, 
                             SecondName     = @SecondName, 
                             ThirdName      = @ThirdName, 
                             LastName       = @LastName, 
                             BirthDate      = @BirthDate, 
                             Address        = @Address, 
                             Gender         = @Gender, 
                             Phone          = @Phone, 
                             Email          = @Email, 
                             PersonalPhoto  = @PersonalPhoto, 
                             CountryID      = @CountryID
                         WHERE PersonID     = @PersonID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", entity.NationalNumber);
                    command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    command.Parameters.AddWithValue("@SecondName", entity.SecondName);
                    command.Parameters.AddWithValue("@ThirdName", entity.ThirdName);
                    command.Parameters.AddWithValue("@LastName", entity.LastName);
                    command.Parameters.AddWithValue("@BirthDate", entity.BirthDate);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Gender", entity.Gender);
                    command.Parameters.AddWithValue("@Phone", entity.Phone);
                    command.Parameters.AddWithValue("@Email", entity.Email);
                    command.Parameters.AddWithValue("@PersonalPhoto", entity.PersonalPhoto);
                    command.Parameters.AddWithValue("@CountryID", entity.CountryID);
                    command.Parameters.AddWithValue("@PersonID", entity.PersonID); 

                    try
                    {
                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        return (rowsAffected > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public bool Update(Person entity)", ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM People 
                                 WHERE PersonID = @PersonID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", id);

                    try
                    {
                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        return (rowsAffected > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public bool Delete(int id)", ex.Message);
                        return false;
                    }
            }
            }
        }

        public PersonViewModel GetByNationalNumberWithCountry(string nationalNumber)
        {
            PersonViewModel person = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE NationalNumber LIKE @NationalNumber;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", '%' + nationalNumber + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                person = _MapReaderToPersonWithCountry(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public PersonViewModel GetByNationalNumberWithCountry(string nationalNumber)", ex.Message);
                        return null;
                    }
                }
            }
            return person;
        }

        public List<PersonViewModel> GetByFirstNameWithCountry(string firstName)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE FirstName LIKE @FirstName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", '%' + firstName + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetByFirstNameWithCountry(string firstName)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public List<PersonViewModel> GetBySecondNameWithCountry(string secondName)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE SecondName LIKE @SecondName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SecondName", '%' + secondName + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetBySecondNameWithCountry(string secondName)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public List<PersonViewModel> GetByThirdNameWithCountry(string thirdName)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE ThirdName LIKE @ThirdName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ThirdName", '%' + thirdName + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetByThirdNameWithCountry(string thirdName)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public List<PersonViewModel> GetByLastNameWithCountry(string lastName)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE LastName LIKE @LastName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LastName", '%' + lastName + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetByLastNameWithCountry(string lastName)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public List<PersonViewModel> GetByNationalityWithCountry(string nationality)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE Countries.CountryName LIKE @Nationality;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nationality", '%' + nationality + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetByNationalityWithCountry(string nationality)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public List<PersonViewModel> GetByGenderWithCountry(bool gender)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE Gender = @Gender;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Gender", gender);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetByGenderWithCountry(bool gender)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public List<PersonViewModel> GetByPhoneWithCountry(string phone)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE Phone LIKE @Phone;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Phone", '%' + phone + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetByPhoneWithCountry(string phone)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public List<PersonViewModel> GetByEmailWithCountry(string email)
        {
            List<PersonViewModel> peopleList = new List<PersonViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT PersonID,
                                        NationalNumber,
                                        FirstName,
                                        SecondName,
                                        ThirdName,
                                        LastName,
                                        Gender,
                                        BirthDate,
                                        PersonalPhoto,
                                        Address,
                                        Countries.CountryName as Nationality,
                                        Phone,
                                        Email
                                 FROM People
                                 LEFT JOIN Countries
                                 ON People.CountryID = Countries.CountryID
                                 WHERE Email LIKE @Email;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", '%' + email + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(_MapReaderToPersonWithCountry(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public List<PersonViewModel> GetByEmailWithCountry(string email)", ex.Message);
                        return null;
                    }
                }
            }
            return peopleList;
        }

        public int GetNumberOfPeople()
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM People;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        count = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("PersonRepository", "public int GetNumberOfPeople()", ex.Message);
                        return -1;
                    }
                }
            }
            return count;
        }
    }
}

