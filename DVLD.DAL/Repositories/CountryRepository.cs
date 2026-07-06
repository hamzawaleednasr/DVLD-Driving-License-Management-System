using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Policy;

namespace DVLD.DAL.Repositories
{
    public class CountryRepository : ICountry
    {
        private readonly string _connectionString;

        public CountryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Country> GetAllCountries()
        {
            List<Country> countries = new List<Country>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Countries";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                countries.Add(new Country
                                {
                                    CountryID = Convert.ToInt32(reader["CountryID"]),
                                    CountryName = Convert.ToString(reader["CountryName"]),
                                });
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("CountryRepository", "public List<Country> GetAllCountries()", ex.Message);
                        return null;
                    }
                }
            }
            return countries;
        }

        public int GetCountryID(string countryName)
        {
            int CountryID = -1;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT CountryID FROM Countries WHERE CountryName LIKE @CountryName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", '%' + countryName + '%');

                    try
                    {
                        connection.Open();

                        CountryID = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        EventLogger.LogError("CountryRepository", "public int GetCountryID(string countryName)", ex.Message);
                        return -1;
                    }
                }
            }
            return CountryID;
        }
    }
}
