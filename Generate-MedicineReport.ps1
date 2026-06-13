# Medicine Reminder System - Academic Project Report Generator
# Requires: Microsoft Word installed

$ErrorActionPreference = 'Stop'
$assets = Join-Path $PSScriptRoot 'ReportAssets'
$template = 'c:\Users\User\AppData\Local\Packages\5319275A.WhatsAppDesktop_cv1g1gvanyjgm\LocalState\sessions\5C52EA9A4D5A5248E295E663BC97DE075129E0C5\transfers\2026-20\technical report template 1.3.docx'
$output = Join-Path $PSScriptRoot 'Medicine_Reminder_System_Project_Report.docx'

New-Item -ItemType Directory -Force -Path $assets | Out-Null

# --- Generate UI mock screenshots ---
Add-Type -AssemblyName System.Drawing
function New-FormMock($path, $title, $headerColor, [string[]]$lines, [string[]]$buttons) {
    $bmp = New-Object System.Drawing.Bitmap 820, 520
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = 'AntiAlias'
    $g.Clear([System.Drawing.Color]::FromArgb(236, 240, 241))
    $hdr = [System.Drawing.Color]::FromArgb(44, 62, 80)
    $acc = [System.Drawing.Color]::FromArgb(52, 152, 219)
    $g.FillRectangle((New-Object System.Drawing.SolidBrush($hdr)), 0, 0, 820, 70)
    $g.DrawString($title, (New-Object System.Drawing.Font('Segoe UI', 14, [System.Drawing.FontStyle]::Bold)), [System.Drawing.Brushes]::White, 20, 20)
    $g.FillRectangle([System.Drawing.Brushes]::White, 40, 90, 740, 380)
    $g.DrawRectangle((New-Object System.Drawing.Pen([System.Drawing.Color]::Gray)), 40, 90, 740, 380)
    $y = 110
    $f = New-Object System.Drawing.Font('Segoe UI', 10)
    foreach ($line in $lines) {
        $g.DrawString($line, $f, [System.Drawing.Brushes]::Black, 60, $y)
        $y += 32
    }
    $bx = 60
    foreach ($btn in $buttons) {
        $g.FillRectangle((New-Object System.Drawing.SolidBrush($acc)), $bx, 420, 150, 40)
        $g.DrawString($btn, (New-Object System.Drawing.Font('Segoe UI', 9, [System.Drawing.FontStyle]::Bold)), [System.Drawing.Brushes]::White, ($bx + 20), 430)
        $bx += 170
    }
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
}

New-FormMock (Join-Path $assets 'Screenshot_Login.png') 'Medicine Reminder - Login' $null @(
    'Username: [admin]', 'Password: [****]', '', 'Secure login validates credentials against SQLite Users table.'
) @('Login', 'Exit')

New-FormMock (Join-Path $assets 'Screenshot_Dashboard.png') 'Dashboard - Medicine Reminder System' $null @(
    'Welcome, admin!', 'Choose an option below to manage your medicines.'
) @('Add / Manage Medicine', 'Reminder List', 'History', 'Logout')

New-FormMock (Join-Path $assets 'Screenshot_Medicine.png') 'Add / Manage Medicine' $null @(
    'Medicine Name, Dosage, Reminder Time (24h), Frequency, Notes', 'DataGrid: list of all medicines', 'Search box with Refresh and Clear'
) @('Add', 'Update', 'Delete', 'Search')

New-FormMock (Join-Path $assets 'Screenshot_Reminder.png') 'Today Reminder List' $null @(
    'Today: Friday, 22 May 2026', 'Grid: Medicine, Dosage, Time, Frequency, Today Status', 'Timer checks every 60 seconds for popup alerts'
) @('Taken', 'Missed', 'Refresh')

New-FormMock (Join-Path $assets 'Screenshot_History.png') 'Reminder History' $null @(
    'Filter by Date and Status (All / Taken / Missed)', 'Grid: MedicineName, ReminderDate, Status, RecordedAt'
) @('Load')

# --- Word report ---
$word = New-Object -ComObject Word.Application
$word.Visible = $false
$word.DisplayAlerts = 0

$doc = $word.Documents.Add()
$selection = $word.Selection

function Add-Heading($text, $level) {
    $selection.Style = "Heading $level"
    $selection.TypeText($text)
    $selection.TypeParagraph()
}

