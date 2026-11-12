using System;
using System.Drawing;
using System.Windows.Forms;
using WaterRefillingStation.Database;
using WaterRefillingStation.Security;

namespace WaterRefillingStation.Forms
{
    public partial class UsersForm : Form
    {
        private const int MinPasswordLength = 6;

        // 0 = no user selected (adding a new one)
        private int selectedUserId = 0;

        public UsersForm()
        {
            InitializeComponent();
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            if (!Session.IsAdmin)
            {
                MessageBox.Show("Only an Admin can manage user accounts.", "Users",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            LoadUsers();
            ResetForm();
        }

        private void LoadUsers()
        {
            dgvUsers.DataSource = Db.Query(
                "SELECT UserID, Username, FullName, Role, " +
                "CASE WHEN IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status " +
                "FROM Users ORDER BY Username");

            if (!dgvUsers.Columns.Contains("UserID")) return;

            dgvUsers.Columns["UserID"].Visible = false;
            dgvUsers.Columns["FullName"].HeaderText = "Full Name";
        }

        private void ResetForm()
        {
            selectedUserId = 0;
            txtUsername.Clear();
            txtFullName.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            cboRole.SelectedItem = "Staff";
            chkActive.Checked = true;
            dgvUsers.ClearSelection();
            SetEditMode(false);
        }

        // Save is for new users. Update and Delete are for a selected user.
        private void SetEditMode(bool editing)
        {
            btnSave.Enabled = !editing;
            btnUpdate.Enabled = editing;
            btnDelete.Enabled = editing;
            lblPasswordHint.Text = editing
                ? "Leave blank to keep the current password"
                : "Minimum " + MinPasswordLength + " characters";
        }

        private bool ValidateInput(bool isNew)
        {
            if (txtUsername.Text.Trim().Length == 0)
            {
                Warn("Please enter a username.");
                txtUsername.Focus();
                return false;
            }
            if (txtFullName.Text.Trim().Length == 0)
            {
                Warn("Please enter the full name.");
                txtFullName.Focus();
                return false;
            }

            string password = txtPassword.Text;
            if (isNew || password.Length > 0)
            {
                if (password.Length < MinPasswordLength)
                {
                    Warn("The password must be at least " + MinPasswordLength + " characters.");
                    txtPassword.Focus();
                    return false;
                }
                if (password != txtConfirmPassword.Text)
                {
                    Warn("The passwords do not match.");
                    txtConfirmPassword.Focus();
                    return false;
                }
            }

            long taken = Convert.ToInt64(Db.Scalar(
                "SELECT COUNT(*) FROM Users WHERE Username = @username COLLATE NOCASE AND UserID <> @id",
                ("@username", txtUsername.Text.Trim()), ("@id", selectedUserId)));
            if (taken > 0)
            {
                Warn("That username is already taken.");
                txtUsername.Focus();
                return false;
            }

            return true;
        }

        // Stops the admin from locking themselves out or leaving the system with no active Admin.
        private bool ValidateAdminRules()
        {
            bool staysActiveAdmin = Convert.ToString(cboRole.SelectedItem) == "Admin" && chkActive.Checked;

            if (selectedUserId == Session.UserID && !staysActiveAdmin)
            {
                Warn("You cannot deactivate your own account or remove your own Admin role.");
                return false;
            }

            if (!staysActiveAdmin && CountOtherActiveAdmins() == 0)
            {
                Warn("There must be at least one active Admin account.");
                return false;
            }

            return true;
        }

        private long CountOtherActiveAdmins()
        {
            return Convert.ToInt64(Db.Scalar(
                "SELECT COUNT(*) FROM Users WHERE Role = 'Admin' AND IsActive = 1 AND UserID <> @id",
                ("@id", selectedUserId)));
        }

        private static void Warn(string message)
        {
            MessageBox.Show(message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(true)) return;

            Db.Execute(
                "INSERT INTO Users (Username, PasswordHash, FullName, Role, IsActive) " +
                "VALUES (@username, @hash, @fullName, @role, @active)",
                ("@username", txtUsername.Text.Trim()),
                ("@hash", PasswordHasher.Hash(txtPassword.Text)),
                ("@fullName", txtFullName.Text.Trim()),
                ("@role", Convert.ToString(cboRole.SelectedItem)),
                ("@active", chkActive.Checked ? 1 : 0));

            LoadUsers();
            ResetForm();
            txtUsername.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserId == 0 || !ValidateInput(false) || !ValidateAdminRules()) return;

            if (txtPassword.Text.Length > 0)
            {
                Db.Execute(
                    "UPDATE Users SET Username = @username, FullName = @fullName, Role = @role, " +
                    "IsActive = @active, PasswordHash = @hash WHERE UserID = @id",
                    ("@username", txtUsername.Text.Trim()),
                    ("@fullName", txtFullName.Text.Trim()),
                    ("@role", Convert.ToString(cboRole.SelectedItem)),
                    ("@active", chkActive.Checked ? 1 : 0),
                    ("@hash", PasswordHasher.Hash(txtPassword.Text)),
                    ("@id", selectedUserId));
            }
            else
            {
                Db.Execute(
                    "UPDATE Users SET Username = @username, FullName = @fullName, Role = @role, " +
                    "IsActive = @active WHERE UserID = @id",
                    ("@username", txtUsername.Text.Trim()),
                    ("@fullName", txtFullName.Text.Trim()),
                    ("@role", Convert.ToString(cboRole.SelectedItem)),
                    ("@active", chkActive.Checked ? 1 : 0),
                    ("@id", selectedUserId));
            }

            LoadUsers();
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserId == 0) return;

            if (selectedUserId == Session.UserID)
            {
                Warn("You cannot delete your own account.");
                return;
            }

            bool isActiveAdmin = Convert.ToInt64(Db.Scalar(
                "SELECT COUNT(*) FROM Users WHERE UserID = @id AND Role = 'Admin' AND IsActive = 1",
                ("@id", selectedUserId))) > 0;
            if (isActiveAdmin && CountOtherActiveAdmins() == 0)
            {
                Warn("There must be at least one active Admin account.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Permanently delete this user? This cannot be undone.\n\n" +
                "Their past deliveries and payments are kept, but will no longer show who entered them.\n" +
                "To only block their login, untick Active and click Update instead.",
                "Users", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (confirm != DialogResult.Yes) return;

            Db.Execute("DELETE FROM Users WHERE UserID = @id", ("@id", selectedUserId));

            LoadUsers();
            ResetForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            txtUsername.Focus();
        }

        // Inactive users are shown in grey.
        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvUsers.Columns.Contains("Status")) return;

            if (Convert.ToString(dgvUsers.Rows[e.RowIndex].Cells["Status"].Value) == "Inactive")
            {
                e.CellStyle.ForeColor = Color.DimGray;
                e.CellStyle.BackColor = Color.Gainsboro;
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
            selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);
            txtUsername.Text = Convert.ToString(row.Cells["Username"].Value);
            txtFullName.Text = Convert.ToString(row.Cells["FullName"].Value);
            cboRole.SelectedItem = Convert.ToString(row.Cells["Role"].Value);
            chkActive.Checked = Convert.ToString(row.Cells["Status"].Value) == "Active";
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            SetEditMode(true);
        }
    }
}