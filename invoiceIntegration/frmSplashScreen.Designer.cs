
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
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.progressBar = new MetroFramework.Controls.MetroProgressBar();
            this.lblLoading = new MetroFramework.Controls.MetroLabel();
            this.timerSplashScreen = new System.Windows.Forms.Timer(this.components);
            this.lblVersion = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackgroundImage = global::invoiceIntegration.Properties.Resources.logo;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(136, 122);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(231, 51);
            this.metroPanel1.TabIndex = 0;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(136, 208);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(231, 13);
            this.progressBar.TabIndex = 1;
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblLoading.Location = new System.Drawing.Point(191, 252);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(91, 19);
            this.lblLoading.TabIndex = 2;
            this.lblLoading.Text = "Yükleniyor...";
            // 
            // timerSplashScreen
            // 
            this.timerSplashScreen.Enabled = true;
            this.timerSplashScreen.Tick += new System.EventHandler(this.timerSplashScreen_Tick);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblVersion.Location = new System.Drawing.Point(191, 373);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(71, 19);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "V . 0 . 0 . 2";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(493, 395);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.metroPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplashScreen";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.SystemShadow;
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroProgressBar progressBar;
        private MetroFramework.Controls.MetroLabel lblLoading;
        private System.Windows.Forms.Timer timerSplashScreen;
        private MetroFramework.Controls.MetroLabel lblVersion;
    }
}