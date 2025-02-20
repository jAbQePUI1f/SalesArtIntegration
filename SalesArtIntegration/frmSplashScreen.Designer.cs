
namespace invoiceIntegration
{
    partial class frmSplashScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplashScreen));
            this.progressBar = new MetroFramework.Controls.MetroProgressBar();
            this.lblLoading = new MetroFramework.Controls.MetroLabel();
            this.invoiceTimer = new System.Windows.Forms.Timer(this.components);
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.progressBarLbl = new System.Windows.Forms.Label();
            this.lblLoading2 = new MetroFramework.Controls.MetroLabel();
            this.invoiceBtn = new MaterialSkin.Controls.MaterialButton();
            this.dispatchBtn = new MaterialSkin.Controls.MaterialButton();
            this.collectionBtn = new MaterialSkin.Controls.MaterialButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.waybillTimer = new System.Windows.Forms.Timer(this.components);
            this.collectionTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(142, 212);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(230, 22);
            this.progressBar.TabIndex = 1;
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblLoading.Location = new System.Drawing.Point(215, 361);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(91, 19);
            this.lblLoading.TabIndex = 2;
            this.lblLoading.Text = "Yükleniyor...";
            // 
            // invoiceTimer
            // 
            this.invoiceTimer.Enabled = true;
            this.invoiceTimer.Tick += new System.EventHandler(this.timerSplashScreen_Tick);
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackColor = System.Drawing.Color.Transparent;
            this.metroPanel1.BackgroundImage = global::SalesArtIntegration.Properties.Resources.logo;
            this.metroPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(142, 123);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(230, 55);
            this.metroPanel1.TabIndex = 0;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // progressBarLbl
            // 
            this.progressBarLbl.AutoSize = true;
            this.progressBarLbl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.progressBarLbl.Location = new System.Drawing.Point(378, 218);
            this.progressBarLbl.Name = "progressBarLbl";
            this.progressBarLbl.Size = new System.Drawing.Size(17, 13);
            this.progressBarLbl.TabIndex = 4;
            this.progressBarLbl.Text = "lbl";
            // 
            // lblLoading2
            // 
            this.lblLoading2.AutoSize = true;
            this.lblLoading2.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblLoading2.Location = new System.Drawing.Point(215, 361);
            this.lblLoading2.Name = "lblLoading2";
            this.lblLoading2.Size = new System.Drawing.Size(87, 19);
            this.lblLoading2.TabIndex = 5;
            this.lblLoading2.Text = "Yükleniyor..";
            this.lblLoading2.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // invoiceBtn
            // 
            this.invoiceBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.invoiceBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.invoiceBtn.Depth = 0;
            this.invoiceBtn.Enabled = false;
            this.invoiceBtn.HighEmphasis = true;
            this.invoiceBtn.Icon = null;
            this.invoiceBtn.Location = new System.Drawing.Point(59, 281);
            this.invoiceBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.invoiceBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.invoiceBtn.Name = "invoiceBtn";
            this.invoiceBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.invoiceBtn.Size = new System.Drawing.Size(127, 36);
            this.invoiceBtn.TabIndex = 6;
            this.invoiceBtn.Text = "Fatura Aktar";
            this.invoiceBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.invoiceBtn.UseAccentColor = false;
            this.invoiceBtn.UseVisualStyleBackColor = true;
            this.invoiceBtn.Click += new System.EventHandler(this.invoiceBtn_Click);
            // 
            // dispatchBtn
            // 
            this.dispatchBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dispatchBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.dispatchBtn.Depth = 0;
            this.dispatchBtn.Enabled = false;
            this.dispatchBtn.HighEmphasis = true;
            this.dispatchBtn.Icon = null;
            this.dispatchBtn.Location = new System.Drawing.Point(194, 281);
            this.dispatchBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dispatchBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.dispatchBtn.Name = "dispatchBtn";
            this.dispatchBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.dispatchBtn.Size = new System.Drawing.Size(133, 36);
            this.dispatchBtn.TabIndex = 7;
            this.dispatchBtn.Text = "İrsaliye Aktar";
            this.dispatchBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.dispatchBtn.UseAccentColor = false;
            this.dispatchBtn.UseVisualStyleBackColor = true;
            this.dispatchBtn.Click += new System.EventHandler(this.dispatchBtn_Click);
            // 
            // collectionBtn
            // 
            this.collectionBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.collectionBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.collectionBtn.Depth = 0;
            this.collectionBtn.Enabled = false;
            this.collectionBtn.HighEmphasis = true;
            this.collectionBtn.Icon = null;
            this.collectionBtn.Location = new System.Drawing.Point(335, 281);
            this.collectionBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.collectionBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.collectionBtn.Name = "collectionBtn";
            this.collectionBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.collectionBtn.Size = new System.Drawing.Size(140, 36);
            this.collectionBtn.TabIndex = 8;
            this.collectionBtn.Text = "Tahsilat Aktar";
            this.collectionBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.collectionBtn.UseAccentColor = false;
            this.collectionBtn.UseVisualStyleBackColor = true;
            this.collectionBtn.Click += new System.EventHandler(this.collectionBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(6, 462);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(234, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "© Copyright 2025 SalesArt. All Rights Reserved.";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(501, 462);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "V . 2 . 0 . 9";
            // 
            // waybillTimer
            // 
            this.waybillTimer.Enabled = true;
            this.waybillTimer.Tick += new System.EventHandler(this.waybillTimer_Tick);
            // 
            // collectionTimer
            // 
            this.collectionTimer.Enabled = true;
            this.collectionTimer.Tick += new System.EventHandler(this.collectionTimer_Tick);
            // 
            // frmSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(566, 485);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.collectionBtn);
            this.Controls.Add(this.dispatchBtn);
            this.Controls.Add(this.invoiceBtn);
            this.Controls.Add(this.lblLoading2);
            this.Controls.Add(this.progressBarLbl);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.metroPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = false;
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_64;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSplashScreen";
            this.Padding = new System.Windows.Forms.Padding(3, 88, 3, 3);
            this.Sizable = false;
            this.Text = "SalesArt Integrator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSplashScreen_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSplashScreen_FormClosed);
            this.Load += new System.EventHandler(this.frmSplashScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroProgressBar progressBar;
        private MetroFramework.Controls.MetroLabel lblLoading;
        private System.Windows.Forms.Timer invoiceTimer;
        private System.Windows.Forms.Label progressBarLbl;
        private MetroFramework.Controls.MetroLabel lblLoading2;
        private MaterialSkin.Controls.MaterialButton invoiceBtn;
        private MaterialSkin.Controls.MaterialButton dispatchBtn;
        private MaterialSkin.Controls.MaterialButton collectionBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer waybillTimer;
        private System.Windows.Forms.Timer collectionTimer;
        protected MetroFramework.Controls.MetroPanel metroPanel1;
    }
}