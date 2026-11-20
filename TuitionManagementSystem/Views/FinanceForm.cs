using System;
using System.Drawing;
using System.Windows.Forms;
using TuitionManagementSystem.Controllers;

namespace TuitionManagementSystem.Views
{
    public partial class FinanceForm : Form
    {
        private FinanceController controller = new FinanceController();
       private int currentUserId;
      
        private TextBox txtFeeProgram, txtFeeTerm, txtCredits, txtRate;
        private DataGridView dgvFees;
        private TextBox txtStudentUserId, txtStudentName, txtStuProgram, txtStuTerm, txtTotalDue;
        private DataGridView dgvStudents;

        private TextBox txtReceipt, txtPayStudentId, txtAmountPaid;
        private ComboBox cmbMethod;
        private DataGridView dgvPayments;

        public FinanceForm(int userId = 1, int startTabIndex = 0)
        {
            InitializeComponent();
            currentUserId = userId;
            SetupUI(startTabIndex);
            RefreshAllData();
        }

        private void SetupUI(int startTabIndex)
        {
            this.Text = "Finance & Tuition Management";
            this.Size = new Size(880, 560);
            this.StartPosition = FormStartPosition.CenterScreen;

            TabControl tabs = new TabControl { Dock = DockStyle.Fill };

            TabPage tabFees = new TabPage("Tuition Fees");
            BuildFeeTab(tabFees);

            TabPage tabStudents = new TabPage("Student Billing");
            BuildStudentTab(tabStudents);

            TabPage tabPayments = new TabPage("Record Payments");
            BuildPaymentTab(tabPayments);

            tabs.TabPages.Add(tabFees);
            tabs.TabPages.Add(tabStudents);
            tabs.TabPages.Add(tabPayments);
            tabs.SelectedIndex = startTabIndex;

            this.Controls.Add(tabs);
        }

