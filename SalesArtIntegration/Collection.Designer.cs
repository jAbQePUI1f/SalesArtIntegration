
namespace invoiceIntegration
{
    partial class Collection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Collection));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anaEkranToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblEndDate = new MetroFramework.Controls.MetroLabel();
            this.btnGetCollection = new MetroFramework.Controls.MetroButton();
            this.lblStartDate = new MetroFramework.Controls.MetroLabel();
            this.lblWaybill = new MetroFramework.Controls.MetroLabel();
            this.btnLastLog = new MetroFramework.Controls.MetroButton();
            this.startDate = new MetroFramework.Controls.MetroDateTime();
            this.cmbCollection = new MetroFramework.Controls.MetroComboBox();
            this.btnCheckLogoConnection = new System.Windows.Forms.Button();
            this.lblLogoConnectionInfo = new System.Windows.Forms.Label();
            this.endDate = new MetroFramework.Controls.MetroDateTime();
            this.btnSendCollection = new MetroFramework.Controls.MetroButton();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridCollection = new System.Windows.Forms.DataGridView();
            this.chk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.materialCard1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCollection)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menüToolStripMenuItem,
            this.anaEkranToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(20, 60);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1091, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menüToolStripMenuItem
            // 
            this.menüToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.menüToolStripMenuItem.Name = "menüToolStripMenuItem";
            this.menüToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menüToolStripMenuItem.Text = "Menü";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // anaEkranToolStripMenuItem
            // 
            this.anaEkranToolStripMenuItem.Name = "anaEkranToolStripMenuItem";
            this.anaEkranToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.anaEkranToolStripMenuItem.Text = "Ana Ekran";
            this.anaEkranToolStripMenuItem.Click += new System.EventHandler(this.anaEkranToolStripMenuItem_Click);
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.BackColor = System.Drawing.Color.LightGray;
            this.lblEndDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblEndDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEndDate.Location = new System.Drawing.Point(377, 7);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(60, 15);
            this.lblEndDate.TabIndex = 53;
            this.lblEndDate.Text = "Bitiş Tarihi";
            // 
            // btnGetCollection
            // 
            //this.btnGetCollection.Location = new System.Drawing.Point(15, 57);
            //this.btnGetCollection.Name = "btnGetCollection";
            //this.btnGetCollection.Size = new System.Drawing.Size(194, 43);
            //this.btnGetCollection.TabIndex = 37;
            //this.btnGetCollection.Text = "Tahsilatları Getir";
            //this.btnGetCollection.UseSelectable = true;
            //this.btnGetCollection.Click += new System.EventHandler(this.btnGetCollection_Click);
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.BackColor = System.Drawing.Color.LightGray;
            this.lblStartDate.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblStartDate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStartDate.Location = new System.Drawing.Point(225, 7);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(88, 15);
            this.lblStartDate.TabIndex = 52;
            this.lblStartDate.Text = "Başlangıç Tarihi";
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
            // btnLastLog
            // 
            
            this.btnLastLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLastLog.Location = new System.Drawing.Point(1076, 43);
            this.btnLastLog.Name = "btnLastLog";
            this.btnLastLog.Size = new System.Drawing.Size(44, 41);
            this.btnLastLog.TabIndex = 54;
            this.btnLastLog.UseSelectable = true;
            this.btnLastLog.Click += new System.EventHandler(this.btnLastLog_Click);
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
            this.startDate.Location = new System.Drawing.Point(225, 26);
            this.startDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(159, 25);
            this.startDate.TabIndex = 55;
            this.startDate.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // cmbCollection
            // 
            //this.cmbCollection.FontSize = MetroFramework.MetroComboBoxSize.Small;
            //this.cmbCollection.FormattingEnabled = true;
            //this.cmbCollection.ItemHeight = 19;
            //this.cmbCollection.Items.AddRange(new object[] {
            //"Nakit",
            //"Kredi Kartı",
            //"Çek"});
            //this.cmbCollection.Location = new System.Drawing.Point(15, 26);
            //this.cmbCollection.Name = "cmbCollection";
            //this.cmbCollection.Size = new System.Drawing.Size(194, 25);
            //this.cmbCollection.TabIndex = 51;
            //this.cmbCollection.Theme = MetroFramework.MetroThemeStyle.Light;
            //this.cmbCollection.UseSelectable = true;
            //this.cmbCollection.SelectedIndexChanged += new System.EventHandler(this.cmbCollection_SelectedIndexChanged);
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
            this.btnCheckLogoConnection.Click += new System.EventHandler(this.btnCheckLogoConnection_Click);
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
            this.endDate.Location = new System.Drawing.Point(390, 26);
            this.endDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(165, 25);
            this.endDate.TabIndex = 56;
            // 
            // btnSendCollection
            // 
            //this.btnSendCollection.Location = new System.Drawing.Point(366, 57);
            //this.btnSendCollection.Name = "btnSendCollection";
            //this.btnSendCollection.Size = new System.Drawing.Size(189, 43);
            //this.btnSendCollection.TabIndex = 40;
            //this.btnSendCollection.Text = "Tahsilatları Aktar";
            //this.btnSendCollection.UseSelectable = true;
            //this.btnSendCollection.Click += new System.EventHandler(this.btnSendCollection_Click);
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.materialCard1.Controls.Add(this.lblStartDate);
            this.materialCard1.Controls.Add(this.lblEndDate);
            this.materialCard1.Controls.Add(this.endDate);
            this.materialCard1.Controls.Add(this.startDate);
            this.materialCard1.Controls.Add(this.btnSendCollection);
            this.materialCard1.Controls.Add(this.cmbCollection);
            this.materialCard1.Controls.Add(this.btnCheckLogoConnection);
            this.materialCard1.Controls.Add(this.lblLogoConnectionInfo);
            this.materialCard1.Controls.Add(this.btnGetCollection);
            this.materialCard1.Controls.Add(this.lblWaybill);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(7, 98);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(1113, 111);
            this.materialCard1.TabIndex = 57;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.chkSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkSelectAll.Location = new System.Drawing.Point(10, 222);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(51, 20);
            this.chkSelectAll.TabIndex = 59;
            this.chkSelectAll.Text = "Seç";
            this.chkSelectAll.UseVisualStyleBackColor = false;
            // 
            // dataGridCollection
            // 
            this.dataGridCollection.AllowUserToAddRows = false;
            this.dataGridCollection.AllowUserToDeleteRows = false;
            this.dataGridCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridCollection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridCollection.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(1, 2, 0, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridCollection.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCollection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chk,
            this.Column9,
            this.number,
            this.Column11,
            this.Column12,
            this.Column13,
            this.customerName,
            this.Column15,
            this.Column16});
            this.dataGridCollection.EnableHeadersVisualStyles = false;
            this.dataGridCollection.Location = new System.Drawing.Point(7, 219);
            this.dataGridCollection.Name = "dataGridCollection";
            this.dataGridCollection.ReadOnly = true;
            this.dataGridCollection.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridCollection.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridCollection.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridCollection.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridCollection.Size = new System.Drawing.Size(1114, 346);
            this.dataGridCollection.TabIndex = 58;
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
            this.Column9.HeaderText = "Tahsilat Tipi";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column9.Width = 110;
            // 
            // number
            // 
            this.number.HeaderText = "Tahsilat No";
            this.number.MinimumWidth = 6;
            this.number.Name = "number";
            this.number.ReadOnly = true;
            this.number.Width = 105;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Tahsilat Tarihi";
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
            // Collection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 578);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.dataGridCollection);
            this.Controls.Add(this.btnLastLog);
            this.Controls.Add(this.materialCard1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Collection";
            this.Text = "Tahsilat Aktarımı";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Collection_FormClosing);
            //this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Collection_FormClosed);
            //this.Load += new System.EventHandler(this.Collection_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCollection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menüToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem anaEkranToolStripMenuItem;
        private MetroFramework.Controls.MetroLabel lblEndDate;
        private MetroFramework.Controls.MetroButton btnGetCollection;
        private MetroFramework.Controls.MetroLabel lblStartDate;
        private MetroFramework.Controls.MetroLabel lblWaybill;
        private MetroFramework.Controls.MetroButton btnLastLog;
        private MetroFramework.Controls.MetroDateTime startDate;
        private MetroFramework.Controls.MetroComboBox cmbCollection;
        private System.Windows.Forms.Button btnCheckLogoConnection;
        private System.Windows.Forms.Label lblLogoConnectionInfo;
        private MetroFramework.Controls.MetroDateTime endDate;
        private MetroFramework.Controls.MetroButton btnSendCollection;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.DataGridView dataGridCollection;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chk;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
    }
}