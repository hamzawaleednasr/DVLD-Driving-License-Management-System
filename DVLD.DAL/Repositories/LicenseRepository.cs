using System;
using System.Collections.Generic;
using System.Data.SqlClient; 
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using DVLD.Core.DTOs;

namespace DVLD.DAL.Repositories
{
    public class LicenseRepository : ILicense
    {
        private readonly string _connectionString;

        public LicenseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private License _MapReaderToLicense(SqlDataReader reader)
        {
            return new License
            {
                LicenseID = Convert.ToInt32(reader["LicenseID"]),
                LicenseIssueDate = Convert.ToDateTime(reader["LicenseIssueDate"]),
                LicenseExpirationDate = Convert.ToDateTime(reader["LicenseExpirationDate"]),
                LicensePaidFees = Convert.ToDouble(reader["LicensePaidFees"]),
                LicenseIssueReason = Convert.ToByte(reader["LicenseIssueReason"]), // TINYINT to byte
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                DriverID = Convert.ToInt32(reader["DriverID"]),
                ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]),
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : reader["Notes"].ToString()
            };
        }

        public List<License> GetAll()
        {
            List<License> licensesList = new List<License>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Licenses";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                licensesList.Add(_MapReaderToLicense(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public List<License> GetAll()", ex.Message);
                        return null;
                    }
                }
            }
            return licensesList;
        }

        public License GetByID(int id)
        {
            License license = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                license = _MapReaderToLicense(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public License GetByID(int id)", ex.Message);
                        return null;
                    }
                }
            }
            return license;
        }

        public int Add(License entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Licenses (LicenseExpirationDate, LicensePaidFees, LicenseIssueReason, Notes, DriverID, ApplicationID, LicenseClassID)
                    SELECT 
                        DATEADD(year, ValidityLength, SYSDATETIME()),
                        @LicensePaidFees, 
                        @LicenseIssueReason, 
                        @Notes, 
                        @DriverID, 
                        @ApplicationID, 
                        @LicenseClassID
                    FROM LicenseClasses
                    WHERE LicenseClassID = @LicenseClassID; 

                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicensePaidFees", entity.LicensePaidFees);
                    command.Parameters.AddWithValue("@LicenseIssueReason", entity.LicenseIssueReason);
                    command.Parameters.AddWithValue("@DriverID", entity.DriverID);
                    command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClassID);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(entity.Notes) ? DBNull.Value : (object)entity.Notes);

                    try
                    {
                        connection.Open();

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public int Add(License entity)", ex.Message);
                        return -1;
                    }
                }
            }
        }

        public bool Update(License entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Licenses 
                                 SET LicenseIssueDate      = @LicenseIssueDate, 
                                     LicenseExpirationDate = @LicenseExpirationDate, 
                                     LicensePaidFees       = @LicensePaidFees, 
                                     LicenseIssueReason    = @LicenseIssueReason, 
                                     IsActive              = @IsActive, 
                                     Notes                 = @Notes, 
                                     DriverID              = @DriverID, 
                                     ApplicationID         = @ApplicationID, 
                                     LicenseClassID        = @LicenseClassID 
                                 WHERE LicenseID           = @LicenseID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseIssueDate", entity.LicenseIssueDate);
                    command.Parameters.AddWithValue("@LicenseExpirationDate", entity.LicenseExpirationDate);
                    command.Parameters.AddWithValue("@LicensePaidFees", entity.LicensePaidFees);
                    command.Parameters.AddWithValue("@LicenseIssueReason", entity.LicenseIssueReason);
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    command.Parameters.AddWithValue("@DriverID", entity.DriverID);
                    command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClassID);
                    command.Parameters.AddWithValue("@LicenseID", entity.LicenseID);

                    command.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(entity.Notes) ? DBNull.Value : (object)entity.Notes);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public bool Update(License entity)", ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Licenses WHERE LicenseID = @LicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", id);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public bool Delete(int id)", ex.Message);
                        return false;
                    }
                }
            }
        }

        public LicenseInfoViewModel GetLicenseInfoByID(int licenseID)
        {
            LicenseInfoViewModel licenseInfo = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                        SELECT 
                            Licenses.LicenseID, 
                            LicenseClasses.LicenseClassTitle, 
                            (People.FirstName + ' ' + ISNULL(People.SecondName, '') + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName) AS FullName, 
                            People.NationalNumber, 
                            People.Gender, 
                            People.BirthDate, 
                            Licenses.DriverID, 
                            Licenses.LicenseIssueDate, 
                            Licenses.LicenseExpirationDate, 
                            Licenses.LicenseIssueReason, 
                            Licenses.IsActive, 
                            Licenses.Notes, 
                            People.PersonalPhoto,
                            CAST(CASE WHEN EXISTS (
                                SELECT 1 FROM DetainedLicenses 
                                WHERE DetainedLicenses.LicenseID = Licenses.LicenseID AND DetainedLicenses.IsReleased = 0
                            ) THEN 1 ELSE 0 END AS BIT) AS IsDetained
                        FROM Licenses
                        INNER JOIN LicenseClasses ON Licenses.LicenseClassID = LicenseClasses.LicenseClassID
                        INNER JOIN Applications ON Licenses.ApplicationID = Applications.ApplicationID
                        INNER JOIN People ON Applications.PersonID = People.PersonID
                        WHERE Licenses.LicenseID = @LicenseID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                licenseInfo = new LicenseInfoViewModel
                                {
                                    LicenseID = Convert.ToInt32(reader["LicenseID"]),
                                    ClassName = reader["LicenseClassTitle"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    NationalNo = reader["NationalNumber"].ToString(),
                                    Gender = Convert.ToByte(reader["Gender"]),
                                    DateOfBirth = Convert.ToDateTime(reader["BirthDate"]),
                                    DriverID = Convert.ToInt32(reader["DriverID"]),
                                    LicenseIssueDate = Convert.ToDateTime(reader["LicenseIssueDate"]),
                                    LicenseExpirationDate = Convert.ToDateTime(reader["LicenseExpirationDate"]),
                                    LicenseIssueReason = Convert.ToByte(reader["LicenseIssueReason"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    Notes = reader["Notes"] == DBNull.Value ? "No Notes" : reader["Notes"].ToString(),
                                    ImagePath = reader["PersonalPhoto"] == DBNull.Value ? null : reader["PersonalPhoto"].ToString(),
                                    IsDetained = Convert.ToBoolean(reader["IsDetained"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public LicenseInfoViewModel GetLicenseInfoByID(int licenseID)", ex.Message);
                        return null;
                    }
            }
            }
            return licenseInfo;
        }

        public int GetActiveLicenseIDByApplicationID(int applicationID)
        {
            int licenseID = -1;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT LicenseID FROM Licenses WHERE ApplicationID = @ApplicationID AND IsActive = 1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            licenseID = id;
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public int GetActiveLicenseIDByApplicationID(int applicationID)", ex.Message);
                        return -1;
                    }
                }
            }
            return licenseID;
        }

        public List<LicenseHistoryViewModel> GetLicenseHistoryByDriverID(int driverID)
        {
            List<LicenseHistoryViewModel> licenses = new List<LicenseHistoryViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT L.LicenseID,
	                   APP.ApplicationID,
	                   LC.LicenseClassTitle AS 'Class Name',
	                   L.LicenseIssueDate AS 'Issue Date',
	                   L.LicenseExpirationDate AS 'Expiration Date',
	                   L.IsActive
                    FROM Licenses L
                    INNER JOIN Applications APP
                    ON L.ApplicationID = APP.ApplicationID
                    INNER JOIN LicenseClasses LC
                    ON L.LicenseClassID = LC.LicenseClassID
                    WHERE L.DriverID = @DriverID;
                ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);

                    try
                    {
                        connection.Open();

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            licenses.Add(
                                new LicenseHistoryViewModel
                                {
                                    LicenseID = Convert.ToInt32(reader["LicenseID"]),
                                    ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                                    ClassName = Convert.ToString(reader["Class Name"]),
                                    IssueDate = Convert.ToDateTime(reader["Issue Date"]),
                                    ExpirationDate = Convert.ToDateTime(reader["Expiration Date"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                }
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseRepository", "public List<LicenseHistoryViewModel> GetLicenseHistoryByDriverID(int driverID)", ex.Message);
                        return null;
                    }
                }
            }
            return licenses;
        }
    }
}