using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using TuitionManagementSystem.Models;

namespace TuitionManagementSystem.Controllers
{
    public class FinanceController
    {
        public DataTable GetFeeStructures()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT fee_id, program, academic_term, credit_hours, amount_per_credit, total_tuition FROM fee_structures";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading fees: " + ex.Message);
            }
            return dt;
        }

        public bool SaveFeeStructure(string program, string term, int credits, decimal ratePerCredit, int actorUserId)
        {
            decimal totalTuition = credits * ratePerCredit;
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = @"INSERT INTO fee_structures (program, academic_term, credit_hours, amount_per_credit, total_tuition)
                                   VALUES (@prog, @term, @cred, @rate, @total)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@prog", program);
                    cmd.Parameters.AddWithValue("@term", term);
                    cmd.Parameters.AddWithValue("@cred", credits);
                    cmd.Parameters.AddWithValue("@rate", ratePerCredit);
                    cmd.Parameters.AddWithValue("@total", totalTuition);
                    cmd.ExecuteNonQuery();

                    LogAction(conn, actorUserId, "Added fee structure for " + program + " (" + term + ")");
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving fee structure: " + ex.Message);
                return false;
            }
        }

        public DataTable GetStudents()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT student_id, user_id, full_name, program, academic_term, total_due, remaining_balance FROM students";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
            return dt;
        }

        public bool SaveStudentBilling(int? userId, string fullName, string program, string term, decimal totalDue, int actorUserId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = @"INSERT INTO students (user_id, full_name, program, academic_term, total_due, remaining_balance)
                                   VALUES (@uid, @name, @prog, @term, @due, @bal)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@uid", userId.HasValue ? (object)userId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@name", fullName);
                    cmd.Parameters.AddWithValue("@prog", program);
                    cmd.Parameters.AddWithValue("@term", term);
                    cmd.Parameters.AddWithValue("@due", totalDue);
                    cmd.Parameters.AddWithValue("@bal", totalDue);
                    cmd.ExecuteNonQuery();

                    LogAction(conn, actorUserId, "Created student billing profile for " + fullName);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving student profile: " + ex.Message);
                return false;
            }
        }

        public DataTable GetPayments()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT payment_id, receipt_number, student_id, amount_paid, payment_method, payment_date, recorded_by FROM payments ORDER BY payment_id DESC";
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payments: " + ex.Message);
            }
            return dt;
        }

        public bool RecordPayment(string receiptNumber, int studentId, decimal amountPaid, string method, int recordedByUserId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string insertSql = @"INSERT INTO payments (receipt_number, student_id, amount_paid, payment_method, recorded_by)
                                       VALUES (@rcpt, @sid, @amt, @meth, @by)";
                    MySqlCommand insertCmd = new MySqlCommand(insertSql, conn);
                    insertCmd.Parameters.AddWithValue("@rcpt", receiptNumber);
                    insertCmd.Parameters.AddWithValue("@sid", studentId);
                    insertCmd.Parameters.AddWithValue("@amt", amountPaid);
                    insertCmd.Parameters.AddWithValue("@meth", method);
                    insertCmd.Parameters.AddWithValue("@by", recordedByUserId > 0 ? recordedByUserId : 1);
                    insertCmd.ExecuteNonQuery();

                    string updateSql = @"UPDATE students
                                       SET remaining_balance = GREATEST(0, remaining_balance - @amt)
                                       WHERE student_id = @sid";
                    MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@amt", amountPaid);
                    updateCmd.Parameters.AddWithValue("@sid", studentId);
                    updateCmd.ExecuteNonQuery();

                    LogAction(conn, recordedByUserId > 0 ? recordedByUserId : 1, "Recorded payment " + receiptNumber + " for student ID " + studentId);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error recording payment: " + ex.Message);
                return false;
            }
        }

        private void LogAction(MySqlConnection conn, int userId, string description)
        {
            if (userId <= 0) return;
            string sql = "INSERT INTO audit_logs (user_id, action_description) VALUES (@uid, @desc)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@uid", userId);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.ExecuteNonQuery();
        }
        public DataTable GetStudentBillingByUsername(string username)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT s.student_id, s.full_name, s.program, s.academic_term, s.total_due, s.remaining_balance
                                   FROM students s
                                   INNER JOIN users u ON s.user_id = u.user_id
                                   WHERE u.username = @uname";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@uname", username);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student billing: " + ex.Message);
            }
            return dt;
        }

        public DataTable GetStudentPaymentsByUsername(string username)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = @"SELECT p.receipt_number, p.amount_paid, p.payment_method, p.payment_date
                                   FROM payments p
                                   INNER JOIN students s ON p.student_id = s.student_id
                                   INNER JOIN users u ON s.user_id = u.user_id
                                   WHERE u.username = @uname
                                   ORDER BY p.payment_id DESC";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@uname", username);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student payments: " + ex.Message);
            }
            return dt;
        }
    }
}
