using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class InternationalLicenseRepository : IInternationalLicense
    {
        private readonly string _connectionString;

        public InternationalLicenseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private InternationalLicense _MapReaderToInternationalLicense(SqlDataReader reader)
        {
            return new InternationalLicense
            {
                InternationalLicenseID = Convert.ToInt32(reader["InternationalLicenseID"]),
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                DriverID = Convert.ToInt32(reader["DriverID"]),
                ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                IssueUsingLocalLicenseID = Convert.ToInt32(reader["IssueUsingLocalLicenseID"]),
                CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"])
            };
        }

        private InternationalLicenseHistory _MapReaderToInternationalLicenseHistory(SqlDataReader reader)
        {
            return new InternationalLicenseHistory
            {
                InternationalLicenseID = Convert.ToInt32(reader["InternationalLicenseID"]),
                ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                LocalLicenseID = Convert.ToInt32(reader["LocalLicenseID"]), 
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]),
                IsActive = Convert.ToBoolean(reader["IsActive"])
            };
        }

        private InternationalLicenseInfo _MapReaderToInternationalLicenseInfo(SqlDataReader reader)
        {
            return new InternationalLicenseInfo
            {
                InternationalLicenseID = Convert.ToInt32(reader["InternationalLicenseID"]),
                ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                LocalLicenseID = Convert.ToInt32(reader["LocalLicenseID"]),
                DriverID = Convert.ToInt32(reader["DriverID"]),
                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                FullName = reader["FullName"].ToString(),
                NationalNumber = reader["NationalNumber"].ToString(),
                BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                Gender = Convert.ToBoolean(reader["Gender"]),
                PersonalPhoto = Convert.ToString(reader["PersonalPhoto"])
            };
        }

        public List<InternationalLicense> GetAll()
        {
            List<InternationalLicense> list = new List<InternationalLicense>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM InternationalLicenses";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(_MapReaderToInternationalLicense(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return list;
        }

        public InternationalLicense GetByID(int id)
        {
            InternationalLicense entity = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                entity = _MapReaderToInternationalLicense(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return entity;
        }

        public int Add(InternationalLicense entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO InternationalLicenses (IssueDate, ExpirationDate, DriverID, ApplicationID, IssueUsingLocalLicenseID, CreatedByUserID)
                    SELECT 
                        SYSDATETIME(), 
                        DATEADD(year, LC.ValidityLength, SYSDATETIME()),
                        @DriverID, 
                        @ApplicationID, 
                        @IssueUsingLocalLicenseID, 
                        @CreatedByUserID
                    FROM Licenses L 
                    INNER JOIN LocalLicenseApplications LLA ON L.ApplicationID = LLA.ApplicationID
                    INNER JOIN LicenseClasses LC ON LLA.LicenseClassID = LC.LicenseClassID
                    WHERE L.LicenseID = @IssueUsingLocalLicenseID;
                    
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", entity.DriverID);
                    command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                    command.Parameters.AddWithValue("@IssueUsingLocalLicenseID", entity.IssueUsingLocalLicenseID);
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

        public bool Update(InternationalLicense entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE InternationalLicenses 
                                 SET IsActive = @IsActive
                                 WHERE InternationalLicenseID = @InternationalLicenseID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    command.Parameters.AddWithValue("@InternationalLicenseID", entity.InternationalLicenseID);

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

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", id);

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

        public List<InternationalLicenseHistory> GetByLicenseIDToHistory(int id)
        {
            List<InternationalLicenseHistory> historyList = new List<InternationalLicenseHistory>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT InternationalLicenseID, ApplicationID, IssueUsingLocalLicenseID AS LocalLicenseID, IssueDate, ExpirationDate, IsActive 
                                 FROM InternationalLicenses 
                                 WHERE IssueUsingLocalLicenseID = @LocalLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalLicenseID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                historyList.Add(_MapReaderToInternationalLicenseHistory(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return historyList;
        }

        public InternationalLicenseInfo GetByIDToShowInfo(int id)
        {
            InternationalLicenseInfo info = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT IL.InternationalLicenseID, IL.ApplicationID, IL.IssueUsingLocalLicenseID AS LocalLicenseID, IL.DriverID, 
                                        IL.IssueDate, IL.ExpirationDate, IL.IsActive, P.NationalNumber, P.BirthDate, P.Gender, P.PersonalPhoto
                                        (P.FirstName + ' ' + ISNULL(P.SecondName + ' ', '') + ISNULL(P.ThirdName + ' ', '') + P.LastName) AS FullName
                                 FROM InternationalLicenses IL
                                 INNER JOIN Drivers D ON IL.DriverID = D.DriverID
                                 INNER JOIN People P ON D.PersonID = P.PersonID
                                 WHERE IL.InternationalLicenseID = @InternationalLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                info = _MapReaderToInternationalLicenseInfo(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return info;
        }

        public int GetNumberOfLocalInternationalLicenses()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM InternationalLicenses";

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

        public List<InternationalLicense> GetByDriverID(int driverID)
        {
            List<InternationalLicense> list = new List<InternationalLicense>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM InternationalLicenses WHERE DriverID = @DriverID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(_MapReaderToInternationalLicense(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return list;
        }
    }
}
