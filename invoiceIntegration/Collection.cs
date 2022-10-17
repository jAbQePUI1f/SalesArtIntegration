using System;
using System.Collections.Generic;
using MetroFramework.Forms;
using System.Windows.Forms;
using invoiceIntegration.config;
using invoiceIntegration.model;
using UnityObjects;
using invoiceIntegration.model.waybill;
using invoiceIntegration.helper;
using static invoiceIntegration.Collection;
using invoiceIntegration.model.Collection;

namespace invoiceIntegration
{
    public partial class Collection : MetroForm
    {
        public Collection()
        {
            InitializeComponent();
        }

        string collectioneType;
        bool isLoggedIn;
        private frmMain.GenericResponse<List<LogoCollectionModel>> logoCollectionModels;
        Helper helper = new Helper();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Aktarım programı kapatılacaktır.", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                LogoApplication.getApplication().UserLogout();
                LogoApplication.getApplication().Disconnect();
                System.Windows.Forms.Application.Exit();
            }
        }
        private void anaEkranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ana Ekrana geçiş yapıyorsunuz..", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                frmSplashScreen splsh = new frmSplashScreen();
                this.Hide();
                splsh.Show();
            }
        }
        private void btnGetCollection_Click(object sender, EventArgs e)
        {
            ApiHelper apiHelper = new ApiHelper();
            Cursor.Current = Cursors.WaitCursor;
            dataGridCollection.Rows.Clear();
            chkSelectAll.Checked = false;
            if (Configuration.getUseCollection())
                logoCollectionModels = apiHelper.GetCollection(startDate, endDate, collectioneType, dataGridCollection);
            else 
                logoCollectionModels = apiHelper.GetCollection(startDate, endDate, collectioneType, dataGridCollection);

            btnSendCollection.Enabled = (dataGridCollection.Rows.Count > 0) ? true : false;
            btnCheckLogoConnection.Enabled = (dataGridCollection.Rows.Count > 0 && !isLoggedIn) ? true : false;
            Cursor.Current = Cursors.Default;
        }
        private void btnSendCollection_Click(object sender, EventArgs e)
        {
            IntegratedHelper integratedHelper = new IntegratedHelper();
            ResponseHelper responseHelper = new ResponseHelper();
            SelectionHelper selectionHelper = new SelectionHelper();
            IntegratedCollectionStatus integratedCollection = new IntegratedCollectionStatus();
            if (dataGridCollection.Rows.Count > 0)
            {
                if (isLoggedIn)
                {
                    List<LogoCollectionModel> selectedCollection = new List<LogoCollectionModel>();
                    integratedCollection = integratedHelper.sendMultipleCollections(selectedCollection);
                    responseHelper.SendResponse(integratedCollection);
                    helper.ShowMessages(integratedCollection);
                    helper.LogFile("Tahsilat Aktarımı Bitti", "-", "-", "-", "-");
                    dataGridCollection.Rows.Clear();
                    btnSendCollection.Enabled = false;
                    btnCheckLogoConnection.Enabled = false;
                    isLoggedIn = false;
                    lblLogoConnectionInfo.Text = "";
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("Logoya Bağlantı Problemi Yaşandı, Tahsilat Aktarılamadı.", "Logo Bağlantı Hatası", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Tahsilat Seçmelisiniz..", "Tahsilat Seçim", MessageBoxButtons.OK);
            }
        }
        private void Collection_Load(object sender, EventArgs e)
        {
            cmbCollection.SelectedIndex = 0;
            lblLogoConnectionInfo.Text = "";
            startDate.Value = DateTime.Now.AddDays(-1);
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
                btnSendCollection.Enabled = false;
                btnCheckLogoConnection.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
        }
        private void cmbCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbCollection.SelectedIndex)
            {
                case 0:
                    collectioneType = "CASH";
                    break;
                case 1:
                    collectioneType = "CREDIT_CARD";
                    break;
                case 2:
                    collectioneType = "PAYMENT_CHECK";
                    break;
            }
        }
        public class GenericResponse<T>
        {
            public T data { get; set; }
            public int responseStatus { get; set; }
            public model.order.Message message { get; set; }
        }
        private void Collection_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isLoggedIn)
            {
                LogoApplication.getApplication().UserLogout();
                LogoApplication.getApplication().Disconnect();
            }
            System.Windows.Forms.Application.Exit();
        }
        private void Collection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoggedIn)
            {
                LogoApplication.getApplication().UserLogout();
                LogoApplication.getApplication().Disconnect();
            }
            System.Windows.Forms.Application.Exit();
        }
        private void btnLastLog_Click(object sender, EventArgs e)
        {
            frmViewLog frm = new frmViewLog();
            frm.ShowDialog();
        }
    }
}

