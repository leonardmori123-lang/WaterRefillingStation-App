namespace WaterRefillingStation.Forms
{
    partial class SuppliesForm
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
            this.lblItemName = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.nudCurrentStock = new System.Windows.Forms.NumericUpDown();
            this.lblMinimum = new System.Windows.Forms.Label();
            this.nudMinimumStock = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.chkLowOnly = new System.Windows.Forms.CheckBox();
            this.dgvSupplies = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrentStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimumStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplies)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(90, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Supplies";
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.Location = new System.Drawing.Point(20, 65);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(63, 13);
            this.lblItemName.TabIndex = 1;
            this.lblItemName.Text = "Item Name *";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(20, 85);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(270, 20);
            this.txtItemName.TabIndex = 2;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(20, 120);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(100, 13);
            this.lblUnit.TabIndex = 3;
            this.lblUnit.Text = "Unit (e.g. pcs, box)";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(20, 140);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(270, 20);
            this.txtUnit.TabIndex = 4;
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(20, 175);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(70, 13);
            this.lblCurrent.TabIndex = 5;
            this.lblCurrent.Text = "Current Stock";
            // 
            // nudCurrentStock
            // 
            this.nudCurrentStock.Location = new System.Drawing.Point(20, 195);
            this.nudCurrentStock.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudCurrentStock.Name = "nudCurrentStock";
            this.nudCurrentStock.Size = new System.Drawing.Size(130, 20);
            this.nudCurrentStock.TabIndex = 6;
            // 
            // lblMinimum
            // 
            this.lblMinimum.AutoSize = true;
            this.lblMinimum.Location = new System.Drawing.Point(160, 175);
            this.lblMinimum.Name = "lblMinimum";
            this.lblMinimum.Size = new System.Drawing.Size(76, 13);
            this.lblMinimum.TabIndex = 7;
            this.lblMinimum.Text = "Minimum Stock";
            // 
            // nudMinimumStock
            // 
            this.nudMinimumStock.Location = new System.Drawing.Point(160, 195);
            this.nudMinimumStock.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudMinimumStock.Name = "nudMinimumStock";
            this.nudMinimumStock.Size = new System.Drawing.Size(130, 20);
            this.nudMinimumStock.TabIndex = 8;
            this.nudMinimumStock.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(20, 245);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 32);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(160, 245);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(130, 32);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(20, 287);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(130, 32);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(160, 287);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(130, 32);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(20, 340);
            this.lblHint.MaximumSize = new System.Drawing.Size(270, 0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(270, 26);
            this.lblHint.TabIndex = 13;
            this.lblHint.Text = "An item is low when its current stock is at or below its minimum stock.";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(320, 65);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(41, 13);
            this.lblSearch.TabIndex = 14;
            this.lblSearch.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(320, 85);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(400, 20);
            this.txtSearch.TabIndex = 15;
            this.txtSearch.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // chkLowOnly
            // 
            this.chkLowOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLowOnly.AutoSize = true;
            this.chkLowOnly.Location = new System.Drawing.Point(735, 87);
            this.chkLowOnly.Name = "chkLowOnly";
            this.chkLowOnly.Size = new System.Drawing.Size(103, 17);
            this.chkLowOnly.TabIndex = 16;
            this.chkLowOnly.Text = "Low stock only";
            this.chkLowOnly.UseVisualStyleBackColor = true;
            this.chkLowOnly.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // dgvSupplies
            // 
            this.dgvSupplies.AllowUserToAddRows = false;
            this.dgvSupplies.AllowUserToDeleteRows = false;
            this.dgvSupplies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSupplies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSupplies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSupplies.Location = new System.Drawing.Point(320, 120);
            this.dgvSupplies.MultiSelect = false;
            this.dgvSupplies.Name = "dgvSupplies";
            this.dgvSupplies.ReadOnly = true;
            this.dgvSupplies.RowHeadersVisible = false;
            this.dgvSupplies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupplies.Size = new System.Drawing.Size(560, 380);
            this.dgvSupplies.TabIndex = 17;
            this.dgvSupplies.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSupplies_CellClick);
            this.dgvSupplies.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSupplies_CellFormatting);
            // 
            // SuppliesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 520);
            this.Controls.Add(this.dgvSupplies);
            this.Controls.Add(this.chkLowOnly);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.nudMinimumStock);
            this.Controls.Add(this.lblMinimum);
            this.Controls.Add(this.nudCurrentStock);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.lblItemName);
            this.Controls.Add(this.lblTitle);
            this.MinimumSize = new System.Drawing.Size(780, 450);
            this.Name = "SuppliesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Supplies";
            this.Load += new System.EventHandler(this.SuppliesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrentStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimumStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.NumericUpDown nudCurrentStock;
        private System.Windows.Forms.Label lblMinimum;
        private System.Windows.Forms.NumericUpDown nudMinimumStock;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkLowOnly;
        private System.Windows.Forms.DataGridView dgvSupplies;
    }
}