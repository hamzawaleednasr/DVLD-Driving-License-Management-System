namespace DVLD.PL.LicenseForms
{
    partial class ShowInternationalLicenseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.internationalLicenseInfoControl1 = new DVLD.PL.UserControls.InternationalLicenseInfoControl();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cairo ExtraBold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(162, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(367, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "Driver International License";
            // 
            // internationalLicenseInfoControl1
            // 
            this.internationalLicenseInfoControl1.InternationalLicenseID = 0;
            this.internationalLicenseInfoControl1.Location = new System.Drawing.Point(12, 118);
            this.internationalLicenseInfoControl1.Name = "internationalLicenseInfoControl1";
            this.internationalLicenseInfoControl1.Size = new System.Drawing.Size(688, 216);
            this.internationalLicenseInfoControl1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(624, 341);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ShowInternationalLicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 373);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.internationalLicenseInfoControl1);
            this.Controls.Add(this.label1);
            this.Name = "ShowInternationalLicenseForm";
            this.Text = "ShowInternationalLicenseForm";
            this.Load += new System.EventHandler(this.ShowInternationalLicenseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private UserControls.InternationalLicenseInfoControl internationalLicenseInfoControl1;
        private System.Windows.Forms.Button btnClose;
    }
}