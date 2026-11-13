using System;
using System.Drawing;
using System.Windows.Forms;
using WaterRefillingStation.Database;

namespace WaterRefillingStation.Forms
{
    public partial class SuppliesForm : Form
    {
        private const int DefaultMinimumStock = 10;

        // 0 = no supply selected (adding a new one)
        private int selectedSupplyId = 0;

        public SuppliesForm()
        {
            InitializeComponent();
        }

        private void SuppliesForm_Load(object sender, EventArgs e)
        {
            LoadSupplies();
            ResetForm();
        }

        private void LoadSupplies()
        {
            string like = "%" + txtSearch.Text.Trim() + "%";

            dgvSupplies.DataSource = Db.Query(
                "SELECT SupplyID, ItemName, Unit, CurrentStock, MinimumStock, " +
                "CASE WHEN CurrentStock <= MinimumStock THEN 'Low' ELSE 'OK' END AS Status " +
                "FROM Supplies " +
                "WHERE (ItemName LIKE @s OR Unit LIKE @s) " +
                "AND (@lowOnly = 0 OR CurrentStock <= MinimumStock) " +
                "ORDER BY ItemName",
                ("@s", like), ("@lowOnly", chkLowOnly.Checked ? 1 : 0));

            if (!dgvSupplies.Columns.Contains("SupplyID")) return;

            dgvSupplies.Columns["SupplyID"].Visible = false;
            dgvSupplies.Columns["ItemName"].HeaderText = "Item";
            dgvSupplies.Columns["CurrentStock"].HeaderText = "In Stock";
            dgvSupplies.Columns["MinimumStock"].HeaderText = "Minimum";
        }

        private void ResetForm()
        {
            selectedSupplyId = 0;
            txtItemName.Clear();
            txtUnit.Clear();
            nudCurrentStock.Value = 0;
            nudMinimumStock.Value = DefaultMinimumStock;
            dgvSupplies.ClearSelection();
            SetEditMode(false);
        }

        // Save is for new supplies. Update and Delete are for a selected supply.
        private void SetEditMode(bool editing)
        {
            btnSave.Enabled = !editing;
            btnUpdate.Enabled = editing;
            btnDelete.Enabled = editing;
        }

        private bool ValidateInput()
        {
            if (txtItemName.Text.Trim().Length == 0)
            {
                Warn("Please enter the item name.");
                txtItemName.Focus();
                return false;
            }

            long taken = Convert.ToInt64(Db.Scalar(
                "SELECT COUNT(*) FROM Supplies WHERE ItemName = @name COLLATE NOCASE AND SupplyID <> @id",
                ("@name", txtItemName.Text.Trim()), ("@id", selectedSupplyId)));
            if (taken > 0)
            {
                Warn("That item already exists.");
                txtItemName.Focus();
                return false;
            }

            return true;
        }

        private static void Warn(string message)
        {
            MessageBox.Show(message, "Supplies", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                "INSERT INTO Supplies (ItemName, Unit, CurrentStock, MinimumStock) " +
                "VALUES (@name, @unit, @stock, @minimum)",
                ("@name", txtItemName.Text.Trim()),
                ("@unit", NullIfEmpty(txtUnit.Text)),
                ("@stock", (int)nudCurrentStock.Value),
                ("@minimum", (int)nudMinimumStock.Value));

            LoadSupplies();
            ResetForm();
            txtItemName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedSupplyId == 0 || !ValidateInput()) return;

            Db.Execute(
                "UPDATE Supplies SET ItemName = @name, Unit = @unit, CurrentStock = @stock, " +
                "MinimumStock = @minimum, UpdatedAt = datetime('now', 'localtime') WHERE SupplyID = @id",
                ("@name", txtItemName.Text.Trim()),
                ("@unit", NullIfEmpty(txtUnit.Text)),
                ("@stock", (int)nudCurrentStock.Value),
                ("@minimum", (int)nudMinimumStock.Value),
                ("@id", selectedSupplyId));

            LoadSupplies();
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedSupplyId == 0) return;

            DialogResult confirm = MessageBox.Show(
                "Delete this supply item?", "Supplies",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            Db.Execute("DELETE FROM Supplies WHERE SupplyID = @id", ("@id", selectedSupplyId));

            LoadSupplies();
            ResetForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            txtItemName.Focus();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            LoadSupplies();
            ResetForm();
        }

        // Low-stock items are shown in red.
        private void dgvSupplies_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvSupplies.Columns.Contains("Status")) return;

            if (Convert.ToString(dgvSupplies.Rows[e.RowIndex].Cells["Status"].Value) == "Low")
            {
                e.CellStyle.ForeColor = Color.DarkRed;
                e.CellStyle.BackColor = Color.MistyRose;
            }
        }

        private void dgvSupplies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSupplies.Rows[e.RowIndex];
            selectedSupplyId = Convert.ToInt32(row.Cells["SupplyID"].Value);
            txtItemName.Text = Convert.ToString(row.Cells["ItemName"].Value);
            txtUnit.Text = Convert.ToString(row.Cells["Unit"].Value);
            nudCurrentStock.Value = Convert.ToDecimal(row.Cells["CurrentStock"].Value);
            nudMinimumStock.Value = Convert.ToDecimal(row.Cells["MinimumStock"].Value);
            SetEditMode(true);
        }
    }
}