using System;
using System.Drawing;
using System.Windows.Forms;
using TuitionManagementSystem.Controllers;

namespace TuitionManagementSystem.Views
{
    public partial class StudentBillingForm : Form
    {
        private FinanceController controller = new FinanceController();
        private string studentUsername;

        private DataGridView dgvBilling;
        private DataGridView dgvHistory;

        public StudentBillingForm(string username)
        {
            InitializeComponent();
            studentUsername = username;
            SetupForm();
            LoadStudentData();
        }

        private void SetupForm()
        {
            this.Text = "My Billing & Payment History";
            this.Size = new Size(820, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblBilling = new Label
            {
                Text = "My Tuition Billing & Balance (" + studentUsername + ")",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            this.Controls.Add(lblBilling);

            dgvBilling = new DataGridView
            {
                Location = new Point(20, 45),
                Size = new Size(765, 140),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvBilling);

            Label lblHistory = new Label
            {
                Text = "My Payment History",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, 200),
                AutoSize = true
            };
            this.Controls.Add(lblHistory);

            dgvHistory = new DataGridView
            {
                Location = new Point(20, 230),
                Size = new Size(765, 225),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvHistory);
        }

        private void LoadStudentData()
        {
            dgvBilling.DataSource = controller.GetStudentBillingByUsername(studentUsername);
            dgvHistory.DataSource = controller.GetStudentPaymentsByUsername(studentUsername);
    
       }
        private void StudentBillingForm_Load(object sender, EventArgs e) { LoadStudentData(); }
    }
}
