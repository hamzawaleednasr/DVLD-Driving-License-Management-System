using System;
using System.Collections.Generic;
using System.Data.SqlClient; 
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class TestRepository : ITest
    {
        private readonly string _connectionString;

        public TestRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Test _MapReaderToTest(SqlDataReader reader)
        {
            return new Test
            {
                TestID = Convert.ToInt32(reader["TestID"]),
                TestResult = Convert.ToBoolean(reader["TestResult"]), 
                CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]),
                TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]),
                TestNotes = reader["TestNotes"] == DBNull.Value ? string.Empty : reader["TestNotes"].ToString()
            };
        }

        public List<Test> GetAll()
        {
            List<Test> testsList = new List<Test>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Tests";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                testsList.Add(_MapReaderToTest(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return testsList;
        }

        public Test GetByID(int id)
        {
            Test test = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Tests WHERE TestID = @TestID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                test = _MapReaderToTest(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return test;
        }

        public int Add(Test entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Tests (TestResult, TestNotes, CreatedByUserID, TestAppointmentID)
                                 VALUES            (@TestResult, @TestNotes, @CreatedByUserID, @TestAppointmentID);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestResult", entity.TestResult);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                    command.Parameters.AddWithValue("@TestAppointmentID", entity.TestAppointmentID);

                    // التحقق من وجود ملاحظات لمنع إدخال نصوص فارغة بدلاً من NULL في الـ DB
                    command.Parameters.AddWithValue("@TestNotes", string.IsNullOrWhiteSpace(entity.TestNotes) ? DBNull.Value : (object)entity.TestNotes);

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

        public bool Update(Test entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Tests 
                                 SET TestResult        = @TestResult, 
                                     TestNotes         = @TestNotes, 
                                     CreatedByUserID   = @CreatedByUserID, 
                                     TestAppointmentID = @TestAppointmentID 
                                 WHERE TestID          = @TestID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestResult", entity.TestResult);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                    command.Parameters.AddWithValue("@TestAppointmentID", entity.TestAppointmentID);
                    command.Parameters.AddWithValue("@TestID", entity.TestID);

                    command.Parameters.AddWithValue("@TestNotes", string.IsNullOrWhiteSpace(entity.TestNotes) ? DBNull.Value : (object)entity.TestNotes);

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
                string query = "DELETE FROM Tests WHERE TestID = @TestID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", id);

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
    }
}