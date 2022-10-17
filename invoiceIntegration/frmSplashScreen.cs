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
using invoiceIntegration.config;

namespace invoiceIntegration
{
    public partial class frmSplashScreen : MaterialSkin.Controls.MaterialForm
    {
        public frmSplashScreen()
        {
            InitializeComponent();
        }
        private void timerSplashScreen_Tick(object sender, EventArgs e)
        {
            invoiceTimer.Enabled = true;
            progressBar.Increment(5);
            progressBarLbl.Text = "%" + progressBar.Value.ToString();
            if (progressBar.Value >= 5 & progressBar.Value <= 25)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 25 & progressBar.Value <= 35)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            else if (progressBar.Value > 35 & progressBar.Value <= 45)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 45 & progressBar.Value <= 65)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            else if (progressBar.Value > 65 & progressBar.Value <= 85)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 85)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            if (progressBar.Value == 100)
            {
                invoiceTimer.Enabled = false;
                frmMain frmmain = new frmMain();
                frmmain.Show();
                this.Hide();
            }
        }
        private void frmSplashScreen_Load(object sender, EventArgs e)
        {
            waybillTimer.Stop();
            invoiceTimer.Stop();
            collectionTimer.Stop();
            progressBarLbl.Hide();
            progressBar.Hide();
            lblLoading.Hide();
            lblLoading2.Hide();

            if (Configuration.getUseCollection())
            {
                collectionBtn.Enabled = true;
            }
            if (Configuration.getUseDispatch())
            {
                dispatchBtn.Enabled = true;
            }
            if (Configuration.getUseInvoice())
            {
                invoiceBtn.Enabled = true;
            }
        }      
       private void waybillTimer_Tick(object sender, EventArgs e)
        {
            waybillTimer.Enabled = true;
            progressBar.Increment(5);
            progressBarLbl.Text = "%" + progressBar.Value.ToString();
            if (progressBar.Value >= 5 & progressBar.Value <= 25)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 25 & progressBar.Value <= 35)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            else if (progressBar.Value > 35 & progressBar.Value <= 45)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 45 & progressBar.Value <= 65)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            else if (progressBar.Value > 65 & progressBar.Value <= 85)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 85)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            if (progressBar.Value == 100)
            {
                waybillTimer.Enabled = false;
                Waybills wyblls = new Waybills();
                wyblls.Show();
                this.Hide();
            }
        }
        private void dispatchBtn_Click(object sender, EventArgs e)
        {
            waybillTimer.Start();
            progressBarLbl.Show();
            progressBar.Show();
            lblLoading.Show();
            lblLoading2.Show();
        }
        private void invoiceBtn_Click(object sender, EventArgs e)
        {
            invoiceTimer.Start();
            progressBarLbl.Show();
            progressBar.Show();
            lblLoading.Show();
            lblLoading2.Show();
        }
        private void collectionBtn_Click(object sender, EventArgs e)
        {
            collectionTimer.Start();
            progressBarLbl.Show();
            progressBar.Show();
            lblLoading.Show();
            lblLoading2.Show();
        }

        private void collectionTimer_Tick(object sender, EventArgs e)
        {
            collectionTimer.Enabled = true;
            progressBar.Increment(12);
            progressBarLbl.Text = "%" + progressBar.Value.ToString();
            if (progressBar.Value >= 5 & progressBar.Value <= 25)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 25 & progressBar.Value <= 35)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            else if (progressBar.Value > 35 & progressBar.Value <= 45)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 45 & progressBar.Value <= 65)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            else if (progressBar.Value > 65 & progressBar.Value <= 85)
            {
                lblLoading2.Show();
                lblLoading.Hide();
            }

            else if (progressBar.Value > 85)
            {
                lblLoading.Show();
                lblLoading2.Hide();
            }

            if (progressBar.Value == 100)
            {
                collectionTimer.Enabled = false;
                Collection collection = new Collection();
                collection.Show();
                this.Hide();
            }
        }
    }
}
