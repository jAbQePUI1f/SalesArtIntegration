namespace invoiceIntegration
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnCheckLogoConnection = new System.Windows.Forms.Button();
            this.lblLogoConnectionInfo = new System.Windows.Forms.Label();
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
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.cmbInvoice = new MetroFramework.Controls.MetroComboBox();
            this.lblInvoice = new MetroFramework.Controls.MetroLabel();
            this.btnGetInvoices = new MetroFramework.Controls.MetroButton();
            this.cmbDispatch = new MetroFramework.Controls.MetroComboBox();
            this.chkDispatch = new MetroFramework.Controls.MetroCheckBox();
            this.btnWaybill = new MetroFramework.Controls.MetroButton();
            this.lblStartDate = new MetroFramework.Controls.MetroLabel();
            this.lblEndDate = new MetroFramework.Controls.MetroLabel();
            this.startDate = new MetroFramework.Controls.MetroDateTime();
            this.endDate = new MetroFramework.Controls.MetroDateTime();
            this.btnXML = new MetroFramework.Controls.MetroButton();
            this.btnLastLog = new MetroFramework.Controls.MetroButton();
            this.btnSendOrderToLogo = new MetroFramework.Controls.MetroButton();
            this.btnSendToLogo = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCheckLogoConnection
            // 
            this.btnCheckLogoConnection.Enabled = false;
            this.btnCheckLogoConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCheckLogoConnection.Location = new System.Drawing.Point(1071, 26);
            this.btnCheckLogoConnection.Name = "btnCheckLogoConnection";
            this.btnCheckLogoConnection.Size = new System.Drawing.Size(207, 46);
            this.btnCheckLogoConnection.TabIndex = 10;
            this.btnCheckLogoConnection.Text = "Bağlantıyı Kontrol Et";
            this.btnCheckLogoConnection.UseVisualStyleBackColor = true;
            this.btnCheckLogoConnection.Click += new System.EventHandler(this.btnCheckLogoConnection_Click);
            // 
            // lblLogoConnectionInfo
            // 
            this.lblLogoConnectionInfo.AutoSize = true;
            this.lblLogoConnectionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLogoConnectionInfo.Location = new System.Drawing.Point(1068, 7);
            this.lblLogoConnectionInfo.Name = "lblLogoConnectionInfo";
            this.lblLogoConnectionInfo.Size = new System.Drawing.Size(25, 16);
            this.lblLogoConnectionInfo.TabIndex = 11;
            this.lblLogoConnectionInfo.Text = "lbl";
            // 
            // dataGridInvoice
            // 
            this.dataGridInvoice.AllowUserToAddRows = false;
            this.dataGridInvoice.AllowUserToOrderColumns = true;
            this.dataGridInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.dataGridInvoice.Location = new System.Drawing.Point(12, 121);
            this.dataGridInvoice.Name = "dataGridInvoice";
            this.dataGridInvoice.Size = new System.Drawing.Size(1266, 593);
            this.dataGridInvoice.TabIndex = 12;
            // 
            // chk
            // 
            this.chk.HeaderText = "Seçim";
            this.chk.Name = "chk";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Fatura Tipi";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // number
            // 
            this.number.HeaderText = "Fatura No";
            this.number.Name = "number";
            this.number.ReadOnly = true;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Fatura Tarihi";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Belge No";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Müşteri Kodu";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            // 
            // customerName
            // 
            this.customerName.HeaderText = "Müşteri Ünvanı";
            this.customerName.Name = "customerName";
            this.customerName.Width = 200;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Toplam İndirim Tutarı";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.Width = 130;
            // 
            // Column15
            // 
            this.Column15.HeaderText = "KDV Tutarı";
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "KDV Hariç Tutar";
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            this.Column16.Width = 120;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(59, 124);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(87, 17);
            this.chkSelectAll.TabIndex = 22;
            this.chkSelectAll.Text = "Tümünü Seç";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // cmbInvoice
            // 
            this.cmbInvoice.FormattingEnabled = true;
            this.cmbInvoice.ItemHeight = 23;
            this.cmbInvoice.Items.AddRange(new object[] {
            "Satış Faturaları",
            "Hasarlı Satış İade Faturaları",
            "Sağlam Satış İade Faturaları",
            "Verilen Hizmet Faturaları",
            "Alınan Hizmet Faturaları",
            "Alım Faturaları",
            "Hasarılı Alım İade Faturaları",
            "Sağlam Alım İade Faturaları"});
            this.cmbInvoice.Location = new System.Drawing.Point(12, 33);
            this.cmbInvoice.Name = "cmbInvoice";
            this.cmbInvoice.Size = new System.Drawing.Size(194, 29);
            this.cmbInvoice.TabIndex = 24;
            this.cmbInvoice.UseSelectable = true;
            this.cmbInvoice.SelectedIndexChanged += new System.EventHandler(this.cmbInvoice_SelectedIndexChanged);
            // 
            // lblInvoice
            // 
            this.lblInvoice.AutoSize = true;
            this.lblInvoice.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblInvoice.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblInvoice.Location = new System.Drawing.Point(12, 12);
            this.lblInvoice.Name = "lblInvoice";
            this.lblInvoice.Size = new System.Drawing.Size(116, 15);
            this.lblInvoice.TabIndex = 25;
            this.lblInvoice.Text = "Fatura Tipiniz Seçiniz";
            // 
            // btnGetInvoices
            // 
            this.btnGetInvoices.Location = new System.Drawing.Point(12, 72);
            this.btnGetInvoices.Name = "btnGetInvoices";
            this.btnGetInvoices.Size = new System.Drawing.Size(194, 43);
            this.btnGetInvoices.TabIndex = 26;
            this.btnGetInvoices.Text = "Faturaları getir";
            this.btnGetInvoices.UseSelectable = true;
            this.btnGetInvoices.Click += new System.EventHandler(this.btnGetInvoices_Click);
            // 
            // cmbDispatch
            // 
            this.cmbDispatch.FormattingEnabled = true;
            this.cmbDispatch.ItemHeight = 23;
            this.cmbDispatch.Items.AddRange(new object[] {
            "Satış İrsaliyesi",
            "Hasarlı Satış İade İrsaliyesi",
            "Sağlam Satış İade İrsaliyesi",
            "Alım İrsaliyesi",
            "Hasarılı Alım İade İrsaliyesi",
            "Sağlam Alım İade İrsaliyesi"});
            this.cmbDispatch.Location = new System.Drawing.Point(610, 32);
            this.cmbDispatch.Name = "cmbDispatch";
            this.cmbDispatch.Size = new System.Drawing.Size(189, 29);
            this.cmbDispatch.TabIndex = 27;
            this.cmbDispatch.UseSelectable = true;
            this.cmbDispatch.Visible = false;
            this.cmbDispatch.SelectedIndexChanged += new System.EventHandler(this.cmbDispatch_SelectedIndexChanged);
            // 
            // chkDispatch
            // 
            this.chkDispatch.AutoSize = true;
            this.chkDispatch.Location = new System.Drawing.Point(610, 10);
            this.chkDispatch.Name = "chkDispatch";
            this.chkDispatch.Size = new System.Drawing.Size(90, 15);
            this.chkDispatch.TabIndex = 28;
            this.chkDispatch.Text = "İrsaliye Aktar";
            this.chkDispatch.UseSelectable = true;
            this.chkDispatch.Visible = false;
            this.chkDispatch.CheckedChanged += new System.EventHandler(this.chkDispatch_CheckedChanged);
            // 
            // btnWaybill
            // 
            this.btnWaybill.Location = new System.Drawing.Point(610, 72);
            this.btnWaybill.Name = "btnWaybill";
            this.btnWaybill.Size = new System.Drawing.Size(189, 43);
            this.btnWaybill.TabIndex = 29;
            this.btnWaybill.Text = "İrsaliyeleri Aktar";
            this.btnWaybill.UseSelectable = true;
            this.btnWaybill.Visible = false;
            this.btnWaybill.Click += new System.EventHandler(this.btnWaybill_Click);
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblStartDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStartDate.Location = new System.Drawing.Point(223, 12);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(88, 15);
            this.lblStartDate.TabIndex = 30;
            this.lblStartDate.Text = "Başlangıç Tarihi";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblEndDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEndDate.Location = new System.Drawing.Point(407, 12);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(60, 15);
            this.lblEndDate.TabIndex = 31;
            this.lblEndDate.Text = "Bitiş Tarihi";
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(223, 33);
            this.startDate.MinimumSize = new System.Drawing.Size(0, 29);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(178, 29);
            this.startDate.TabIndex = 32;
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(407, 33);
            this.endDate.MinimumSize = new System.Drawing.Size(0, 29);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(175, 29);
            this.endDate.TabIndex = 33;
            // 
            // btnXML
            // 
            this.btnXML.Location = new System.Drawing.Point(820, 72);
            this.btnXML.Name = "btnXML";
            this.btnXML.Size = new System.Drawing.Size(243, 43);
            this.btnXML.TabIndex = 34;
            this.btnXML.Text = "Faturaları XML\'e Aktar";
            this.btnXML.UseSelectable = true;
            this.btnXML.Visible = false;
            this.btnXML.Click += new System.EventHandler(this.btnXML_Click);
            // 
            // btnLastLog
            // 
            this.btnLastLog.Location = new System.Drawing.Point(1071, 76);
            this.btnLastLog.Name = "btnLastLog";
            this.btnLastLog.Size = new System.Drawing.Size(207, 41);
            this.btnLastLog.TabIndex = 35;
            this.btnLastLog.Text = "Log Kayıtları";
            this.btnLastLog.UseSelectable = true;
            this.btnLastLog.Click += new System.EventHandler(this.btnLastLog_Click);
            // 
            // btnSendOrderToLogo
            // 
            this.btnSendOrderToLogo.Location = new System.Drawing.Point(820, 25);
            this.btnSendOrderToLogo.Name = "btnSendOrderToLogo";
            this.btnSendOrderToLogo.Size = new System.Drawing.Size(243, 43);
            this.btnSendOrderToLogo.TabIndex = 36;
            this.btnSendOrderToLogo.Text = "Siparişleri Logoya Aktar";
            this.btnSendOrderToLogo.UseSelectable = true;
            this.btnSendOrderToLogo.Visible = false;
            this.btnSendOrderToLogo.Click += new System.EventHandler(this.btnSendOrderToLogo_Click);
            // 
            // btnSendToLogo
            // 
            this.btnSendToLogo.Location = new System.Drawing.Point(223, 72);
            this.btnSendToLogo.Name = "btnSendToLogo";
            this.btnSendToLogo.Size = new System.Drawing.Size(359, 43);
            this.btnSendToLogo.TabIndex = 37;
            this.btnSendToLogo.Text = "Faturaları Logoya Aktar";
            this.btnSendToLogo.UseSelectable = true;
            this.btnSendToLogo.Click += new System.EventHandler(this.btnSendToLogo_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 730);
            this.Controls.Add(this.btnSendToLogo);
            this.Controls.Add(this.btnSendOrderToLogo);
            this.Controls.Add(this.btnLastLog);
            this.Controls.Add(this.btnXML);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.btnWaybill);
            this.Controls.Add(this.chkDispatch);
            this.Controls.Add(this.cmbDispatch);
            this.Controls.Add(this.btnGetInvoices);
            this.Controls.Add(this.lblInvoice);
            this.Controls.Add(this.cmbInvoice);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.dataGridInvoice);
            this.Controls.Add(this.lblLogoConnectionInfo);
            this.Controls.Add(this.btnCheckLogoConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCheckLogoConnection;
        private System.Windows.Forms.Label lblLogoConnectionInfo;
        private System.Windows.Forms.DataGridView dataGridInvoice;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private MetroFramework.Controls.MetroComboBox cmbInvoice;
        private MetroFramework.Controls.MetroLabel lblInvoice;
        private MetroFramework.Controls.MetroButton btnGetInvoices;
        private MetroFramework.Controls.MetroComboBox cmbDispatch;
        private MetroFramework.Controls.MetroCheckBox chkDispatch;
        private MetroFramework.Controls.MetroButton btnWaybill;
        private MetroFramework.Controls.MetroLabel lblStartDate;
        private MetroFramework.Controls.MetroLabel lblEndDate;
        private MetroFramework.Controls.MetroDateTime startDate;
        private MetroFramework.Controls.MetroDateTime endDate;
        private MetroFramework.Controls.MetroButton btnXML;
        private MetroFramework.Controls.MetroButton btnLastLog;
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
        private MetroFramework.Controls.MetroButton btnSendOrderToLogo;
        private MetroFramework.Controls.MetroButton btnSendToLogo;
    }
}

