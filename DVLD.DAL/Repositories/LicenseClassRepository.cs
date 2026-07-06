using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class LicenseClassRepository : ILicenseClass
    {
        private readonly string _connectionString;

        public LicenseClassRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private LicenseClass _MapReaderToLicenseClass(SqlDataReader reader)
        {
            return new LicenseClass
            {
                LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]),
                LicenseClassTitle = reader["LicenseClassTitle"].ToString(),
                LicenseClassDescription = reader["LicenseClassDescription"].ToString(),
                LicenseClassFees = Convert.ToDouble(reader["LicenseClassFees"]),
                MinimumAllowedAge = Convert.ToByte(reader["MinimumAllowedAge"]),
                ValidityLength = Convert.ToByte(reader["ValidityLength"])
            };
        }

        public List<LicenseClass> GetAll()
        {
            List<LicenseClass> licenseClassesList = new List<LicenseClass>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM LicenseClasses";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                licenseClassesList.Add(_MapReaderToLicenseClass(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseClassRepository", "public List<LicenseClass> GetAll()", ex.Message);
                        return null;
                    }
                }
            }
            return licenseClassesList;
        }

        public LicenseClass GetByID(int id)
        {
            LicenseClass licenseClass = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                licenseClass = _MapReaderToLicenseClass(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseClassRepository", "public LicenseClass GetByID(int id)", ex.Message);
                        return null;
                    }
                }
            }
            return licenseClass;
        }

        public bool Update(LicenseClass licenseClass)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE LicenseClasses 
                                 SET LicenseClassTitle       = @LicenseClassTitle, 
                                     LicenseClassDescription = @LicenseClassDescription, 
                                     LicenseClassFees        = @LicenseClassFees, 
                                     MinimumAllowedAge       = @MinimumAllowedAge, 
                                     ValidityLength          = @ValidityLength 
                                 WHERE LicenseClassID        = @LicenseClassID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassTitle", licenseClass.LicenseClassTitle);
                    command.Parameters.AddWithValue("@LicenseClassDescription", licenseClass.LicenseClassDescription);
                    command.Parameters.AddWithValue("@LicenseClassFees", licenseClass.LicenseClassFees);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", licenseClass.MinimumAllowedAge);
                    command.Parameters.AddWithValue("@ValidityLength", licenseClass.ValidityLength);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClass.LicenseClassID);

                    try
                    {
                        connection.Open();
                        return (command.ExecuteNonQuery() > 0);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("LicenseClassRepository", "public bool Update(LicenseClass licenseClass)", ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
