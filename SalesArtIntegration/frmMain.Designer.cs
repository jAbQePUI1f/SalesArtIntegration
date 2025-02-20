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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.lblStartDate = new MetroFramework.Controls.MetroLabel();
            this.lblEndDate = new MetroFramework.Controls.MetroLabel();
            this.startDate = new MetroFramework.Controls.MetroDateTime();
            this.endDate = new MetroFramework.Controls.MetroDateTime();
            this.btnXML = new MetroFramework.Controls.MetroButton();
            this.btnLastLog = new MetroFramework.Controls.MetroButton();
            this.btnSendOrderToLogo = new MetroFramework.Controls.MetroButton();
            this.btnSendToLogo = new MetroFramework.Controls.MetroButton();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.anaMenüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hakkındaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).BeginInit();
            this.materialCard1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCheckLogoConnection
            // 
            this.btnCheckLogoConnection.Enabled = false;
            this.btnCheckLogoConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCheckLogoConnection.Location = new System.Drawing.Point(995, 66);
            this.btnCheckLogoConnection.Name = "btnCheckLogoConnection";
            this.btnCheckLogoConnection.Size = new System.Drawing.Size(149, 46);
            this.btnCheckLogoConnection.TabIndex = 10;
            this.btnCheckLogoConnection.Text = "Bağlantıyı Kontrol Et";
            this.btnCheckLogoConnection.UseVisualStyleBackColor = true;
            this.btnCheckLogoConnection.Click += new System.EventHandler(this.btnCheckLogoConnection_Click);
            // 
            // lblLogoConnectionInfo
            // 
            this.lblLogoConnectionInfo.AutoSize = true;
            this.lblLogoConnectionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLogoConnectionInfo.Location = new System.Drawing.Point(998, 48);
            this.lblLogoConnectionInfo.Name = "lblLogoConnectionInfo";
            this.lblLogoConnectionInfo.Size = new System.Drawing.Size(20, 15);
            this.lblLogoConnectionInfo.TabIndex = 11;
            this.lblLogoConnectionInfo.Text = "lbl";
            // 
            // dataGridInvoice
            // 
            this.dataGridInvoice.AllowUserToAddRows = false;
            this.dataGridInvoice.AllowUserToDeleteRows = false;
            this.dataGridInvoice.AllowUserToOrderColumns = true;
            this.dataGridInvoice.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
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
            this.dataGridInvoice.Location = new System.Drawing.Point(12, 231);
            this.dataGridInvoice.Name = "dataGridInvoice";
            this.dataGridInvoice.RowHeadersWidth = 51;
            this.dataGridInvoice.Size = new System.Drawing.Size(1154, 379);
            this.dataGridInvoice.TabIndex = 12;
            // 
            // chk
            // 
            this.chk.FillWeight = 45F;
            this.chk.Frozen = true;
            this.chk.HeaderText = "     ";
            this.chk.MinimumWidth = 50;
            this.chk.Name = "chk";
            this.chk.ToolTipText = "Tümünü Seçiniz..";
            this.chk.Width = 50;
            // 
            // Column9
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column9.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column9.Frozen = true;
            this.Column9.HeaderText = "Fatura Tipi";
            this.Column9.MinimumWidth = 120;
            this.Column9.Name = "Column9";
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column9.Width = 120;
            // 
            // number
            // 
            this.number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.number.DefaultCellStyle = dataGridViewCellStyle2;
            this.number.Frozen = true;
            this.number.HeaderText = "Fatura No";
            this.number.MinimumWidth = 100;
            this.number.Name = "number";
            // 
            // Column11
            // 
            this.Column11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column11.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column11.Frozen = true;
            this.Column11.HeaderText = "Fatura Tarihi";
            this.Column11.MinimumWidth = 100;
            this.Column11.Name = "Column11";
            // 
            // Column12
            // 
            this.Column12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column12.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column12.Frozen = true;
            this.Column12.HeaderText = "Belge No";
            this.Column12.MinimumWidth = 50;
            this.Column12.Name = "Column12";
            // 
            // Column13
            // 
            this.Column13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column13.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column13.Frozen = true;
            this.Column13.HeaderText = "Müşteri Kodu";
            this.Column13.MinimumWidth = 100;
            this.Column13.Name = "Column13";
            this.Column13.Width = 110;
            // 
            // customerName
            // 
            this.customerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.customerName.DefaultCellStyle = dataGridViewCellStyle6;
            this.customerName.HeaderText = "Müşteri Ünvanı";
            this.customerName.MinimumWidth = 100;
            this.customerName.Name = "customerName";
            this.customerName.Width = 160;
            // 
            // Column14
            // 
            this.Column14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column14.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column14.HeaderText = "Toplam İnd. Tutarı";
            this.Column14.MinimumWidth = 100;
            this.Column14.Name = "Column14";
            this.Column14.Width = 120;
            // 
            // Column15
            // 
            this.Column15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column15.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column15.HeaderText = "KDV Tutarı";
            this.Column15.MinimumWidth = 100;
            this.Column15.Name = "Column15";
            // 
            // Column16
            // 
            this.Column16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column16.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column16.HeaderText = "KDV Hariç Tutar";
            this.Column16.MinimumWidth = 100;
            this.Column16.Name = "Column16";
            this.Column16.Width = 120;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkSelectAll.Location = new System.Drawing.Point(17, 234);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(91, 17);
            this.chkSelectAll.TabIndex = 22;
            this.chkSelectAll.Text = "Tümünü Seç  ";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // cmbInvoice
            // 
            this.cmbInvoice.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbInvoice.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cmbInvoice.FormattingEnabled = true;
            this.cmbInvoice.ItemHeight = 19;
            this.cmbInvoice.Items.AddRange(new object[] {
            "Satış Faturaları",
            "Hasarlı Satış İade Faturaları",
            "Sağlam Satış İade Faturaları",
            "Verilen Hizmet Faturaları",
            "Alınan Hizmet Faturaları",
            "Alım Faturaları",
            "Hasarılı Alım İade Faturaları",
            "Sağlam Alım İade Faturaları"});
            this.cmbInvoice.Location = new System.Drawing.Point(6, 27);
            this.cmbInvoice.Name = "cmbInvoice";
            this.cmbInvoice.Size = new System.Drawing.Size(199, 25);
            this.cmbInvoice.TabIndex = 24;
            this.cmbInvoice.UseSelectable = true;
            this.cmbInvoice.SelectedIndexChanged += new System.EventHandler(this.cmbInvoice_SelectedIndexChanged);
            // 
            // lblInvoice
            // 
            this.lblInvoice.AutoSize = true;
            this.lblInvoice.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblInvoice.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblInvoice.Location = new System.Drawing.Point(6, 6);
            this.lblInvoice.Name = "lblInvoice";
            this.lblInvoice.Size = new System.Drawing.Size(63, 15);
            this.lblInvoice.TabIndex = 25;
            this.lblInvoice.Text = "Tip Seçiniz";
            // 
            // btnGetInvoices
            // 
            this.btnGetInvoices.Location = new System.Drawing.Point(6, 66);
            this.btnGetInvoices.Name = "btnGetInvoices";
            this.btnGetInvoices.Size = new System.Drawing.Size(199, 43);
            this.btnGetInvoices.TabIndex = 26;
            this.btnGetInvoices.Text = "Belgeleri Listele";
            this.btnGetInvoices.UseSelectable = true;
            this.btnGetInvoices.Click += new System.EventHandler(this.btnGetInvoices_Click);
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblStartDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStartDate.Location = new System.Drawing.Point(292, 5);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(89, 15);
            this.lblStartDate.TabIndex = 30;
            this.lblStartDate.Text = "Başlangıç Tarihi";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblEndDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEndDate.Location = new System.Drawing.Point(505, 5);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(61, 15);
            this.lblEndDate.TabIndex = 31;
            this.lblEndDate.Text = "Bitiş Tarihi";
            // 
            // startDate
            // 
            this.startDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startDate.DisplayFocus = true;
            this.startDate.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.startDate.ImeMode = System.Windows.Forms.ImeMode.On;
            this.startDate.Location = new System.Drawing.Point(292, 26);
            this.startDate.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.startDate.MinDate = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            this.startDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(191, 25);
            this.startDate.TabIndex = 32;
            this.startDate.Theme = MetroFramework.MetroThemeStyle.Light;
            this.startDate.UseStyleColors = true;
            // 
            // endDate
            // 
            this.endDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.endDate.DisplayFocus = true;
            this.endDate.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.endDate.ImeMode = System.Windows.Forms.ImeMode.On;
            this.endDate.Location = new System.Drawing.Point(505, 27);
            this.endDate.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.endDate.MinDate = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            this.endDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(192, 25);
            this.endDate.TabIndex = 33;
            this.endDate.Theme = MetroFramework.MetroThemeStyle.Light;
            this.endDate.UseStyleColors = true;
            // 
            // btnXML
            // 
            this.btnXML.Location = new System.Drawing.Point(796, 66);
            this.btnXML.Name = "btnXML";
            this.btnXML.Size = new System.Drawing.Size(180, 43);
            this.btnXML.TabIndex = 34;
            this.btnXML.Text = "XML Çıkart";
            this.btnXML.UseSelectable = true;
            this.btnXML.Visible = false;
            this.btnXML.Click += new System.EventHandler(this.btnXML_Click);
            // 
            // btnLastLog
            // 
            this.btnLastLog.BackgroundImage = global::SalesArtIntegration.Properties.Resources._1button_logs;
            this.btnLastLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLastLog.Location = new System.Drawing.Point(1128, 43);
            this.btnLastLog.Name = "btnLastLog";
            this.btnLastLog.Size = new System.Drawing.Size(38, 41);
            this.btnLastLog.TabIndex = 35;
            this.btnLastLog.UseSelectable = true;
            this.btnLastLog.Click += new System.EventHandler(this.btnLastLog_Click);
            // 
            // btnSendOrderToLogo
            // 
            this.btnSendOrderToLogo.Location = new System.Drawing.Point(796, 17);
            this.btnSendOrderToLogo.Name = "btnSendOrderToLogo";
            this.btnSendOrderToLogo.Size = new System.Drawing.Size(180, 43);
            this.btnSendOrderToLogo.TabIndex = 36;
            this.btnSendOrderToLogo.Text = "Siparişleri Aktar";
            this.btnSendOrderToLogo.UseSelectable = true;
            this.btnSendOrderToLogo.Visible = false;
            this.btnSendOrderToLogo.Click += new System.EventHandler(this.btnSendOrderToLogo_Click);
            // 
            // btnSendToLogo
            // 
            this.btnSendToLogo.Location = new System.Drawing.Point(505, 66);
            this.btnSendToLogo.Name = "btnSendToLogo";
            this.btnSendToLogo.Size = new System.Drawing.Size(192, 43);
            this.btnSendToLogo.TabIndex = 37;
            this.btnSendToLogo.Text = "Aktarım Başlat";
            this.btnSendToLogo.UseSelectable = true;
            this.btnSendToLogo.Click += new System.EventHandler(this.btnSendToLogo_Click);
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.materialCard1.Controls.Add(this.btnSendToLogo);
            this.materialCard1.Controls.Add(this.btnSendOrderToLogo);
            this.materialCard1.Controls.Add(this.btnCheckLogoConnection);
            this.materialCard1.Controls.Add(this.lblLogoConnectionInfo);
            this.materialCard1.Controls.Add(this.btnXML);
            this.materialCard1.Controls.Add(this.cmbInvoice);
            this.materialCard1.Controls.Add(this.endDate);
            this.materialCard1.Controls.Add(this.lblInvoice);
            this.materialCard1.Controls.Add(this.startDate);
            this.materialCard1.Controls.Add(this.btnGetInvoices);
            this.materialCard1.Controls.Add(this.lblEndDate);
            this.materialCard1.Controls.Add(this.lblStartDate);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(12, 91);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(1154, 123);
            this.materialCard1.TabIndex = 51;
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
            this.menuStrip1.Size = new System.Drawing.Size(1176, 24);
            this.menuStrip1.TabIndex = 52;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // anaMenüToolStripMenuItem
            // 
            this.anaMenüToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.hakkındaToolStripMenuItem});
            this.anaMenüToolStripMenuItem.Name = "anaMenüToolStripMenuItem";
            this.anaMenüToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.anaMenüToolStripMenuItem.Text = "Menü";
            this.anaMenüToolStripMenuItem.ToolTipText = "Ana Menü\'den çıkış yapmak için Exit tuşuna tıklayın.";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // hakkındaToolStripMenuItem
            // 
            this.hakkındaToolStripMenuItem.Name = "hakkındaToolStripMenuItem";
            this.hakkındaToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.hakkındaToolStripMenuItem.Text = "Hakkında";
            this.hakkındaToolStripMenuItem.Click += new System.EventHandler(this.hakkındaToolStripMenuItem_Click);
            // 
            // menüToolStripMenuItem
            // 
            this.menüToolStripMenuItem.Name = "menüToolStripMenuItem";
            this.menüToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.menüToolStripMenuItem.Text = "Ana Ekran";
            this.menüToolStripMenuItem.ToolTipText = "Ana Ekrana yönlendireleceksiniz..";
            this.menüToolStripMenuItem.Click += new System.EventHandler(this.menüToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1216, 626);
            this.ControlBox = false;
            this.Controls.Add(this.btnLastLog);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.dataGridInvoice);
            this.Controls.Add(this.materialCard1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.SystemShadow;
            this.Text = "Fatura Aktarımı";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridInvoice)).EndInit();
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private MetroFramework.Controls.MetroLabel lblStartDate;
        private MetroFramework.Controls.MetroLabel lblEndDate;
        private MetroFramework.Controls.MetroButton btnXML;
        private MetroFramework.Controls.MetroButton btnLastLog;
        private MetroFramework.Controls.MetroButton btnSendOrderToLogo;
        private MetroFramework.Controls.MetroButton btnSendToLogo;
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

