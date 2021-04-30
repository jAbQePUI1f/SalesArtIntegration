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
using invoiceIntegration.controller;
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
        bool isProducerCode = Configuration.getIsProducerCode();
        bool useCypheCode = Configuration.getUseCypheCode();
        string cypheCode = Configuration.getCypheCode();
        bool isBarcode = Configuration.getIsBarcode();
        bool useDefaultNumber = Configuration.getUseDefaultNumber();
        bool useShortDate = Configuration.getUseShortDate();
        bool useShipCode = Configuration.getUseShipCode();
        bool XMLTransferInfo = Configuration.getXMLTransferInfo();
        bool XMLTransferForOrder = Configuration.getXMLTransferForOrder();
        int distributorId = Configuration.getDistributorId();
        bool useDispatch = Configuration.getUseDispatch();
        bool integrationForMikroERP = Configuration.getIntegrationForMikroERP();
        string shipAgentCode = Configuration.getShipAgentCode();
        string campaignLineNo = Configuration.getCampaignLineNo();
        bool orderTransferToLogoInfo = Configuration.getOrderTransferToLogoInfo();

        IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
        IntegratedWaybillStatus integratedWaybills = new IntegratedWaybillStatus();
        IntegratedOrderStatus integratedOrders = new IntegratedOrderStatus();

        GenericResponse<List<LogoInvoiceJson>> jsonInvoices = new GenericResponse<List<LogoInvoiceJson>>();
        GenericResponse<List<LogoWaybillJson>> jsonWaybills = new GenericResponse<List<LogoWaybillJson>>();
        GenericResponse<OrderResponse> jsonOrders = new GenericResponse<OrderResponse>();
        LogoDataReader reader = new LogoDataReader();
        UnityApplication unity = LogoApplication.getApplication();
        Helper helper = new Helper();
        bool isLoggedIn = false;
        string invoiceType;
        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbInvoice.SelectedIndex = 0;
            lblLogoConnectionInfo.Text = "";
            startDate.Value = DateTime.Now.AddDays(-1);
            if (useDispatch)
                chkDispatch.Visible = true;

            if (XMLTransferInfo)
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

            if (orderTransferToLogoInfo)
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
        public void GetInvoices()
        {
            GridHelper gridHelper = new GridHelper();
            RestClient restClient = new RestClient(Configuration.getUrl());
            RestRequest restRequest = new RestRequest("/integration/invoices?", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            GetTransferableInvoicesRequest req = new GetTransferableInvoicesRequest()
            {
                startDate = startDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                endDate = endDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                invoiceTypes = new List<string>()
            };
            req.invoiceTypes.Add(invoiceType.ToString());
            restRequest.AddParameter("distributorId", distributorId, ParameterType.QueryString);
            restRequest.AddJsonBody(req);
            var requestResponse = restClient.Execute<LogoInvoice>(restRequest);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            jsonInvoices = JsonConvert.DeserializeObject<GenericResponse<List<LogoInvoiceJson>>>(requestResponse.Content, settings);
            gridHelper.FillGrid(jsonInvoices.data.Cast<dynamic>().ToList(), dataGridInvoice, constants.ListType.LogoInvoiceJson);
        }
        public void GetWaybills()
        {
            GridHelper gridHelper = new GridHelper();
            RestClient restClient = new RestClient(Configuration.getUrl());
            RestRequest restRequest = new RestRequest("/integration/waybills?", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            GetTransferableWaybillsRequest req = new GetTransferableWaybillsRequest()
            {
                startDate = startDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                endDate = endDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                waybillTypes = new List<string>()
            };
            req.waybillTypes.Add(invoiceType.ToString());
            restRequest.AddParameter("distributorId", distributorId, ParameterType.QueryString);
            restRequest.AddJsonBody(req);
            var requestResponse = restClient.Execute<LogoWaybill>(restRequest);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            jsonWaybills = JsonConvert.DeserializeObject<GenericResponse<List<LogoWaybillJson>>>(requestResponse.Content, settings);
            gridHelper.FillGrid(jsonWaybills.data.Cast<dynamic>().ToList(), dataGridInvoice, constants.ListType.LogoWaybillJson);
        }
        public void GetOrders()
        {
            GridHelper gridHelper = new GridHelper();
            RestClient restClient = new RestClient(Configuration.getUrl());
            RestRequest restRequest = new RestRequest("/integration/orders/", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            GetTransferableOrdersRequest req = new GetTransferableOrdersRequest()
            {
                startDate = startDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                endDate = endDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                distributorId = distributorId
            };
            restRequest.AddJsonBody(req);
            var requestResponse = restClient.Execute<OrderResponse>(restRequest);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            jsonOrders = JsonConvert.DeserializeObject<GenericResponse<OrderResponse>>(requestResponse.Content, settings);
            gridHelper.FillOrdersToGrid(jsonOrders.data, dataGridInvoice);
        }
        public IntegratedInvoiceStatus sendMultipleInvoice(List<LogoInvoice> invoices)
        {
            string remoteInvoiceNumber = "";
            string message = "";
            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();
            try
            {
                if (isLoggedIn)
                {
                    foreach (var invoice in invoices)
                    {
                        Data newInvoice = unity.NewDataObject(DataObjectType.doSalesInvoice);
                        remoteInvoiceNumber = reader.getInvoiceNumberByDocumentNumber(invoice.number);
                        // salesArttaki invoice number , logoda documentNumber alanına yazılıyor.
                        if (remoteInvoiceNumber != "")
                        {
                            IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(invoice.number + " belge numaralı fatura, sistemde zaten mevcut. Kontrol Ediniz", invoice.number, remoteInvoiceNumber, true);
                            receivedInvoices.Add(recievedInvoice);
                        }
                        else
                        {
                            if (invoice.type == (int)InvoiceType.SELLING ||
                                invoice.type == (int)InvoiceType.SELLING_RETURN ||
                                invoice.type == (int)InvoiceType.SELLING_SERVICE)
                            {
                                newInvoice = unity.NewDataObject(DataObjectType.doSalesInvoice);
                            }
                            else
                            {
                                newInvoice = unity.NewDataObject(DataObjectType.doPurchInvoice);
                            }
                            newInvoice.New();
                            newInvoice.DataFields.FieldByName("TYPE").Value = invoice.type;
                            if (useDefaultNumber)
                            {
                                newInvoice.DataFields.FieldByName("NUMBER").Value = "~";
                            }
                            else
                            {
                                newInvoice.DataFields.FieldByName("NUMBER").Value = invoice.number; // düzenlecek
                            }
                            newInvoice.DataFields.FieldByName("DOC_NUMBER").Value = invoice.documentNumber;
                            if (useShortDate)
                            {
                                newInvoice.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(invoice.date.ToShortDateString());
                            }
                            else
                            {
                                newInvoice.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(invoice.date.ToString("dd-MM-yyyy"));
                            }
                            newInvoice.DataFields.FieldByName("TIME").Value = helper.Hour(invoice.date.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
                            newInvoice.DataFields.FieldByName("AUXIL_CODE").Value = invoice.salesmanCode;
                            newInvoice.DataFields.FieldByName("ARP_CODE").Value = invoice.customerCode;
                            newInvoice.DataFields.FieldByName("SOURCE_WH").Value = invoice.wareHouseCode;
                            newInvoice.DataFields.FieldByName("SOURCE_COST_GRP").Value = invoice.wareHouseCode;
                            newInvoice.DataFields.FieldByName("DEPARTMENT").Value = helper.getDepartment();
                            if (useShipCode)
                            {
                                newInvoice.DataFields.FieldByName("SHIPLOC_CODE").Value = invoice.customerBranchCode;
                                newInvoice.DataFields.FieldByName("SHIPLOC_DEF").Value = invoice.customerBranchName;
                            }
                            if (useCypheCode)
                            {
                                newInvoice.DataFields.FieldByName("AUTH_CODE").Value = cypheCode;
                            }
                            newInvoice.DataFields.FieldByName("DIVISION").Value = invoice.distributorBranchCode;
                            newInvoice.DataFields.FieldByName("TOTAL_DISCOUNTS").Value = invoice.discountTotal;
                            newInvoice.DataFields.FieldByName("TOTAL_DISCOUNTED").Value = invoice.netTotal - invoice.discountTotal;
                            newInvoice.DataFields.FieldByName("ADD_DISCOUNTS").Value = invoice.discountTotal;
                            newInvoice.DataFields.FieldByName("TOTAL_VAT").Value = invoice.vatTotal;
                            newInvoice.DataFields.FieldByName("TOTAL_GROSS").Value = invoice.grossTotal;
                            newInvoice.DataFields.FieldByName("TOTAL_NET").Value = invoice.netTotal;
                            newInvoice.DataFields.FieldByName("NOTES1").Value = invoice.note + "Şube:" + invoice.customerBranchCode + "_" + invoice.customerBranchName;
                            newInvoice.DataFields.FieldByName("TC_NET").Value = invoice.netTotal;
                            newInvoice.DataFields.FieldByName("SINGLE_PAYMENT").Value = invoice.netTotal;
                            newInvoice.DataFields.FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;
                            newInvoice.DataFields.FieldByName("SALESMAN_CODE").Value = invoice.salesmanCode;
                            newInvoice.DataFields.FieldByName("SHIPPING_AGENT").Value = shipAgentCode;
                            // hizmet faturaları için irsaliye alanları doldurulmadı
                            if (invoice.type != (int)InvoiceType.BUYING_SERVICE && invoice.type != (int)InvoiceType.SELLING_SERVICE)
                            {
                                Lines dispatches_lines = newInvoice.DataFields.FieldByName("DISPATCHES").Lines;
                                if (dispatches_lines.AppendLine())
                                {
                                    dispatches_lines[0].FieldByName("TYPE").Value = invoice.type;
                                    if (useDefaultNumber)
                                    {
                                        dispatches_lines[0].FieldByName("NUMBER").Value = "~";
                                    }
                                    else
                                    {
                                        dispatches_lines[0].FieldByName("NUMBER").Value = invoice.number; // düzenlecek
                                    }
                                    if (useShortDate)
                                    {
                                        dispatches_lines[0].FieldByName("DATE").Value = Convert.ToDateTime(invoice.date.ToShortDateString());
                                    }
                                    else
                                    {
                                        dispatches_lines[0].FieldByName("DATE").Value = Convert.ToDateTime(invoice.date.ToString("dd-MM-yyyy"));
                                    }
                                    dispatches_lines[0].FieldByName("DOC_NUMBER").Value = invoice.number;
                                    dispatches_lines[0].FieldByName("INVOICE_NUMBER").Value = invoice.number;
                                    dispatches_lines[0].FieldByName("ARP_CODE").Value = invoice.customerCode;
                                    dispatches_lines[0].FieldByName("SOURCE_WH").Value = invoice.wareHouseCode;
                                    dispatches_lines[0].FieldByName("SOURCE_COST_GRP").Value = invoice.wareHouseCode;
                                    dispatches_lines[0].FieldByName("DIVISION").Value = invoice.distributorBranchCode;
                                    dispatches_lines[0].FieldByName("INVOICED").Value = 1;
                                    dispatches_lines[0].FieldByName("SHIPPING_AGENT").Value = shipAgentCode;
                                    dispatches_lines[0].FieldByName("SHIP_DATE").Value = invoice.date.AddDays(2).ToString("dd.MM.yyyy");
                                    dispatches_lines[0].FieldByName("SHIP_TIME").Value = helper.Hour(invoice.date.AddDays(2)).ToString();
                                    dispatches_lines[0].FieldByName("DISP_STATUS").Value = 1;
                                    dispatches_lines[0].FieldByName("TOTAL_DISCOUNTS").Value = invoice.discountTotal;
                                    dispatches_lines[0].FieldByName("TOTAL_DISCOUNTED").Value = invoice.netTotal - invoice.discountTotal;
                                    dispatches_lines[0].FieldByName("ADD_DISCOUNTS").Value = invoice.discountTotal;
                                    dispatches_lines[0].FieldByName("TOTAL_VAT").Value = invoice.vatTotal;
                                    dispatches_lines[0].FieldByName("TOTAL_GROSS").Value = invoice.grossTotal;
                                    dispatches_lines[0].FieldByName("TOTAL_NET").Value = invoice.netTotal;
                                    dispatches_lines[0].FieldByName("NOTES1").Value = "ST Notu: " + invoice.note + " Sevk :" + invoice.customerBranchCode;
                                }
                            }
                            Lines newInvoiceLines = newInvoice.DataFields.FieldByName("TRANSACTIONS").Lines;
                            for (int i = 0; i < invoice.details.Count; i++)
                            {
                                if (newInvoiceLines.AppendLine())
                                {
                                    InvoiceDetail detail = invoice.details[i];
                                    if (detail.type == 2)
                                    {
                                        newInvoiceLines[i].FieldByName("TYPE").Value = detail.type;
                                        newInvoiceLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble(Math.Round(detail.rate, 2));
                                        newInvoiceLines[i].FieldByName("DESCRIPTION").Value = detail.name;
                                        newInvoiceLines[i].FieldByName("SOURCEINDEX").Value = invoice.wareHouseCode;
                                        newInvoiceLines[i].FieldByName("SOURCECOSTGRP").Value = invoice.wareHouseCode;
                                        if (campaignLineNo.Length > 0)
                                        {
                                            Lines newCampaignInfoLine = newInvoiceLines[i].FieldByName("CAMPAIGN_INFOS").Lines;
                                            if (newCampaignInfoLine.AppendLine())
                                            {
                                                newCampaignInfoLine[newCampaignInfoLine.Count - 1].FieldByName("CAMPCODE1").Value = detail.name;
                                                newCampaignInfoLine[newCampaignInfoLine.Count - 1].FieldByName("CAMP_LN_NO").Value = campaignLineNo;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        newInvoiceLines[i].FieldByName("TYPE").Value = detail.type;
                                        if (isProducerCode) // bazı distlerde üürn kodları producerCode a yazılı ,
                                        {
                                            string productCodeByProducer = reader.getProductCodeByProducerCode(detail.code);
                                            if (productCodeByProducer != "" && productCodeByProducer != null)
                                                newInvoiceLines[i].FieldByName("MASTER_CODE").Value = productCodeByProducer;
                                            else
                                            {
                                                helper.LogFile("producer code hata", "", "", "", productCodeByProducer);
                                                string productDetailMessage = detail.code + " Kodlu Ürün Logoda Bulunamadı ";
                                                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(productDetailMessage, invoice.number, remoteInvoiceNumber, false);
                                                receivedInvoices.Add(recievedInvoice);
                                                integratedInvoices.integratedInvoices = receivedInvoices;
                                                integratedInvoices.distributorId = distributorId;
                                                return integratedInvoices;
                                            }
                                        }
                                        else if (isBarcode)   //sümerde ürün kodları barkoda göre getirilecek
                                        {
                                            string productCodeByBarcode = reader.getProductCodeByBarcode(detail.barcode);
                                            if (productCodeByBarcode != "" && productCodeByBarcode != null)
                                                newInvoiceLines[i].FieldByName("MASTER_CODE").Value = productCodeByBarcode;
                                            else
                                            {
                                                string productDetailMessage = detail.barcode + " Barkodlu Ürün Logoda bulunamadı ..";
                                                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(productDetailMessage, invoice.number, remoteInvoiceNumber, false);
                                                receivedInvoices.Add(recievedInvoice);
                                                integratedInvoices.integratedInvoices = receivedInvoices;
                                                integratedInvoices.distributorId = distributorId;
                                                return integratedInvoices;
                                            }
                                        }
                                        else if (invoice.type == (int)InvoiceType.SELLING_SERVICE || invoice.type == (int)InvoiceType.BUYING_SERVICE)
                                        {
                                            newInvoiceLines[i].FieldByName("MASTER_CODE").Value =
                                                reader.getServiceCodeBySalesArtServiceCode(detail.code, invoice.type == 4 ? 1 : 2);
                                            newInvoiceLines[i].FieldByName("TYPE").Value = 4;
                                        }
                                        else
                                            newInvoiceLines[i].FieldByName("MASTER_CODE").Value = detail.code;

                                        newInvoiceLines[i].FieldByName("MASTER_DEF").Value = detail.name;
                                        newInvoiceLines[i].FieldByName("DESCRIPTION").Value = invoice.note;
                                        newInvoiceLines[i].FieldByName("SOURCEINDEX").Value = invoice.wareHouseCode;
                                        newInvoiceLines[i].FieldByName("SOURCECOSTGRP").Value = invoice.wareHouseCode;
                                        newInvoiceLines[i].FieldByName("QUANTITY").Value = detail.quantity;
                                        newInvoiceLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.price);
                                        newInvoiceLines[i].FieldByName("TOTAL").Value = detail.total;
                                        newInvoiceLines[i].FieldByName("CURR_PRICE").Value = 160;  // currency TL

                                        if (invoice.type == (int)InvoiceType.SELLING_SERVICE || invoice.type == (int)InvoiceType.BUYING_SERVICE)
                                            newInvoiceLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit("HİZMET_" + detail.unitCode);
                                        else
                                            newInvoiceLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit(detail.unitCode);
                                        newInvoiceLines[i].FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;
                                        newInvoiceLines[i].FieldByName("VAT_RATE").Value = Convert.ToInt32(detail.vatRate);
                                        newInvoiceLines[i].FieldByName("VAT_AMOUNT").Value = detail.vatAmount;
                                        newInvoiceLines[i].FieldByName("VAT_BASE").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                        newInvoiceLines[i].FieldByName("TOTAL_NET").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                        newInvoiceLines[i].FieldByName("SALEMANCODE").Value = invoice.salesmanCode;
                                        newInvoiceLines[i].FieldByName("MONTH").Value = DateTime.Now.Month;
                                        newInvoiceLines[i].FieldByName("YEAR").Value = DateTime.Now.Year;
                                        newInvoiceLines[i].FieldByName("BARCODE").Value = detail.barcode;
                                        // satış , satış iade ve verilen hizmet ise satış fiyatı üzerinden çalışsın denildi
                                        if (invoice.type == (int)InvoiceType.SELLING || invoice.type == (int)InvoiceType.SELLING_RETURN || invoice.type == (int)InvoiceType.SELLING_SERVICE)
                                        {
                                            newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 2;
                                        }
                                        else
                                        {
                                            newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 1;
                                        }
                                        if (invoice.type == (int)InvoiceType.SELLING_RETURN)  // iade faturaları 
                                        {
                                            newInvoiceLines[i].FieldByName("RET_COST_TYPE").Value = 1;
                                        }
                                    }
                                }
                            }
                            Lines paymentList = newInvoice.DataFields.FieldByName("PAYMENT_LIST").Lines;
                            newInvoice.DataFields.FieldByName("EINVOICE").Value = reader.getEInvoiceByCustomerCode(invoice.customerCode);
                            newInvoice.DataFields.FieldByName("PROFILE_ID").Value = reader.getProfileIDByCustomerCode(invoice.customerCode);
                            newInvoice.DataFields.FieldByName("AFFECT_RISK").Value = 0;
                            newInvoice.DataFields.FieldByName("DOC_DATE").Value = invoice.documentDate.ToShortDateString();
                            newInvoice.DataFields.FieldByName("EXIMVAT").Value = 0;
                            newInvoice.FillAccCodes();
                            newInvoice.CreateCompositeLines();
                            newInvoice.ReCalculate();
                            ValidateErrors err = newInvoice.ValidateErrors;
                            newInvoice.ExportToXML("SALES_INVOICES", @"C:\invoices.xml");
                            helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
                            if (newInvoice.Post())
                            {
                                var integratedInvoiceRef = newInvoice.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
                                newInvoice.Read(integratedInvoiceRef);
                                remoteInvoiceNumber = newInvoice.DataFields.FieldByName("NUMBER").Value;
                                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(message, invoice.number, remoteInvoiceNumber, true);
                                receivedInvoices.Add(recievedInvoice);
                            }
                            else
                            {
                                if (newInvoice.ErrorCode != 0)
                                {
                                    message = "DBError(" + newInvoice.ErrorCode.ToString() + ")-" + newInvoice.ErrorDesc + newInvoice.DBErrorDesc;
                                    IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(message, invoice.number, remoteInvoiceNumber, false);
                                    receivedInvoices.Add(recievedInvoice);
                                }
                                else if (newInvoice.ValidateErrors.Count > 0)
                                {
                                    for (int i = 0; i < err.Count; i++)
                                    {
                                        message += err[i].Error;
                                    }
                                    IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(message, invoice.number, remoteInvoiceNumber, false);
                                    receivedInvoices.Add(recievedInvoice);
                                }
                            }
                            helper.LogFile("POST Bitti", "-", "-", "-", "-");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Logoya Bağlantı Problemi Yaşandı, Faturalar Aktarılamadı.", "Logo Bağlantı Hatası", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(ex.Message.ToString(), "", remoteInvoiceNumber, false);
                receivedInvoices.Add(recievedInvoice);
            }
            finally
            {
                unity.UserLogout();
                unity.Disconnect();
                isLoggedIn = false;
                message = "";
            }
            integratedInvoices.integratedInvoices = receivedInvoices;
            integratedInvoices.distributorId = distributorId;
            return integratedInvoices;
        }
        public IntegratedWaybillStatus sendMultipleDespatch(List<LogoWaybill> despatches)
        {

            string remoteDespatchNumber = "";
            string message = "";

            List<IntegratedWaybillDto> receivedWaybills = new List<IntegratedWaybillDto>();

            try
            {
                if (isLoggedIn)
                {
                    foreach (var despatch in despatches)
                    {  // boolean dön 
                        Data newDespatch = unity.NewDataObject(DataObjectType.doSalesDispatch);

                        //8 satış , 3 Satış iade ,9 verilen hizmet
                        if (useDispatch)
                        {
                            newDespatch = unity.NewDataObject(DataObjectType.doSalesDispatch);
                        }
                        else
                        {
                            newDespatch = unity.NewDataObject(DataObjectType.doPurchDispatch);
                        }
                        newDespatch.New();
                        newDespatch.DataFields.FieldByName("TYPE").Value = despatch.type;
                        newDespatch.DataFields.FieldByName("NUMBER").Value = despatch.number; // düzenlecek
                        newDespatch.DataFields.FieldByName("DOC_NUMBER").Value = despatch.documentNumber;

                        if (useShortDate)
                        {
                            newDespatch.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(despatch.date.ToShortDateString());
                        }
                        else
                        {
                            newDespatch.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(despatch.date.ToString("dd-MM-yyyy"));
                        }

                        newDespatch.DataFields.FieldByName("TIME").Value = helper.Hour(despatch.date.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
                        newDespatch.DataFields.FieldByName("AUXIL_CODE").Value = despatch.salesmanCode;
                        newDespatch.DataFields.FieldByName("ARP_CODE").Value = despatch.customerCode;
                        newDespatch.DataFields.FieldByName("SOURCE_WH").Value = despatch.wareHouseCode;
                        newDespatch.DataFields.FieldByName("SOURCE_COST_GRP").Value = despatch.wareHouseCode;
                        newDespatch.DataFields.FieldByName("DEPARTMENT").Value = helper.getDepartment();
                        newDespatch.DataFields.FieldByName("SHIPPING_AGENT").Value = shipAgentCode;
                        newDespatch.DataFields.FieldByName("SHIP_DATE").Value = despatch.date.AddDays(2).ToString("dd.MM.yyyy");
                        newDespatch.DataFields.FieldByName("SHIP_TIME").Value = helper.Hour(despatch.date.AddDays(2)).ToString();
                        newDespatch.DataFields.FieldByName("DISP_STATUS").Value = 1;


                        if (useCypheCode)
                        {
                            newDespatch.DataFields.FieldByName("AUTH_CODE").Value = cypheCode;
                        }

                        newDespatch.DataFields.FieldByName("DIVISION").Value = despatch.distributorBranchCode;
                        newDespatch.DataFields.FieldByName("TOTAL_DISCOUNTS").Value = despatch.discountTotal;
                        newDespatch.DataFields.FieldByName("TOTAL_DISCOUNTED").Value = despatch.netTotal - despatch.discountTotal;
                        newDespatch.DataFields.FieldByName("ADD_DISCOUNTS").Value = despatch.discountTotal;

                        newDespatch.DataFields.FieldByName("TOTAL_VAT").Value = despatch.vatTotal;
                        newDespatch.DataFields.FieldByName("TOTAL_GROSS").Value = despatch.grossTotal;
                        newDespatch.DataFields.FieldByName("TOTAL_NET").Value = despatch.netTotal;
                        newDespatch.DataFields.FieldByName("NOTES1").Value = "ST Notu: " + despatch.note + " Sevk :" + despatch.customerBranchCode + "_" + despatch.customerBranchName;
                        newDespatch.DataFields.FieldByName("TC_NET").Value = despatch.netTotal;

                        newDespatch.DataFields.FieldByName("PAYMENT_CODE").Value = despatch.paymentCode;
                        newDespatch.DataFields.FieldByName("SALESMANCODE").Value = despatch.salesmanCode;

                        Lines newWaybillLines = newDespatch.DataFields.FieldByName("TRANSACTIONS").Lines;

                        for (int i = 0; i < despatch.details.Count; i++)
                        {
                            if (newWaybillLines.AppendLine())
                            {
                                WaybillDetail detail = despatch.details[i];
                                if (detail.type == 2)
                                {
                                    newWaybillLines[i].FieldByName("TYPE").Value = detail.type;
                                    newWaybillLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble(Math.Round(detail.rate, 2));
                                    newWaybillLines[i].FieldByName("DESCRIPTION").Value = detail.name;
                                    //newWaybillLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble((100 * Convert.ToDouble(detail.discountTotal)) / Convert.ToDouble(detail.grossTotal));

                                }
                                else
                                {
                                    newWaybillLines[i].FieldByName("TYPE").Value = detail.type;

                                    if (isProducerCode) // bazı distlerde üürn kodları producerCode a yazılı ,
                                    {
                                        newWaybillLines[i].FieldByName("MASTER_CODE").Value = reader.getProductCodeByProducerCode(detail.code);
                                    }
                                    else
                                    {
                                        newWaybillLines[i].FieldByName("MASTER_CODE").Value = detail.code;
                                    }

                                    newWaybillLines[i].FieldByName("SOURCEINDEX").Value = despatch.wareHouseCode;
                                    newWaybillLines[i].FieldByName("SOURCECOSTGRP").Value = despatch.wareHouseCode;
                                    newWaybillLines[i].FieldByName("QUANTITY").Value = detail.quantity;
                                    newWaybillLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.price);
                                    newWaybillLines[i].FieldByName("TOTAL").Value = detail.total;
                                    newWaybillLines[i].FieldByName("CURR_PRICE").Value = 160;  // currency TL
                                                                                               //newInvoiceLines[i].FieldByName("UNIT_CODE").Value = "AD";
                                    newWaybillLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit(detail.unitCode);
                                    newWaybillLines[i].FieldByName("PAYMENT_CODE").Value = despatch.paymentCode;

                                    //newInvoiceLines[i].FieldByName("UNIT_CONV1").Value = 1; //adet carpanı
                                    //newInvoiceLines[i].FieldByName("UNIT_CONV2").Value = 12;  // koli carpanı
                                    newWaybillLines[i].FieldByName("VAT_RATE").Value = Convert.ToInt32(detail.vatRate);
                                    newWaybillLines[i].FieldByName("VAT_AMOUNT").Value = detail.vatAmount;
                                    newWaybillLines[i].FieldByName("VAT_BASE").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                    newWaybillLines[i].FieldByName("TOTAL_NET").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                    newWaybillLines[i].FieldByName("SALEMANCODE").Value = despatch.salesmanCode;
                                    newWaybillLines[i].FieldByName("MONTH").Value = DateTime.Now.Month;
                                    newWaybillLines[i].FieldByName("YEAR").Value = DateTime.Now.Year;
                                    //newInvoiceLines[i].FieldByName("EDT_CURR").Value = 1;
                                    //newInvoiceLines[i].FieldByName("UNIT_GLOBAL_CODE").Value = "NIU";
                                    newWaybillLines[i].FieldByName("BARCODE").Value = detail.barcode;

                                    if (despatch.type == 3 || despatch.type == 8 || despatch.type == 9) // satış , satış iade ve verilen hizmet ise satış fiyatı üzerinden çalışsın denildi
                                    {

                                        newWaybillLines[i].FieldByName("PRCLISTTYPE").Value = 2;
                                    }
                                    else
                                    {
                                        newWaybillLines[i].FieldByName("PRCLISTTYPE").Value = 1;
                                    }

                                    if (despatch.type == 3)  // iade faturaları 
                                    {
                                        newWaybillLines[i].FieldByName("RET_COST_TYPE").Value = 1;
                                    }
                                }
                            }
                        }


                        newDespatch.DataFields.FieldByName("EINVOICE").Value = reader.getEInvoiceByCustomerCode(despatch.customerCode);


                        newDespatch.DataFields.FieldByName("AFFECT_RISK").Value = 0;
                        newDespatch.DataFields.FieldByName("DOC_DATE").Value = despatch.documentDate.ToShortDateString();

                        //newInvoice.DataFields.FieldByName("EARCHIVEDETR_INTPAYMENTTYPE").Value = 0;
                        //newInvoice.DataFields.FieldByName("EBOOK_DOCDATE").Value = "06.07.2015";
                        //newInvoice.DataFields.FieldByName("EBOOK_DOCNR").Value = "1234";
                        //newInvoice.DataFields.FieldByName("EBOOK_DOCTYPE").Value = 5;
                        //newInvoice.DataFields.FieldByName("EBOOK_PAYTYPE").Value = "COKSECMELI";
                        //newInvoice.DataFields.FieldByName("EBOOK_NOPAY").Value = 1;

                        newDespatch.FillAccCodes();
                        newDespatch.CreateCompositeLines();
                        newDespatch.ReCalculate();

                        ValidateErrors err = newDespatch.ValidateErrors;

                        newDespatch.ExportToXML("SALES_INVOICES", @"C:\invoices.xml");
                        helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
                        if (newDespatch.Post())
                        {
                            var integratedWaybillRef = newDespatch.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
                            newDespatch.Read(integratedWaybillRef);

                            remoteDespatchNumber = newDespatch.DataFields.FieldByName("NUMBER").Value;

                            IntegratedWaybillDto recievedWaybill = new IntegratedWaybillDto(message, despatch.number, remoteDespatchNumber, true);
                            receivedWaybills.Add(recievedWaybill);
                        }
                        else
                        {
                            if (newDespatch.ErrorCode != 0)
                            {
                                message = "DBError(" + newDespatch.ErrorCode.ToString() + ")-" + newDespatch.ErrorDesc + newDespatch.DBErrorDesc;
                                IntegratedWaybillDto recievedWaybill = new IntegratedWaybillDto(message, despatch.number, remoteDespatchNumber, false);
                                receivedWaybills.Add(recievedWaybill);
                            }
                            else if (newDespatch.ValidateErrors.Count > 0)
                            {
                                for (int i = 0; i < err.Count; i++)
                                {
                                    message += err[i].Error;
                                }

                                IntegratedWaybillDto recievedWaybill = new IntegratedWaybillDto(message, despatch.number, remoteDespatchNumber, false);
                                receivedWaybills.Add(recievedWaybill);
                            }
                        }
                        helper.LogFile("POST Bitti", "-", "-", "-", "-");
                    }
                }
                else
                {
                    MessageBox.Show("Logoya Bağlantı Problemi Yaşandı, Faturalar Aktarılamadı.", "Logo Bağlantı Hatası", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                IntegratedWaybillDto recievedWaybill = new IntegratedWaybillDto(ex.Message.ToString(), "", remoteDespatchNumber, false);
                receivedWaybills.Add(recievedWaybill);
            }
            finally
            {
                unity.UserLogout();
                unity.Disconnect();
                isLoggedIn = false;
                message = "";

            }

            integratedWaybills.integratedWaybills = receivedWaybills;
            integratedWaybills.distributorId = distributorId;

            string basarili = "";
            string basarisiz = "";
            foreach (var item in integratedWaybills.integratedWaybills)
            {
                if (item.successfullyIntegrated)
                {
                    helper.LogFile("Fatura Aktarım Bilgisi", item.invoiceNumber, item.remoteInvoiceNumber, "AKTARIM BAŞARILI", item.errorMessage);
                    basarili += " Logo Fatura Numarası   : " + item.remoteInvoiceNumber;
                }
                else
                {
                    helper.LogFile("Fatura Aktarım Bilgisi", item.invoiceNumber, item.remoteInvoiceNumber, "AKTARIM BAŞARISIZ..!!!", item.errorMessage);
                    basarisiz += item.invoiceNumber + " numaralı fatura için : " + item.errorMessage;
                }
            }
            string msj = "Başarılı : " + basarili + "    Başarısız : " + basarisiz;
            if (MessageBox.Show(msj, "Aktarılan/Aktarılamayan Fatura Bilgileri", MessageBoxButtons.OK) == DialogResult.OK)
            { Clipboard.SetText(msj); }

            return integratedWaybills;
        }
        public IntegratedOrderStatus sendMultipleOrder(List<Order> orders)
        {
            string message = "";
            long remoteOrderId = 0;

            List<IntegratedOrderDto> receivedOrders = new List<IntegratedOrderDto>();

            try
            {
                if (isLoggedIn)
                {
                    foreach (var order in orders)
                    {  // boolean dön 
                        Data newOrder = unity.NewDataObject(DataObjectType.doSalesOrderSlip);

                        // remoteInvoiceNumber = reader.getInvoiceNumberByDocumentNumber(order.number);  // salesArttaki invoice number , logoda documentNumber alanına yazılıyor.
                        //if (remoteInvoiceNumber != "")
                        //{
                        //    IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(invoice.number + " belge numaralı fatura, sistemde zaten mevcut. Kontrol Ediniz", invoice.number, remoteInvoiceNumber, true);
                        //    receivedInvoices.Add(recievedInvoice);
                        //}
                        //else
                        //{
                        //8 satış , 3 Satış iade ,9 verilen hizmet
                        if (order.type == 3)
                        {
                            newOrder = unity.NewDataObject(DataObjectType.doSalesOrderSlip);
                        }
                        //else
                        //{
                        //    newInvoice = unity.NewDataObject(DataObjectType.doPurchInvoice);
                        //}
                        newOrder.New();
                        newOrder.DataFields.FieldByName("TYPE").Value = order.type;

                        if (useDefaultNumber)
                        {
                            newOrder.DataFields.FieldByName("NUMBER").Value = "~";
                        }
                        else
                        {
                            newOrder.DataFields.FieldByName("NUMBER").Value = order.receiptNumber;
                        }

                        //order.DataFields.FieldByName("RC_RATE").Value = 1;
                        // order.DataFields.FieldByName("CURRSEL_TOTAL").Value = 1;
                        // order.DataFields.FieldByName("DATA_SITEID").Value = 1;


                        if (useShortDate)
                        {
                            newOrder.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(order.orderDate.ToShortDateString());
                        }
                        else
                        {
                            newOrder.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(order.orderDate.ToString("dd-MM-yyyy"));
                        }

                        newOrder.DataFields.FieldByName("TIME").Value = helper.Hour(order.deliveryDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
                        newOrder.DataFields.FieldByName("SALESMAN_CODE").Value = order.salesman.code;
                        newOrder.DataFields.FieldByName("ARP_CODE").Value = order.customer.code;
                        newOrder.DataFields.FieldByName("SOURCE_WH").Value = order.warehouse.code;
                        newOrder.DataFields.FieldByName("SOURCE_COST_GRP").Value = order.warehouse.code;
                        newOrder.DataFields.FieldByName("ORDER_STATUS").Value = 1;
                        //newOrder.DataFields.FieldByName("DEPARTMENT").Value = helper.getDepartment();


                        if (useCypheCode)
                        {
                            newOrder.DataFields.FieldByName("AUTH_CODE").Value = order.customer.code.Substring(2, 2);
                        }

                        newOrder.DataFields.FieldByName("AUXIL_CODE").Value = order.customer.code.Substring(4, 2);


                        //newInvoice.DataFields.FieldByName("POST_FLAGS").Value = 241;
                        newOrder.DataFields.FieldByName("DIVISION").Value = order.customerBranch.code;

                        newOrder.DataFields.FieldByName("TOTAL_VAT").Value = order.vatTotal;
                        newOrder.DataFields.FieldByName("TOTAL_GROSS").Value = order.grossTotal;
                        newOrder.DataFields.FieldByName("TOTAL_NET").Value = order.preVatNetTotal;
                        newOrder.DataFields.FieldByName("NOTES1").Value = order.salesmanNote;
                        newOrder.DataFields.FieldByName("PAYMENT_CODE").Value = order.paymentType.code;
                        //newOrder.DataFields.FieldByName("SHIPPING_AGENT").Value = shipAgentCode;


                        Lines newOrderLines = newOrder.DataFields.FieldByName("TRANSACTIONS").Lines;

                        for (int i = 0; i < order.details.Count; i++)
                        {
                            if (newOrderLines.AppendLine())
                            {
                                OrderDetail detail = order.details[i];

                                if (detail.type == 2)  // indirim
                                {
                                    newOrderLines[i].FieldByName("TYPE").Value = detail.type;
                                    newOrderLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble(Math.Round(detail.rate, 2));
                                    newOrderLines[i].FieldByName("DUE_DATE").Value = Convert.ToDateTime(order.orderDate.ToString("dd-MM-yyyy"));
                                    newOrderLines[i].FieldByName("SALESMAN_CODE").Value = order.salesman.code;

                                }
                                else
                                {

                                    newOrderLines[i].FieldByName("TYPE").Value = 0;
                                    newOrderLines[i].FieldByName("MASTER_CODE").Value = detail.productCode;
                                    // newOrderLines[i].FieldByName("SOURCEINDEX").Value = order.warehouse.code;
                                    //  newOrderLines[i].FieldByName("SOURCECOSTGRP").Value = order.warehouse.code;
                                    newOrderLines[i].FieldByName("QUANTITY").Value = detail.quantity;
                                    newOrderLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.orderItemPrice);
                                    newOrderLines[i].FieldByName("TOTAL").Value = detail.grossTotal;
                                    newOrderLines[i].FieldByName("CURR_PRICE").Value = 160;  // currency TL
                                    newOrderLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit(detail.unitCode);
                                    newOrderLines[i].FieldByName("PAYMENT_CODE").Value = order.paymentType.code;
                                    newOrderLines[i].FieldByName("DUE_DATE").Value = Convert.ToDateTime(order.orderDate.ToString("dd-MM-yyyy"));

                                    newOrderLines[i].FieldByName("VAT_RATE").Value = Convert.ToInt32(detail.vatRate);
                                    newOrderLines[i].FieldByName("VAT_AMOUNT").Value = detail.vatTotal;
                                    //newOrderLines[i].FieldByName("SALEMANCODE").Value = order.salesman.code;
                                    //newOrderLines[i].FieldByName("SALESMAN_CODE").Value = order.salesman.code;
                                    //newOrderLines[i].FieldByName("MONTH").Value = DateTime.Now.Month;
                                    //newOrderLines[i].FieldByName("YEAR").Value = DateTime.Now.Year;
                                    //newInvoiceLines[i].FieldByName("EDT_CURR").Value = 1;
                                    //newInvoiceLines[i].FieldByName("UNIT_GLOBAL_CODE").Value = "NIU";
                                    //newOrderLines[i].FieldByName("BARCODE").Value = detail.productBarcode;

                                    newOrderLines[i].FieldByName("PRCLISTTYPE").Value = 2;
                                }
                            }
                        }

                        newOrder.FillAccCodes();
                        newOrder.CreateCompositeLines();
                        newOrder.ReCalculate();


                        ValidateErrors err = newOrder.ValidateErrors;

                        newOrder.ExportToXML("SALES_ORDERS", @"C:\orders.xml");
                        helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
                        if (newOrder.Post())
                        {
                            var integratedOrderRef = newOrder.DataFields.FieldByName("INTERNAL_REFERENCE").Value;

                            //newOrder.Read(integratedOrderRef);

                            //remoteNumber = newOrder.DataFields.FieldByName("NUMBER").Value;

                            IntegratedOrderDto recievedOrder = new IntegratedOrderDto(message, integratedOrderRef, order.orderId, true);
                            receivedOrders.Add(recievedOrder);

                            //IntegratedOrderForMessageDto integratedOrderForMessage = new IntegratedOrderForMessageDto(message,remoteNumber,order.orderId,true);

                        }
                        else
                        {
                            if (newOrder.ErrorCode != 0)
                            {
                                message = "DBError(" + newOrder.ErrorCode.ToString() + ")-" + newOrder.ErrorDesc + newOrder.DBErrorDesc;
                                IntegratedOrderDto recievedOrder = new IntegratedOrderDto(message, remoteOrderId, order.orderId, false);
                                receivedOrders.Add(recievedOrder);
                            }
                            else if (newOrder.ValidateErrors.Count > 0)
                            {
                                for (int i = 0; i < err.Count; i++)
                                {
                                    message += err[i].Error;
                                }

                                IntegratedOrderDto recievedOrder = new IntegratedOrderDto(message, remoteOrderId, order.orderId, false);
                                receivedOrders.Add(recievedOrder);
                            }
                        }
                        helper.LogFile("POST Bitti", "-", "-", "-", "-");
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("Logoya Bağlantı Problemi Yaşandı, Siparişler Aktarılamadı.", "Logo Bağlantı Hatası", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                IntegratedOrderDto recievedOrder = new IntegratedOrderDto(ex.Message.ToString(), remoteOrderId, 0, false);
                receivedOrders.Add(recievedOrder);
            }
            finally
            {
                unity.UserLogout();
                unity.Disconnect();
                isLoggedIn = false;
                message = "";

            }

            integratedOrders.orders = receivedOrders;
            integratedOrders.distributorId = distributorId;

            return integratedOrders;
        }
        private void btnXML_Click(object sender, EventArgs e)
        {
            if (dataGridInvoice.Rows.Count > 0)
            {
                SaveToXml();
            }
            else
            {
                MessageBox.Show("Fatura Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
            }
        }
        public void SaveToXml()
        {
            SelectionHelper selectionHelper = new SelectionHelper();
            XmlHelper xmlHelper = new XmlHelper();
            integratedInvoices = null;
            //var selectedInvoices1 = GetSelectedInvoices();
            var selectedInvoices = selectionHelper.GetSelectedInvoices(dataGridInvoice, jsonInvoices);
            Cursor.Current = Cursors.WaitCursor;
            helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
            //IntegratedInvoiceStatus status = null; 
            if (XMLTransferForOrder)
                integratedInvoices = xmlHelper.OrderListExportToXml(selectedInvoices);
            else
                integratedInvoices = xmlHelper.InvoiceListExportToXml(selectedInvoices);
            helper.ShowMessages(integratedInvoices);
            helper.LogFile("Fatura Aktarım Bitti", "-", "-", "-", "-");
            dataGridInvoice.Rows.Clear();
            Cursor.Current = Cursors.Default;
        }
        public IntegratedInvoiceStatus sendMultipleInvoicesForMikro(List<LogoInvoiceJson> invoices)
        {
            string remoteNumber = "";
            string message = "";

            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();

            try
            {
                foreach (var invoice in invoices)
                {
                    remoteNumber = reader.checkInvoiceNumber(invoice.number, invoice.customerCode);
                    if (remoteNumber != "")
                    {
                        IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(invoice.number + " belge numaralı fatura, sistemde zaten mevcut. Kontrol Ediniz", invoice.number, remoteNumber, false);
                        receivedInvoices.Add(recievedInvoice);
                    }
                    else
                    {
                        string guid = reader.createInvoice(invoice);
                        if (guid != "" && guid.Length > 0)
                        {
                            IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(message, invoice.number, guid, true);
                            receivedInvoices.Add(recievedInvoice);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(ex.Message.ToString(), "", remoteNumber, false);
                receivedInvoices.Add(recievedInvoice);
            }
            finally
            {
                message = "";

            }

            integratedInvoices.integratedInvoices = receivedInvoices;
            integratedInvoices.distributorId = distributorId;

            return integratedInvoices;
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
            Cursor.Current = Cursors.WaitCursor;
            dataGridInvoice.Rows.Clear();
            chkSelectAll.Checked = false;
            if (chkDispatch.Checked)
                GetWaybills();
            else if (orderTransferToLogoInfo)
                GetOrders();
            else GetInvoices();
            btnSendToLogo.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn) ? true : false;
            btnCheckLogoConnection.Enabled = (dataGridInvoice.Rows.Count > 0 && !isLoggedIn) ? true : false;
            Cursor.Current = Cursors.Default;
        }
        private void btnSendToLogo_Click(object sender, EventArgs e)
        {
            //integratedInvoices = null;
            IntegratedInvoiceStatus status = new IntegratedInvoiceStatus();
            ResponseHelper responseHelper = new ResponseHelper();
            SelectionHelper selectionHelper = new SelectionHelper();
            if (dataGridInvoice.Rows.Count > 0)
            {
                List<LogoInvoiceJson> selectedInvoicesForMikro = new List<LogoInvoiceJson>();
                List<LogoInvoice> selectedInvoices = new List<LogoInvoice>();
                if (integrationForMikroERP)
                    selectedInvoicesForMikro = selectionHelper.GetSelectedInvoicesForMikro(dataGridInvoice, jsonInvoices);
                else
                    //selectedInvoices = GetSelectedInvoices();
                    selectedInvoices = selectionHelper.GetSelectedInvoices(dataGridInvoice, jsonInvoices);
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
                if (integrationForMikroERP)
                    status = sendMultipleInvoicesForMikro(selectedInvoicesForMikro);
                else
                    status = sendMultipleInvoice(selectedInvoices);
                //MessageBox.Show("Aktarım Başarılı");
                //status = sendMultipleInvoice(selectedInvoices);
                responseHelper.SendResponse(status);
                helper.ShowMessages(status);
                helper.LogFile("Fatura Aktarım Bitti", "-", "-", "-", "-");
                dataGridInvoice.Rows.Clear();
                btnSendToLogo.Enabled = false;
                btnCheckLogoConnection.Enabled = false;
                lblLogoConnectionInfo.Text = "";
                Cursor.Current = Cursors.Default;
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
            if (dataGridInvoice.Rows.Count > 0)
            {
                //List<LogoWaybill> selectedWaybills = GetSelectedWaybills();
                List<LogoWaybill> selectedWaybills = selectionHelper.GetSelectedWaybills(dataGridInvoice, jsonWaybills);
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("İrsaliye Aktarım Basladı", "-", "-", "-", "-");
                IntegratedWaybillStatus status = sendMultipleDespatch(selectedWaybills);//sendMultipleInvoice(selectedInvoices);
                                                                                        //IntegratedInvoiceStatus status = sendMultipleInvoice(selectedInvoices);
                responseHelper.SendResponse(status);
                helper.ShowMessages(status);
                helper.LogFile("İrsaliye Aktarım Bitti", "-", "-", "-", "-");
                dataGridInvoice.Rows.Clear();
                btnSendToLogo.Enabled = false;
                btnCheckLogoConnection.Enabled = false;
                lblLogoConnectionInfo.Text = "";

                Cursor.Current = Cursors.Default;
            }
            else MessageBox.Show("İrsaliye Seçmelisiniz..", "İrsaliye Seçim", MessageBoxButtons.OK);
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
            if (dataGridInvoice.Rows.Count > 0)
            {
                //List<Order> selectedOrders = GetSelectedOrders();
                List<Order> selectedOrders = selectionHelper.GetSelectedOrders(dataGridInvoice, jsonOrders);
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("Sipariş Aktarım Basladı", "-", "-", "-", "-");
                IntegratedOrderStatus status = sendMultipleOrder(selectedOrders);
                responseHelper.SendResponse(status);
                helper.ShowMessages(status);
                helper.LogFile("Sipariş Aktarım Bitti", "-", "-", "-", "-");
                dataGridInvoice.Rows.Clear();
                btnSendToLogo.Enabled = false;
                btnCheckLogoConnection.Enabled = false;
                lblLogoConnectionInfo.Text = "";
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Sipariş Seçmelisiniz..", "Sipariş Seçim", MessageBoxButtons.OK);
            }
        }
    }

}