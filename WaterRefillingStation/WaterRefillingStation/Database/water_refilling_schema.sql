-- Water Refilling Station Management System | SQLite schema (v1)
-- Dates are stored as text in 'yyyy-MM-dd' format. Safe to run more than once.

PRAGMA foreign_keys = ON;

-- PasswordHash is created by the app (never plain text).
CREATE TABLE IF NOT EXISTS Users (
    UserID        INTEGER PRIMARY KEY AUTOINCREMENT,
    Username      TEXT    NOT NULL UNIQUE,
    PasswordHash  TEXT    NOT NULL,
    FullName      TEXT    NOT NULL,
    Role          TEXT    NOT NULL CHECK (Role IN ('Admin', 'Staff')),
    IsActive      INTEGER NOT NULL DEFAULT 1 CHECK (IsActive IN (0, 1)),
    CreatedAt     TEXT    NOT NULL DEFAULT (datetime('now', 'localtime'))
);

-- Set IsActive = 0 instead of deleting a customer who has deliveries.
CREATE TABLE IF NOT EXISTS Customers (
    CustomerID    INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName      TEXT    NOT NULL,
    Address       TEXT    NOT NULL,
    Contact       TEXT,
    IsActive      INTEGER NOT NULL DEFAULT 1 CHECK (IsActive IN (0, 1)),
    CreatedAt     TEXT    NOT NULL DEFAULT (datetime('now', 'localtime'))
);

-- Status flow: Pending -> Delivered.
-- TotalAmount is computed by the app: GallonsRefilled * PricePerGallon.
CREATE TABLE IF NOT EXISTS Deliveries (
    DeliveryID        INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerID        INTEGER NOT NULL,
    DeliveryDate      TEXT    NOT NULL,
    GallonsRefilled   INTEGER NOT NULL DEFAULT 0 CHECK (GallonsRefilled >= 0),
    GallonsDelivered  INTEGER NOT NULL DEFAULT 0 CHECK (GallonsDelivered >= 0),
    PricePerGallon    REAL    NOT NULL CHECK (PricePerGallon >= 0),
    TotalAmount       REAL    NOT NULL CHECK (TotalAmount >= 0),
    Status            TEXT    NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Delivered')),
    Notes             TEXT,
    CreatedBy         INTEGER,
    CreatedAt         TEXT    NOT NULL DEFAULT (datetime('now', 'localtime')),
    FOREIGN KEY (CustomerID) REFERENCES Customers (CustomerID) ON DELETE RESTRICT,
    FOREIGN KEY (CreatedBy)  REFERENCES Users (UserID)         ON DELETE SET NULL
);

-- Partial payments are allowed. Paid/Unpaid is derived in vw_DeliveryBalance.
CREATE TABLE IF NOT EXISTS Payments (
    PaymentID     INTEGER PRIMARY KEY AUTOINCREMENT,
    DeliveryID    INTEGER NOT NULL,
    AmountPaid    REAL    NOT NULL CHECK (AmountPaid > 0),
    PaymentDate   TEXT    NOT NULL,
    ReceivedBy    INTEGER,
    CreatedAt     TEXT    NOT NULL DEFAULT (datetime('now', 'localtime')),
    FOREIGN KEY (DeliveryID) REFERENCES Deliveries (DeliveryID) ON DELETE CASCADE,
    FOREIGN KEY (ReceivedBy) REFERENCES Users (UserID)          ON DELETE SET NULL
);

-- Low stock when CurrentStock <= MinimumStock (see vw_LowStock).
CREATE TABLE IF NOT EXISTS Supplies (
    SupplyID      INTEGER PRIMARY KEY AUTOINCREMENT,
    ItemName      TEXT    NOT NULL UNIQUE,
    Unit          TEXT,
    CurrentStock  INTEGER NOT NULL DEFAULT 0 CHECK (CurrentStock >= 0),
    MinimumStock  INTEGER NOT NULL DEFAULT 10 CHECK (MinimumStock >= 0),
    UpdatedAt     TEXT    NOT NULL DEFAULT (datetime('now', 'localtime'))
);

CREATE INDEX IF NOT EXISTS idx_deliveries_customer ON Deliveries (CustomerID);
CREATE INDEX IF NOT EXISTS idx_deliveries_date     ON Deliveries (DeliveryDate);
CREATE INDEX IF NOT EXISTS idx_deliveries_status   ON Deliveries (Status);
CREATE INDEX IF NOT EXISTS idx_payments_delivery   ON Payments (DeliveryID);

-- One row per delivery with amount paid, balance, and Paid/Unpaid.
CREATE VIEW IF NOT EXISTS vw_DeliveryBalance AS
SELECT
    d.DeliveryID,
    d.CustomerID,
    c.FullName                                            AS CustomerName,
    d.DeliveryDate,
    d.GallonsRefilled,
    d.GallonsDelivered,
    d.PricePerGallon,
    d.TotalAmount,
    d.Status,
    ROUND(IFNULL(p.TotalPaid, 0), 2)                      AS AmountPaid,
    ROUND(d.TotalAmount - IFNULL(p.TotalPaid, 0), 2)      AS Balance,
    CASE
        WHEN ROUND(d.TotalAmount - IFNULL(p.TotalPaid, 0), 2) <= 0 THEN 'Paid'
        ELSE 'Unpaid'
    END                                                   AS PaymentStatus
FROM Deliveries d
JOIN Customers c ON c.CustomerID = d.CustomerID
LEFT JOIN (
    SELECT DeliveryID, SUM(AmountPaid) AS TotalPaid
    FROM Payments
    GROUP BY DeliveryID
) p ON p.DeliveryID = d.DeliveryID;

-- One row per customer with totals and unpaid balance.
CREATE VIEW IF NOT EXISTS vw_CustomerBalance AS
SELECT
    c.CustomerID,
    c.FullName,
    c.Contact,
    COUNT(b.DeliveryID)                         AS TotalDeliveries,
    IFNULL(SUM(b.GallonsDelivered), 0)          AS TotalGallonsDelivered,
    ROUND(IFNULL(SUM(b.TotalAmount), 0), 2)     AS TotalBilled,
    ROUND(IFNULL(SUM(b.AmountPaid), 0), 2)      AS TotalPaid,
    ROUND(IFNULL(SUM(b.Balance), 0), 2)         AS TotalBalance
FROM Customers c
LEFT JOIN vw_DeliveryBalance b ON b.CustomerID = c.CustomerID
GROUP BY c.CustomerID, c.FullName, c.Contact;

-- Supplies at or below their minimum level (low-stock warning).
CREATE VIEW IF NOT EXISTS vw_LowStock AS
SELECT SupplyID, ItemName, Unit, CurrentStock, MinimumStock
FROM Supplies
WHERE CurrentStock <= MinimumStock;

PRAGMA user_version = 1;