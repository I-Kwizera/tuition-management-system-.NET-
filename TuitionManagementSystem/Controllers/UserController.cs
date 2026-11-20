using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using TuitionManagementSystem.Models;

namespace TuitionManagementSystem.Controllers
{
    public class UserController
    {
        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT user_id, username, email, role, created_at FROM users";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }

            return dt;
        }

        public bool AddUser(string username, string email, string password, string role, int adminId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string sql = "INSERT INTO users (username, email, password_hash, role) VALUES (@u, @e, @p, @r)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", password);
                    cmd.Parameters.AddWithValue("@r", role);
                    cmd.ExecuteNonQuery();

                    AddAuditLog(conn, adminId, "Added user: " + username + " (" + role + ")");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message);
                return false;
            }
        }

        public bool UpdateUser(int userId, string username, string email, string role, string newPassword, int adminId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string sql;
                    MySqlCommand cmd;

                    if (newPassword != "")
                    {
                        sql = "UPDATE users SET username=@u, email=@e, role=@r, password_hash=@p WHERE user_id=@id";
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@p", newPassword);
                    }
                    else
                    {
                        sql = "UPDATE users SET username=@u, email=@e, role=@r WHERE user_id=@id";
                        cmd = new MySqlCommand(sql, conn);
                    }

                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@r", role);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();

                    AddAuditLog(conn, adminId, "Updated user ID: " + userId + " (" + username + ")");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
                return false;
            }
        }

        private void AddAuditLog(MySqlConnection conn, int adminId, string action)
        {
            if (adminId <= 0) return;

            string sql = "INSERT INTO audit_logs (user_id, action_description) VALUES (@uid, @act)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@uid", adminId);
            cmd.Parameters.AddWithValue("@act", action);
            cmd.ExecuteNonQuery();
        }
    }
}
