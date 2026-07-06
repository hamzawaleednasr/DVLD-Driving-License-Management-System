using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class TestAppointmentRepository : ITestAppointment
    {
        private readonly string _connectionString;

        public TestAppointmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private TestAppointment _MapReaderToTestAppointment(SqlDataReader reader)
        {
            return new TestAppointment
            {
                TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]),
                TestAppointmentDate = Convert.ToDateTime(reader["TestAppointmentDate"]),
                TestAppointmentPaidFees = Convert.ToDouble(reader["TestAppointmentPaidFees"]),
                IsLocked = Convert.ToBoolean(reader["IsLocked"]),
                LocalLicenseApplicationID = Convert.ToInt32(reader["LocalLicenseApplicationID"]),
                TestTypeID = Convert.ToInt32(reader["TestTypeID"]),
                CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]),
                RetakeTestApplicationID = reader["RetakeTestApplicationID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["RetakeTestApplicationID"])
            };
        }

        private TestAppointmentViewModel _MapReaderToTestAppointmentViewModel(SqlDataReader reader)
        {
            return new TestAppointmentViewModel
            {
                LocalLicenseApplicationID = Convert.ToInt32(reader["LocalLicenseApplicationID"]),
                TestAppointmentPaidFees = Convert.ToDouble(reader["TestAppointmentPaidFees"]),
                LicenseClassTitle = reader["LicenseClassTitle"].ToString(),
                FullName = reader["FullName"].ToString(),
                TestAppointmentDate = Convert.ToDateTime(reader["TestAppointmentDate"])
            };
        }

        private const string _BaseViewQuery = @"
            SELECT TA.LocalLicenseApplicationID,
                   TA.TestAppointmentPaidFees,
                   LC.LicenseClassTitle,
                   (P.FirstName + ' ' + ISNULL(P.SecondName + ' ', '') + ISNULL(P.ThirdName + ' ', '') + P.LastName) AS FullName,
                   TA.TestAppointmentDate
            FROM TestAppointments TA
            INNER JOIN LocalLicenseApplications LLA ON TA.LocalLicenseApplicationID = LLA.LocalLicenseApplicationID
            INNER JOIN LicenseClasses LC ON LLA.LicenseClassID = LC.LicenseClassID
            INNER JOIN Applications APP ON LLA.ApplicationID = APP.ApplicationID
            INNER JOIN People P ON APP.PersonID = P.PersonID";

        public List<TestAppointment> GetAll()
        {
            List<TestAppointment> list = new List<TestAppointment>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TestAppointments";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(_MapReaderToTestAppointment(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public List<TestAppointment> GetAll()", ex.Message);
                        return null;
                    }
                }
            }
            return list;
        }

        public TestAppointment GetByID(int id)
        {
            TestAppointment entity = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                entity = _MapReaderToTestAppointment(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public TestAppointment GetByID(int id)", ex.Message);
                        return null;
                    }
                }
            }
            return entity;
        }

        public TestAppointmentViewModel GetByIDToView(int id)
        {
            TestAppointmentViewModel viewModel = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _BaseViewQuery + " WHERE TA.TestAppointmentID = @TestAppointmentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                viewModel = _MapReaderToTestAppointmentViewModel(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public TestAppointmentViewModel GetByIDToView(int id)", ex.Message);
                        return null;
                    }
                }
            }
            return viewModel;
        }

        public List<TestAppointmentDto> GetBy_TestTypeID_LocalLicenseApplicationID(int TestTypeID, int LocalLicenseApplicationID)
        {
            List<TestAppointmentDto> testAppointments = new List<TestAppointmentDto>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TestAppointmentID, TestAppointmentDate, TestAppointmentPaidFees, IsLocked FROM TestAppointments 
                                 WHERE TestTypeID = @TestTypeID AND LocalLicenseApplicationID = @LocalLicenseApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

                    try
                    {
                        connection.Open();

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            testAppointments.Add(
                                new TestAppointmentDto
                                {
                                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]),
                                    TestAppointmentDate = Convert.ToDateTime(reader["TestAppointmentDate"]),
                                    TestAppointmentPaidFees = Convert.ToInt32(reader["TestAppointmentPaidFees"]),
                                    IsLocked = Convert.ToBoolean(reader["IsLocked"]),
                                }
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public List<TestAppointmentDto> GetBy_TestTypeID_LocalLicenseApplicationID(int TestTypeID, int LocalLicenseApplicationID)", ex.Message);
                        return null;
                    }
                }
            }
            return testAppointments;
        }

        public int Add(TestAppointment entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO TestAppointments (TestAppointmentDate, TestAppointmentPaidFees, LocalLicenseApplicationID, TestTypeID, CreatedByUserID, RetakeTestApplicationID)
                    SELECT 
                        @TestAppointmentDate,
                        TestTypeFees,
                        @LocalLicenseApplicationID,
                        @TestTypeID,
                        @CreatedByUserID,
                        @RetakeTestApplicationID
                    FROM TestTypes
                    WHERE TestTypeID = @TestTypeID;

                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentDate", entity.TestAppointmentDate);
                    command.Parameters.AddWithValue("@LocalLicenseApplicationID", entity.LocalLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", entity.TestTypeID);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                    command.Parameters.AddWithValue("@RetakeTestApplicationID", (object)entity.RetakeTestApplicationID ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public int Add(TestAppointment entity)", ex.Message);
                        return -1;
                    }
                }
            }
        }

        public bool Update(TestAppointment entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE TestAppointments 
                                 SET TestAppointmentDate       = @TestAppointmentDate, 
                                     TestAppointmentPaidFees   = @TestAppointmentPaidFees, 
                                     IsLocked                  = @IsLocked, 
                                     LocalLicenseApplicationID = @LocalLicenseApplicationID, 
                                     TestTypeID                = @TestTypeID, 
                                     CreatedByUserID           = @CreatedByUserID, 
                                     RetakeTestApplicationID   = @RetakeTestApplicationID 
                                 WHERE TestAppointmentID       = @TestAppointmentID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentDate", entity.TestAppointmentDate);
                    command.Parameters.AddWithValue("@TestAppointmentPaidFees", entity.TestAppointmentPaidFees);
                    command.Parameters.AddWithValue("@IsLocked", entity.IsLocked);
                    command.Parameters.AddWithValue("@LocalLicenseApplicationID", entity.LocalLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", entity.TestTypeID);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                    command.Parameters.AddWithValue("@TestAppointmentID", entity.TestAppointmentID);

                    command.Parameters.AddWithValue("@RetakeTestApplicationID", (object)entity.RetakeTestApplicationID ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public bool Update(TestAppointment entity)", ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", id);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public bool Delete(int id)", ex.Message);
                        return false;
                    }
                }
            }
        }

        public int GetNumberOfTestAppointments()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM TestAppointments";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestAppointmentRepository", "public int GetNumberOfTestAppointments()", ex.Message);
                        return -1;
                    }
                }
            }
        }
    }
}
