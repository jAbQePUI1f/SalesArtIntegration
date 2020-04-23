namespace invoiceIntegration
{
    partial class Form1
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
            this.cmbInvoice = new System.Windows.Forms.ComboBox();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.btnGetInvoices = new System.Windows.Forms.Button();
            this.btnSendToLogo = new System.Windows.Forms.Button();
            this.lblInvoice = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCheckLogoConnection = new System.Windows.Forms.Button();
            this.lblLogoConnectionInfo = new System.Windows.Forms.Label();
            this.dataGridInvoice = new System.Windows.Forms.DataGridView();
            this.btnLastLog = new System.Windows.Forms.Button();
            this.chkDispatch = new System.Windows.Forms.CheckBox();
            this.lblDispatch = new System.Windows.Forms.Label();
            this.cmbDispatch = new System.Windows.Forms.ComboBox();
            this.btnWaybill = new System.Windows.Forms.Button();
            this.btnXML = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbInvoice
            // 
            this.cmbInvoice.FormattingEnabled = true;
            this.cmbInvoice.Items.AddRange(new object[] {
            "Satış Faturaları",
            "Hasarlı Satış İade Faturaları",
            "Sağlam Satış İade Faturaları",
            "Verilen Hizmet Faturaları",
            "Alınan Hizmet Faturaları",
            "Alım Faturaları",
            "Hasarılı Alım İade Faturaları",
            "Sağlam Alım İade Faturaları"});
            this.cmbInvoice.Location = new System.Drawing.Point(14, 42);
            this.cmbInvoice.Name = "cmbInvoice";
            this.cmbInvoice.Size = new System.Drawing.Size(149, 21);
            this.cmbInvoice.TabIndex = 1;
            this.cmbInvoice.SelectedIndexChanged += new System.EventHandler(this.cmbInvoice_SelectedIndexChanged);
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDate.Location = new System.Drawing.Point(324, 43);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(124, 20);
            this.startDate.TabIndex = 2;
            // 
            // endDate
            // 
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDate.Location = new System.Drawing.Point(463, 43);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(116, 20);
            this.endDate.TabIndex = 3;
            // 
            // btnGetInvoices
            // 
            this.btnGetInvoices.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGetInvoices.Location = new System.Drawing.Point(12, 72);
            this.btnGetInvoices.Name = "btnGetInvoices";
            this.btnGetInvoices.Size = new System.Drawing.Size(224, 43);
            this.btnGetInvoices.TabIndex = 4;
            this.btnGetInvoices.Text = "Getir";
            this.btnGetInvoices.UseVisualStyleBackColor = true;
            this.btnGetInvoices.Click += new System.EventHandler(this.btnGetInvoices_Click);
            // 
            // btnSendToLogo
            // 
            this.btnSendToLogo.Enabled = false;
            this.btnSendToLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSendToLogo.Location = new System.Drawing.Point(1001, 67);
            this.btnSendToLogo.Name = "btnSendToLogo";
            this.btnSendToLogo.Size = new System.Drawing.Size(153, 43);
            this.btnSendToLogo.TabIndex = 5;
            this.btnSendToLogo.Text = "Faturaları Logoya Aktar";
            this.btnSendToLogo.UseVisualStyleBackColor = true;
            this.btnSendToLogo.Click += new System.EventHandler(this.btnSendToLogo_Click);
            // 
            // lblInvoice
            // 
            this.lblInvoice.AutoSize = true;
            this.lblInvoice.Location = new System.Drawing.Point(11, 28);
            this.lblInvoice.Name = "lblInvoice";
            this.lblInvoice.Size = new System.Drawing.Size(108, 13);
            this.lblInvoice.TabIndex = 7;
            this.lblInvoice.Text = "Fatura Tipini Seçiniz :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(321, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Başlangıç Tarihi:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(459, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Bitiş Tarihi:";
            // 
            // btnCheckLogoConnection
            // 
            this.btnCheckLogoConnection.Enabled = false;
            this.btnCheckLogoConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCheckLogoConnection.Location = new System.Drawing.Point(242, 72);
            this.btnCheckLogoConnection.Name = "btnCheckLogoConnection";
            this.btnCheckLogoConnection.Size = new System.Drawing.Size(131, 43);
            this.btnCheckLogoConnection.TabIndex = 10;
            this.btnCheckLogoConnection.Text = "Bağlantıyı Kontrol Et";
            this.btnCheckLogoConnection.UseVisualStyleBackColor = true;
            this.btnCheckLogoConnection.Click += new System.EventHandler(this.btnCheckLogoConnection_Click);
            // 
            // lblLogoConnectionInfo
            // 
            this.lblLogoConnectionInfo.AutoSize = true;
            this.lblLogoConnectionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLogoConnectionInfo.Location = new System.Drawing.Point(379, 85);
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
            this.dataGridInvoice.Size = new System.Drawing.Size(1145, 593);
            this.dataGridInvoice.TabIndex = 12;
            // 
            // btnLastLog
            // 
            this.btnLastLog.Location = new System.Drawing.Point(1082, 39);
            this.btnLastLog.Name = "btnLastLog";
            this.btnLastLog.Size = new System.Drawing.Size(72, 22);
            this.btnLastLog.TabIndex = 13;
            this.btnLastLog.Text = "Loglar";
            this.btnLastLog.UseVisualStyleBackColor = true;
            this.btnLastLog.Click += new System.EventHandler(this.btnLastLog_Click);
            // 
            // chkDispatch
            // 
            this.chkDispatch.AutoSize = true;
            this.chkDispatch.Location = new System.Drawing.Point(168, 8);
            this.chkDispatch.Name = "chkDispatch";
            this.chkDispatch.Size = new System.Drawing.Size(86, 17);
            this.chkDispatch.TabIndex = 19;
            this.chkDispatch.Text = "İrsaliye Aktar";
            this.chkDispatch.UseVisualStyleBackColor = true;
            this.chkDispatch.Visible = false;
            this.chkDispatch.CheckedChanged += new System.EventHandler(this.chkDispatch_CheckedChanged);
            // 
            // lblDispatch
            // 
            this.lblDispatch.AutoSize = true;
            this.lblDispatch.Location = new System.Drawing.Point(165, 28);
            this.lblDispatch.Name = "lblDispatch";
            this.lblDispatch.Size = new System.Drawing.Size(110, 13);
            this.lblDispatch.TabIndex = 18;
            this.lblDispatch.Text = "İrsaliye Tipini Seçiniz :";
            this.lblDispatch.Visible = false;
            // 
            // cmbDispatch
            // 
            this.cmbDispatch.FormattingEnabled = true;
            this.cmbDispatch.Items.AddRange(new object[] {
            "Satış İrsaliyesi",
            "Hasarlı Satış İade İrsaliyesi",
            "Sağlam Satış İade İrsaliyesi",
            "Alım İrsaliyesi",
            "Hasarılı Alım İade İrsaliyesi",
            "Sağlam Alım İade İrsaliyesi"});
            this.cmbDispatch.Location = new System.Drawing.Point(168, 42);
            this.cmbDispatch.Name = "cmbDispatch";
            this.cmbDispatch.Size = new System.Drawing.Size(149, 21);
            this.cmbDispatch.TabIndex = 17;
            this.cmbDispatch.Visible = false;
            this.cmbDispatch.SelectedIndexChanged += new System.EventHandler(this.cmbDispatch_SelectedIndexChanged);
            // 
            // btnWaybill
            // 
            this.btnWaybill.Enabled = false;
            this.btnWaybill.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnWaybill.Location = new System.Drawing.Point(842, 67);
            this.btnWaybill.Name = "btnWaybill";
            this.btnWaybill.Size = new System.Drawing.Size(153, 43);
            this.btnWaybill.TabIndex = 20;
            this.btnWaybill.Text = "İrsaliyeleri Aktar";
            this.btnWaybill.UseVisualStyleBackColor = true;
            this.btnWaybill.Visible = false;
            this.btnWaybill.Click += new System.EventHandler(this.btnWaybill_Click);
            // 
            // btnXML
            // 
            this.btnXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnXML.Location = new System.Drawing.Point(683, 67);
            this.btnXML.Name = "btnXML";
            this.btnXML.Size = new System.Drawing.Size(153, 43);
            this.btnXML.TabIndex = 21;
            this.btnXML.Text = "Faturaları XML Kaydet";
            this.btnXML.UseVisualStyleBackColor = true;
            this.btnXML.Visible = false;
            this.btnXML.Click += new System.EventHandler(this.btnXML_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(59, 129);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(87, 17);
            this.chkSelectAll.TabIndex = 22;
            this.chkSelectAll.Text = "Tümünü Seç";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
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
            // 
            // Column15
            // 
            this.Column15.HeaderText = "KDV Tutarı";
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "KDV Hatiç Tutar";
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 730);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnXML);
            this.Controls.Add(this.btnWaybill);
            this.Controls.Add(this.chkDispatch);
            this.Controls.Add(this.lblDispatch);
            this.Controls.Add(this.cmbDispatch);
            this.Controls.Add(this.btnLastLog);
            this.Controls.Add(this.dataGridInvoice);
            this.Controls.Add(this.lblLogoConnectionInfo);
            this.Controls.Add(this.btnCheckLogoConnection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblInvoice);
            this.Controls.Add(this.btnSendToLogo);
            this.Controls.Add(this.btnGetInvoices);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.cmbInvoice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "salesArt Fatura Aktarım Arayüzü";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbInvoice;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.Button btnGetInvoices;
        private System.Windows.Forms.Button btnSendToLogo;
        private System.Windows.Forms.Label lblInvoice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCheckLogoConnection;
        private System.Windows.Forms.Label lblLogoConnectionInfo;
        private System.Windows.Forms.DataGridView dataGridInvoice;
        private System.Windows.Forms.Button btnLastLog;
        private System.Windows.Forms.CheckBox chkDispatch;
        private System.Windows.Forms.Label lblDispatch;
        private System.Windows.Forms.ComboBox cmbDispatch;
        private System.Windows.Forms.Button btnWaybill;
        private System.Windows.Forms.Button btnXML;
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
        private System.Windows.Forms.CheckBox chkSelectAll;
    }
}

