namespace invoiceIntegration
{
    partial class frmViewLog
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnLogDelete = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.Window;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(20, 60);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(934, 337);
            this.listBox1.TabIndex = 0;
            // 
            // btnLogDelete
            // 
            this.btnLogDelete.Location = new System.Drawing.Point(810, 26);
            this.btnLogDelete.Name = "btnLogDelete";
            this.btnLogDelete.Size = new System.Drawing.Size(144, 28);
            this.btnLogDelete.TabIndex = 0;
            this.btnLogDelete.Text = "Temizle";
            this.btnLogDelete.UseSelectable = true;
            this.btnLogDelete.Click += new System.EventHandler(this.btnLogDelete_Click);
            // 
            // frmViewLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(974, 417);
            this.Controls.Add(this.btnLogDelete);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmViewLog";
            this.Text = "Log Kayıtları";
            this.Load += new System.EventHandler(this.LogViewForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private MetroFramework.Controls.MetroButton btnLogDelete;
    }
}