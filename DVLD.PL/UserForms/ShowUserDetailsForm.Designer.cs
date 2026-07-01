namespace DVLD.PL.UserForms
{
    partial class ShowUserDetailsForm
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
            this.btnClose = new System.Windows.Forms.Button();
            this.userDetailsControl1 = new DVLD.PL.UserControls.UserDetailsControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cairo ExtraBold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(289, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Details";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Cairo ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(634, 544);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(154, 46);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // userDetailsControl1
            // 
            this.userDetailsControl1.Location = new System.Drawing.Point(-1, 89);
            this.userDetailsControl1.Name = "userDetailsControl1";
            this.userDetailsControl1.Size = new System.Drawing.Size(801, 446);
            this.userDetailsControl1.TabIndex = 2;
            this.userDetailsControl1.UserID = 0;
            // 
            // ShowUserDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 602);
            this.Controls.Add(this.userDetailsControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Name = "ShowUserDetailsForm";
            this.Text = "User Details";
            this.Load += new System.EventHandler(this.ShowUserDetailsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private UserControls.UserDetailsControl userDetailsControl1;
    }
}