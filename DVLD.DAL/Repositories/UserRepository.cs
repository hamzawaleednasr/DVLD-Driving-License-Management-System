using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using DVLD.Core.DTOs;
using DVLD.DAL.Entities;
using DVLD.DAL.Interfaces;

namespace DVLD.DAL.Repositories
{
    public class UserRepository : IUser
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private User _MapReaderToUser(SqlDataReader reader)
        {
            return new User
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                PersonID = Convert.ToInt32(reader["PersonID"]),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                Username = reader["Username"].ToString(),
                Password = reader["Password"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
            };
        }

        private UserViewModel _MapReaderToUserModelView(SqlDataReader reader)
        {
            return new UserViewModel
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                PersonID = Convert.ToInt32(reader["PersonID"]),
                FullName = reader["FullName"].ToString(),
                Username = reader["Username"].ToString(),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
            };
        }

        private const string _baseViewQuery = @"
            SELECT Users.UserID, Users.PersonID, 
                   (People.FirstName + ' ' + ISNULL(People.SecondName + ' ', '') + ISNULL(People.ThirdName + ' ', '') + People.LastName) AS FullName, 
                   Users.Username, Users.IsActive 
            FROM Users 
            INNER JOIN People ON Users.PersonID = People.PersonID";

        public List<User> GetAll()
        {
            List<User> usersList = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usersList.Add(_MapReaderToUser(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return usersList;
        }

        public List<UserViewModel> GetAllToView()
        {
            List<UserViewModel> usersViewList = new List<UserViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(_baseViewQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usersViewList.Add(_MapReaderToUserModelView(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return usersViewList;
        }

        public User GetByID(int id)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = _MapReaderToUser(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return user;
        }

        public UserViewModel GetByIDToView(int userID)
        {
            UserViewModel userView = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE Users.UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userView = _MapReaderToUserModelView(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return userView;
        }

        public User GetByUsername(string username)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = _MapReaderToUser(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return user;
        }

        public int Add(User entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Users (PersonID, Username, Password)
                                 VALUES            (@PersonID, @Username, @Password);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", entity.PersonID);
                    command.Parameters.AddWithValue("@Username", entity.Username);
                    command.Parameters.AddWithValue("@Password", entity.Password);

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

        public bool Update(User entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Users 
                                 SET IsActive = @IsActive, 
                                     Username = @Username 
                                 WHERE UserID = @UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    command.Parameters.AddWithValue("@Username", entity.Username);
                    command.Parameters.AddWithValue("@UserID", entity.UserID);

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
                string query = "DELETE FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);

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

        public UserViewModel GetByPersonIDToView(int personID)
        {
            UserViewModel userView = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE Users.PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userView = _MapReaderToUserModelView(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return userView;
        }

        public UserViewModel GetByUsernameToView(string username)
        {
            UserViewModel userView = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE Users.Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userView = _MapReaderToUserModelView(reader);
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return userView;
        }

        public List<UserViewModel> GetByFullNameToView(string fullName)
        {
            List<UserViewModel> usersViewList = new List<UserViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE (People.FirstName + ' ' + ISNULL(People.SecondName + ' ', '') + ISNULL(People.ThirdName + ' ', '') + People.LastName) LIKE @FullName";

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
                                usersViewList.Add(_MapReaderToUserModelView(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return usersViewList;
        }

        public List<UserViewModel> GetByActivityToView(bool isActive)
        {
            List<UserViewModel> usersViewList = new List<UserViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = _baseViewQuery + " WHERE Users.IsActive = @IsActive";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", isActive);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usersViewList.Add(_MapReaderToUserModelView(reader));
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return usersViewList;
        }

        public int GetNumberOfUsers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(0) FROM Users";

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

        public bool ChangePassword(int userID, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Password", newPassword);
                    command.Parameters.AddWithValue("@UserID", userID);

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
