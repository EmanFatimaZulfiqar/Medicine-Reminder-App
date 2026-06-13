# Medicine Reminder System — Setup Instructions

This is a **Windows Forms (.NET Framework 4.8)** semester-style project using **SQLite** via **Microsoft.Data.Sqlite**.

## What you need

1. **Windows 10 or newer**
2. **Visual Studio 2022** (Community is fine) with the workload **“.NET desktop development”**
3. **.NET Framework 4.8 Developer Pack** (Visual Studio normally installs this with the workload)

## Step 1 — Open the solution

1. Unzip or copy the `MedicineReminderApp` folder to your computer.
2. Open **Visual Studio**.
3. Use **File → Open → Project/Solution** and select:

   `MedicineReminderApp\MedicineReminderApp.sln`

## Step 2 — Restore NuGet packages

1. In **Solution Explorer**, right-click the solution and choose **Restore NuGet Packages**.
2. Wait until restore finishes (needs internet the first time).

Alternatively, from a **Developer Command Prompt** in the solution folder:

`dotnet restore MedicineReminderApp\MedicineReminderApp.csproj`

## Step 3 — Build and run

1. Set **Configuration** to **Debug** and **Any CPU**.
2. Press **F5** (Run with debugging) or **Ctrl+F5** (Run without debugging).

The first run **creates** the SQLite file:

`medicine_reminder.db`

in the output folder, for example:

`MedicineReminderApp\MedicineReminderApp\bin\Debug\net48\`

Tables **Users**, **Medicines**, and **ReminderHistory** are created automatically in code (`DatabaseHelper.InitializeDatabase`).

## Step 4 — Login

Default credentials (hardcoded **and** stored in the database):

- **Username:** `admin`  
- **Password:** `admin`

You can also add more users directly in the `Users` table for practice.

## Using the app (short demo script)

1. **Dashboard** — open **Add / Manage Medicine** and add a few rows (use **Reminder time** like `09:00` in 24-hour form).
2. **Reminder List** — see today’s schedule; use **Taken** / **Missed** to write to **ReminderHistory**.
3. **History** — pick a date and **All / Taken / Missed**, then **Load**.
4. Leave the **Reminder** window open around a reminder minute to see a **MessageBox** alert (timer checks every **60 seconds**).

## Optional — View the database

Use **DB Browser for SQLite** or another SQLite tool and open `medicine_reminder.db` from the `bin\Debug\net48` (or `Release`) folder.

## Reference SQL file

For printed reports or viva notes, see:

`MedicineReminderApp\Database\schema.sql`

## Troubleshooting

| Problem | What to try |
|--------|-------------|
| **NuGet restore fails** | Check internet; update Visual Studio; retry restore. |
| **Missing .NET Framework 4.8** | Install “.NET Framework 4.8 Developer Pack” from Microsoft. |
| **Designer does not open** | Use **Build → Rebuild Solution**; ensure WinForms SDK project opens in VS 2022. |

## Project structure (simple overview)

- `Program.cs` — starts the app and initializes the database.
- `Database\DatabaseHelper.cs` — connection string, `CREATE TABLE`, and simple query helpers.
- `Forms\` — `LoginForm`, `DashboardForm`, `MedicineForm`, `ReminderForm`, `HistoryForm`.
- `UiTheme.cs` — shared colors for a consistent UI.
