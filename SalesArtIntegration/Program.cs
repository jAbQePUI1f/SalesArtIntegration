﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace invoiceIntegration
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool AcikUygulamaVar = false;
            Mutex m = new Mutex(true, "PaketServis", out AcikUygulamaVar);
            if (AcikUygulamaVar)
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSplashScreen());
            }
            else
            {
                MessageBox.Show("Invoice Integrator zaten çalışıyor!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}