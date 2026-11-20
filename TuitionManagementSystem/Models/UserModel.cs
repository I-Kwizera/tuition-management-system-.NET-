using System;
using MySql.Data.MySqlClient;

namespace TuitionManagementSystem.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public static UserModel AuthenticateUser(string usernameOrEmail, string password)
        {
            UserModel user = null;

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT user_id, username, email, role FROM users WHERE (username = @input OR email = @input) AND password_hash = @password";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@input", usernameOrEmail);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserModel
                            {
                                UserId = reader.GetInt32("user_id"),
                                Username = reader.GetString("username"),
                                Email = reader.GetString("email"),
                                Role = reader.GetString("role")
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
