using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DVLD.DAL.Repositories
{
    public class DriverRepository : IDriver
    {
        private readonly string _connectionString;

        public DriverRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Driver _MapReaderToDriver(SqlDataReader reader)
        {
            return new Driver
            {
                DriverID = Convert.ToInt32(reader["DriverID"]),
                PersonID = Convert.ToInt32(reader["PersonID"]),
                CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }

        private DriverViewModel _MapReaderToDriverViewModel(SqlDataReader reader)
        {
            return new DriverViewModel
            {
                DriverID = Convert.ToInt32(reader["DriverID"]),
                PersonID = Convert.ToInt32(reader["PersonID"]),
                NationalNumber = reader["NationalNumber"].ToString(),
                FullName = reader["FullName"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                DriverLicenses = Convert.ToInt32(reader["ActiveLicenses"])
            };
        }

        private const string _baseViewQuery = @"
            SELECT Drivers.DriverID, Drivers.PersonID, Drivers.CreatedAt, People.NationalNumber,
                   (People.FirstName + ' ' + ISNULL(People.SecondName + ' ', '') + ISNULL(People.ThirdName + ' ', '') + People.LastName) AS FullName,
                   (SELECT COUNT(*) FROM Licenses WHERE Licenses.DriverID = Drivers.DriverID) AS ActiveLicenses
            FROM Drivers
            INNER JOIN People ON Drivers.PersonID = People.PersonID";

        public List<Driver> GetAll()
        {
            List<Driver> driversList = new List<Driver>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Drivers";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                driversList.Add(_MapReaderToDriver(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driversList;
        }

        public List<DriverViewModel> GetAllToView()
        {
            List<DriverViewModel> driversViewList = new List<DriverViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(_baseViewQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                driversViewList.Add(_MapReaderToDriverViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driversViewList;
        }
        
        public Driver GetByID(int id)
        {
            Driver driver = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                driver = _MapReaderToDriver(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driver;
        }

        public Driver GetByPersonID(int personID)
        {
            Driver driver = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                driver = _MapReaderToDriver(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driver;
        }
        
        public DriverViewModel GetByIDToView(int id)
        {
            DriverViewModel driverView = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE Drivers.DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                driverView = _MapReaderToDriverViewModel(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driverView;
        }
        
        public int Add(Driver entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID)
                                 VALUES              (@PersonID, @CreatedByUserID);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", entity.PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);

                    try
                    {
                        connection.Open();
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
        }
        
        public bool Update(Driver entity)
        {
            throw new Exception("You cannot update Driver!");
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Drivers WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", id);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public DriverViewModel GetByPersonIDToView(int personID)
        {
            DriverViewModel driverView = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE Drivers.PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                driverView = _MapReaderToDriverViewModel(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driverView;
        }

        public DriverViewModel GetByNationalNumberToView(string nationalNumber)
        {
            DriverViewModel driverView = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE People.NationalNumber = @NationalNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", nationalNumber);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                driverView = _MapReaderToDriverViewModel(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driverView;
        }

        public List<DriverViewModel> GetByFullNameToView(string fullName)
        {
            List<DriverViewModel> driversViewList = new List<DriverViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + @" WHERE (People.FirstName + ' ' + ISNULL(People.SecondName + ' ', '') + 
                                                         ISNULL(People.ThirdName + ' ', '') + People.LastName) LIKE @FullName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", "%" + fullName + "%");

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                driversViewList.Add(_MapReaderToDriverViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return driversViewList;
        }

        public int GetNumberOfDrivers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM Drivers";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
        }
    }
}