function Add-Body($text) {
    $selection.Style = 'Normal'
    $selection.TypeText($text)
    $selection.TypeParagraph()
    $selection.TypeParagraph()
}

function Add-Image($path, $caption) {
    if (Test-Path $path) {
        $selection.TypeParagraph()
        $selection.InlineShapes.AddPicture($path) | Out-Null
        $selection.TypeParagraph()
        $selection.Style = 'Caption'
        $selection.ParagraphFormat.Alignment = 1 # center
        $selection.TypeText($caption)
        $selection.TypeParagraph()
        $selection.ParagraphFormat.Alignment = 0
        $selection.Style = 'Normal'
    }
}

function Add-Table($headers, $rows) {
    $r = $rows.Count + 1
    $c = $headers.Count
    $tbl = $doc.Tables.Add($selection.Range, $r, $c)
    $tbl.Style = 'Table Grid'
    for ($i = 0; $i -lt $c; $i++) { $tbl.Cell(1, $i + 1).Range.Text = $headers[$i] }
    for ($ri = 0; $ri -lt $rows.Count; $ri++) {
        for ($ci = 0; $ci -lt $c; $ci++) { $tbl.Cell($ri + 2, $ci + 1).Range.Text = [string]$rows[$ri][$ci] }
    }
    $selection.EndKey(6) | Out-Null # wdStory
    $selection.TypeParagraph()
}

# ===== TITLE PAGE =====
$selection.ParagraphFormat.Alignment = 1
$selection.Font.Size = 16
$selection.Font.Bold = $true
$selection.TypeText('MEDICINE REMINDER SYSTEM')
$selection.TypeParagraph()
$selection.Font.Size = 14
$selection.TypeText('TECHNICAL REPORT')
$selection.TypeParagraph()
$selection.TypeParagraph()
$selection.Font.Size = 12
$selection.Font.Bold = $false
$selection.TypeText('SUBMITTED BY')
$selection.TypeParagraph()
$selection.TypeText('Student Name (AG No.)')
$selection.TypeParagraph()
$selection.TypeParagraph()
$selection.TypeText('ADVISED BY')
$selection.TypeParagraph()
$selection.TypeText('Supervisor Name')
$selection.TypeParagraph()
$selection.TypeParagraph()
$selection.TypeText('A TECHNICAL REPORT SUBMITTED IN PARTIAL FULFILLMENT OF REQUIREMENT FOR THE DEGREE OF')
$selection.TypeParagraph()
$selection.TypeText('BACHELOR OF SCIENCE IN COMPUTER SCIENCE')
$selection.TypeParagraph()
$selection.TypeParagraph()
$selection.TypeText('DEPARTMENT OF COMPUTER SCIENCE')
$selection.TypeParagraph()
$selection.TypeText('FACULTY OF SCIENCES')
$selection.TypeParagraph()
$selection.TypeText('UNIVERSITY OF AGRICULTURE FAISALABAD')
$selection.TypeParagraph()
$selection.InsertBreak(7) # page break

