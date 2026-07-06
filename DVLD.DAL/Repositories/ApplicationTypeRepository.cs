using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DVLD.DAL.Repositories
{
    public class ApplicationTypeRepository : IApplicationType
    {
        private readonly string _connectionString;
    
        public ApplicationTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private ApplicationType _MapReaderToApplicationType(SqlDataReader reader)
        {
            return new ApplicationType
            {
                ApplicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]),
                ApplicationTypeTitle = reader["ApplicationTypeTitle"].ToString(),
                ApplicationTypeFees = Convert.ToDouble(reader["ApplicationTypeFees"])
            };
        }

        public bool Update(ApplicationType applicationType)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE ApplicationTypes 
                                 SET ApplicationTypeTitle = @ApplicationTypeTitle, 
                                     ApplicationTypeFees  = @ApplicationTypeFees 
                                 WHERE ApplicationTypeID  = @ApplicationTypeID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", applicationType.ApplicationTypeTitle);
                    command.Parameters.AddWithValue("@ApplicationTypeFees", applicationType.ApplicationTypeFees);
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationType.ApplicationTypeID);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("ApplicationTypeRepository", "public bool Update(ApplicationType applicationType)", ex.Message);
                        return false;
                    }
                }
            }
        }

        public List<ApplicationType> GetAll()
        {
            List<ApplicationType> applicationTypesList = new List<ApplicationType>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM ApplicationTypes";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applicationTypesList.Add(_MapReaderToApplicationType(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("ApplicationTypeRepository", "public List<ApplicationType> GetAll()", ex.Message);
                        return null;
                    }
                }
            }
            return applicationTypesList;
        }

        public ApplicationType GetByID(int id)
        {
            ApplicationType applicationType = new ApplicationType();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                applicationType = _MapReaderToApplicationType(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("ApplicationTypeRepository", "public ApplicationType GetByID(int id)", ex.Message);
                        return null;
                    }
                }
            }
            return applicationType;
        }

        public int GetNumberOfApplicationTypes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM ApplicationTypes";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("ApplicationTypeRepository", "public int GetNumberOfApplicationTypes()", ex.Message);
                        return -1;
                    }
                }
            }
        }
    }
}