        private void BuildFeeTab(TabPage page)
        {
            page.Controls.Add(new Label { Text = "Program:", Location = new Point(20, 25), AutoSize = true });
            txtFeeProgram = new TextBox { Location = new Point(125, 22), Width = 170 };
            page.Controls.Add(txtFeeProgram);

            page.Controls.Add(new Label { Text = "Term:", Location = new Point(20, 65), AutoSize = true });
            txtFeeTerm = new TextBox { Location = new Point(125, 62), Width = 170 };
            page.Controls.Add(txtFeeTerm);

            page.Controls.Add(new Label { Text = "Credit Hours:", Location = new Point(20, 105), AutoSize = true });
            txtCredits = new TextBox { Location = new Point(125, 102), Width = 170 };
            page.Controls.Add(txtCredits);

            page.Controls.Add(new Label { Text = "Rate / Credit:", Location = new Point(20, 145), AutoSize = true });
            txtRate = new TextBox { Location = new Point(125, 142), Width = 170 };
            page.Controls.Add(txtRate);

            Button btnSaveFee = new Button { Text = "Save Fee Rate", Location = new Point(20, 190), Width = 140, Height = 34 };
            btnSaveFee.Click += BtnSaveFee_Click;
            page.Controls.Add(btnSaveFee);

            dgvFees = new DataGridView
            {
                Location = new Point(320, 22),
                Size = new Size(515, 430),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            page.Controls.Add(dgvFees);
        }

        private void BuildStudentTab(TabPage page)
        {
            page.Controls.Add(new Label { Text = "User ID (opt):", Location = new Point(20, 25), AutoSize = true });
            txtStudentUserId = new TextBox { Location = new Point(125, 22), Width = 170 };
            page.Controls.Add(txtStudentUserId);

            page.Controls.Add(new Label { Text = "Full Name:", Location = new Point(20, 65), AutoSize = true });
            txtStudentName = new TextBox { Location = new Point(125, 62), Width = 170 };
            page.Controls.Add(txtStudentName);

            page.Controls.Add(new Label { Text = "Program:", Location = new Point(20, 105), AutoSize = true });
            txtStuProgram = new TextBox { Location = new Point(125, 102), Width = 170 };
            page.Controls.Add(txtStuProgram);

            page.Controls.Add(new Label { Text = "Term:", Location = new Point(20, 145), AutoSize = true });
            txtStuTerm = new TextBox { Location = new Point(125, 142), Width = 170 };
            page.Controls.Add(txtStuTerm);

            page.Controls.Add(new Label { Text = "Total Due:", Location = new Point(20, 185), AutoSize = true });
            txtTotalDue = new TextBox { Location = new Point(125, 182), Width = 170 };
            page.Controls.Add(txtTotalDue);

            Button btnSaveStudent = new Button { Text = "Save Billing", Location = new Point(20, 230), Width = 140, Height = 34 };
            btnSaveStudent.Click += BtnSaveStudent_Click;
            page.Controls.Add(btnSaveStudent);

            dgvStudents = new DataGridView
            {
                Location = new Point(320, 22),
                Size = new Size(515, 430),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            page.Controls.Add(dgvStudents);
        }

        private void BuildPaymentTab(TabPage page)
        {
            page.Controls.Add(new Label { Text = "Receipt #:", Location = new Point(20, 25), AutoSize = true });
            txtReceipt = new TextBox { Location = new Point(125, 22), Width = 170 };
            page.Controls.Add(txtReceipt);

            page.Controls.Add(new Label { Text = "Student ID:", Location = new Point(20, 65), AutoSize = true });
            txtPayStudentId = new TextBox { Location = new Point(125, 62), Width = 170 };
            page.Controls.Add(txtPayStudentId);

            page.Controls.Add(new Label { Text = "Amount Paid:", Location = new Point(20, 105), AutoSize = true });
            txtAmountPaid = new TextBox { Location = new Point(125, 102), Width = 170 };
            page.Controls.Add(txtAmountPaid);

            page.Controls.Add(new Label { Text = "Method:", Location = new Point(20, 145), AutoSize = true });
            cmbMethod = new ComboBox
            {
                Location = new Point(125, 142),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMethod.Items.AddRange(new object[] { "Cash", "Bank", "Online" });
            cmbMethod.SelectedIndex = 0;
            page.Controls.Add(cmbMethod);

            Button btnSavePayment = new Button { Text = "Save Payment", Location = new Point(20, 190), Width = 140, Height = 34 };
            btnSavePayment.Click += BtnSavePayment_Click;
            page.Controls.Add(btnSavePayment);

            dgvPayments = new DataGridView
            {
                Location = new Point(320, 22),
                Size = new Size(515, 430),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            page.Controls.Add(dgvPayments);
        }
       private void BtnSaveFee_Click(object sender, EventArgs e)
        {
            if (txtFeeProgram.Text.Trim() == "" || txtFeeTerm.Text.Trim() == "" ||
                !int.TryParse(txtCredits.Text.Trim(), out int credits) ||
                !decimal.TryParse(txtRate.Text.Trim(), out decimal rate))
            {
                MessageBox.Show("Enter valid program, term, numeric credit hours, and rate.");
                return;
            }

            if (controller.SaveFeeStructure(txtFeeProgram.Text.Trim(), txtFeeTerm.Text.Trim(), credits, rate, currentUserId))
            {
                MessageBox.Show("Fee structure saved.");
                txtFeeProgram.Text = "";
                txtFeeTerm.Text = "";
                txtCredits.Text = "";
                txtRate.Text = "";
                RefreshAllData();
            }
        }

         private void BtnSaveStudent_Click(object sender, EventArgs e)
        {
            if (txtStudentName.Text.Trim() == "" || txtStuProgram.Text.Trim() == "" ||
                txtStuTerm.Text.Trim() == "" || !decimal.TryParse(txtTotalDue.Text.Trim(), out decimal due))
            {
                MessageBox.Show("Enter student full name, program, term, and valid total due.");
                return;
            }

            int? uid = null;
            if (int.TryParse(txtStudentUserId.Text.Trim(), out int parsedUid))
            {
                uid = parsedUid;
            }

            if (controller.SaveStudentBilling(uid, txtStudentName.Text.Trim(), txtStuProgram.Text.Trim(), txtStuTerm.Text.Trim(), due, currentUserId))
            {
                MessageBox.Show("Student billing saved.");
                txtStudentUserId.Text = "";
                txtStudentName.Text = "";
                txtStuProgram.Text = "";
                txtStuTerm.Text = "";
                txtTotalDue.Text = "";
                RefreshAllData();
            }
        }

       private void BtnSavePayment_Click(object sender, EventArgs e)
        {
            if (txtReceipt.Text.Trim() == "" ||
                !int.TryParse(txtPayStudentId.Text.Trim(), out int studentId) ||
                !decimal.TryParse(txtAmountPaid.Text.Trim(), out decimal amount))
            {
                MessageBox.Show("Enter receipt number, valid Student ID, and valid payment amount.");
                return;
            }

           if (controller.RecordPayment(txtReceipt.Text.Trim(), studentId, amount, cmbMethod.SelectedItem.ToString(), currentUserId))
            {
                MessageBox.Show("Payment recorded and balance updated.");
                txtReceipt.Text = "";
                txtPayStudentId.Text = "";
                txtAmountPaid.Text = "";
                RefreshAllData();
            }
        }

         private void RefreshAllData()
        {
            dgvFees.DataSource = controller.GetFeeStructures();
            dgvStudents.DataSource = controller.GetStudents();
            dgvPayments.DataSource = controller.GetPayments();
        }
        private void FinanceForm_Load(object sender, EventArgs e)
        {
            RefreshAllData();
        }
    }
}