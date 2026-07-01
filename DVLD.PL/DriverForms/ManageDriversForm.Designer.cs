namespace DVLD.PL.DriverForms
{
    partial class ManageDriversForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.dtgDrivers = new System.Windows.Forms.DataGridView();
            this.cmbFilters = new System.Windows.Forms.ComboBox();
            this.txtFiltering = new System.Windows.Forms.TextBox();
            this.lblFilterBy = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDriversNumber = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgDrivers)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cairo", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(300, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "Manage Drivers";
            // 
            // dtgDrivers
            // 
            this.dtgDrivers.AllowUserToAddRows = false;
            this.dtgDrivers.AllowUserToDeleteRows = false;
            this.dtgDrivers.BackgroundColor = System.Drawing.Color.White;
            this.dtgDrivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgDrivers.Location = new System.Drawing.Point(15, 130);
            this.dtgDrivers.Name = "dtgDrivers";
            this.dtgDrivers.Size = new System.Drawing.Size(770, 280);
            this.dtgDrivers.TabIndex = 1;
            // 
            // cmbFilters
            // 
            this.cmbFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilters.FormattingEnabled = true;
            this.cmbFilters.Items.AddRange(new object[] {
            "-None-",
            "Driver ID",
            "Person ID",
            "National No",
            "Full Name"});
            this.cmbFilters.Location = new System.Drawing.Point(90, 95);
            this.cmbFilters.Name = "cmbFilters";
            this.cmbFilters.Size = new System.Drawing.Size(130, 21);
            this.cmbFilters.TabIndex = 2;
            this.cmbFilters.SelectedIndexChanged += new System.EventHandler(this.cmbFilters_SelectedIndexChanged);
            // 
            // txtFiltering
            // 
            this.txtFiltering.Location = new System.Drawing.Point(230, 95);
            this.txtFiltering.Name = "txtFiltering";
            this.txtFiltering.Size = new System.Drawing.Size(150, 20);
            this.txtFiltering.TabIndex = 3;
            this.txtFiltering.Visible = false;
            this.txtFiltering.TextChanged += new System.EventHandler(this.txtFiltering_TextChanged);
            this.txtFiltering.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFiltering_KeyPress);
            // 
            // lblFilterBy
            // 
            this.lblFilterBy.AutoSize = true;
            this.lblFilterBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterBy.Location = new System.Drawing.Point(15, 93);
            this.lblFilterBy.Name = "lblFilterBy";
            this.lblFilterBy.Size = new System.Drawing.Size(68, 16);
            this.lblFilterBy.TabIndex = 4;
            this.lblFilterBy.Text = "Filter By:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 425);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "# Records:";
            // 
            // lblDriversNumber
            // 
            this.lblDriversNumber.AutoSize = true;
            this.lblDriversNumber.Font = new System.Drawing.Font("Cairo", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriversNumber.Location = new System.Drawing.Point(95, 421);
            this.lblDriversNumber.Name = "lblDriversNumber";
            this.lblDriversNumber.Size = new System.Drawing.Size(32, 24);
            this.lblDriversNumber.TabIndex = 6;
            this.lblDriversNumber.Text = "N/A";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Cairo", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(695, 420);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ManageDriversForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 465);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblDriversNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFilterBy);
            this.Controls.Add(this.txtFiltering);
            this.Controls.Add(this.cmbFilters);
            this.Controls.Add(this.dtgDrivers);
            this.Controls.Add(this.label1);
            this.Name = "ManageDriversForm";
            this.Text = "Manage Drivers";
            this.Load += new System.EventHandler(this.ManageDriversForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgDrivers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgDrivers;
        private System.Windows.Forms.ComboBox cmbFilters;
        private System.Windows.Forms.TextBox txtFiltering;
        private System.Windows.Forms.Label lblFilterBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDriversNumber;
        private System.Windows.Forms.Button btnClose;
    }
}