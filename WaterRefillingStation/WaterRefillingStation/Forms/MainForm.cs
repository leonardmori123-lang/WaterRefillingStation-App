using System;
using System.Windows.Forms;
using WaterRefillingStation.Database;
using WaterRefillingStation.Security;

namespace WaterRefillingStation.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome, " + Session.FullName + " (" + Session.Role + ")";
            btnUsers.Visible = Session.IsAdmin;
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd");

            lblGallonsToday.Text = Convert.ToString(Db.Scalar(
                "SELECT IFNULL(SUM(GallonsDelivered), 0) FROM Deliveries " +
                "WHERE Status = 'Delivered' AND DeliveryDate = @today",
                ("@today", today)));

            lblPending.Text = Convert.ToString(Db.Scalar(
                "SELECT COUNT(*) FROM Deliveries WHERE Status = 'Pending'"));

            decimal unpaid = Convert.ToDecimal(Db.Scalar(
                "SELECT IFNULL(SUM(Balance), 0) FROM vw_DeliveryBalance WHERE Balance > 0"));
            lblUnpaid.Text = "\u20B1 " + unpaid.ToString("N2");

            dgvPending.DataSource = Db.Query(
                "SELECT CustomerName, DeliveryDate, GallonsRefilled FROM vw_DeliveryBalance " +
                "WHERE Status = 'Pending' ORDER BY DeliveryDate");
            SetHeader(dgvPending, "CustomerName", "Customer");
            SetHeader(dgvPending, "DeliveryDate", "Date");
            SetHeader(dgvPending, "GallonsRefilled", "Gallons");

            dgvLowStock.DataSource = Db.Query(
                "SELECT ItemName, CurrentStock, MinimumStock FROM vw_LowStock ORDER BY ItemName");
            SetHeader(dgvLowStock, "ItemName", "Item");
            SetHeader(dgvLowStock, "CurrentStock", "In Stock");
            SetHeader(dgvLowStock, "MinimumStock", "Minimum");

            lblLowStock.Text = dgvLowStock.Rows.Count.ToString();
        }

        private static void SetHeader(DataGridView grid, string column, string text)
        {
            if (grid.Columns.Contains(column))
                grid.Columns[column].HeaderText = text;
        }

        // Opens a screen as a dialog, then refreshes the dashboard when it closes.
        private void OpenForm(Form form)
        {
            using (form)
            {
                form.ShowDialog(this);
            }
            LoadDashboard();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            OpenForm(new CustomersForm());
        }

        private void btnDeliveries_Click(object sender, EventArgs e)
        {
            OpenForm(new DeliveriesForm());
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            OpenForm(new PaymentsForm());
        }

        private void btnSupplies_Click(object sender, EventArgs e)
        {
            OpenForm(new SuppliesForm());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            OpenForm(new ReportsForm());
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            OpenForm(new UsersForm());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Session.End();
            Close();
        }
    }
}