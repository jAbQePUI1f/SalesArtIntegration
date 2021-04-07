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
            if (progressBar.Value == 100)
            {
                timerSplashScreen.Enabled = false;
                frmMain frmmain = new frmMain();
                frmmain.Show();
                this.Hide();
            }
        }
    }
}
