using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MedicineReminderApp
{
    public partial class MedicineForm : Form
    {
        public MedicineForm()
        {
            InitializeComponent();
        }

        private void MedicineForm_Load(object sender, EventArgs e)
        {
            cmbFrequency.Items.AddRange(new object[]
            {
                "Once daily",
                "Twice daily",
                "Three times daily",
                "Weekly",
                "As needed"
            });
            cmbFrequency.SelectedIndex = 0;
            LoadMedicines();
        }

        private void LoadMedicines()
        {
            var sql = "SELECT Id AS [Medicine ID], MedicineName AS [Medicine Name], " +
                      "Dosage, ReminderTime AS [Reminder Time], Frequency, Notes " +
                      "FROM Medicines ORDER BY Id;";
            dgvMedicines.DataSource = DatabaseHelper.ExecuteQuery(sql);
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtMedicineId.Clear();
            txtMedicineName.Clear();
            txtDosage.Clear();
            txtReminderTime.Clear();
            cmbFrequency.SelectedIndex = 0;
            txtNotes.Clear();
            dgvMedicines.ClearSelection();
        }

        private void dgvMedicines_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMedicines.CurrentRow == null || dgvMedicines.CurrentRow.IsNewRow)
            {
                return;
            }

            var row = dgvMedicines.CurrentRow;
            if (row.Cells["Medicine ID"].Value == null || row.Cells["Medicine ID"].Value == DBNull.Value)
            {
                return;
            }

            txtMedicineId.Text = row.Cells["Medicine ID"].Value.ToString();
            txtMedicineName.Text = row.Cells["Medicine Name"].Value?.ToString();
            txtDosage.Text = row.Cells["Dosage"].Value?.ToString();
            txtReminderTime.Text = row.Cells["Reminder Time"].Value?.ToString();
            txtNotes.Text = row.Cells["Notes"].Value?.ToString();

            var frequency = row.Cells["Frequency"].Value?.ToString();
            var index = cmbFrequency.Items.IndexOf(frequency);
            cmbFrequency.SelectedIndex = index >= 0 ? index : 0;
        }

        private bool TryReadInputs(out string name, out string dosage, out string reminderTime,
            out string frequency, out string notes)
        {
            name = txtMedicineName.Text.Trim();
            dosage = txtDosage.Text.Trim();
            reminderTime = txtReminderTime.Text.Trim();
            frequency = cmbFrequency.SelectedItem?.ToString() ?? string.Empty;
            notes = txtNotes.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Medicine name is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(dosage))
            {
                MessageBox.Show("Dosage is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var timeFormats = new[] { "HH:mm", "H:mm" };
            if (!DateTime.TryParseExact(reminderTime, timeFormats, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _))
            {
                MessageBox.Show("Reminder time must be in 24-hour format, for example 08:30 or 14:00.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(frequency))
            {
                MessageBox.Show("Please choose a frequency.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DateTime.TryParseExact(reminderTime, timeFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var normalized);
            reminderTime = normalized.ToString("HH:mm", CultureInfo.InvariantCulture);

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!TryReadInputs(out var name, out var dosage, out var reminderTime,
                    out var frequency, out var notes))
            {
                return;
            }

            var sql = @"INSERT INTO Medicines (MedicineName, Dosage, ReminderTime, Frequency, Notes)
                        VALUES (@n, @d, @t, @f, @notes);";

            var rows = DatabaseHelper.ExecuteNonQuery(sql,
                new SqliteParameter("@n", name),
                new SqliteParameter("@d", dosage),
                new SqliteParameter("@t", reminderTime),
                new SqliteParameter("@f", frequency),
                new SqliteParameter("@notes", string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes));

            if (rows > 0)
            {
                MessageBox.Show("Medicine added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMedicines();
            }
            else
            {
                MessageBox.Show("Could not add medicine.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMedicineId.Text))
            {
                MessageBox.Show("Select a medicine from the grid first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMedicineId.Text, out var id))
            {
                MessageBox.Show("Invalid medicine id.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryReadInputs(out var name, out var dosage, out var reminderTime,
                    out var frequency, out var notes))
            {
                return;
            }

            var sql = @"UPDATE Medicines
                        SET MedicineName = @n,
                            Dosage = @d,
                            ReminderTime = @t,
                            Frequency = @f,
                            Notes = @notes
                        WHERE Id = @id;";

            var rows = DatabaseHelper.ExecuteNonQuery(sql,
                new SqliteParameter("@n", name),
                new SqliteParameter("@d", dosage),
                new SqliteParameter("@t", reminderTime),
                new SqliteParameter("@f", frequency),
                new SqliteParameter("@notes", string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes),
                new SqliteParameter("@id", id));

            if (rows > 0)
            {
                MessageBox.Show("Medicine updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMedicines();
            }
            else
            {
                MessageBox.Show("No rows were updated.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMedicineId.Text))
            {
                MessageBox.Show("Select a medicine from the grid first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMedicineId.Text, out var id))
            {
                return;
            }

            var confirm = MessageBox.Show("Delete this medicine and its history records?",
                "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            DatabaseHelper.ExecuteNonQuery("DELETE FROM ReminderHistory WHERE MedicineId = @id;",
                new SqliteParameter("@id", id));

            var rows = DatabaseHelper.ExecuteNonQuery("DELETE FROM Medicines WHERE Id = @id;",
                new SqliteParameter("@id", id));

            if (rows > 0)
            {
                MessageBox.Show("Medicine deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMedicines();
            }
            else
            {
                MessageBox.Show("Could not delete medicine.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var term = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(term))
            {
                LoadMedicines();
                return;
            }

            var sql = "SELECT Id AS [Medicine ID], MedicineName AS [Medicine Name], " +
                      "Dosage, ReminderTime AS [Reminder Time], Frequency, Notes " +
                      "FROM Medicines WHERE MedicineName LIKE @p ORDER BY Id;";
            var pattern = "%" + term + "%";
            dgvMedicines.DataSource = DatabaseHelper.ExecuteQuery(sql,
                new SqliteParameter("@p", pattern));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadMedicines();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
