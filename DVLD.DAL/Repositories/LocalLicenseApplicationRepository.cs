using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DVLD.DAL.Repositories
{
    public class LocalLicenseApplicationRepository : ILocalLicenseApplication
    {
        private readonly string _connectionString;

        public LocalLicenseApplicationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private LocalLicenseApplication _MapReaderToLocalLicenseApplication(SqlDataReader reader)
        {
            return new LocalLicenseApplication
            {
                LocalLicenseApplicationID = Convert.ToInt32(reader["LocalLicenseApplicationID"]),
                ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                LicenseClassID = Convert.ToInt32(reader["LicenseClassID"])
            };
        }

        private LocalLicenseApplicationViewModel _MapReaderToLocalLicenseApplicationViewModel(SqlDataReader reader)
        {
            return new LocalLicenseApplicationViewModel
            {
                LocalLicenseApplicationID = Convert.ToInt32(reader["LocalLicenseApplicationID"]),
                DrivingClass = reader["Driving Class"].ToString(),
                NationalNumber = reader["NationalNumber"].ToString(),
                FullName = reader["FullName"].ToString(),
                Status = reader["ApplicationStatus"].ToString(),
                ApplicationDate = Convert.ToDateTime(reader["CreatedAt"]),
                PassedTests = Convert.ToInt32(reader["Passed Tests"])
            };
        }

        private const string _BaseViewQuery = @"
            SELECT LLA.LocalLicenseApplicationID,
	               LC.LicenseClassTitle AS [Driving Class],
	               P.NationalNumber,
	               (P.FirstName + ' ' + ISNULL(P.SecondName + ' ', '') + ISNULL(P.ThirdName + ' ', '') + P.LastName) AS FullName,
	               APP.CreatedAt,
	               (SELECT COUNT(*) FROM TestAppointments TA INNER JOIN Tests T ON TA.TestAppointmentID = T.TestAppointmentID WHERE TA.LocalLicenseApplicationID = LLA.LocalLicenseApplicationID AND T.TestResult = 1) AS [Passed Tests],
	               APP.ApplicationStatus
            FROM LocalLicenseApplications LLA
            INNER JOIN LicenseClasses LC
            ON LLA.LicenseClassID = LC.LicenseClassID
            INNER JOIN Applications APP
            ON LLA.ApplicationID = APP.ApplicationID
            INNER JOIN People P
            ON APP.PersonID = P.PersonID";

        public List<LocalLicenseApplication> GetAll()
        {
            List<LocalLicenseApplication> list = new List<LocalLicenseApplication>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM LocalLicenseApplications";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(_MapReaderToLocalLicenseApplication(reader));
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

        public List<LocalLicenseApplicationViewModel> GetAllToView()
        {
            List<LocalLicenseApplicationViewModel> viewList = new List<LocalLicenseApplicationViewModel>();
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
                                viewList.Add(_MapReaderToLocalLicenseApplicationViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return viewList;
        }

        public LocalLicenseApplication GetByID(int id)
        {
            LocalLicenseApplication entity = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM LocalLicenseApplications WHERE LocalLicenseApplicationID = @LocalLicenseApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalLicenseApplicationID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                entity = _MapReaderToLocalLicenseApplication(reader);
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

        public LocalLicenseApplicationViewModel GetByIDToView(int id)
        {
            LocalLicenseApplicationViewModel viewModel = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + " WHERE LLA.LocalLicenseApplicationID = @LocalLicenseApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalLicenseApplicationID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                viewModel = _MapReaderToLocalLicenseApplicationViewModel(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return viewModel;
        }

        public int Add(LocalLicenseApplication entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO LocalLicenseApplications (ApplicationID, LicenseClassID)
                                 VALUES                               (@ApplicationID, @LicenseClassID);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClassID);

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

        public bool Update(LocalLicenseApplication entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE LocalLicenseApplications 
                                 SET ApplicationID = @ApplicationID, 
                                     LicenseClassID = @LicenseClassID 
                                 WHERE LocalLicenseApplicationID = @LocalLicenseApplicationID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", entity.LicenseClassID);
                    command.Parameters.AddWithValue("@LocalLicenseApplicationID", entity.LocalLicenseApplicationID);

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
                string query = "DELETE FROM LocalLicenseApplications WHERE LocalLicenseApplicationID = @LocalLicenseApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalLicenseApplicationID", id);

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

        public List<LocalLicenseApplicationViewModel> GetByNationalNumberToView(string nationalNumber)
        {
            List<LocalLicenseApplicationViewModel> localLicenseApplications = new List<LocalLicenseApplicationViewModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + " WHERE P.NationalNumber LIKE @NationalNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", '%' + nationalNumber + '%');

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                localLicenseApplications.Add(_MapReaderToLocalLicenseApplicationViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return localLicenseApplications;
        }

        public List<LocalLicenseApplicationViewModel> GetByFullNameToView(string fullName)
        {
            List<LocalLicenseApplicationViewModel> viewList = new List<LocalLicenseApplicationViewModel>();
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
                                viewList.Add(_MapReaderToLocalLicenseApplicationViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return viewList;
        }

        public List<LocalLicenseApplicationViewModel> GetByStatusToView(string status)
        {
            List<LocalLicenseApplicationViewModel> viewList = new List<LocalLicenseApplicationViewModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + " WHERE APP.ApplicationStatus = @Status";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                viewList.Add(_MapReaderToLocalLicenseApplicationViewModel(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return viewList;
        }

        public int GetNumberOfLocalLicenseApplication()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM LocalLicenseApplications";

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

