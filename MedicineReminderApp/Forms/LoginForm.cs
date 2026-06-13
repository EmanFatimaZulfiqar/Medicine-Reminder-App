using System;
using System.Windows.Forms;

namespace MedicineReminderApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            var isHardcodedAdmin = username.Equals("admin", StringComparison.OrdinalIgnoreCase)
                                   && password == "1234";

            var isDatabaseUser = false;
            if (!isHardcodedAdmin)
            {
                var sql = "SELECT COUNT(*) FROM Users WHERE Username = @u AND Password = @p;";
                var count = Convert.ToInt64(DatabaseHelper.ExecuteScalar(sql,
                    new Microsoft.Data.Sqlite.SqliteParameter("@u", username),
                    new Microsoft.Data.Sqlite.SqliteParameter("@p", password)));
                isDatabaseUser = count > 0;
            }

            if (isHardcodedAdmin || isDatabaseUser)
            {
                Session.CurrentUsername = username;
                var dashboard = new DashboardForm();
                dashboard.FormClosed += (_, __) => Close();
                Hide();
                dashboard.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
