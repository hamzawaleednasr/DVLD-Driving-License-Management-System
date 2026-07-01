using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class ApplicationRepository : IApplication
    {
        private readonly string _connectionString;

        public ApplicationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Application _MapReaderToApplication(SqlDataReader reader)
        {
            return new Application
            {
                ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                ApplicationStatus = reader["ApplicationStatus"].ToString(),
                ApplicationLastStatusDate = Convert.ToDateTime(reader["ApplicationLastStatusDate"]),
                ApplicationPaidFees = Convert.ToDouble(reader["ApplicationPaidFees"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                PersonID = Convert.ToInt32(reader["PersonID"]),
                ApplicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]),
                CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"])
            };
        }

        public List<Application> GetAll()
        {
            List<Application> applicationsList = new List<Application>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Applications";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applicationsList.Add(_MapReaderToApplication(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return applicationsList;
        }

        public Application GetByID(int id)
        {
            Application application = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                application = _MapReaderToApplication(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return application;
        }

        public int Add(Application entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Applications (ApplicationStatus, ApplicationPaidFees, PersonID, ApplicationTypeID, CreatedByUserID)
                    SELECT 
                        @ApplicationStatus, 
                        ApplicationTypeFees,
                        @PersonID, 
                        @ApplicationTypeID, 
                        @CreatedByUserID
                    FROM ApplicationTypes
                    WHERE ApplicationTypeID = @ApplicationTypeID; 

                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationStatus", entity.ApplicationStatus);
                    command.Parameters.AddWithValue("@PersonID", entity.PersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", entity.ApplicationTypeID);
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

        public bool Update(Application entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Applications 
                                 SET ApplicationStatus         = @ApplicationStatus, 
                                     ApplicationLastStatusDate = @ApplicationLastStatusDate, 
                                     ApplicationPaidFees       = @ApplicationPaidFees, 
                                     PersonID                  = @PersonID, 
                                     ApplicationTypeID         = @ApplicationTypeID, 
                                     CreatedByUserID           = @CreatedByUserID 
                                 WHERE ApplicationID           = @ApplicationID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationStatus", entity.ApplicationStatus);
                    command.Parameters.AddWithValue("@ApplicationLastStatusDate", entity.ApplicationLastStatusDate);
                    command.Parameters.AddWithValue("@ApplicationPaidFees", entity.ApplicationPaidFees);
                    command.Parameters.AddWithValue("@PersonID", entity.PersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", entity.ApplicationTypeID);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserID);
                    command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationID);

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
                string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", id);

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