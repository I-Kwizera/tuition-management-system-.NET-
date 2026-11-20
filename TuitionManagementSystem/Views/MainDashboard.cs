using System;
using System.Drawing;
using System.Windows.Forms;

namespace TuitionManagementSystem.Views
{
    public partial class MainDashboard : Form
    {
        private string currentUserRole;
        private string currentUsername;

        public MainDashboard(string role, string username)
        {
            InitializeComponent();
            currentUserRole = role;
            currentUsername = username;

            ConfigureDashboard();
        }

        private void ConfigureDashboard()
        {
            this.Text = "Tuition Management System - Dashboard [" + currentUserRole + "]";
            this.Width = 900;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblWelcome = new Label();
            lblWelcome.Text = "Welcome, " + currentUsername + "!";
            lblWelcome.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblWelcome.Location = new Point(30, 30);
            lblWelcome.AutoSize = true;
            this.Controls.Add(lblWelcome);

            Label lblRole = new Label();
            lblRole.Text = "Logged in as: " + currentUserRole;
            lblRole.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblRole.ForeColor = Color.DimGray;
            lblRole.Location = new Point(33, 70);
            lblRole.AutoSize = true;
            this.Controls.Add(lblRole);

            int startY = 120;

            if (currentUserRole == "Admin")
            {
                AddDashboardButton("Manage User Accounts", startY, (s, e) =>
                {
                    OpenUserManagement();
                });
            }
            else if (currentUserRole == "FinanceOfficer")
            {
                AddDashboardButton("Record Student Payment", startY, (s, e) =>
                {
                    new FinanceForm(1, 2).ShowDialog();
                });
                AddDashboardButton("Setup Tuition Rates", startY + 60, (s, e) =>
                {
                    new FinanceForm(1, 0).ShowDialog();
                });
            }
            else if (currentUserRole == "Student")
            {
                AddDashboardButton("View Billing & Balance", startY, (s, e) =>
                {
                    new StudentBillingForm(currentUsername).ShowDialog();
                });
                AddDashboardButton("Download Payment Receipts", startY + 60, (s, e) =>
                {
                    new StudentBillingForm(currentUsername).ShowDialog();
                });
            }
        }

        private void AddDashboardButton(string text, int topPosition, EventHandler onClick)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            btn.Width = 250;
            btn.Height = 45;
            btn.Location = new Point(33, topPosition);
            btn.UseVisualStyleBackColor = true;
            btn.Click += onClick;
            this.Controls.Add(btn);
            btn.BringToFront();
        }

        private void OpenUserManagement()
        {
            if (currentUserRole == "Admin")
            {
                UserManagementForm form = new UserManagementForm();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Access Denied: Admin role required.");
            }
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenUserManagement();
        }

        private void recordPaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentUserRole == "FinanceOfficer" || currentUserRole == "Admin")
            {
                new FinanceForm(1, 2).ShowDialog();
            }
            else
            {
                MessageBox.Show("Access Denied: Finance Officer role required.");
            }
        }

        private void viewBillingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentUserRole == "Student")
            {
                new StudentBillingForm(currentUsername).ShowDialog();
            }
            else
            {
                MessageBox.Show("Student billing view is for Student accounts.");
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }
        private void manageUsersToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenUserManagement();
        }

        private void recordPaymentsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            recordPaymentsToolStripMenuItem_Click(sender, e);
        }

        private void viewBillingBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewBillingToolStripMenuItem_Click(sender, e);
        }

        private void logoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            logoutToolStripMenuItem_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void MainDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}