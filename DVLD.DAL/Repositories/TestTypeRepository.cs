using DVLD.DAL.Entities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class TestTypeRepository : ITestType
    {
        private readonly string _connectionString;

        public TestTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private TestType _MapReaderToTestType(SqlDataReader reader)
        {
            return new TestType
            {
                TestTypeID = Convert.ToInt32(reader["TestTypeID"]),
                TestTypeTitle = reader["TestTypeTitle"].ToString(),
                TestTypeDescription = reader["TestTypeDescription"].ToString(),
                TestTypeFees = Convert.ToDouble(reader["TestTypeFees"])
            };
        }

        public List<TestType> GetAll()
        {
            List<TestType> testTypesList = new List<TestType>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TestTypes";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                testTypesList.Add(_MapReaderToTestType(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestTypeRepository", "public List<TestType> GetAll()", ex.Message);
                        return null;
                    }
                }
            }
            return testTypesList;
        }

        public bool Update(TestType testType)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE TestTypes 
                                 SET TestTypeTitle       = @TestTypeTitle, 
                                     TestTypeDescription = @TestTypeDescription, 
                                     TestTypeFees        = @TestTypeFees 
                                 WHERE TestTypeID        = @TestTypeID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeTitle", testType.TestTypeTitle);
                    command.Parameters.AddWithValue("@TestTypeDescription", testType.TestTypeDescription);
                    command.Parameters.AddWithValue("@TestTypeFees", testType.TestTypeFees);
                    command.Parameters.AddWithValue("@TestTypeID", testType.TestTypeID);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestTypeRepository", "public bool Update(TestType testType)", ex.Message);
                        return false;
                    }
                }
            }
        }

        public int GetNumberOfTestTypes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM TestTypes";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestTypeRepository", "public int GetNumberOfTestTypes()", ex.Message);
                        return -1;
                    }
                }
            }
        }

        public TestType GetByID(int id)
        {
            TestType testType = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                testType = _MapReaderToTestType(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("TestTypeRepository", "public TestType GetByID(int id)", ex.Message);
                        return null;
                    }
                }
            }
            return testType;
        }
    }
}