
namespace invoiceIntegration
{
    partial class Waybills
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Waybills));
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridInvoice = new System.Windows.Forms.DataGridView();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblEndDate = new MetroFramework.Controls.MetroLabel();
            this.lblStartDate = new MetroFramework.Controls.MetroLabel();
            this.btnWaybill = new MetroFramework.Controls.MetroButton();
            this.cmbDispatch = new MetroFramework.Controls.MetroComboBox();
            this.btnGetWaybbill = new MetroFramework.Controls.MetroButton();
            this.btnCheckLogoConnection = new System.Windows.Forms.Button();
            this.lblWaybill = new MetroFramework.Controls.MetroLabel();
            this.lblLogoConnectionInfo = new System.Windows.Forms.Label();
            this.endDate = new MetroFramework.Controls.MetroDateTime();
            this.startDate = new MetroFramework.Controls.MetroDateTime();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.anaMenüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLastLog = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).BeginInit();
            this.materialCard1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.chkSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkSelectAll.Location = new System.Drawing.Point(13, 222);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(51, 20);
            this.chkSelectAll.TabIndex = 24;
            this.chkSelectAll.Text = "Seç";
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged_1);
            // 
            // dataGridInvoice
            // 
            this.dataGridInvoice.AllowUserToAddRows = false;
            this.dataGridInvoice.AllowUserToDeleteRows = false;
            this.dataGridInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridInvoice.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridInvoice.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(1, 2, 0, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridInvoice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.Column9,
            this.number,
            this.Column11,
            this.Column12,
            this.Column13,
            this.customerName,
            this.Column14,
            this.Column15,
            this.Column16});
            this.dataGridInvoice.EnableHeadersVisualStyles = false;
            this.dataGridInvoice.Location = new System.Drawing.Point(10, 219);
            this.dataGridInvoice.Name = "dataGridInvoice";
            this.dataGridInvoice.ReadOnly = true;
            this.dataGridInvoice.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridInvoice.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridInvoice.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridInvoice.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridInvoice.Size = new System.Drawing.Size(1114, 373);
            this.dataGridInvoice.TabIndex = 15;
            this.dataGridInvoice.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridInvoice_CellContentClick);
            // 
            // chk
            // 
            this.chk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.chk.FillWeight = 50F;
            this.chk.HeaderText = "";
            this.chk.Name = "chk";
            this.chk.ReadOnly = true;
            this.chk.Width = 35;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Fatura Tipi";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column9.Width = 110;
            // 
            // number
            // 
            this.number.HeaderText = "Fatura No";
            this.number.MinimumWidth = 6;
            this.number.Name = "number";
            this.number.ReadOnly = true;
            this.number.Width = 110;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Fatura Tarihi";
            this.Column11.MinimumWidth = 6;
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Belge No";
            this.Column12.MinimumWidth = 6;
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Müşteri Kodu";
            this.Column13.MinimumWidth = 6;
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.Width = 115;
            // 
            // customerName
            // 
            this.customerName.HeaderText = "Müşteri Ünvanı";
            this.customerName.MinimumWidth = 6;
            this.customerName.Name = "customerName";
            this.customerName.ReadOnly = true;
            this.customerName.Width = 175;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Toplam İnd. Tutarı";
            this.Column14.MinimumWidth = 6;
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.Width = 125;
            // 
            // Column15
            // 
            this.Column15.HeaderText = "KDV Tutarı";
            this.Column15.MinimumWidth = 6;
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            this.Column15.Width = 105;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "KDV Hariç Tutar";
            this.Column16.MinimumWidth = 6;
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            this.Column16.Width = 115;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.BackColor = System.Drawing.Color.LightGray;
            this.lblEndDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblEndDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEndDate.Location = new System.Drawing.Point(406, 101);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(60, 15);
            this.lblEndDate.TabIndex = 42;
            this.lblEndDate.Text = "Bitiş Tarihi";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.BackColor = System.Drawing.Color.LightGray;
            this.lblStartDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblStartDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStartDate.Location = new System.Drawing.Point(242, 101);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(88, 15);
            this.lblStartDate.TabIndex = 41;
            this.lblStartDate.Text = "Başlangıç Tarihi";
            // 
            // btnWaybill
            // 
            this.btnWaybill.Location = new System.Drawing.Point(356, 57);
            this.btnWaybill.Name = "btnWaybill";
            this.btnWaybill.Size = new System.Drawing.Size(189, 43);
            this.btnWaybill.TabIndex = 40;
            this.btnWaybill.Text = "İrsaliyeleri Aktar";
            this.btnWaybill.UseSelectable = true;
            this.btnWaybill.Click += new System.EventHandler(this.btnWaybill_Click_1);
            // 
            // cmbDispatch
            // 
            this.cmbDispatch.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cmbDispatch.FormattingEnabled = true;
            this.cmbDispatch.ItemHeight = 19;
            this.cmbDispatch.Items.AddRange(new object[] {
            "Satış İrsaliyesi",
            "Hasarlı Satış İade İrsaliyesi",
            "Sağlam Satış İade İrsaliyesi",
            "Alım İrsaliyesi",
            "Hasarılı Alım İade İrsaliyesi",
            "Sağlam Alım İade İrsaliyesi"});
            this.cmbDispatch.Location = new System.Drawing.Point(31, 118);
            this.cmbDispatch.Name = "cmbDispatch";
            this.cmbDispatch.Size = new System.Drawing.Size(194, 25);
            this.cmbDispatch.TabIndex = 38;
            this.cmbDispatch.Theme = MetroFramework.MetroThemeStyle.Light;
            this.cmbDispatch.UseSelectable = true;
            this.cmbDispatch.SelectedIndexChanged += new System.EventHandler(this.cmbDispatch_SelectedIndexChanged_1);
            // 
            // btnGetWaybbill
            // 
            this.btnGetWaybbill.Location = new System.Drawing.Point(15, 57);
            this.btnGetWaybbill.Name = "btnGetWaybbill";
            this.btnGetWaybbill.Size = new System.Drawing.Size(194, 43);
            this.btnGetWaybbill.TabIndex = 37;
            this.btnGetWaybbill.Text = "İrsaliyeleri Getir";
            this.btnGetWaybbill.UseSelectable = true;
            this.btnGetWaybbill.Click += new System.EventHandler(this.btnGetWaybbill_Click);
            // 
            // btnCheckLogoConnection
            // 
            this.btnCheckLogoConnection.Enabled = false;
            this.btnCheckLogoConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCheckLogoConnection.Location = new System.Drawing.Point(930, 54);
            this.btnCheckLogoConnection.Name = "btnCheckLogoConnection";
            this.btnCheckLogoConnection.Size = new System.Drawing.Size(160, 46);
            this.btnCheckLogoConnection.TabIndex = 36;
            this.btnCheckLogoConnection.Text = "Bağlantıyı Kontrol Et";
            this.btnCheckLogoConnection.UseVisualStyleBackColor = true;
            this.btnCheckLogoConnection.Click += new System.EventHandler(this.btnCheckLogoConnection_Click_1);
            // 
            // lblWaybill
            // 
            this.lblWaybill.AutoSize = true;
            this.lblWaybill.BackColor = System.Drawing.Color.LightGray;
            this.lblWaybill.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblWaybill.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblWaybill.Location = new System.Drawing.Point(17, 7);
            this.lblWaybill.Name = "lblWaybill";
            this.lblWaybill.Size = new System.Drawing.Size(62, 15);
            this.lblWaybill.TabIndex = 46;
            this.lblWaybill.Text = "Tip Seçiniz";
            // 
            // lblLogoConnectionInfo
            // 
            this.lblLogoConnectionInfo.AutoSize = true;
            this.lblLogoConnectionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLogoConnectionInfo.Location = new System.Drawing.Point(927, 36);
            this.lblLogoConnectionInfo.Name = "lblLogoConnectionInfo";
            this.lblLogoConnectionInfo.Size = new System.Drawing.Size(20, 15);
            this.lblLogoConnectionInfo.TabIndex = 47;
            this.lblLogoConnectionInfo.Text = "lbl";
            // 
            // endDate
            // 
            this.endDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.endDate.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.endDate.Location = new System.Drawing.Point(406, 120);
            this.endDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(155, 25);
            this.endDate.TabIndex = 49;
            // 
            // startDate
            // 
            this.startDate.AllowDrop = true;
            this.startDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startDate.CalendarMonthBackground = System.Drawing.SystemColors.InactiveCaption;
            this.startDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startDate.DisplayFocus = true;
            this.startDate.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.startDate.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.startDate.Location = new System.Drawing.Point(242, 119);
            this.startDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(158, 25);
            this.startDate.TabIndex = 48;
            this.startDate.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.materialCard1.Controls.Add(this.btnWaybill);
            this.materialCard1.Controls.Add(this.btnCheckLogoConnection);
            this.materialCard1.Controls.Add(this.lblLogoConnectionInfo);
            this.materialCard1.Controls.Add(this.btnGetWaybbill);
            this.materialCard1.Controls.Add(this.lblWaybill);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(14, 91);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(1107, 111);
            this.materialCard1.TabIndex = 50;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.anaMenüToolStripMenuItem,
            this.menüToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(20, 60);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1096, 24);
            this.menuStrip1.TabIndex = 51;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // anaMenüToolStripMenuItem
            // 
            this.anaMenüToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.anaMenüToolStripMenuItem.Name = "anaMenüToolStripMenuItem";
            this.anaMenüToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.anaMenüToolStripMenuItem.Text = "Menü";
            this.anaMenüToolStripMenuItem.ToolTipText = "Ana Menü\'den çıkış yapmak için Exit tuşuna tıklayın.";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // menüToolStripMenuItem
            // 
            this.menüToolStripMenuItem.Name = "menüToolStripMenuItem";
            this.menüToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.menüToolStripMenuItem.Text = "Ana Ekran";
            this.menüToolStripMenuItem.ToolTipText = "Ana Ekrana yönlendireleceksiniz..";
            this.menüToolStripMenuItem.Click += new System.EventHandler(this.menüToolStripMenuItem_Click);
            // 
            // btnLastLog
            // 
            this.btnLastLog.BackgroundImage = global::invoiceIntegration.Properties.Resources._1button_logs;
            this.btnLastLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLastLog.Location = new System.Drawing.Point(1063, 44);
            this.btnLastLog.Name = "btnLastLog";
            this.btnLastLog.Size = new System.Drawing.Size(44, 41);
            this.btnLastLog.TabIndex = 45;
            this.btnLastLog.UseSelectable = true;
            this.btnLastLog.Click += new System.EventHandler(this.btnLastLog_Click_1);
            // 
            // Waybills
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1136, 607);
            this.Controls.Add(this.btnLastLog);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.cmbDispatch);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.dataGridInvoice);
            this.Controls.Add(this.materialCard1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Waybills";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.SystemShadow;
            this.Text = "İrsaliye Aktarımı";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Waybills_FormClosed);
            this.Load += new System.EventHandler(this.Waybills_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).EndInit();
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.DataGridView dataGridInvoice;
        private MetroFramework.Controls.MetroButton btnLastLog;
        private MetroFramework.Controls.MetroLabel lblEndDate;
        private MetroFramework.Controls.MetroLabel lblStartDate;
        private MetroFramework.Controls.MetroButton btnWaybill;
        private MetroFramework.Controls.MetroComboBox cmbDispatch;
        private MetroFramework.Controls.MetroButton btnGetWaybbill;
        private System.Windows.Forms.Button btnCheckLogoConnection;
        private MetroFramework.Controls.MetroLabel lblWaybill;
        private System.Windows.Forms.Label lblLogoConnectionInfo;
        private MetroFramework.Controls.MetroDateTime endDate;
        private MetroFramework.Controls.MetroDateTime startDate;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem anaMenüToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menüToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
    }
}