using System;
using System.Globalization;
using System.Windows.Forms;
using WaterRefillingStation.Database;
using WaterRefillingStation.Security;

namespace WaterRefillingStation.Forms
{
    public partial class PaymentsForm : Form
    {
        // 0 = no payment selected (adding a new one)
        private int selectedPaymentId = 0;

        private long editingDeliveryId = 0;
        private decimal editingAmount = 0m;

        private bool suppressEvents = true;

        public PaymentsForm()
        {
            InitializeComponent();
        }

        private void PaymentsForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            LoadPayments();
            ResetForm();

            if (cboCustomer.Items.Count == 0)
                MessageBox.Show("There are no customers yet. Please add a customer first.",
                    "Payments", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Includes hidden customers who have deliveries, so their balances can still be paid.
        private void LoadCustomers()
        {
            cboCustomer.DisplayMember = "FullName";
            cboCustomer.ValueMember = "CustomerID";
            cboCustomer.DataSource = Db.Query(
                "SELECT CustomerID, FullName FROM Customers " +
                "WHERE IsActive = 1 OR CustomerID IN (SELECT CustomerID FROM Deliveries) " +
                "ORDER BY FullName");
            cboCustomer.SelectedIndex = -1;
        }

        private void LoadPayments()
        {
            string like = "%" + txtSearch.Text.Trim() + "%";

            dgvPayments.DataSource = Db.Query(
                "SELECT p.PaymentID, p.DeliveryID, b.CustomerID, b.CustomerName, b.DeliveryDate, " +
                "b.TotalAmount, p.AmountPaid, p.PaymentDate, b.Balance, b.PaymentStatus, " +
                "u.FullName AS ReceivedBy " +
                "FROM Payments p " +
                "JOIN vw_DeliveryBalance b ON b.DeliveryID = p.DeliveryID " +
                "LEFT JOIN Users u ON u.UserID = p.ReceivedBy " +
                "WHERE b.CustomerName LIKE @s OR b.DeliveryDate LIKE @s OR p.PaymentDate LIKE @s " +
                "ORDER BY p.PaymentDate DESC, p.PaymentID DESC",
                ("@s", like));

            if (!dgvPayments.Columns.Contains("PaymentID")) return;

            dgvPayments.Columns["PaymentID"].Visible = false;
            dgvPayments.Columns["DeliveryID"].Visible = false;
            dgvPayments.Columns["CustomerID"].Visible = false;
            dgvPayments.Columns["CustomerName"].HeaderText = "Customer";
            dgvPayments.Columns["DeliveryDate"].HeaderText = "Delivery Date";
            dgvPayments.Columns["TotalAmount"].HeaderText = "Delivery Total";
            dgvPayments.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns["AmountPaid"].HeaderText = "Paid";
            dgvPayments.Columns["AmountPaid"].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns["PaymentDate"].HeaderText = "Payment Date";
            dgvPayments.Columns["Balance"].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns["PaymentStatus"].HeaderText = "Status";
            dgvPayments.Columns["ReceivedBy"].HeaderText = "Received By";
        }

        // Lists the customer's unpaid deliveries, oldest first.
        // includeDeliveryId keeps an already-paid delivery in the list when editing its payment.
        private void LoadDeliveryChoices(long customerId, long includeDeliveryId)
        {
            suppressEvents = true;

            cboDelivery.DisplayMember = "DisplayText";
            cboDelivery.ValueMember = "DeliveryID";
            cboDelivery.DataSource = Db.Query(
                "SELECT DeliveryID, " +
                "DeliveryDate || '  |  ' || GallonsRefilled || ' gal  |  Balance ' || printf('%.2f', Balance) AS DisplayText " +
                "FROM vw_DeliveryBalance " +
                "WHERE CustomerID = @customer AND (Balance > 0 OR DeliveryID = @include) " +
                "ORDER BY DeliveryDate, DeliveryID",
                ("@customer", customerId), ("@include", includeDeliveryId));

            if (includeDeliveryId > 0)
                cboDelivery.SelectedValue = includeDeliveryId;
            else
                cboDelivery.SelectedIndex = cboDelivery.Items.Count > 0 ? 0 : -1;

            suppressEvents = false;
        }

        private void ResetForm()
        {
            suppressEvents = true;

            selectedPaymentId = 0;
            editingDeliveryId = 0;
            editingAmount = 0m;

            cboCustomer.SelectedIndex = -1;
            cboDelivery.DataSource = null;
            dtpPaymentDate.Value = DateTime.Today;
            SetAmount(0m, 0m);
            dgvPayments.ClearSelection();

            suppressEvents = false;
            SetEditMode(false);
        }

        // Save is for new payments. Update and Delete are for a selected payment.
        // The customer and delivery cannot be changed once a payment exists.
        private void SetEditMode(bool editing)
        {
            btnSave.Enabled = !editing;
            btnUpdate.Enabled = editing;
            btnDelete.Enabled = editing;
            cboCustomer.Enabled = !editing;
            cboDelivery.Enabled = !editing;
        }

        // payable = the most that can be paid on the selected delivery.
        // The box's Maximum is left alone on purpose: the box would silently cap a typed amount,
        // so ValidateInput checks it against payable and shows a message instead.
        private void SetAmount(decimal payable, decimal value)
        {
            txtAmountDue.Text = "\u20B1 " + payable.ToString("N2");
            nudAmountPaid.Value = value;
        }

        private decimal GetPayable(long deliveryId)
        {
            object balance = Db.Scalar(
                "SELECT Balance FROM vw_DeliveryBalance WHERE DeliveryID = @id",
                ("@id", deliveryId));

            decimal payable = balance == null ? 0m : Math.Round(Convert.ToDecimal(balance), 2);
            if (selectedPaymentId > 0 && deliveryId == editingDeliveryId)
                payable += editingAmount;

            return Math.Max(payable, 0m);
        }

        // For a new payment, the amount defaults to the full balance of the selected delivery.
        private void ShowNewPaymentAmount()
        {
            if (cboDelivery.SelectedIndex < 0 || cboDelivery.SelectedValue == null)
            {
                SetAmount(0m, 0m);
                return;
            }

            decimal payable = GetPayable(Convert.ToInt64(cboDelivery.SelectedValue));
            SetAmount(payable, payable);
        }

        private bool ValidateInput()
        {
            if (cboCustomer.SelectedIndex < 0)
            {
                Warn("Please select a customer.");
                cboCustomer.Focus();
                return false;
            }
            if (cboDelivery.SelectedIndex < 0 || cboDelivery.SelectedValue == null)
            {
                Warn("This customer has no unpaid deliveries.");
                cboCustomer.Focus();
                return false;
            }
            if (nudAmountPaid.Value <= 0)
            {
                Warn("The payment amount must be more than 0.");
                nudAmountPaid.Focus();
                return false;
            }

            decimal payable = GetPayable(Convert.ToInt64(cboDelivery.SelectedValue));
            if (nudAmountPaid.Value > payable)
            {
                Warn("The payment cannot be more than the balance (" + payable.ToString("N2") + ").");
                nudAmountPaid.Focus();
                return false;
            }

            return true;
        }

        private static void Warn(string message)
        {
            MessageBox.Show(message, "Payments", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static string ToDbDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static object CurrentUserId()
        {
            return Session.IsLoggedIn ? (object)Session.UserID : null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            Db.Execute(
                "INSERT INTO Payments (DeliveryID, AmountPaid, PaymentDate, ReceivedBy) " +
                "VALUES (@delivery, @amount, @date, @user)",
                ("@delivery", Convert.ToInt64(cboDelivery.SelectedValue)),
                ("@amount", (double)nudAmountPaid.Value),
                ("@date", ToDbDate(dtpPaymentDate.Value)),
                ("@user", CurrentUserId()));

            LoadPayments();
            ResetForm();
            cboCustomer.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedPaymentId == 0 || !ValidateInput()) return;

            Db.Execute(
                "UPDATE Payments SET AmountPaid = @amount, PaymentDate = @date WHERE PaymentID = @id",
                ("@amount", (double)nudAmountPaid.Value),
                ("@date", ToDbDate(dtpPaymentDate.Value)),
                ("@id", selectedPaymentId));

            LoadPayments();
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPaymentId == 0) return;

            DialogResult confirm = MessageBox.Show(
                "Delete this payment? The delivery's balance will go back up.", "Payments",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            Db.Execute("DELETE FROM Payments WHERE PaymentID = @id", ("@id", selectedPaymentId));

            LoadPayments();
            ResetForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            cboCustomer.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadPayments();
            ResetForm();
        }

        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEvents) return;

            if (cboCustomer.SelectedIndex < 0 || cboCustomer.SelectedValue == null)
            {
                cboDelivery.DataSource = null;
                SetAmount(0m, 0m);
                return;
            }

            LoadDeliveryChoices(Convert.ToInt64(cboCustomer.SelectedValue), 0);
            ShowNewPaymentAmount();
        }

        private void cboDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressEvents) return;
            ShowNewPaymentAmount();
        }

        private void dgvPayments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPayments.Rows[e.RowIndex];
            long customerId = Convert.ToInt64(row.Cells["CustomerID"].Value);

            selectedPaymentId = Convert.ToInt32(row.Cells["PaymentID"].Value);
            editingDeliveryId = Convert.ToInt64(row.Cells["DeliveryID"].Value);
            editingAmount = Math.Round(Convert.ToDecimal(row.Cells["AmountPaid"].Value), 2);

            suppressEvents = true;
            cboCustomer.SelectedValue = customerId;
            suppressEvents = false;

            LoadDeliveryChoices(customerId, editingDeliveryId);

            dtpPaymentDate.Value = DateTime.ParseExact(
                Convert.ToString(row.Cells["PaymentDate"].Value), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            SetAmount(GetPayable(editingDeliveryId), editingAmount);
            SetEditMode(true);
        }
    }
}