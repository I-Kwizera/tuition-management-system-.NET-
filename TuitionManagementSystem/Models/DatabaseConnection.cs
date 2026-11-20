using System;
using MySql.Data.MySqlClient;

namespace TuitionManagementSystem.Models
{
    public class DatabaseConnection
    {
      
        private static string connectionString = "server=localhost; database=tuition_management_db; uid=root; pwd=;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Database connection error: " + ex.Message);
            }
        }
    }
}
