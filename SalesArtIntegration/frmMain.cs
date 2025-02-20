using invoiceIntegration.config;
using invoiceIntegration.helper;
using invoiceIntegration.model;
using invoiceIntegration.model.order;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace invoiceIntegration
{
    public partial class frmMain : MetroForm
    {

        public frmMain()
        {
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        GenericResponse<List<LogoInvoiceJson>> jsonInvoices = new GenericResponse<List<LogoInvoiceJson>>();
        GenericResponse<OrderResponse> jsonOrders = new GenericResponse<OrderResponse>();

        Helper helper = new Helper();
        bool isLoggedIn = false;
        string invoiceType;
        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbInvoice.SelectedIndex = 0;
            lblLogoConnectionInfo.Text = "";
            startDate.Value = DateTime.Now.AddDays(-1);
           
            if (Configuration.getXMLTransferInfo())
            {
                btnCheckLogoConnection.Visible = false;
                btnSendToLogo.Visible = false;
                btnSendOrderToLogo.Visible = false;
                btnXML.Visible = true;
                if (Configuration.getXMLTransferForOrder())
                {
                    btnXML.Text = "Siparişleri XML'e Aktar";
                }
            }

            if (Configuration.getIntegrationForMikroERP())
            {
                btnSendToLogo.Text = "Faturaları Mikroya Aktar";
                isLoggedIn = true;
            }

            if (Configuration.getOrderTransferToLogoInfo())
            {
                btnSendOrderToLogo.Visible = true;
                btnSendToLogo.Visible = false;
                btnXML.Visible = false;
                cmbInvoice.Items.Clear();
                cmbInvoice.Items.Add("Satış Siparişleri");
                cmbInvoice.SelectedIndex = 0;
            }
        }
        void CheckLogin()
        {
            Cursor.Current = Cursors.WaitCursor;
            isLoggedIn = LogoApplication.getApplication().Login(Configuration.getLogoUserName(), Configuration.getLogoPassword(), int.Parse(Configuration.getCompanyCode()), int.Parse(Configuration.getSeason()));
            if (isLoggedIn)
            {
                lblLogoConnectionInfo.ForeColor = System.Drawing.Color.Green;
                lblLogoConnectionInfo.Text = "Logo Bağlantısı Başarılı";
                btnCheckLogoConnection.Enabled = false;
            }
            else
            {
                lblLogoConnectionInfo.ForeColor = System.Drawing.Color.Red;
                lblLogoConnectionInfo.Text = "Logo Bağlantısı Başarısız";
                btnSendToLogo.Enabled = false;
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
        private void btnXML_Click(object sender, EventArgs e)
        {
            if (dataGridInvoice.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                XmlHelper xmlHelper = new XmlHelper();
                xmlHelper.SaveToXml(dataGridInvoice, jsonInvoices);
                dataGridInvoice.Rows.Clear();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Fatura Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
            }
        }
        private void btnCheckLogoConnection_Click(object sender, EventArgs e)
        {
            helper.LogFile("Login Kontolü Basladı", "-", "-", "-", "-");
            try
            {
                CheckLogin();
            }
            catch (Exception)
            {
                MessageBox.Show("Logo Objects ile bağlantı kurulamıyor. Lütfen firmanızın Bilgi İşlem Departmanı ile iletişime geçiniz.", "Bağlantı Sorunu", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            helper.LogFile("Login Kontolü Bitti", "-", "-", "-", "-");
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isLoggedIn)
            {
                LogoApplication.getApplication().UserLogout();
                LogoApplication.getApplication().Disconnect();
            }
            System.Windows.Forms.Application.Exit();
        }
        private void btnGetInvoices_Click(object sender, EventArgs e)
        {
            ApiHelper apiHelper = new ApiHelper(); 
            Cursor.Current = Cursors.WaitCursor;
            dataGridInvoice.Rows.Clear();
            chkSelectAll.Checked = false;
            if (Configuration.getOrderTransferToLogoInfo())
              jsonOrders = apiHelper.GetOrders(startDate, endDate,dataGridInvoice);
            else jsonInvoices = apiHelper.GetInvoices(startDate,endDate,invoiceType,dataGridInvoice);
            btnSendToLogo.Enabled = (dataGridInvoice.Rows.Count > 0 ) ? true : false;
            btnCheckLogoConnection.Enabled = (dataGridInvoice.Rows.Count > 0 && !isLoggedIn) ? true : false;
            Cursor.Current = Cursors.Default;
        }
        private void btnSendToLogo_Click(object sender, EventArgs e)
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
                    if (Configuration.getIntegrationForMikroERP())
                        selectedInvoicesForMikro = selectionHelper.GetSelectedInvoicesForMikro(dataGridInvoice, jsonInvoices);
                    else
                        selectedInvoices = selectionHelper.GetSelectedInvoices(dataGridInvoice, jsonInvoices);
                    Cursor.Current = Cursors.WaitCursor;
                    helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
                    if (Configuration.getIntegrationForMikroERP())
                        integratedInvoices = integratedHelper.sendMultipleInvoicesForMikro(selectedInvoicesForMikro);
                    else
                        integratedInvoices = integratedHelper.sendMultipleInvoice(selectedInvoices);
                    responseHelper.SendResponse(integratedInvoices);
                    helper.ShowMessages(integratedInvoices);
                    helper.LogFile("Fatura Aktarım Bitti", "-", "-", "-", "-");
                    dataGridInvoice.Rows.Clear();
                    btnSendToLogo.Enabled = false;
                    btnCheckLogoConnection.Enabled = false;
                    isLoggedIn = false;
                    lblLogoConnectionInfo.Text = "";
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Logoya Bağlantı Problemi Yaşandı, Faturalar Aktarılamadı.", "Logo Bağlantı Hatası", MessageBoxButtons.OK);
                }
                btnSendToLogo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Fatura Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
                btnSendToLogo.Enabled = true;
            }
        }
        private void btnLastLog_Click(object sender, EventArgs e)
        {
            frmViewLog frm = new frmViewLog();
            frm.ShowDialog();
        }
        private void cmbInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbInvoice.SelectedIndex)
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
                    invoiceType = "SELLING_SERVICE";
                    break;
                case 4:
                    invoiceType = "BUYING_SERVICE";
                    break;
                case 5:
                    invoiceType = "BUYING";
                    break;
                case 6:
                    invoiceType = "DAMAGED_BUYING_RETURN";
                    break;
                case 7:
                    invoiceType = "BUYING_RETURN";
                    break;
            }
        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
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
        private void btnSendOrderToLogo_Click(object sender, EventArgs e)
        {
            ResponseHelper responseHelper = new ResponseHelper();
            SelectionHelper selectionHelper = new SelectionHelper();
            IntegratedHelper integratedHelper = new IntegratedHelper();
            if (dataGridInvoice.Rows.Count > 0)
            {
                if (isLoggedIn)
                {
                    List<Order> selectedOrders = selectionHelper.GetSelectedOrders(dataGridInvoice, jsonOrders);
                    Cursor.Current = Cursors.WaitCursor;
                    helper.LogFile("Sipariş Aktarım Basladı", "-", "-", "-", "-");
                    IntegratedOrderStatus status = integratedHelper.sendMultipleOrder(selectedOrders);
                    responseHelper.SendResponse(status);
                    helper.ShowMessages(status);
                    helper.LogFile("Sipariş Aktarım Bitti", "-", "-", "-", "-");
                    dataGridInvoice.Rows.Clear();
                    btnSendToLogo.Enabled = false;
                    btnCheckLogoConnection.Enabled = false;
                    lblLogoConnectionInfo.Text = "";
                    isLoggedIn = false;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Logoya Bağlantı Problemi Yaşandı, Faturalar Aktarılamadı.", "Logo Bağlantı Hatası", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Sipariş Seçmelisiniz..", "Sipariş Seçim", MessageBoxButtons.OK);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Aktarım programı kapatılacaktır", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                LogoApplication.getApplication().UserLogout();
                LogoApplication.getApplication().Disconnect();
                System.Windows.Forms.Application.Exit();
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
        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}