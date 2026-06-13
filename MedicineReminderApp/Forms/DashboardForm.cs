using System;
using System.Windows.Forms;

namespace MedicineReminderApp
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome, " + Session.CurrentUsername + "!";
        }

        private void btnMedicines_Click(object sender, EventArgs e)
        {
            using (var form = new MedicineForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnReminders_Click(object sender, EventArgs e)
        {
            using (var form = new ReminderForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            using (var form = new HistoryForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Session.CurrentUsername = null;
                Close();
            }
        }
    }
}
