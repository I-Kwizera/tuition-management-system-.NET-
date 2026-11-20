using System;
using System.Windows.Forms;
using TuitionManagementSystem.Controllers;
using TuitionManagementSystem.Models;

namespace TuitionManagementSystem.Views
{
    public partial class LoginForm : Form
    {
        private AuthController _authController;

        public LoginForm()
        {
            InitializeComponent();
            _authController = new AuthController();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string input = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            UserModel user = _authController.Login(input, password, out string error);

            if (user != null)
            {
                MessageBox.Show($"Welcome {user.Username} ({user.Role})", "Success");

                MainDashboard dashboard = new MainDashboard(user.Role, user.Username);
                dashboard.Show();

                
                this.Hide();
            }
            else
            {
                MessageBox.Show(error, "Error");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}