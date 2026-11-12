# Water Refilling Station Management System

Windows Forms app (.NET Framework 4.8, C#) with an embedded SQLite database and an Inno Setup installer.
Track B project: a desktop tool for small water refilling stations to record customers, gallon refills
and deliveries, payments, and supplies.

## Setup

1. Clone this repo and open `WaterRefillingStation\WaterRefillingStation.sln` in Visual Studio.
2. Build the solution. NuGet restores `System.Data.SQLite.Core` automatically.
3. Run the app. The database is created at `%LocalAppData%\WaterRefillingStation\station.db`.
   To reset your local data, close the app and delete that file.

First-run login: `admin` / `admin123` (change it after the first login).

## Project structure

```
WaterRefillingStation-App/
├── README.md
├── .gitignore
├── Installer/                      Inno Setup script (setup.iss)
└── WaterRefillingStation/          solution folder
    ├── WaterRefillingStation.sln
    └── WaterRefillingStation/      project folder
        ├── Program.cs
        ├── Database/
        │   ├── Db.cs
        │   └── water_refilling_schema.sql    (Build Action: Embedded Resource)
        ├── Security/
        │   ├── PasswordHasher.cs
        │   └── Session.cs
        └── Forms/                  each form = .cs + .Designer.cs + .resx
            ├── LoginForm
            ├── MainForm
            ├── CustomersForm
            ├── DeliveriesForm
            ├── PaymentsForm
            ├── SuppliesForm
            └── ReportsForm
```

## Designing forms (Visual Studio designer)

- Build every screen by dragging controls in the designer. Do not create controls in code.
- Do not edit `.Designer.cs` by hand. Change things in the designer or the Properties window.
- Name controls with a prefix: `btn` Button, `txt` TextBox, `lbl` Label, `dgv` DataGridView,
  `cbo` ComboBox, `dtp` DateTimePicker, `nud` NumericUpDown.
- Set `Dock` or `Anchor` so controls resize with the window.
- Keep the look default for now. Colors, fonts, and styling come in a later pass.
- Only edit your own forms. Two people editing the same `.Designer.cs` causes Git conflicts.

Simple layout for each form: input fields at the top, buttons in the middle, a DataGridView below.

| Form | Main controls |
|---|---|
| LoginForm | txtUsername, txtPassword (UseSystemPasswordChar = true), btnLogin |
| MainForm | lblGallonsToday, lblPending, lblUnpaid, lblLowStock, buttons to open each form, btnLogout |
| CustomersForm | txtFullName, txtAddress, txtContact, dgvCustomers, btnSave, btnUpdate, btnDelete, btnClear |
| DeliveriesForm | cboCustomer, dtpDeliveryDate, nudGallonsRefilled, nudGallonsDelivered, nudPrice, lblTotal, cboStatus, dgvDeliveries, btnSave, btnUpdate, btnDelete, btnClear |
| PaymentsForm | dgvDeliveries (unpaid), nudAmount, dtpPaymentDate, lblBalance, btnAddPayment, dgvPayments |
| SuppliesForm | txtItemName, txtUnit, nudCurrentStock, nudMinimumStock, dgvSupplies, btnSave, btnUpdate, btnDelete, btnClear |
| ReportsForm | dtpFrom, dtpTo, cboCustomer, btnSearch, dgvReport, btnExportCsv |

## Using the database helper

Always use `Db` and pass values as parameters. Never join values into the SQL text.
Store dates as text in `yyyy-MM-dd` format.

```csharp
dgvDeliveries.DataSource = Db.Query(
    "SELECT * FROM vw_DeliveryBalance WHERE CustomerID = @id", ("@id", customerId));

long id = Db.Insert(
    "INSERT INTO Payments (DeliveryID, AmountPaid, PaymentDate) VALUES (@d, @a, @date)",
    ("@d", deliveryId), ("@a", 125.0), ("@date", DateTime.Today.ToString("yyyy-MM-dd")));

object pending = Db.Scalar("SELECT COUNT(*) FROM Deliveries WHERE Status = 'Pending'");
```

## Logged-in user

After login, `Session` holds the current user. Use it for the `CreatedBy` and `ReceivedBy` columns.

```csharp
Session.UserID     // int
Session.FullName   // string
Session.IsAdmin    // true for Admin, false for Staff
```

## Tables and views

| Name | Purpose |
|---|---|
| Users | Login (Admin or Staff) |
| Customers | Customer list |
| Deliveries | Gallons refilled and delivered, date, price, total, status (Pending or Delivered) |
| Payments | Payments per delivery (partial payments allowed) |
| Supplies | Items with current and minimum stock |
| vw_DeliveryBalance | Per delivery: amount paid, balance, Paid/Unpaid. Use for Payments and Reports |
| vw_CustomerBalance | Per customer totals and unpaid balance |
| vw_LowStock | Supplies at or below minimum. Use for the low-stock warning |

## Common queries

```sql
-- Gallons delivered today
SELECT IFNULL(SUM(GallonsDelivered), 0) FROM Deliveries
WHERE Status = 'Delivered' AND DeliveryDate = date('now', 'localtime');

-- Pending deliveries
SELECT COUNT(*) FROM Deliveries WHERE Status = 'Pending';

-- Total unpaid balance
SELECT IFNULL(SUM(Balance), 0) FROM vw_DeliveryBalance WHERE Balance > 0;

-- Report by date range and optional customer
SELECT * FROM vw_DeliveryBalance
WHERE DeliveryDate BETWEEN @from AND @to
  AND (@customerId IS NULL OR CustomerID = @customerId)
ORDER BY DeliveryDate, CustomerName;
```

## Who builds what

| Module | Owner |
|---|---|
| Database, MainForm and dashboard, Customers, Deliveries, Login, installer | Leo |
| Payments and Low-stock warning (SuppliesForm) | [Name 1] |
| Reports (filter by date or customer, export to CSV) | [Name 2] |

## Team rules

- Work on your own branch (`feature/payments`, `feature/reports`, `feature/low-stock`) and open a pull request. Do not push to `main` directly.
- Do not edit `water_refilling_schema.sql` yourself. If you need a new column or table, message Leo.
- Commit small and often, with clear messages.