using System;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MedicineReminderApp
{
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            dtpFilter.Value = DateTime.Today;
            cmbStatus.Items.AddRange(new object[] { "All", "Taken", "Missed" });
            cmbStatus.SelectedIndex = 0;
            LoadHistory();
        }

        private void LoadHistory()
        {
            var dateText = dtpFilter.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var status = cmbStatus.SelectedItem?.ToString() ?? "All";

            var sql = @"
SELECT
    h.Id AS HistoryId,
    h.MedicineId,
    m.MedicineName,
    h.ReminderDate,
    h.Status,
    h.RecordedAt
FROM ReminderHistory h
INNER JOIN Medicines m ON m.Id = h.MedicineId
WHERE h.ReminderDate = @d";

            if (status != "All")
            {
                sql += " AND h.Status = @s";
            }

            sql += " ORDER BY h.RecordedAt DESC;";

            if (status == "All")
            {
                dgvHistory.DataSource = DatabaseHelper.ExecuteQuery(sql,
                    new SqliteParameter("@d", dateText));
            }
            else
            {
                dgvHistory.DataSource = DatabaseHelper.ExecuteQuery(sql,
                    new SqliteParameter("@d", dateText),
                    new SqliteParameter("@s", status));
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadHistory();
        }
    }
}
