using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WaterRefillingStation.Database;

namespace WaterRefillingStation.Forms
{
    public partial class ReportsForm : Form
    {
        private const string RepDeliveries = "Deliveries";
        private const string RepDaily = "Daily Summary";
        private const string RepCustomer = "Sales by Customer";
        private const string RepPayments = "Payments Received";
        private const string RepUnpaid = "Unpaid Balances";
        private const string RepStock = "Supplies Stock";

        private DataTable currentReport;
        private bool isLoading = true;

        public ReportsForm()
        {
            InitializeComponent();
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
            dtpTo.Value = today;

            cboReportType.SelectedIndex = 0;
            isLoading = false;

            UpdateDateControls();
            GenerateReport();
        }

        // Unpaid Balances and Supplies Stock show the situation right now, so they ignore the dates.
        private static bool UsesDates(string report)
        {
            return report != RepUnpaid && report != RepStock;
        }

        private void UpdateDateControls()
        {
            bool enabled = UsesDates(Convert.ToString(cboReportType.SelectedItem));
            dtpFrom.Enabled = enabled;
            dtpTo.Enabled = enabled;
        }

        private static string ToDbDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void cboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            UpdateDateControls();
            GenerateReport();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            string report = Convert.ToString(cboReportType.SelectedItem);

            if (UsesDates(report) && dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("The 'From' date must be on or before the 'To' date.", "Reports",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentReport = BuildReport(report, ToDbDate(dtpFrom.Value), ToDbDate(dtpTo.Value));
            dgvReport.DataSource = currentReport;
            FormatGrid();
            lblSummary.Text = BuildSummary(report, currentReport);
        }

        private static DataTable BuildReport(string report, string from, string to)
        {
            switch (report)
            {
                case RepDaily:
                    return Db.Query(
                        "SELECT DeliveryDate AS \"Date\", COUNT(*) AS \"Deliveries\", " +
                        "SUM(GallonsRefilled) AS \"Refilled\", SUM(GallonsDelivered) AS \"Delivered\", " +
                        "ROUND(SUM(TotalAmount), 2) AS \"Total\", " +
                        "ROUND(SUM(TotalAmount - Balance), 2) AS \"Paid\", " +
                        "ROUND(SUM(Balance), 2) AS \"Balance\" " +
                        "FROM vw_DeliveryBalance " +
                        "WHERE DeliveryDate BETWEEN @from AND @to " +
                        "GROUP BY DeliveryDate ORDER BY DeliveryDate",
                        ("@from", from), ("@to", to));

                case RepCustomer:
                    return Db.Query(
                        "SELECT CustomerName AS \"Customer\", COUNT(*) AS \"Deliveries\", " +
                        "SUM(GallonsRefilled) AS \"Refilled\", SUM(GallonsDelivered) AS \"Delivered\", " +
                        "ROUND(SUM(TotalAmount), 2) AS \"Total\", " +
                        "ROUND(SUM(TotalAmount - Balance), 2) AS \"Paid\", " +
                        "ROUND(SUM(Balance), 2) AS \"Balance\" " +
                        "FROM vw_DeliveryBalance " +
                        "WHERE DeliveryDate BETWEEN @from AND @to " +
                        "GROUP BY CustomerID, CustomerName ORDER BY SUM(TotalAmount) DESC",
                        ("@from", from), ("@to", to));

                case RepPayments:
                    return Db.Query(
                        "SELECT p.PaymentDate AS \"Payment Date\", b.CustomerName AS \"Customer\", " +
                        "b.DeliveryDate AS \"Delivery Date\", p.AmountPaid AS \"Amount Paid\", " +
                        "u.FullName AS \"Received By\" " +
                        "FROM Payments p " +
                        "JOIN vw_DeliveryBalance b ON b.DeliveryID = p.DeliveryID " +
                        "LEFT JOIN Users u ON u.UserID = p.ReceivedBy " +
                        "WHERE p.PaymentDate BETWEEN @from AND @to " +
                        "ORDER BY p.PaymentDate, p.PaymentID",
                        ("@from", from), ("@to", to));

                case RepUnpaid:
                    return Db.Query(
                        "SELECT CustomerName AS \"Customer\", COUNT(*) AS \"Unpaid Deliveries\", " +
                        "MIN(DeliveryDate) AS \"Oldest Delivery\", ROUND(SUM(Balance), 2) AS \"Balance\" " +
                        "FROM vw_DeliveryBalance WHERE Balance > 0 " +
                        "GROUP BY CustomerID, CustomerName ORDER BY SUM(Balance) DESC");

                case RepStock:
                    return Db.Query(
                        "SELECT ItemName AS \"Item\", Unit AS \"Unit\", CurrentStock AS \"In Stock\", " +
                        "MinimumStock AS \"Minimum\", " +
                        "CASE WHEN CurrentStock <= MinimumStock THEN 'Low' ELSE 'OK' END AS \"Status\" " +
                        "FROM Supplies ORDER BY ItemName");

                default: // RepDeliveries
                    return Db.Query(
                        "SELECT DeliveryDate AS \"Date\", CustomerName AS \"Customer\", " +
                        "GallonsRefilled AS \"Refilled\", GallonsDelivered AS \"Delivered\", " +
                        "PricePerGallon AS \"Price\", TotalAmount AS \"Total\", " +
                        "ROUND(TotalAmount - Balance, 2) AS \"Paid\", Balance AS \"Balance\", " +
                        "Status AS \"Status\", PaymentStatus AS \"Payment\" " +
                        "FROM vw_DeliveryBalance " +
                        "WHERE DeliveryDate BETWEEN @from AND @to " +
                        "ORDER BY DeliveryDate, DeliveryID",
                        ("@from", from), ("@to", to));
            }
        }

        private void FormatGrid()
        {
            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                switch (col.Name)
                {
                    case "Price":
                    case "Total":
                    case "Paid":
                    case "Balance":
                    case "Amount Paid":
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break;
                    case "Refilled":
                    case "Delivered":
                    case "Deliveries":
                    case "Unpaid Deliveries":
                    case "In Stock":
                    case "Minimum":
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break;
                }
            }
        }

