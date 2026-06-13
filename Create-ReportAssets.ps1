$assets = Join-Path $PSScriptRoot 'ReportAssets'
New-Item -ItemType Directory -Force -Path $assets | Out-Null
Add-Type -AssemblyName System.Drawing

function New-FormMock {
    param([string]$Path, [string]$Title, [string[]]$Lines, [string[]]$Buttons)
    $bmp = New-Object System.Drawing.Bitmap 820, 520
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = 'AntiAlias'
    $g.Clear([System.Drawing.Color]::FromArgb(236, 240, 241))
    $hdr = [System.Drawing.Color]::FromArgb(44, 62, 80)
    $acc = [System.Drawing.Color]::FromArgb(52, 152, 219)
    $g.FillRectangle((New-Object System.Drawing.SolidBrush($hdr)), 0, 0, 820, 70)
    $g.DrawString($Title, (New-Object System.Drawing.Font('Segoe UI', 14, [System.Drawing.FontStyle]::Bold)), [System.Drawing.Brushes]::White, 20, 20)
    $g.FillRectangle([System.Drawing.Brushes]::White, 40, 90, 740, 380)
    $g.DrawRectangle((New-Object System.Drawing.Pen([System.Drawing.Color]::Gray)), 40, 90, 740, 380)
    $y = 110
    $f = New-Object System.Drawing.Font('Segoe UI', 10)
    foreach ($line in $Lines) { $g.DrawString($line, $f, [System.Drawing.Brushes]::Black, 60, $y); $y += 32 }
    $bx = 60
    foreach ($btn in $Buttons) {
        $g.FillRectangle((New-Object System.Drawing.SolidBrush($acc)), $bx, 420, 150, 40)
        $g.DrawString($btn, (New-Object System.Drawing.Font('Segoe UI', 9, [System.Drawing.FontStyle]::Bold)), [System.Drawing.Brushes]::White, ($bx + 15), 430)
        $bx += 170
    }
    $bmp.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
}

New-FormMock (Join-Path $assets 'Screenshot_Login.png') 'Medicine Reminder - Login' @('Username: admin','Password: ****','Validates against Users table') @('Login','Exit')
New-FormMock (Join-Path $assets 'Screenshot_Dashboard.png') 'Dashboard' @('Welcome, admin!','Choose an option below') @('Medicines','Reminders','History','Logout')
New-FormMock (Join-Path $assets 'Screenshot_Medicine.png') 'Manage Medicine' @('Medicine Name, Dosage, Time (HH:mm), Frequency','DataGridView listing all records') @('Add','Update','Delete','Search')
New-FormMock (Join-Path $assets 'Screenshot_Reminder.png') 'Reminder List' @('Today schedule with status column','Popup alert when time matches') @('Taken','Missed','Refresh')
New-FormMock (Join-Path $assets 'Screenshot_History.png') 'History' @('Filter by Date and Status (All/Taken/Missed)') @('Load')
Write-Host "Screenshots created in $assets"
