using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

var baseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
var assetsDir = Path.Combine(baseDir, "ReportAssets");
var outputPath = Path.Combine(baseDir, "Medicine_Reminder_System_Project_Report.docx");
Directory.CreateDirectory(assetsDir);

using var doc = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document);
var mainPart = doc.AddMainDocumentPart();
mainPart.Document = new Document(new Body());
var body = mainPart.Document.Body!;

void PageBreak() => body.Append(new Paragraph(new Run(new Break { Type = BreakValues.Page })));

void Heading(string text, int level)
{
    var p = new Paragraph();
    var props = new ParagraphProperties(new ParagraphStyleId { Val = level == 1 ? "Heading1" : level == 2 ? "Heading2" : "Heading3" });
    p.Append(props);
    var run = new Run();
    run.Append(new RunProperties(new Bold(), new FontSize { Val = level == 1 ? "32" : level == 2 ? "28" : "24" }));
    run.Append(new Text(text) { Space = SpaceProcessingModeValues.Preserve });
    p.Append(run);
    body.Append(p);
}

void Para(string text)
{
    foreach (var line in text.Split('\n'))
    {
        var p = new Paragraph();
        var run = new Run(new Text(line.TrimEnd()) { Space = SpaceProcessingModeValues.Preserve });
        p.Append(run);
        body.Append(p);
    }
    body.Append(new Paragraph());
}

void CenterPara(string text, bool bold = false, string size = "28")
{
    var p = new Paragraph(new ParagraphProperties(
        new Justification { Val = JustificationValues.Center }));
    var rp = new RunProperties(new FontSize { Val = size });
    if (bold) rp.Append(new Bold());
    var run = new Run(rp, new Text(text) { Space = SpaceProcessingModeValues.Preserve });
    p.Append(run);
    body.Append(p);
}

void AddImage(string imagePath, string caption)
{
    if (!File.Exists(imagePath)) return;
    var partType = ImagePartType.Png;
    var ext = Path.GetExtension(imagePath).ToLowerInvariant();
    if (ext is ".jpg" or ".jpeg") partType = ImagePartType.Jpeg;
    var imagePart = mainPart.AddImagePart(partType);
    using (var stream = File.OpenRead(imagePath))
        imagePart.FeedData(stream);
    var relId = mainPart.GetIdOfPart(imagePart);

    const long emusPerInch = 914400;
    var widthEmus = (long)(6.5 * emusPerInch);
    var heightEmus = (long)(4.0 * emusPerInch);

    var element = new Drawing(
        new DW.Inline(
            new DW.Extent { Cx = widthEmus, Cy = heightEmus },
            new DW.EffectExtent { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
            new DW.DocProperties { Id = (UInt32Value)1U, Name = Path.GetFileName(imagePath) },
            new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks { NoChangeAspect = true }),
            new A.Graphic(
                new A.GraphicData(
                    new PIC.Picture(
                        new PIC.NonVisualPictureProperties(
                            new PIC.NonVisualDrawingProperties { Id = 0U, Name = Path.GetFileName(imagePath) },
                            new PIC.NonVisualPictureDrawingProperties()),
                        new PIC.BlipFill(
                            new A.Blip { Embed = relId },
                            new A.Stretch(new A.FillRectangle())),
                        new PIC.ShapeProperties(
                            new A.Transform2D(
                                new A.Offset { X = 0L, Y = 0L },
                                new A.Extents { Cx = widthEmus, Cy = heightEmus }),
                            new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle }))
                ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
        ) { DistanceFromTop = 0U, DistanceFromBottom = 0U, DistanceFromLeft = 0U, DistanceFromRight = 0U });

    var imgPara = new Paragraph(new ParagraphProperties(new Justification { Val = JustificationValues.Center }));
    imgPara.Append(new Run(element));
    body.Append(imgPara);

    var cap = new Paragraph(new ParagraphProperties(new Justification { Val = JustificationValues.Center }));
    cap.Append(new Run(new RunProperties(new Italic()), new Text(caption) { Space = SpaceProcessingModeValues.Preserve }));
    body.Append(cap);
    body.Append(new Paragraph());
}

void Table(string[] headers, List<string[]> rows)
{
    var table = new Table();
    table.AppendChild(new TableProperties(
        new TableBorders(
            new TopBorder { Val = BorderValues.Single, Size = 4 },
            new BottomBorder { Val = BorderValues.Single, Size = 4 },
            new LeftBorder { Val = BorderValues.Single, Size = 4 },
            new RightBorder { Val = BorderValues.Single, Size = 4 },
            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
            new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 })));

    var headerRow = new TableRow();
    foreach (var h in headers)
    {
        var cell = new TableCell(new Paragraph(new Run(new RunProperties(new Bold()), new Text(h))));
        headerRow.Append(cell);
    }
    table.Append(headerRow);

    foreach (var row in rows)
    {
        var tr = new TableRow();
        foreach (var cellText in row)
            tr.Append(new TableCell(new Paragraph(new Run(new Text(cellText) { Space = SpaceProcessingModeValues.Preserve }))));
        table.Append(tr);
    }
    body.Append(table);
    body.Append(new Paragraph());
}

