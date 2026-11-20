using System;
using System.Drawing;
using System.Windows.Forms;
using TuitionManagementSystem.Controllers;

namespace TuitionManagementSystem.Views
{
    public partial class UserManagementForm : Form
    {
        private UserController controller = new UserController();
        private int currentAdminId;

        private TextBox txtId;
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private ComboBox cmbRole;
        private DataGridView dgvUsers;

        public UserManagementForm(int adminId = 0)
        {
            InitializeComponent();
            currentAdminId = adminId;
            SetupForm();
            LoadUsers();
        }

        private void SetupForm()
        {
            this.Text = "User Management";
            this.Size = new Size(800, 460);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Controls.Add(new Label { Text = "ID:", Location = new Point(20, 25), AutoSize = true });
            txtId = new TextBox { Location = new Point(105, 22), Width = 70, ReadOnly = true };
            this.Controls.Add(txtId);

            this.Controls.Add(new Label { Text = "Username:", Location = new Point(20, 65), AutoSize = true });
            txtUsername = new TextBox { Location = new Point(105, 62), Width = 170 };
            this.Controls.Add(txtUsername);

            this.Controls.Add(new Label { Text = "Email:", Location = new Point(20, 105), AutoSize = true });
            txtEmail = new TextBox { Location = new Point(105, 102), Width = 170 };
            this.Controls.Add(txtEmail);

            this.Controls.Add(new Label { Text = "Password:", Location = new Point(20, 145), AutoSize = true });
            txtPassword = new TextBox { Location = new Point(105, 142), Width = 170, UseSystemPasswordChar = true };
            this.Controls.Add(txtPassword);

            this.Controls.Add(new Label { Text = "Role:", Location = new Point(20, 185), AutoSize = true });
            cmbRole = new ComboBox
            {
                Location = new Point(105, 182),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("FinanceOfficer");
            cmbRole.Items.Add("Student");
            cmbRole.SelectedIndex = 0;
            this.Controls.Add(cmbRole);

            Button btnAdd = new Button { Text = "Add", Location = new Point(20, 235), Width = 80, Height = 32 };
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            Button btnUpdate = new Button { Text = "Update", Location = new Point(108, 235), Width = 80, Height = 32 };
            btnUpdate.Click += BtnUpdate_Click;
            this.Controls.Add(btnUpdate);

            Button btnClear = new Button { Text = "Clear", Location = new Point(196, 235), Width = 79, Height = 32 };
            btnClear.Click += (s, e) => ClearForm();
            this.Controls.Add(btnClear);

            dgvUsers = new DataGridView
            {
                Location = new Point(300, 22),
                Size = new Size(465, 380),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvUsers.CellClick += DgvUsers_CellClick;
            this.Controls.Add(dgvUsers);
        }

        private void LoadUsers()
        {
            dgvUsers.DataSource = controller.GetAllUsers();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" || txtEmail.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Enter username, email, and password.");
                return;
            }

            bool ok = controller.AddUser(
                txtUsername.Text.Trim(),
                txtEmail.Text.Trim(),
                txtPassword.Text.Trim(),
                cmbRole.SelectedItem.ToString(),
                currentAdminId
            );

            if (ok)
            {
                MessageBox.Show("User added.");
                LoadUsers();
                ClearForm();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim() == "")
            {
                MessageBox.Show("Click a user in the table first.");
                return;
            }

            int id = int.Parse(txtId.Text);
            bool ok = controller.UpdateUser(
                id,
                txtUsername.Text.Trim(),
                txtEmail.Text.Trim(),
                cmbRole.SelectedItem.ToString(),
                txtPassword.Text.Trim(),
                currentAdminId
            );

            if (ok)
            {
                MessageBox.Show("User updated.");
                LoadUsers();
                ClearForm();
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                txtId.Text = row.Cells["user_id"].Value.ToString();
                txtUsername.Text = row.Cells["username"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                cmbRole.SelectedItem = row.Cells["role"].Value.ToString();
                txtPassword.Text = "";
            }
        }

        private void ClearForm()
        {
            txtId.Text = "";
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cmbRole.SelectedIndex = 0;
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {

        }
    }
}