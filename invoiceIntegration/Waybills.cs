using System;
using System.Collections.Generic;
using MetroFramework.Forms;
using System.Windows.Forms;
using invoiceIntegration.config;
using invoiceIntegration.model;
using UnityObjects;
using invoiceIntegration.model.waybill;
using invoiceIntegration.helper;
using static invoiceIntegration.frmMain;
namespace invoiceIntegration
{
    public partial class Waybills : MetroForm
    {
        public Waybills()
        {
            InitializeComponent();
        }
        
        Helper helper = new Helper();
        bool isLoggedIn = false;
        string invoiceType;
        private frmMain.GenericResponse<List<LogoWaybillJson>> jsonWaybills;
        void CheckLogin()
        {
            Cursor.Current = Cursors.WaitCursor;
            isLoggedIn = LogoApplication.getApplication().Login(Configuration.getLogoUserName(), Configuration.getLogoPassword(), int.Parse(Configuration.getCompanyCode()), int.Parse(Configuration.getSeason()));
            if (isLoggedIn)
            {
                lblLogoConnectionInfo.ForeColor = System.Drawing.Color.Green;
                lblLogoConnectionInfo.Text = "Logo Bağlantısı Başarılı";
                btnWaybill.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn) ? true : false;
                btnCheckLogoConnection.Enabled = false;
            }
            else
            {
                lblLogoConnectionInfo.ForeColor = System.Drawing.Color.Red;
                lblLogoConnectionInfo.Text = "Logo Bağlantısı Başarısız";
                btnWaybill.Enabled = false;
                btnCheckLogoConnection.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
        }
        public class GenericResponse<T>
        {
            public T data { get; set; }
            public int responseStatus { get; set; }
            public model.order.Message message { get; set; }
        }
        private void Waybills_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (isLoggedIn)
            {
                LogoApplication.getApplication().UserLogout();
                LogoApplication.getApplication().Disconnect();
            }
            System.Windows.Forms.Application.Exit();
        }
        private void btnGetWaybbill_Click(object sender, EventArgs e)
        {
            ApiHelper apiHelper = new ApiHelper();
            Cursor.Current = Cursors.WaitCursor;
            dataGridInvoice.Rows.Clear();
            chkSelectAll.Checked = false;
            //jsonWaybills = apiHelper.GetWaybills(startDate, endDate, invoiceType: invoiceType, dataGridInvoice: dataGridInvoice);
            jsonWaybills = apiHelper.GetWaybills(startDate, endDate, invoiceType, dataGridInvoice);
          
            btnWaybill.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn) ? true : false;
            btnCheckLogoConnection.Enabled = (dataGridInvoice.Rows.Count > 0 && !isLoggedIn) ? true : false;
            Cursor.Current = Cursors.Default;
        }
        private void cmbDispatch_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (cmbDispatch.SelectedIndex)
            {
                case 0:
                    invoiceType = "SELLING";
                    break;
                case 1:
                    invoiceType = "DAMAGED_SELLING_RETURN";
                    break;
                case 2:
                    invoiceType = "SELLING_RETURN";
                    break;
                case 3:
                    invoiceType = "BUYING";
                    break;
                case 4:
                    invoiceType = "DAMAGED_BUYING_RETURN";
                    break;
                case 5:
                    invoiceType = "BUYING_RETURN";
                    break;
            }
        }
        private void btnCheckLogoConnection_Click_1(object sender, EventArgs e)
        {
            helper.LogFile("Login Kontolü Basladı", "-", "-", "-", "-");
            try
            {
                CheckLogin();
            }
            catch (Exception)
            {
                MessageBox.Show("Logo ile bağlantı kurulamıyor. Lütfen IT Departmanı ile iletişime geçiniz...", "Bağlantı Sorunu", MessageBoxButtons.OK);
            }
            helper.LogFile("Login Kontolü Bitti", "-", "-", "-", "-");
        }
        private void btnLastLog_Click_1(object sender, EventArgs e)
        {
            frmViewLog frm = new frmViewLog();
            frm.ShowDialog();
        }
        private void btnWaybill_Click_1(object sender, EventArgs e)
        {
            IntegratedHelper integratedHelper = new IntegratedHelper();
            ResponseHelper responseHelper = new ResponseHelper();
            SelectionHelper selectionHelper = new SelectionHelper();
            IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
            if (dataGridInvoice.Rows.Count > 0)
            {
                if (isLoggedIn)
                {
                    List<LogoInvoiceJson> selectedInvoicesForMikro = new List<LogoInvoiceJson>();
                    List<LogoInvoice> selectedInvoices = new List<LogoInvoice>();
                    integratedInvoices = integratedHelper.sendMultipleInvoice(selectedInvoices);
                    responseHelper.SendResponse(integratedInvoices);
                    helper.ShowMessages(integratedInvoices);
                    helper.LogFile("Fatura Aktarım Bitti", "-", "-", "-", "-");
                    dataGridInvoice.Rows.Clear();
                    btnWaybill.Enabled = false;
                    btnCheckLogoConnection.Enabled = false;
                    isLoggedIn = false;
                    lblLogoConnectionInfo.Text = "";
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Logoya Bağlantı Problemi Yaşandı, İrsaliyeler Aktarılamadı.", "Logo Bağlantı Hatası", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("İrsaliye Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
            }
        }
        private void chkSelectAll_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                foreach (DataGridViewRow row in dataGridInvoice.Rows)
                {
                    row.Cells["chk"].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridInvoice.Rows)
                {
                    row.Cells["chk"].Value = false;
                }
            }
        }
        private void menüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ana Ekrana geçiş yapıyorsunuz..", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                frmSplashScreen splsh = new frmSplashScreen();
                this.Hide();
                splsh.Show();
            }
        }
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Aktarım programı kapatılacaktır", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                LogoApplication.getApplication().UserLogout();
                LogoApplication.getApplication().Disconnect();
                System.Windows.Forms.Application.Exit();
            }
        }
        private void Waybills_Load(object sender, EventArgs e)
        {
            cmbDispatch.SelectedIndex = 0;
            lblLogoConnectionInfo.Text = "";
        }

        private void dataGridInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
