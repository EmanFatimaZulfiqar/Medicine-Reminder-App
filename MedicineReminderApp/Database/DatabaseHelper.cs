using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MedicineReminderApp
{
    
    internal static class DatabaseHelper
    {
        private static string DatabaseFilePath =>
            Path.Combine(Application.StartupPath, "medicine_reminder.db");

  
        public static string ConnectionString =>
            "Data Source=" + DatabaseFilePath + ";";

 
        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

              
                var createUsers = @"
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL
);";

                var createMedicines = @"
CREATE TABLE IF NOT EXISTS Medicines (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MedicineName TEXT NOT NULL,
    Dosage TEXT NOT NULL,
    ReminderTime TEXT NOT NULL,
    Frequency TEXT NOT NULL,
    Notes TEXT
);";

                var createHistory = @"
CREATE TABLE IF NOT EXISTS ReminderHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MedicineId INTEGER NOT NULL,
    ReminderDate TEXT NOT NULL,
    Status TEXT NOT NULL,
    RecordedAt TEXT NOT NULL,
    FOREIGN KEY (MedicineId) REFERENCES Medicines(Id)
);";

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "PRAGMA foreign_keys = ON;";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = createUsers;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = createMedicines;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = createHistory;
                    cmd.ExecuteNonQuery();

                   
                    cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = 'admin';";
                    var count = Convert.ToInt64(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        cmd.CommandText =
                            "INSERT INTO Users (Username, Password) VALUES ('admin', 'admin');";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }


        public static DataTable ExecuteQuery(string sql, params SqliteParameter[] parameters)
        {
            var dataSet = new DataSet { EnforceConstraints = false };
            var table = new DataTable();
            dataSet.Tables.Add(table);

            using (var connection = new SqliteConnection(ConnectionString))
            using (var command = new SqliteCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                }

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    table.Load(reader);
                }
            }

            return table;
        }

       
        public static int ExecuteNonQuery(string sql, params SqliteParameter[] parameters)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            using (var command = new SqliteCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                }

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        
        public static object ExecuteScalar(string sql, params SqliteParameter[] parameters)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            using (var command = new SqliteCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                }

                connection.Open();
                return command.ExecuteScalar();
            }
        }
    }
}
