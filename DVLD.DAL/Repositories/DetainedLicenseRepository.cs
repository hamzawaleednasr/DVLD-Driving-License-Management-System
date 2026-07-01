using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DVLD.DAL.Repositories
{
    public class DetainedLicenseRepository : IDetainedLicense
    {
        private readonly string _connectionString;

        public DetainedLicenseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private DetainedLicense _MapReaderToDetainedLicense(SqlDataReader reader)
        {
            return new DetainedLicense
            {
                DetainedLicenseID = Convert.ToInt32(reader["DetainedLicenseID"]),
                DetainDate = Convert.ToDateTime(reader["DetainDate"]),
                FineFees = Convert.ToDouble(reader["FineFees"]),
                ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReleaseDate"]),
                IsReleased = Convert.ToBoolean(reader["IsReleased"]),
                LicenseID = Convert.ToInt32(reader["LicenseID"]),
                CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]),
                ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ReleasedByUserID"]),
                ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ReleaseApplicationID"]),
            };
        }

        private DetainedLicenseViewModel _MapReaderToDetainedLicenseViewModel(SqlDataReader reader)
        {
            return new DetainedLicenseViewModel
            {
                DetainedLicenseID = Convert.ToInt32(reader["DetainedLicenseID"]),
                DetainDate = Convert.ToDateTime(reader["DetainDate"]),
                FineFees = Convert.ToDouble(reader["FineFees"]),
                ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReleaseDate"]),
                IsReleased = Convert.ToBoolean(reader["IsReleased"]),
                LicenseID = Convert.ToInt32(reader["LicenseID"]),
                ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ReleaseApplicationID"]),
                FullName = Convert.ToString(reader["FullName"]),
                NationalNumber = Convert.ToString(reader["NationalNumber"]),
            };
        }

        private const string _BaseViewQuery = @"
            SELECT DL.DetainedLicenseID,
	               DL.DetainDate,
	               DL.FineFees,
	               DL.ReleaseDate,
	               DL.IsReleased,
	               DL.LicenseID,
	               (P.FirstName + ' ' + ISNULL(P.SecondName + ' ', '') + ISNULL(P.ThirdName + ' ', '') + P.LastName) AS FullName,
	               P.NationalNumber,
	               DL.ReleaseApplicationID
            FROM DetainedLicenses DL
            INNER JOIN Licenses L 
            ON DL.LicenseID = L.LicenseID
            INNER JOIN Applications APP
            ON L.ApplicationID = APP.ApplicationID
            INNER JOIN People P
            ON APP.PersonID = P.PersonID;
        ";

        public List<DetainedLicense> GetAll()
        {
            List<DetainedLicense> detainedLicenses = new List<DetainedLicense>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM DetainedLicenses";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detainedLicenses.Add(_MapReaderToDetainedLicense(reader));
                            } 

                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return detainedLicenses;
        }

        public List<DetainedLicenseViewModel> GetAllToView()
        {
            List<DetainedLicenseViewModel> detainedLicensesView = new List<DetainedLicenseViewModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(_BaseViewQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detainedLicensesView.Add(_MapReaderToDetainedLicenseViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return detainedLicensesView;
        }

        public DetainedLicense GetByID(int id)
        {
            DetainedLicense detainedLicense = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM DetainedLicenses WHERE DetainedLicenseID = @DetainedLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainedLicenseID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                detainedLicense = _MapReaderToDetainedLicense(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return detainedLicense;
        }

        public DetainedLicenseViewModel GetByIDToView(int id)
        {
            DetainedLicenseViewModel detainedLicenseView = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + " WHERE DL.DetainedLicenseID = @DetainedLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainedLicenseID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                detainedLicenseView = _MapReaderToDetainedLicenseViewModel(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return detainedLicenseView;
        }
        
        public int Add(DetainedLicense entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO DetainedLicenses (FineFees, LicenseID, CreatedByUserID)
                                 VALUES                       (@FineFees, @LicenseID, @CreatedByUserID);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FineFees", entity.FineFees);
                    command.Parameters.AddWithValue("@LicenseID", entity.LicenseID);
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

        public bool Update(DetainedLicense entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE DetainedLicenses 
                                 SET DetainDate           = @DetainDate, 
                                     FineFees             = @FineFees, 
                                     ReleaseDate          = @ReleaseDate, 
                                     IsReleased           = @IsReleased, 
                                     LicenseID            = @LicenseID, 
                                     CreatedByUserID      = @CreatedByUserID, 
                                     ReleasedByUserID     = @ReleasedByUserID, 
                                     ReleaseApplicationID = @ReleaseApplicationID 
                                 WHERE DetainedLicenseID  = @DetainedLicenseID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainDate", entity.DetainDate);
                    command.Parameters.AddWithValue("@FineFees", entity.FineFees);
                    command.Parameters.AddWithValue("@IsReleased", entity.IsReleased);
                    command.Parameters.AddWithValue("@LicenseID", entity.LicenseID);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                    command.Parameters.AddWithValue("@DetainedLicenseID", entity.DetainedLicenseID);
                    command.Parameters.AddWithValue("@ReleaseDate", (object)entity.ReleaseDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ReleasedByUserID", (object)entity.ReleasedByUserID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", (object)entity.ReleaseApplicationID ?? DBNull.Value);

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
                string query = "DELETE FROM DetainedLicenses WHERE DetainedLicenseID = @DetainedLicenseID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainedLicenseID", id);

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

        public DetainedLicenseViewModel GetByNationalNumberToView(string nationalNumber)
        {
            DetainedLicenseViewModel detainedLicenseView = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + " WHERE P.NationalNumber = @NationalNumber";

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
                                detainedLicenseView = _MapReaderToDetainedLicenseViewModel(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return detainedLicenseView;
        }

        public List<DetainedLicenseViewModel> GetByFullNameToView(string fullName)
        {
            List<DetainedLicenseViewModel> detainedLicensesView = new List<DetainedLicenseViewModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + @" WHERE (P.FirstName + ' ' + ISNULL(P.SecondName + ' ', '') + 
                                                         ISNULL(P.ThirdName + ' ', '') + P.LastName) LIKE @FullName";

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
                                detainedLicensesView.Add(_MapReaderToDetainedLicenseViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return detainedLicensesView;
        }

        public List<DetainedLicenseViewModel> GetByReleaseStatusToView(bool isRelease)
        {
            List<DetainedLicenseViewModel> detainedLicensesView = new List<DetainedLicenseViewModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + " WHERE DL.IsReleased = @IsReleased";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsReleased", isRelease);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detainedLicensesView.Add(_MapReaderToDetainedLicenseViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return detainedLicensesView;
        }

        public int GetNumberOfDetainedLicenses()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM DetainedLicenses";

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

