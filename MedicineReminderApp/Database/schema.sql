
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Medicines (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MedicineName TEXT NOT NULL,
    Dosage TEXT NOT NULL,
    ReminderTime TEXT NOT NULL,
    Frequency TEXT NOT NULL,
    Notes TEXT
);

CREATE TABLE IF NOT EXISTS ReminderHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MedicineId INTEGER NOT NULL,
    ReminderDate TEXT NOT NULL,
    Status TEXT NOT NULL,
    RecordedAt TEXT NOT NULL,
    FOREIGN KEY (MedicineId) REFERENCES Medicines(Id)
);

INSERT INTO Users (Username, Password)
SELECT 'admin', 'admin'
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin');
