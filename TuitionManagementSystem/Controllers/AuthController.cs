using TuitionManagementSystem.Models;

namespace TuitionManagementSystem.Controllers
{
    public class AuthController
    {
        public UserModel Login(string usernameOrEmail, string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Please enter username/email and password.";
                return null;
            }

            try
            {
                UserModel user = UserModel.AuthenticateUser(usernameOrEmail, password);
                if (user == null)
                {
                    errorMessage = "Invalid credentials.";
                }
                return user;
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }
    }
}