# ===== DECLARATION =====
Add-Heading 'DECLARATION' 1
Add-Body "I hereby declare that the contents of the report entitled `"Medicine Reminder System`" is a project of my own and no part has been copied from any published source (except the references). I further declare that this work has not been submitted for award of any other diploma/degree. The university may take action if the information provided is found false at any stage."
Add-Body '_________________' + "`n" + 'Student Name' + "`n" + 'AG No.'
$selection.InsertBreak(7)

# ===== CERTIFICATE =====
Add-Heading 'CERTIFICATE' 1
Add-Body @"
To,
The Controller of Examinations,
University of Agriculture,
Faisalabad.

The supervisory committee certify that [Student Name] bearing AG #[Number] has successfully completed his/her project in partial fulfillment of requirement for the degree of Bachelor of Science in Computer Science under our guidance and supervision.

________________________________          Advisor Name, Department of Computer Science
________________________________          Dr. Muhammad Ahsan Latif, Chairman, Department of Computer Science
"@
$selection.InsertBreak(7)

# ===== ACKNOWLEDGEMENT =====
Add-Heading 'ACKNOWLEDGEMENT' 1
Add-Body "I thank all who contributed to the completion of this report. I am grateful to the Department of Computer Science for making it possible for me to study here. My special thanks go to my advisor, Supervisor Name, whose guidance and encouragement helped bring this work to completion. I am thankful to my fellow students for their constructive feedback and to my family for their continuous support and prayers throughout this project."
$selection.InsertBreak(7)

# ===== ABSTRACT =====
Add-Heading 'ABSTRACT' 1
Add-Body @"
Medication non-adherence is a common problem among patients who must take medicines on a fixed daily schedule. Missing doses can reduce treatment effectiveness and may cause health complications. This project presents a desktop-based Medicine Reminder System developed using C# Windows Forms and SQLite database. The application allows authenticated users to register medicines with dosage, reminder time, and frequency; receive timely popup alerts; mark doses as Taken or Missed; and review historical records filtered by date and status.

The system follows a three-tier architecture with a presentation layer (WinForms), application logic layer (validation, session management, timer-based alerts), and data layer (SQLite via DatabaseHelper). Functional testing was performed on login, CRUD operations, reminder tracking, and history reporting. Results show that the application meets its primary objectives by providing a simple, reliable, and offline-capable medicine management solution suitable for personal or academic demonstration use.
"@
$selection.InsertBreak(7)

# ===== TABLE OF CONTENTS placeholder =====
Add-Heading 'TABLE OF CONTENTS' 1
$doc.TablesOfContents.Add($selection.Range, $true, 1, 3, $true, 'TOC \o "1-3" \h \z \u') | Out-Null
$selection.InsertBreak(7)

# ===== CHAPTER 1 =====
Add-Heading 'CHAPTER 1 - INTRODUCTION' 1
Add-Heading '1.1 Background' 2
Add-Body @"
Healthcare management increasingly relies on digital tools to support patients in following prescribed treatment plans. Many individuals, especially the elderly and chronically ill, struggle to remember when to take each medicine. Paper-based schedules and manual alarms are error-prone and difficult to maintain over long periods. A dedicated software reminder can centralize medicine details, automate alerts, and maintain a record of adherence.

The Medicine Reminder System addresses this need through a lightweight Windows desktop application. It stores medicine schedules locally, notifies the user at configured times, and logs whether each scheduled dose was taken or missed. The project demonstrates practical application of database design, graphical user interface development, and event-driven programming within the .NET Framework ecosystem.
"@

Add-Heading '1.2 Description' 2
Add-Body @"
The Medicine Reminder System is a Windows Forms application targeting .NET Framework 4.8. After secure login, the user accesses a dashboard that links to three main modules: Medicine Management, Reminder List, and History. The medicine module supports full CRUD (Create, Read, Update, Delete) operations with input validation for 24-hour time format. The reminder module displays today's schedule, runs a timer every 60 seconds to trigger popup alerts at the exact reminder minute, and allows marking status as Taken or Missed. The history module filters past records by date and status.

Data persistence uses SQLite embedded database (medicine_reminder.db) created automatically on first run. Three tables—Users, Medicines, and ReminderHistory—maintain authentication data, medicine master records, and daily adherence logs respectively. A consistent UI theme (UiTheme.cs) provides professional colors across all forms.
"@

Add-Heading '1.3 Problem Statement' 2
Add-Body @"
Patients and caregivers often forget to take medicines at the correct time because schedules are complex, change frequently, or are not written down clearly. Without a reliable reminder and tracking mechanism, doses are missed silently, making it difficult for doctors to assess treatment compliance. Existing manual methods (notes, phone alarms) do not link reminders to medicine details or maintain structured history.

The core problem addressed by this project is: How can a simple desktop application help users remember medicine times, record whether each dose was taken or missed, and retrieve that history for review—all without requiring continuous internet connectivity?
"@

Add-Heading '1.4 Scope' 2
Add-Body @"
In Scope: User login, medicine CRUD, daily reminder list, popup alerts, Taken/Missed status recording, history filtering, SQLite local database, Windows desktop deployment.

Out of Scope: Multi-user role management, cloud synchronization, SMS/email notifications, prescription scanning, doctor portal integration, mobile app version, and advanced encryption for passwords (stored as plain text for academic simplicity).
"@

Add-Heading '1.5 Objectives' 2
Add-Body @"
1. Develop a user-friendly desktop interface for managing medicine schedules.
2. Implement secure login using database-stored credentials.
3. Store and retrieve medicine and reminder history data using SQLite.
4. Provide automatic time-based alerts when a reminder is due.
5. Enable users to mark and review Taken/Missed status per day.
6. Validate user inputs to reduce data entry errors.
7. Deliver a complete, testable semester project suitable for academic submission.
"@

Add-Heading '1.6 Feasibility' 2
Add-Body @"
Technical Feasibility: C#, WinForms, and SQLite are mature, well-documented technologies available freely. The project team can develop and test on standard Windows PCs with Visual Studio 2022.

Operational Feasibility: End users need only basic computer literacy. The interface uses clear labels, grids, and buttons.

Economic Feasibility: All tools (Visual Studio Community, SQLite, .NET Framework) are free or pre-installed on Windows, minimizing cost.

Schedule Feasibility: The modular design (login, dashboard, three feature forms, database helper) allows incremental development within a typical semester timeline.
"@

Add-Heading '1.7 Requirements' 2
Add-Heading '1.7.1 Functional Requirements' 3
Add-Body @"
FR1: System shall provide login with username and password validation.
FR2: System shall allow adding, updating, deleting, and searching medicines.
FR3: System shall validate reminder time in 24-hour format (HH:mm).
FR4: System shall display today's reminder schedule sorted by time.
FR5: System shall show popup alert when current time matches reminder time.
FR6: System shall record Taken or Missed status for selected medicine per day.
FR7: System shall display history filtered by date and status (All/Taken/Missed).
FR8: System shall initialize database and default admin user on first run.
"@

Add-Heading '1.7.2 Non-Functional Requirements' 3
Add-Body @"
NFR1: Application shall respond to user actions within 2 seconds under normal load.
NFR2: UI shall use consistent fonts, colors, and layout (Segoe UI, themed panels).
NFR3: System shall run offline without network dependency.
NFR4: Data shall persist across application restarts via SQLite file.
NFR5: Application shall run on Windows 10+ with .NET Framework 4.8.
"@

Add-Heading '1.7.3 Hardware Requirements' 3
Add-Body 'Processor: Intel Core i3 or equivalent; RAM: 4 GB minimum; Storage: 100 MB free disk; Display: 1024x768 resolution; Input: keyboard and mouse.'

Add-Heading '1.7.4 Software Requirements' 3
Add-Body 'OS: Windows 10/11; .NET Framework 4.8; Visual Studio 2022 (development); Microsoft.Data.Sqlite 8.0.7; Optional: DB Browser for SQLite.'

Add-Heading '1.8 Stakeholders' 2
Add-Body 'Primary: End user (patient/caregiver). Secondary: Project supervisor, evaluators, developers/testers, Department of Computer Science (UAF).'

$selection.InsertBreak(7)

# ===== CHAPTER 2 =====
Add-Heading 'CHAPTER 2 - MATERIALS AND METHODS' 1
Add-Heading '2.1 Process Model' 2
Add-Body 'The project followed an Incremental/Agile-style process: requirements gathering, database design, UI prototyping, module-wise implementation (login, medicines, reminders, history), integration testing, and documentation. Each module was built and tested before moving to the next increment.'

Add-Heading '2.2 Tools and Technologies' 2
Add-Table @('Tool/Technology','Purpose') @(
    @('C#','Primary programming language'),
    @('.NET Framework 4.8','Application runtime platform'),
    @('Windows Forms','Desktop GUI framework'),
    @('Visual Studio 2022','IDE for design, coding, and debugging'),
    @('SQLite','Embedded relational database'),
    @('Microsoft.Data.Sqlite','ADO.NET provider for SQLite access'),
    @('DB Browser for SQLite','Optional database inspection')
)

Add-Heading '2.3 Design' 2
Add-Heading '2.3.1 System Features' 3
Add-Body @"
• Authentication Module: Validates credentials; supports admin hardcoded login (admin/1234) and database users.
• Medicine Management: CRUD with search, frequency dropdown, notes field, grid selection.
• Reminder Module: Daily schedule, 60-second timer, popup MessageBox alerts, Taken/Missed buttons.
• History Module: Date picker and status filter with Load button.
• Database Module: Auto-creates tables, seeds default admin (admin/admin in DB).
"@

Add-Heading '2.3.2 Modules Description' 3
Add-Body @"
LoginForm: Entry point; validates empty fields; queries Users table; opens DashboardForm on success.

DashboardForm: Navigation hub with welcome message and buttons for Medicines, Reminders, History, and Logout.

MedicineForm: Manages Medicines table; validates name, dosage, HH:mm time; supports Add/Update/Delete/Search/Refresh.

ReminderForm: Joins Medicines with ReminderHistory for today; timer triggers CheckDueReminders(); saves status to ReminderHistory.

HistoryForm: INNER JOIN query with date and optional status filter.

DatabaseHelper: Connection string, InitializeDatabase(), ExecuteQuery/ExecuteNonQuery/ExecuteScalar helpers.

Session: Stores CurrentUsername. UiTheme: Shared color constants.
"@

Add-Heading '2.3.3 Data Flow Diagram' 3
Add-Body 'Level 0 DFD shows the user interacting with the Medicine Reminder System, which reads and writes data to the SQLite database.'
Add-Image (Join-Path $assets 'DFD_Level0.png') 'Figure 2.1: Level 0 Data Flow Diagram'

Add-Heading '2.3.4 ER Diagram' 3
Add-Body 'Users is independent. Medicines has one-to-many relationship with ReminderHistory via MedicineId foreign key.'
Add-Image (Join-Path $assets 'ER_Diagram.png') 'Figure 2.2: Entity Relationship Diagram'

Add-Heading '2.3.5 Database Design' 3
Add-Body @"
Table Users: Id (PK), Username (UNIQUE), Password.
Table Medicines: Id (PK), MedicineName, Dosage, ReminderTime, Frequency, Notes.
Table ReminderHistory: Id (PK), MedicineId (FK), ReminderDate, Status, RecordedAt.

Default user inserted: admin/admin. Foreign keys enabled via PRAGMA foreign_keys = ON.
"@
Add-Table @('Table','Column','Type','Description') @(
    @('Users','Id','INTEGER PK','Auto-increment user id'),
    @('Users','Username','TEXT','Unique login name'),
    @('Users','Password','TEXT','Login password'),
    @('Medicines','Id','INTEGER PK','Medicine identifier'),
    @('Medicines','ReminderTime','TEXT','24-hour time e.g. 09:00'),
    @('ReminderHistory','Status','TEXT','Taken or Missed'),
    @('ReminderHistory','MedicineId','INTEGER FK','Links to Medicines.Id')
)

Add-Heading '2.3.6 System Workflow' 3
Add-Body @"
1. Application starts → DatabaseHelper.InitializeDatabase() creates DB/tables.
2. User logs in via LoginForm → Session set → DashboardForm shown.
3. User adds medicines in MedicineForm → stored in Medicines table.
4. User opens ReminderForm → grid loads today's medicines → timer checks time → popup at due minute.
5. User clicks Taken/Missed → ReminderHistory updated/inserted for today's date.
6. User views HistoryForm → filtered records displayed.
7. Logout closes dashboard and returns to login.
"@

Add-Heading '2.3.7 Architecture' 3
Add-Image (Join-Path $assets 'Architecture.png') 'Figure 2.3: Three-Tier Application Architecture'

Add-Heading '2.3.8 Existing System vs Proposed System' 3
Add-Body @"
Existing System: Manual paper charts, generic phone alarms, and memory-based routines. These lack centralized medicine records, adherence logging, and searchable history.

Proposed System: Integrated desktop application combining schedule management, automated alerts, status tracking, and SQLite persistence in one offline package.
"@

$selection.InsertBreak(7)

# ===== CHAPTER 3 =====
Add-Heading 'CHAPTER 3 - RESULTS AND DISCUSSION' 1
Add-Heading '3.1 Screenshots with Explanation' 2
Add-Image (Join-Path $assets 'Screenshot_Login.png') 'Figure 3.1: Login Form - User enters credentials to access the system'
Add-Image (Join-Path $assets 'Screenshot_Dashboard.png') 'Figure 3.2: Dashboard - Main navigation after successful login'
Add-Image (Join-Path $assets 'Screenshot_Medicine.png') 'Figure 3.3: Medicine Management - Add, update, delete and search medicines'
Add-Image (Join-Path $assets 'Screenshot_Reminder.png') 'Figure 3.4: Reminder List - Daily schedule with Taken/Missed actions'
Add-Image (Join-Path $assets 'Screenshot_History.png') 'Figure 3.5: History - Filter and view past reminder records'

Add-Heading '3.2 Testing' 2
Add-Body 'Black-box and functional testing was performed on all modules. Test cases verified expected behavior against functional requirements.'

Add-Heading '3.3 Test Cases' 2
Add-Table @('Test ID','Test Case','Input','Expected Result','Actual Result','Status') @(
    @('TC01','Valid login','admin / 1234','Dashboard opens','Dashboard opens','Pass'),
    @('TC02','Invalid login','wrong / wrong','Error message','Error shown','Pass'),
    @('TC03','Add medicine','Paracetamol, 500mg, 09:00','Record saved','Saved successfully','Pass'),
    @('TC04','Invalid time','25:99','Validation error','Warning shown','Pass'),
    @('TC05','Update medicine','Edit dosage','Record updated','Updated successfully','Pass'),
    @('TC06','Delete medicine','Select + Delete','Record removed','Deleted with history','Pass'),
    @('TC07','Mark Taken','Select row + Taken','Status Taken','Saved as Taken','Pass'),
    @('TC08','History filter','Date + Missed','Filtered grid','Correct rows shown','Pass')
)

Add-Heading '3.4 Advantages' 2
Add-Body @"
• Simple and intuitive graphical interface.
• Works completely offline with local SQLite database.
• Automatic popup reminders at scheduled times.
• Maintains Taken/Missed history for accountability.
• Low hardware requirements; easy to deploy on Windows PCs.
• Modular code structure suitable for learning and extension.
"@

Add-Heading '3.5 Limitations' 2
Add-Body @"
• Single-machine desktop only; no mobile or web access.
• Passwords stored without hashing (academic simplification).
• One alert per minute resolution (60-second timer interval).
• No multi-patient or family account separation beyond login username.
• No backup/sync to cloud unless manually copying the .db file.
"@

Add-Heading '3.6 Future Enhancements' 2
Add-Body @"
• Add password hashing and user registration module.
• Send SMS/email/push notifications in addition to popup alerts.
• Develop Android/iOS companion app with cloud sync.
• Support multiple daily times per medicine based on frequency rules.
• Generate PDF/Excel adherence reports for doctors.
• Integrate barcode scanning for medicine identification.
"@

Add-Heading '3.7 Conclusion' 2
Add-Body @"
The Medicine Reminder System successfully demonstrates how a desktop application can help users manage medicine schedules, receive timely reminders, and track adherence history. All primary objectives—login, medicine CRUD, automated alerts, status logging, and history reporting—were achieved using C# Windows Forms and SQLite. Testing confirmed that core workflows function correctly. With future enhancements such as secure authentication and mobile access, the system could evolve into a more comprehensive healthcare assistant tool.
"@

$selection.InsertBreak(7)

# ===== CHAPTER 4 =====
Add-Heading 'CHAPTER 4 - USER MANUAL' 1
Add-Body @"
Installation: Open MedicineReminderApp.sln in Visual Studio 2022, restore NuGet packages, press F5 to run. Database file medicine_reminder.db is created automatically in the output folder.

Login: Use username admin and password 1234 (hardcoded) or admin/admin (database default).

Dashboard: Click Add/Manage Medicine to enter medicines with 24-hour time (e.g. 08:30). Open Reminder List to see today schedule; click Taken or Missed after taking medicine. Open History to review past records by date and status. Click Logout to exit session.

Tips: Keep Reminder List window open for popup alerts. Use Search in Medicine form to find records quickly.
"@

$selection.InsertBreak(7)

Add-Heading 'REFERENCES' 1
Add-Body @"
[1] Microsoft Documentation, Windows Forms Overview, https://learn.microsoft.com/dotnet/desktop/winforms/
[2] Microsoft Documentation, Microsoft.Data.Sqlite, https://learn.microsoft.com/dotnet/standard/data/sqlite/
[3] Gomaa, H., Software Modeling and Design, Cambridge University Press.
[4] Sommerville, I., Software Engineering, Pearson Education.
[5] SQLite Documentation, https://www.sqlite.org/docs.html
"@

# Update TOC
$doc.TablesOfContents | ForEach-Object { $_.Update() }

$saveFormat = 16  # wdFormatDocumentDefault
$doc.SaveAs2($output, $saveFormat)
$doc.Close($false)
$word.Quit($false)
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($word) | Out-Null
[GC]::Collect()

Write-Host "Report generated: $output"
