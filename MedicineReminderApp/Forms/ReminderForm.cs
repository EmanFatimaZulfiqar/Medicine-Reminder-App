using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MedicineReminderApp
{
    public partial class ReminderForm : Form
    {

        private readonly HashSet<string> _shownAlertsToday = new HashSet<string>();

        private string _lastAlertDateKey = string.Empty;

        public ReminderForm()
        {
            InitializeComponent();
        }

        private void ReminderForm_Load(object sender, EventArgs e)
        {
            lblToday.Text = "Today: " + DateTime.Today.ToString("dddd, dd MMMM yyyy", CultureInfo.CurrentCulture);
            RefreshReminderGrid();
            CheckDueReminders();
            reminderTimer.Start();
        }

        private void ReminderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            reminderTimer.Stop();
        }

        private void RefreshReminderGrid()
        {
            var today = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var sql = @"
SELECT
    m.Id AS MedicineId,
    m.MedicineName,
    m.Dosage,
    m.ReminderTime,
    m.Frequency,
    h.Status AS TodayStatus
FROM Medicines m
LEFT JOIN ReminderHistory h
    ON h.MedicineId = m.Id AND h.ReminderDate = @today
ORDER BY m.ReminderTime;";

            dgvReminders.DataSource = DatabaseHelper.ExecuteQuery(sql,
                new SqliteParameter("@today", today));
        }

        private void reminderTimer_Tick(object sender, EventArgs e)
        {
            
            var todayKey = DateTime.Today.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            if (_lastAlertDateKey != todayKey)
            {
                _shownAlertsToday.Clear();
                _lastAlertDateKey = todayKey;
            }

            RefreshReminderGrid();
            CheckDueReminders();
        }

        
        private void CheckDueReminders()
        {
            var now = DateTime.Now;
            foreach (DataGridViewRow row in dgvReminders.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                var idObj = row.Cells["MedicineId"].Value;
                var nameObj = row.Cells["MedicineName"].Value;
                var timeObj = row.Cells["ReminderTime"].Value;
                if (idObj == null || timeObj == null)
                {
                    continue;
                }

                var idText = idObj.ToString();
                var medicineName = nameObj?.ToString() ?? "Medicine";
                var timeText = timeObj.ToString();

                var formats = new[] { "HH:mm", "H:mm" };
                if (!DateTime.TryParseExact(timeText, formats, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var reminderClock))
                {
                    continue;
                }

                if (now.Hour != reminderClock.Hour || now.Minute != reminderClock.Minute)
                {
                    continue;
                }

                var alertKey = idText + "_" + now.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
                if (_shownAlertsToday.Contains(alertKey))
                {
                    continue;
                }

                _shownAlertsToday.Add(alertKey);

                MessageBox.Show(
                    "Time to take: " + medicineName + Environment.NewLine +
                    "Dosage row: " + (row.Cells["Dosage"].Value?.ToString() ?? string.Empty),
                    "Medicine reminder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private int GetSelectedMedicineId()
        {
            if (dgvReminders.CurrentRow == null || dgvReminders.CurrentRow.IsNewRow)
            {
                return -1;
            }

            var value = dgvReminders.CurrentRow.Cells["MedicineId"].Value;
            if (value == null || value == DBNull.Value)
            {
                return -1;
            }

            return int.TryParse(value.ToString(), out var id) ? id : -1;
        }

        private void SaveTodayStatus(string status)
        {
            var id = GetSelectedMedicineId();
            if (id < 0)
            {
                MessageBox.Show("Please select a medicine row first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var today = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            var exists = Convert.ToInt64(DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM ReminderHistory WHERE MedicineId = @id AND ReminderDate = @d;",
                new SqliteParameter("@id", id),
                new SqliteParameter("@d", today)));

            int rows;
            if (exists > 0)
            {
                rows = DatabaseHelper.ExecuteNonQuery(
                    @"UPDATE ReminderHistory
                      SET Status = @s, RecordedAt = @t
                      WHERE MedicineId = @id AND ReminderDate = @d;",
                    new SqliteParameter("@s", status),
                    new SqliteParameter("@t", now),
                    new SqliteParameter("@id", id),
                    new SqliteParameter("@d", today));
            }
            else
            {
                rows = DatabaseHelper.ExecuteNonQuery(
                    @"INSERT INTO ReminderHistory (MedicineId, ReminderDate, Status, RecordedAt)
                      VALUES (@id, @d, @s, @t);",
                    new SqliteParameter("@id", id),
                    new SqliteParameter("@d", today),
                    new SqliteParameter("@s", status),
                    new SqliteParameter("@t", now));
            }

            if (rows > 0)
            {
                MessageBox.Show("Saved as " + status + ".", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshReminderGrid();
            }
            else
            {
                MessageBox.Show("Could not save status.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaken_Click(object sender, EventArgs e)
        {
            SaveTodayStatus("Taken");
        }

        private void btnMissed_Click(object sender, EventArgs e)
        {
            SaveTodayStatus("Missed");
        }

        private void btnRefreshNow_Click(object sender, EventArgs e)
        {
            RefreshReminderGrid();
        }
    }
}
