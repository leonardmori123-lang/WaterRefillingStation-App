using System;
using System.Windows.Forms;
using WaterRefillingStation.Database;

namespace WaterRefillingStation.Forms
{
    public partial class CustomersForm : Form
    {
        // 0 = no customer selected (adding a new one)
        private int selectedCustomerId = 0;

        public CustomersForm()
        {
            InitializeComponent();
        }

        private void CustomersForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            ResetForm();
        }

        private void LoadCustomers()
        {
            string like = "%" + txtSearch.Text.Trim() + "%";

            dgvCustomers.DataSource = Db.Query(
                "SELECT CustomerID, FullName, Address, Contact FROM Customers " +
                "WHERE IsActive = 1 AND (FullName LIKE @s OR Address LIKE @s OR Contact LIKE @s) " +
                "ORDER BY FullName",
                ("@s", like));

            if (dgvCustomers.Columns.Contains("CustomerID"))
            {
                dgvCustomers.Columns["CustomerID"].Visible = false;
                dgvCustomers.Columns["FullName"].HeaderText = "Full Name";
            }
        }

        private void ResetForm()
        {
            selectedCustomerId = 0;
            txtFullName.Clear();
            txtAddress.Clear();
            txtContact.Clear();
            dgvCustomers.ClearSelection();
            SetEditMode(false);
        }

        // Save is for new customers. Update and Delete are for a selected customer.
        private void SetEditMode(bool editing)
        {
            btnSave.Enabled = !editing;
            btnUpdate.Enabled = editing;
            btnDelete.Enabled = editing;
        }

        private bool ValidateInput()
        {
            if (txtFullName.Text.Trim().Length == 0)
            {
                Warn("Please enter the customer's full name.");
                txtFullName.Focus();
                return false;
            }
            if (txtAddress.Text.Trim().Length == 0)
            {
                Warn("Please enter the customer's address.");
                txtAddress.Focus();
                return false;
            }
            return true;
        }

        private static void Warn(string message)
        {
            MessageBox.Show(message, "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static string NullIfEmpty(string text)
        {
            text = text.Trim();
            return text.Length == 0 ? null : text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            Db.Execute(
                "INSERT INTO Customers (FullName, Address, Contact) VALUES (@name, @address, @contact)",
                ("@name", txtFullName.Text.Trim()),
                ("@address", txtAddress.Text.Trim()),
                ("@contact", NullIfEmpty(txtContact.Text)));

            LoadCustomers();
            ResetForm();
            txtFullName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0 || !ValidateInput()) return;

            Db.Execute(
                "UPDATE Customers SET FullName = @name, Address = @address, Contact = @contact " +
                "WHERE CustomerID = @id",
                ("@name", txtFullName.Text.Trim()),
                ("@address", txtAddress.Text.Trim()),
                ("@contact", NullIfEmpty(txtContact.Text)),
                ("@id", selectedCustomerId));

            LoadCustomers();
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0) return;

            DialogResult confirm = MessageBox.Show(
                "Delete this customer?", "Customers",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            long deliveries = Convert.ToInt64(Db.Scalar(
                "SELECT COUNT(*) FROM Deliveries WHERE CustomerID = @id",
                ("@id", selectedCustomerId)));

            if (deliveries == 0)
            {
                Db.Execute("DELETE FROM Customers WHERE CustomerID = @id", ("@id", selectedCustomerId));
            }
            else
            {
                // Customers with delivery records are hidden, not deleted, to keep the records.
                Db.Execute("UPDATE Customers SET IsActive = 0 WHERE CustomerID = @id", ("@id", selectedCustomerId));
                MessageBox.Show(
                    "This customer has delivery records, so they were hidden instead of deleted.",
                    "Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            LoadCustomers();
            ResetForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            txtFullName.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers();
            ResetForm();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
            selectedCustomerId = Convert.ToInt32(row.Cells["CustomerID"].Value);
            txtFullName.Text = Convert.ToString(row.Cells["FullName"].Value);
            txtAddress.Text = Convert.ToString(row.Cells["Address"].Value);
            txtContact.Text = Convert.ToString(row.Cells["Contact"].Value);
            SetEditMode(true);
        }
    }
}