using System;
using System.Data;
using System.Windows.Forms;
using WaterRefillingStation.Database;
using WaterRefillingStation.Security;

namespace WaterRefillingStation.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Please enter your username and password.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable result = Db.Query(
                "SELECT UserID, PasswordHash, FullName, Role FROM Users " +
                "WHERE Username = @username AND IsActive = 1",
                ("@username", username));

            if (result.Rows.Count == 0 ||
                !PasswordHasher.Verify(password, Convert.ToString(result.Rows[0]["PasswordHash"])))
            {
                MessageBox.Show("Invalid username or password.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            DataRow row = result.Rows[0];
            Session.Start(
                Convert.ToInt32(row["UserID"]),
                username,
                Convert.ToString(row["FullName"]),
                Convert.ToString(row["Role"]));

            DialogResult = DialogResult.OK;
        }
    }
}   