// Title page
CenterPara("MEDICINE REMINDER SYSTEM", true, "36");
CenterPara("TECHNICAL REPORT", true, "32");
body.Append(new Paragraph());
CenterPara("SUBMITTED BY");
CenterPara("Student Name (AG No.)");
body.Append(new Paragraph());
CenterPara("ADVISED BY");
CenterPara("Supervisor Name");
body.Append(new Paragraph());
CenterPara("A TECHNICAL REPORT SUBMITTED IN PARTIAL FULFILLMENT OF");
CenterPara("REQUIREMENT FOR THE DEGREE OF");
CenterPara("BACHELOR OF SCIENCE IN COMPUTER SCIENCE");
body.Append(new Paragraph());
CenterPara("DEPARTMENT OF COMPUTER SCIENCE");
CenterPara("FACULTY OF SCIENCES");
CenterPara("UNIVERSITY OF AGRICULTURE FAISALABAD");
PageBreak();

Heading("DECLARATION", 1);
Para("I hereby declare that the contents of the report entitled \"Medicine Reminder System\" is a project of my own and no part has been copied from any published source (except the references). I further declare that this work has not been submitted for award of any other diploma/degree.");
Para("_________________\nStudent Name\nAG No.");
PageBreak();

Heading("CERTIFICATE", 1);
Para(@"To,
The Controller of Examinations,
University of Agriculture, Faisalabad.

The supervisory committee certify that [Student Name] bearing AG #[Number] has successfully completed his/her project in partial fulfillment of requirement for the degree of Bachelor of Science in Computer Science under our guidance and supervision.

________________________________    Advisor Name, Department of Computer Science
________________________________    Dr. Muhammad Ahsan Latif, Chairman, Department of Computer Science");
PageBreak();

Heading("ACKNOWLEDGEMENT", 1);
Para("I thank all who contributed to the completion of this report. I am grateful to the Department of Computer Science for making it possible for me to study here. My special thanks go to my advisor, Supervisor Name, whose guidance helped bring this work to completion. I am thankful to my fellow students and family for their support throughout this project.");
PageBreak();

Heading("ABSTRACT", 1);
Para(@"Medication non-adherence is a common problem among patients who must take medicines on a fixed daily schedule. Missing doses can reduce treatment effectiveness and may cause health complications. This project presents a desktop-based Medicine Reminder System developed using C# Windows Forms and SQLite database. The application allows authenticated users to register medicines with dosage, reminder time, and frequency; receive timely popup alerts; mark doses as Taken or Missed; and review historical records filtered by date and status.

The system follows a three-tier architecture with a presentation layer (WinForms), application logic layer (validation, session management, timer-based alerts), and data layer (SQLite via DatabaseHelper). Functional testing was performed on login, CRUD operations, reminder tracking, and history reporting. Results show that the application meets its primary objectives by providing a simple, reliable, and offline-capable medicine management solution.");
PageBreak();

Heading("TABLE OF CONTENTS", 1);
Para("[Update table of contents in Microsoft Word: References → Table of Contents → Automatic Table 1]");
PageBreak();

// Chapter 1
Heading("CHAPTER 1 - INTRODUCTION", 1);
Heading("1.1 Background", 2);
Para(@"Healthcare management increasingly relies on digital tools to support patients in following prescribed treatment plans. Many individuals struggle to remember when to take each medicine. Paper-based schedules and manual alarms are error-prone. The Medicine Reminder System addresses this need through a lightweight Windows desktop application that stores schedules locally, notifies users at configured times, and logs adherence.");

Heading("1.2 Description", 2);
Para(@"The Medicine Reminder System is a Windows Forms application targeting .NET Framework 4.8. After login, users access a dashboard linking to Medicine Management, Reminder List, and History modules. Data persists in SQLite (medicine_reminder.db) with tables Users, Medicines, and ReminderHistory.");

Heading("1.3 Problem Statement", 2);
Para(@"Patients often forget medicines at the correct time because schedules are complex or not documented clearly. Without reliable reminders and tracking, doses are missed silently. This project solves: How can a desktop application help users remember medicine times, record Taken/Missed status, and retrieve history offline?");

Heading("1.4 Scope", 2);
Para("In Scope: Login, medicine CRUD, daily reminders, popup alerts, status logging, history filter, SQLite database.\nOut of Scope: Cloud sync, mobile app, SMS alerts, password hashing, multi-tenant roles.");

Heading("1.5 Objectives", 2);
Para("1. Develop user-friendly desktop interface.\n2. Implement secure login.\n3. Store data in SQLite.\n4. Provide automatic time-based alerts.\n5. Record and review Taken/Missed history.\n6. Validate inputs.\n7. Deliver complete academic project.");

Heading("1.6 Feasibility", 2);
Para("Technical, operational, economic, and schedule feasibility confirmed using free tools (Visual Studio Community, SQLite, .NET Framework 4.8) on standard Windows PCs.");

Heading("1.7 Requirements", 2);
Heading("1.7.1 Functional Requirements", 3);
Para("FR1: Login validation. FR2: Medicine CRUD and search. FR3: 24-hour time validation. FR4: Today's reminder list. FR5: Popup alerts. FR6: Taken/Missed recording. FR7: History filter. FR8: Auto database initialization.");

Heading("1.7.2 Non-Functional Requirements", 3);
Para("NFR1: Response within 2 seconds. NFR2: Consistent UI theme. NFR3: Offline operation. NFR4: Data persistence. NFR5: Windows 10+ with .NET 4.8.");

Heading("1.7.3 Hardware Requirements", 3);
Para("Intel Core i3+, 4 GB RAM, 100 MB disk, 1024x768 display, keyboard and mouse.");

Heading("1.7.4 Software Requirements", 3);
Para("Windows 10/11, .NET Framework 4.8, Visual Studio 2022, Microsoft.Data.Sqlite 8.0.7.");

Heading("1.8 Stakeholders", 2);
Para("End user (patient/caregiver), supervisor, evaluators, developers, Department of Computer Science UAF.");
PageBreak();

// Chapter 2
Heading("CHAPTER 2 - MATERIALS AND METHODS", 1);
Heading("2.1 Process Model", 2);
Para("Incremental development: requirements, database design, UI modules (login, medicines, reminders, history), integration testing, documentation.");

Heading("2.2 Tools and Technologies", 2);
Table(new[] { "Tool", "Purpose" }, new List<string[]>
{
    new[] { "C#", "Programming language" },
    new[] { ".NET Framework 4.8", "Runtime platform" },
    new[] { "Windows Forms", "GUI framework" },
    new[] { "Visual Studio 2022", "Development IDE" },
    new[] { "SQLite", "Embedded database" },
    new[] { "Microsoft.Data.Sqlite", "Data access provider" }
});

Heading("2.3 Design", 2);
Heading("2.3.1 System Features", 3);
Para("Authentication, Medicine CRUD with search, Reminder list with timer alerts, History filter, automatic database setup.");

Heading("2.3.2 Modules Description", 3);
Para(@"LoginForm: Validates credentials (admin/1234 hardcoded or DB users); opens DashboardForm.
DashboardForm: Navigation hub with welcome message.
MedicineForm: CRUD on Medicines table with HH:mm validation.
ReminderForm: Timer-based alerts; Taken/Missed saves to ReminderHistory.
HistoryForm: Filter by date and status.
DatabaseHelper: InitializeDatabase, ExecuteQuery, ExecuteNonQuery, ExecuteScalar.
Session & UiTheme: Username storage and shared colors.");

Heading("2.3.3 Data Flow Diagram", 3);
Para("Level 0 DFD: User ↔ Medicine Reminder System ↔ SQLite Database.");
AddImage(Path.Combine(assetsDir, "DFD_Level0.png"), "Figure 2.1: Level 0 Data Flow Diagram");

Heading("2.3.4 ER Diagram", 3);
Para("Users (standalone). Medicines 1:N ReminderHistory via MedicineId FK.");
AddImage(Path.Combine(assetsDir, "ER_Diagram.png"), "Figure 2.2: Entity Relationship Diagram");

Heading("2.3.5 Database Design", 3);
Table(new[] { "Table", "Key Columns", "Description" }, new List<string[]>
{
    new[] { "Users", "Id, Username, Password", "Authentication" },
    new[] { "Medicines", "Id, MedicineName, Dosage, ReminderTime, Frequency", "Medicine master" },
    new[] { "ReminderHistory", "Id, MedicineId, ReminderDate, Status", "Daily adherence log" }
});

Heading("2.3.6 System Workflow", 3);
Para("Start → Initialize DB → Login → Dashboard → Manage Medicines → Reminders (alerts) → Mark Taken/Missed → View History → Logout.");

Heading("2.3.7 Architecture", 3);
AddImage(Path.Combine(assetsDir, "Architecture.png"), "Figure 2.3: Three-Tier Architecture");

Heading("2.3.8 Existing System vs Proposed System", 3);
Para("Existing: Manual notes and phone alarms without records. Proposed: Integrated desktop app with schedules, alerts, and searchable history.");

PageBreak();

// Chapter 3
Heading("CHAPTER 3 - RESULTS AND DISCUSSION", 1);
Heading("3.1 Screenshots with Explanation", 2);
AddImage(Path.Combine(assetsDir, "Screenshot_Login.png"), "Figure 3.1: Login Form");
AddImage(Path.Combine(assetsDir, "Screenshot_Dashboard.png"), "Figure 3.2: Dashboard");
AddImage(Path.Combine(assetsDir, "Screenshot_Medicine.png"), "Figure 3.3: Medicine Management");
AddImage(Path.Combine(assetsDir, "Screenshot_Reminder.png"), "Figure 3.4: Reminder List");
AddImage(Path.Combine(assetsDir, "Screenshot_History.png"), "Figure 3.5: History");

Heading("3.2 Testing", 2);
Para("Black-box functional testing performed on all modules against requirements.");

Heading("3.3 Test Cases", 2);
Table(new[] { "ID", "Test Case", "Input", "Expected", "Status" }, new List<string[]>
{
    new[] { "TC01", "Valid login", "admin/1234", "Dashboard", "Pass" },
    new[] { "TC02", "Invalid login", "wrong/wrong", "Error", "Pass" },
    new[] { "TC03", "Add medicine", "Paracetamol 09:00", "Saved", "Pass" },
    new[] { "TC04", "Invalid time", "25:99", "Warning", "Pass" },
    new[] { "TC05", "Mark Taken", "Select+Taken", "Status Taken", "Pass" },
    new[] { "TC06", "History filter", "Date+Missed", "Filtered rows", "Pass" }
});

Heading("3.4 Advantages", 2);
Para("Simple UI, offline SQLite, automatic alerts, adherence history, low hardware needs, modular code.");

Heading("3.5 Limitations", 2);
Para("Desktop only, plain-text passwords, 60-second timer granularity, no cloud sync, single-user focus.");

Heading("3.6 Future Enhancements", 2);
Para("Password hashing, SMS/email alerts, mobile app, cloud sync, PDF reports, barcode scanning, multi-user roles.");

Heading("3.7 Conclusion", 2);
Para("The Medicine Reminder System successfully meets its objectives for schedule management, alerts, and history tracking using C# WinForms and SQLite, suitable for academic demonstration and personal use.");
PageBreak();

Heading("CHAPTER 4 - USER MANUAL", 1);
Para(@"Installation: Open MedicineReminderApp.sln in Visual Studio 2022, restore NuGet, press F5.
Login: admin / 1234 (or admin/admin from database).
Dashboard: Use buttons for Medicines, Reminders, History, Logout.
Medicines: Enter name, dosage, time (HH:mm), frequency; use Add/Update/Delete/Search.
Reminders: Mark Taken/Missed; keep window open for popup alerts.
History: Select date and status, click Load.");
PageBreak();

Heading("REFERENCES", 1);
Para(@"[1] Microsoft, Windows Forms, https://learn.microsoft.com/dotnet/desktop/winforms/
[2] Microsoft, Microsoft.Data.Sqlite, https://learn.microsoft.com/dotnet/standard/data/sqlite/
[3] Gomaa, H., Software Modeling and Design.
[4] Sommerville, I., Software Engineering.
[5] SQLite Documentation, https://www.sqlite.org/docs.html");

mainPart.Document.Save();
Console.WriteLine($"Report saved: {outputPath}");
