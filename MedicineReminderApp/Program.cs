using System;
using System.Windows.Forms;

namespace MedicineReminderApp
{
    internal static class Program
    {
        
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DatabaseHelper.InitializeDatabase();

            Application.Run(new LoginForm());
        }
    }
}
