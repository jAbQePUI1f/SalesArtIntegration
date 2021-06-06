using invoiceIntegration.config;
using invoiceIntegration.model;
using invoiceIntegration.repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityObjects;
using RestSharp;
using System.Linq;
using System.Data;
using invoiceIntegration.model.waybill;
using invoiceIntegration.helper;
using invoiceIntegration.model.order;
using MetroFramework.Forms;
using System.Threading;

namespace invoiceIntegration
{
    public partial class frmMain : MetroForm
    {

        public frmMain()
        {
            InitializeComponent();
            bool programRunningControl;
            Mutex mutex = new Mutex(true, System.Windows.Forms.Application.ProductName, out programRunningControl);
            if (!programRunningControl)
            {
                MessageBox.Show("Program Zaten Çalışıyor", "UYARI!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }
            this.FormBorderStyle = FormBorderStyle.None;
        }

        bool integrationForMikroERP = Configuration.getIntegrationForMikroERP();
        GenericResponse<List<LogoInvoiceJson>> jsonInvoices = new GenericResponse<List<LogoInvoiceJson>>();
        GenericResponse<List<LogoWaybillJson>> jsonWaybills = new GenericResponse<List<LogoWaybillJson>>();
        GenericResponse<OrderResponse> jsonOrders = new GenericResponse<OrderResponse>();
 
        UnityApplication unity = LogoApplication.getApplication();
        Helper helper = new Helper();
        bool isLoggedIn = false;
        string invoiceType;
        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbInvoice.SelectedIndex = 0;
            lblLogoConnectionInfo.Text = "";
            startDate.Value = DateTime.Now.AddDays(-1);
            if (Configuration.getUseDispatch())
                chkDispatch.Visible = true;

            if (Configuration.getXMLTransferInfo())
            {
                btnCheckLogoConnection.Visible = false;
                btnSendToLogo.Visible = false;
                btnSendOrderToLogo.Visible = false;
                btnXML.Visible = true;
            }

            if (integrationForMikroERP)
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
            isLoggedIn = unity.Login(Configuration.getLogoUserName(), Configuration.getLogoPassword(), int.Parse(Configuration.getCompanyCode()), int.Parse(Configuration.getSeason()));
            if (isLoggedIn)
            {
                lblLogoConnectionInfo.ForeColor = System.Drawing.Color.Green;
                lblLogoConnectionInfo.Text = "Logo Bağlantısı Başarılı";
                btnSendToLogo.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn && chkDispatch.Checked != true) ? true : false;
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
                MessageBox.Show("Logo ile bağlantı kurulamıyor. Lütfen IT Departmanı ile iletişime geçiniz...", "Bağlantı Sorunu", MessageBoxButtons.OK);
            }
            helper.LogFile("Login Kontolü Bitti", "-", "-", "-", "-");
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isLoggedIn)
            {
                unity.UserLogout();
                unity.Disconnect();
            }
            System.Windows.Forms.Application.Exit();
        }
        private void btnGetInvoices_Click(object sender, EventArgs e)
        {
            ApiHelper apiHelper = new ApiHelper(); 
            Cursor.Current = Cursors.WaitCursor;
            dataGridInvoice.Rows.Clear();
            chkSelectAll.Checked = false;
            if (chkDispatch.Checked)
                jsonWaybills = apiHelper.GetWaybills(startDate,endDate,invoiceType,dataGridInvoice);
            else if (Configuration.getOrderTransferToLogoInfo())
              jsonOrders= apiHelper.GetOrders(startDate, endDate,dataGridInvoice);
            else jsonInvoices = apiHelper.GetInvoices(startDate,endDate,invoiceType,dataGridInvoice);
            btnSendToLogo.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn) ? true : false;
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
                    if (integrationForMikroERP)
                        selectedInvoicesForMikro = selectionHelper.GetSelectedInvoicesForMikro(dataGridInvoice, jsonInvoices);
                    else
                        selectedInvoices = selectionHelper.GetSelectedInvoices(dataGridInvoice, jsonInvoices);
                    Cursor.Current = Cursors.WaitCursor;
                    helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
                    if (integrationForMikroERP)
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
            }
            else
            {
                MessageBox.Show("Fatura Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
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
        private void cmbDispatch_SelectedIndexChanged(object sender, EventArgs e)
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
        private void chkDispatch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDispatch.Checked)
            {
                cmbInvoice.Enabled = false;
                lblInvoice.Enabled = false;
                cmbDispatch.Visible = true;
                cmbDispatch.Enabled = true;
                btnWaybill.Visible = true;
                btnWaybill.Enabled = true;
                btnSendToLogo.Enabled = false;
                dataGridInvoice.Rows.Clear();
                cmbDispatch.SelectedIndex = 0;
            }
            else
            {
                cmbInvoice.Enabled = true;
                lblInvoice.Enabled = true;
                cmbDispatch.Enabled = false;
                btnWaybill.Enabled = false;
                cmbInvoice.SelectedIndex = 0;
                cmbInvoice_SelectedIndexChanged(0, EventArgs.Empty);
            }
        }
        private void btnWaybill_Click(object sender, EventArgs e)
        {
            SelectionHelper selectionHelper = new SelectionHelper();
            ResponseHelper responseHelper = new ResponseHelper();
            IntegratedHelper integratedHelper = new IntegratedHelper();
            if (dataGridInvoice.Rows.Count > 0)
            {
                if (isLoggedIn)
                {
                    List<LogoWaybill> selectedWaybills = selectionHelper.GetSelectedWaybills(dataGridInvoice, jsonWaybills);
                    Cursor.Current = Cursors.WaitCursor;
                    helper.LogFile("İrsaliye Aktarım Basladı", "-", "-", "-", "-");
                    IntegratedWaybillStatus status = integratedHelper.sendMultipleDespatch(selectedWaybills);
                    responseHelper.SendResponse(status);
                    helper.ShowMessages(status);
                    helper.LogFile("İrsaliye Aktarım Bitti", "-", "-", "-", "-");
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
                MessageBox.Show("İrsaliye Seçmelisiniz..", "İrsaliye Seçim", MessageBoxButtons.OK);
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
    }

}