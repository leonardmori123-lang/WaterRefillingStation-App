using System;
using System.Globalization;
using System.Windows.Forms;
using WaterRefillingStation.Database;
using WaterRefillingStation.Security;

namespace WaterRefillingStation.Forms
{
    public partial class DeliveriesForm : Form
    {
        // 0 = no delivery selected (adding a new one)
        private int selectedDeliveryId = 0;
        private bool isLoading = true;

        public DeliveriesForm()
        {
            InitializeComponent();
        }

        private void DeliveriesForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            cboFilter.SelectedIndex = 0;
            isLoading = false;

            LoadDeliveries();
            ResetForm();

            if (cboCustomer.Items.Count == 0)
                MessageBox.Show("There are no customers yet. Please add a customer first.",
                    "Deliveries", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadCustomers()
        {
            cboCustomer.DisplayMember = "FullName";
            cboCustomer.ValueMember = "CustomerID";
            cboCustomer.DataSource = Db.Query(
                "SELECT CustomerID, FullName FROM Customers WHERE IsActive = 1 ORDER BY FullName");
            cboCustomer.SelectedIndex = -1;
        }

        private void LoadDeliveries()
        {
            string like = "%" + txtSearch.Text.Trim() + "%";
            string status = Convert.ToString(cboFilter.SelectedItem);

            dgvDeliveries.DataSource = Db.Query(
                "SELECT DeliveryID, CustomerID, CustomerName, DeliveryDate, GallonsRefilled, " +
                "GallonsDelivered, PricePerGallon, TotalAmount, Status, PaymentStatus " +
                "FROM vw_DeliveryBalance " +
                "WHERE CustomerName LIKE @s AND (@status = 'All' OR Status = @status) " +
                "ORDER BY DeliveryDate DESC, DeliveryID DESC",
                ("@s", like), ("@status", status));

            if (!dgvDeliveries.Columns.Contains("DeliveryID")) return;

            dgvDeliveries.Columns["DeliveryID"].Visible = false;
            dgvDeliveries.Columns["CustomerID"].Visible = false;
            dgvDeliveries.Columns["CustomerName"].HeaderText = "Customer";
            dgvDeliveries.Columns["DeliveryDate"].HeaderText = "Date";
            dgvDeliveries.Columns["GallonsRefilled"].HeaderText = "Refilled";
            dgvDeliveries.Columns["GallonsDelivered"].HeaderText = "Delivered";
            dgvDeliveries.Columns["PricePerGallon"].HeaderText = "Price";
            dgvDeliveries.Columns["PricePerGallon"].DefaultCellStyle.Format = "N2";
            dgvDeliveries.Columns["TotalAmount"].HeaderText = "Total";
            dgvDeliveries.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
            dgvDeliveries.Columns["PaymentStatus"].HeaderText = "Payment";
        }

        private void ResetForm()
        {
            selectedDeliveryId = 0;
            cboCustomer.SelectedIndex = -1;
            dtpDeliveryDate.Value = DateTime.Today;
            nudGallonsRefilled.Value = 0;
            nudGallonsDelivered.Value = 0;
            cboStatus.SelectedIndex = 0;
            dgvDeliveries.ClearSelection();
            SetEditMode(false, false);
            UpdateTotal();
        }

        // Save is for new deliveries. Update, Delete, and Mark as Delivered are for a selected one.
        private void SetEditMode(bool editing, bool isPending)
        {
            btnSave.Enabled = !editing;
            btnUpdate.Enabled = editing;
            btnDelete.Enabled = editing;
            btnMarkDelivered.Enabled = editing && isPending;
        }

        private void UpdateTotal()
        {
            decimal total = nudGallonsRefilled.Value * nudPrice.Value;
            lblTotal.Text = "Total: \u20B1 " + total.ToString("N2");
        }

        private bool ValidateInput()
        {
            if (cboCustomer.SelectedIndex < 0)
            {
                Warn("Please select a customer.");
                cboCustomer.Focus();
                return false;
            }
            if (nudGallonsRefilled.Value <= 0)
            {
                Warn("Gallons refilled must be more than 0.");
                nudGallonsRefilled.Focus();
                return false;
            }
            if (nudGallonsDelivered.Value > nudGallonsRefilled.Value)
            {
                Warn("Gallons delivered cannot be more than gallons refilled.");
                nudGallonsDelivered.Focus();
                return false;
            }
            if (nudPrice.Value <= 0)
            {
                Warn("Price per gallon must be more than 0.");
                nudPrice.Focus();
                return false;
            }
            if (Convert.ToString(cboStatus.SelectedItem) == "Delivered" && nudGallonsDelivered.Value <= 0)
            {
                Warn("Enter the gallons delivered, or set the status to Pending.");
                nudGallonsDelivered.Focus();
                return false;
            }
            return true;
        }

        private static void Warn(string message)
        {
            MessageBox.Show(message, "Deliveries", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static string ToDbDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static object CurrentUserId()
        {
            return Session.IsLoggedIn ? (object)Session.UserID : null;
        }

        private double CalculateTotal()
        {
            return (double)Math.Round(nudGallonsRefilled.Value * nudPrice.Value, 2);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            Db.Execute(
                "INSERT INTO Deliveries (CustomerID, DeliveryDate, GallonsRefilled, GallonsDelivered, " +
                "PricePerGallon, TotalAmount, Status, CreatedBy) " +
                "VALUES (@customer, @date, @refilled, @delivered, @price, @total, @status, @user)",
                ("@customer", cboCustomer.SelectedValue),
                ("@date", ToDbDate(dtpDeliveryDate.Value)),
                ("@refilled", (int)nudGallonsRefilled.Value),
                ("@delivered", (int)nudGallonsDelivered.Value),
                ("@price", (double)nudPrice.Value),
                ("@total", CalculateTotal()),
                ("@status", Convert.ToString(cboStatus.SelectedItem)),
                ("@user", CurrentUserId()));

            LoadDeliveries();
            ResetForm();
            cboCustomer.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedDeliveryId == 0 || !ValidateInput()) return;

            Db.Execute(
                "UPDATE Deliveries SET CustomerID = @customer, DeliveryDate = @date, " +
                "GallonsRefilled = @refilled, GallonsDelivered = @delivered, PricePerGallon = @price, " +
                "TotalAmount = @total, Status = @status WHERE DeliveryID = @id",
                ("@customer", cboCustomer.SelectedValue),
                ("@date", ToDbDate(dtpDeliveryDate.Value)),
                ("@refilled", (int)nudGallonsRefilled.Value),
                ("@delivered", (int)nudGallonsDelivered.Value),
                ("@price", (double)nudPrice.Value),
                ("@total", CalculateTotal()),
                ("@status", Convert.ToString(cboStatus.SelectedItem)),
                ("@id", selectedDeliveryId));

            LoadDeliveries();
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedDeliveryId == 0) return;

            DialogResult confirm = MessageBox.Show(
                "Delete this delivery? Its payments will also be deleted.", "Deliveries",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            Db.Execute("DELETE FROM Deliveries WHERE DeliveryID = @id", ("@id", selectedDeliveryId));

            LoadDeliveries();
            ResetForm();
        }

        // Sets the delivery date to today. If no delivered count was entered, uses the refilled count.
        private void btnMarkDelivered_Click(object sender, EventArgs e)
        {
            if (selectedDeliveryId == 0) return;

            Db.Execute(
                "UPDATE Deliveries SET Status = 'Delivered', DeliveryDate = @today, " +
                "GallonsDelivered = CASE WHEN GallonsDelivered = 0 THEN GallonsRefilled ELSE GallonsDelivered END " +
                "WHERE DeliveryID = @id",
                ("@today", ToDbDate(DateTime.Today)),
                ("@id", selectedDeliveryId));

            LoadDeliveries();
            ResetForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            cboCustomer.Focus();
        }

        private void nudTotal_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotal();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            LoadDeliveries();
            ResetForm();
        }

        private void dgvDeliveries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvDeliveries.Rows[e.RowIndex];
            selectedDeliveryId = Convert.ToInt32(row.Cells["DeliveryID"].Value);

            cboCustomer.SelectedValue = Convert.ToInt32(row.Cells["CustomerID"].Value);
            dtpDeliveryDate.Value = DateTime.ParseExact(
                Convert.ToString(row.Cells["DeliveryDate"].Value), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            nudGallonsRefilled.Value = Convert.ToDecimal(row.Cells["GallonsRefilled"].Value);
            nudGallonsDelivered.Value = Convert.ToDecimal(row.Cells["GallonsDelivered"].Value);
            nudPrice.Value = Convert.ToDecimal(row.Cells["PricePerGallon"].Value);

            string status = Convert.ToString(row.Cells["Status"].Value);
            cboStatus.SelectedItem = status;
            SetEditMode(true, status == "Pending");
            UpdateTotal();

            if (cboCustomer.SelectedIndex < 0)
                Warn("This delivery belongs to a hidden customer. Pick a customer before updating it.");
        }
    }
}