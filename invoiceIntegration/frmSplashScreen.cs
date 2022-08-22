using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invoiceIntegration
{
    public partial class frmSplashScreen : MetroForm
    {
        public frmSplashScreen()
        {
            InitializeComponent();
        }
        private void timerSplashScreen_Tick(object sender, EventArgs e)
        {
            timerSplashScreen.Enabled = true;
            progressBar.Increment(5);
            label1.Text = "%" + progressBar.Value.ToString();
            if ( progressBar.Value >= 5 & progressBar.Value <= 25 )
            {
                lblLoading2.Show();
                lblLoading.Visible = false;
            }

            else if (progressBar.Value > 25 & progressBar.Value <= 35)
            {
                lblLoading.Show();
                lblLoading2.Visible = false;
            }

            else if (progressBar.Value > 35 & progressBar.Value <= 45)
            {
                lblLoading2.Show();
                lblLoading.Visible = false;
            }

            else if (progressBar.Value > 45 & progressBar.Value <= 65)
            {
                lblLoading.Show();
                lblLoading2.Visible = false;
            }

            else if (progressBar.Value > 65 & progressBar.Value <= 85)
            {
                lblLoading2.Show();
                lblLoading.Visible = false;
            }

            else if (progressBar.Value > 85)
            {
                lblLoading.Show();
                lblLoading2.Visible = false;
            }

            if (progressBar.Value == 100)
            {
                timerSplashScreen.Enabled = false;
                frmMain frmmain = new frmMain();
                frmmain.Show();
                this.Hide();
            }
        }

        private void frmSplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
