using System;
using System.Windows.Forms;
using WaterRefillingStation.Database;
using WaterRefillingStation.Forms;
using WaterRefillingStation.Security;

namespace WaterRefillingStation
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Db.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The database could not be set up:\n\n" + ex.Message,
                    "Water Refilling Station",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            while (true)
            {
                using (var login = new LoginForm())
                {
                    if (login.ShowDialog() != DialogResult.OK) return;
                }

                Application.Run(new MainForm());

                // Still logged in means the window was closed with X, so exit.
                // Logout calls Session.End(), so the login screen shows again.
                if (Session.IsLoggedIn) return;
            }
        }
    }
}