        private static decimal Sum(DataTable table, string column)
        {
            if (!table.Columns.Contains(column)) return 0m;

            decimal total = 0m;
            foreach (DataRow row in table.Rows)
            {
                if (row[column] != DBNull.Value)
                    total += Convert.ToDecimal(row[column]);
            }
            return total;
        }

        private static string Peso(decimal amount)
        {
            return "\u20B1 " + amount.ToString("N2");
        }

        private static string BuildSummary(string report, DataTable table)
        {
            if (table.Rows.Count == 0) return "No records found.";

            switch (report)
            {
                case RepDaily:
                case RepCustomer:
                    return Sum(table, "Deliveries").ToString("N0") + " deliveries   |   " +
                        Sum(table, "Delivered").ToString("N0") + " gallons delivered   |   Total " +
                        Peso(Sum(table, "Total")) + "   |   Paid " + Peso(Sum(table, "Paid")) +
                        "   |   Balance " + Peso(Sum(table, "Balance"));

                case RepPayments:
                    return table.Rows.Count + " payments   |   Received " + Peso(Sum(table, "Amount Paid"));

                case RepUnpaid:
                    return table.Rows.Count + " customers with unpaid deliveries   |   Total unpaid " +
                        Peso(Sum(table, "Balance"));

                case RepStock:
                    int low = 0;
                    foreach (DataRow row in table.Rows)
                        if (Convert.ToString(row["Status"]) == "Low") low++;
                    return table.Rows.Count + " items   |   " + low + " low on stock";

                default:
                    return table.Rows.Count + " deliveries   |   " +
                        Sum(table, "Delivered").ToString("N0") + " gallons delivered   |   Total " +
                        Peso(Sum(table, "Total")) + "   |   Paid " + Peso(Sum(table, "Paid")) +
                        "   |   Balance " + Peso(Sum(table, "Balance"));
            }
        }

        // Low-stock items are shown in red, like on the Supplies screen.
        private void dgvReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvReport.Columns.Contains("In Stock") ||
                !dgvReport.Columns.Contains("Status")) return;

            if (Convert.ToString(dgvReport.Rows[e.RowIndex].Cells["Status"].Value) == "Low")
            {
                e.CellStyle.ForeColor = Color.DarkRed;
                e.CellStyle.BackColor = Color.MistyRose;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (currentReport == null || currentReport.Rows.Count == 0)
            {
                MessageBox.Show("There is nothing to export. Generate a report first.", "Reports",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV file (*.csv)|*.csv";
                dialog.FileName = Convert.ToString(cboReportType.SelectedItem) + " " +
                    DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                StringBuilder sb = new StringBuilder();

                string[] headers = new string[currentReport.Columns.Count];
                for (int i = 0; i < headers.Length; i++)
                    headers[i] = Csv(currentReport.Columns[i].ColumnName);
                sb.AppendLine(string.Join(",", headers));

                foreach (DataRow row in currentReport.Rows)
                {
                    string[] cells = new string[currentReport.Columns.Count];
                    for (int i = 0; i < cells.Length; i++)
                        cells[i] = Csv(Convert.ToString(row[i], CultureInfo.InvariantCulture));
                    sb.AppendLine(string.Join(",", cells));
                }

                try
                {
                    File.WriteAllText(dialog.FileName, sb.ToString(), new UTF8Encoding(true));
                    MessageBox.Show("Report exported.", "Reports",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not save the file:\n" + ex.Message, "Reports",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string Csv(string value)
        {
            if (value == null) return "";
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }
    }
}
