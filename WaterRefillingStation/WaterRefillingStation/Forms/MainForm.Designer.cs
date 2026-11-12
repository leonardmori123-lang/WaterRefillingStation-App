namespace WaterRefillingStation.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.grpGallons = new System.Windows.Forms.GroupBox();
            this.lblGallonsToday = new System.Windows.Forms.Label();
            this.grpPending = new System.Windows.Forms.GroupBox();
            this.lblPending = new System.Windows.Forms.Label();
            this.grpUnpaid = new System.Windows.Forms.GroupBox();
            this.lblUnpaid = new System.Windows.Forms.Label();
            this.grpLowStock = new System.Windows.Forms.GroupBox();
            this.lblLowStock = new System.Windows.Forms.Label();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnDeliveries = new System.Windows.Forms.Button();
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnSupplies = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.lblPendingList = new System.Windows.Forms.Label();
            this.dgvPending = new System.Windows.Forms.DataGridView();
            this.lblLowList = new System.Windows.Forms.Label();
            this.dgvLowStock = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPending)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).BeginInit();
            this.grpGallons.SuspendLayout();
            this.grpPending.SuspendLayout();
            this.grpUnpaid.SuspendLayout();
            this.grpLowStock.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(275, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Water Refilling Station";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(22, 52);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(47, 13);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome";
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.Location = new System.Drawing.Point(785, 15);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnUsers
            // 
            this.btnUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsers.Location = new System.Drawing.Point(675, 15);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(100, 30);
            this.btnUsers.TabIndex = 17;
            this.btnUsers.Text = "Users";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // grpGallons
            // 
            this.grpGallons.Controls.Add(this.lblGallonsToday);
            this.grpGallons.Location = new System.Drawing.Point(20, 85);
            this.grpGallons.Name = "grpGallons";
            this.grpGallons.Size = new System.Drawing.Size(205, 90);
            this.grpGallons.TabIndex = 3;
            this.grpGallons.TabStop = false;
            this.grpGallons.Text = "Gallons delivered today";
            // 
            // lblGallonsToday
            // 
            this.lblGallonsToday.AutoSize = true;
            this.lblGallonsToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGallonsToday.Location = new System.Drawing.Point(15, 28);
            this.lblGallonsToday.Name = "lblGallonsToday";
            this.lblGallonsToday.Size = new System.Drawing.Size(30, 31);
            this.lblGallonsToday.TabIndex = 0;
            this.lblGallonsToday.Text = "0";
            // 
            // grpPending
            // 
            this.grpPending.Controls.Add(this.lblPending);
            this.grpPending.Location = new System.Drawing.Point(240, 85);
            this.grpPending.Name = "grpPending";
            this.grpPending.Size = new System.Drawing.Size(205, 90);
            this.grpPending.TabIndex = 4;
            this.grpPending.TabStop = false;
            this.grpPending.Text = "Pending deliveries";
            // 
            // lblPending
            // 
            this.lblPending.AutoSize = true;
            this.lblPending.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPending.Location = new System.Drawing.Point(15, 28);
            this.lblPending.Name = "lblPending";
            this.lblPending.Size = new System.Drawing.Size(30, 31);
            this.lblPending.TabIndex = 0;
            this.lblPending.Text = "0";
            // 
            // grpUnpaid
            // 
            this.grpUnpaid.Controls.Add(this.lblUnpaid);
            this.grpUnpaid.Location = new System.Drawing.Point(460, 85);
            this.grpUnpaid.Name = "grpUnpaid";
            this.grpUnpaid.Size = new System.Drawing.Size(205, 90);
            this.grpUnpaid.TabIndex = 5;
            this.grpUnpaid.TabStop = false;
            this.grpUnpaid.Text = "Unpaid balance";
            // 
            // lblUnpaid
            // 
            this.lblUnpaid.AutoSize = true;
            this.lblUnpaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnpaid.Location = new System.Drawing.Point(15, 28);
            this.lblUnpaid.Name = "lblUnpaid";
            this.lblUnpaid.Size = new System.Drawing.Size(105, 31);
            this.lblUnpaid.TabIndex = 0;
            this.lblUnpaid.Text = "\u20B1 0.00";
            // 
            // grpLowStock
            // 
            this.grpLowStock.Controls.Add(this.lblLowStock);
            this.grpLowStock.Location = new System.Drawing.Point(680, 85);
            this.grpLowStock.Name = "grpLowStock";
            this.grpLowStock.Size = new System.Drawing.Size(205, 90);
            this.grpLowStock.TabIndex = 6;
            this.grpLowStock.TabStop = false;
            this.grpLowStock.Text = "Low-stock items";
            // 
            // lblLowStock
            // 
            this.lblLowStock.AutoSize = true;
            this.lblLowStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowStock.Location = new System.Drawing.Point(15, 28);
            this.lblLowStock.Name = "lblLowStock";
            this.lblLowStock.Size = new System.Drawing.Size(30, 31);
            this.lblLowStock.TabIndex = 0;
            this.lblLowStock.Text = "0";
            // 
            // btnCustomers
            // 
            this.btnCustomers.Location = new System.Drawing.Point(20, 195);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(150, 45);
            this.btnCustomers.TabIndex = 7;
            this.btnCustomers.Text = "Customers";
            this.btnCustomers.UseVisualStyleBackColor = true;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // btnDeliveries
            // 
            this.btnDeliveries.Location = new System.Drawing.Point(197, 195);
            this.btnDeliveries.Name = "btnDeliveries";
            this.btnDeliveries.Size = new System.Drawing.Size(150, 45);
            this.btnDeliveries.TabIndex = 8;
            this.btnDeliveries.Text = "Deliveries";
            this.btnDeliveries.UseVisualStyleBackColor = true;
            this.btnDeliveries.Click += new System.EventHandler(this.btnDeliveries_Click);
            // 
            // btnPayments
            // 
            this.btnPayments.Location = new System.Drawing.Point(374, 195);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Size = new System.Drawing.Size(150, 45);
            this.btnPayments.TabIndex = 9;
            this.btnPayments.Text = "Payments";
            this.btnPayments.UseVisualStyleBackColor = true;
            this.btnPayments.Click += new System.EventHandler(this.btnPayments_Click);
            // 
            // btnSupplies
            // 
            this.btnSupplies.Location = new System.Drawing.Point(551, 195);
            this.btnSupplies.Name = "btnSupplies";
            this.btnSupplies.Size = new System.Drawing.Size(150, 45);
            this.btnSupplies.TabIndex = 10;
            this.btnSupplies.Text = "Supplies";
            this.btnSupplies.UseVisualStyleBackColor = true;
            this.btnSupplies.Click += new System.EventHandler(this.btnSupplies_Click);
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(728, 195);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(150, 45);
            this.btnReports.TabIndex = 11;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // lblPendingList
            // 
            this.lblPendingList.AutoSize = true;
            this.lblPendingList.Location = new System.Drawing.Point(20, 260);
            this.lblPendingList.Name = "lblPendingList";
            this.lblPendingList.Size = new System.Drawing.Size(191, 13);
            this.lblPendingList.TabIndex = 12;
            this.lblPendingList.Text = "Pending deliveries (still to go out)";
            // 
            // dgvPending
            // 
            this.dgvPending.AllowUserToAddRows = false;
            this.dgvPending.AllowUserToDeleteRows = false;
            this.dgvPending.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvPending.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPending.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPending.Location = new System.Drawing.Point(20, 280);
            this.dgvPending.MultiSelect = false;
            this.dgvPending.Name = "dgvPending";
            this.dgvPending.ReadOnly = true;
            this.dgvPending.RowHeadersVisible = false;
            this.dgvPending.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPending.Size = new System.Drawing.Size(430, 240);
            this.dgvPending.TabIndex = 13;
            // 
            // lblLowList
            // 
            this.lblLowList.AutoSize = true;
            this.lblLowList.Location = new System.Drawing.Point(460, 260);
            this.lblLowList.Name = "lblLowList";
            this.lblLowList.Size = new System.Drawing.Size(103, 13);
            this.lblLowList.TabIndex = 14;
            this.lblLowList.Text = "Low-stock supplies";
            // 
            // dgvLowStock
            // 
            this.dgvLowStock.AllowUserToAddRows = false;
            this.dgvLowStock.AllowUserToDeleteRows = false;
            this.dgvLowStock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLowStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLowStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLowStock.Location = new System.Drawing.Point(460, 280);
            this.dgvLowStock.MultiSelect = false;
            this.dgvLowStock.Name = "dgvLowStock";
            this.dgvLowStock.ReadOnly = true;
            this.dgvLowStock.RowHeadersVisible = false;
            this.dgvLowStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLowStock.Size = new System.Drawing.Size(425, 240);
            this.dgvLowStock.TabIndex = 15;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 540);
            this.Controls.Add(this.dgvLowStock);
            this.Controls.Add(this.lblLowList);
            this.Controls.Add(this.dgvPending);
            this.Controls.Add(this.lblPendingList);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnSupplies);
            this.Controls.Add(this.btnPayments);
            this.Controls.Add(this.btnDeliveries);
            this.Controls.Add(this.btnCustomers);
            this.Controls.Add(this.grpLowStock);
            this.Controls.Add(this.grpUnpaid);
            this.Controls.Add(this.grpPending);
            this.Controls.Add(this.grpGallons);
            this.Controls.Add(this.btnUsers);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTitle);
            this.MinimumSize = new System.Drawing.Size(916, 579);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Water Refilling Station";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPending)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).EndInit();
            this.grpGallons.ResumeLayout(false);
            this.grpGallons.PerformLayout();
            this.grpPending.ResumeLayout(false);
            this.grpPending.PerformLayout();
            this.grpUnpaid.ResumeLayout(false);
            this.grpUnpaid.PerformLayout();
            this.grpLowStock.ResumeLayout(false);
            this.grpLowStock.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.GroupBox grpGallons;
        private System.Windows.Forms.Label lblGallonsToday;
        private System.Windows.Forms.GroupBox grpPending;
        private System.Windows.Forms.Label lblPending;
        private System.Windows.Forms.GroupBox grpUnpaid;
        private System.Windows.Forms.Label lblUnpaid;
        private System.Windows.Forms.GroupBox grpLowStock;
        private System.Windows.Forms.Label lblLowStock;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.Button btnDeliveries;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Button btnSupplies;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Label lblPendingList;
        private System.Windows.Forms.DataGridView dgvPending;
        private System.Windows.Forms.Label lblLowList;
        private System.Windows.Forms.DataGridView dgvLowStock;
    }
}