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
using System.IO;
using invoiceIntegration.model.waybill;
using invoiceIntegration.helper;
using System.Xml;

namespace invoiceIntegration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        } 

        string logoUserName = Configuration.getLogoUserName();
        string logoPassword = Configuration.getLogoPassword();
        string season = Configuration.getSeason();
        string companyCode = Configuration.getCompanyCode();
        bool isProducerCode = Configuration.getIsProducerCode();
        bool useCypheCode = Configuration.getUseCypheCode();
        string cypheCode = Configuration.getCypheCode();
        bool isBarcode = Configuration.getIsBarcode();
        bool useDefaultNumber = Configuration.getUseDefaultNumber();
        bool useShortDate = Configuration.getUseShortDate();
        bool useShipCode = Configuration.getUseShipCode();
        bool XMLTransferInfo = Configuration.getXMLTransferInfo();
        int distributorId = Configuration.getDistributorId();
        bool useDispatch = Configuration.getUseDispatch();
        string url = Configuration.getUrl();
         
        IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
        IntegratedWaybillStatus integratedWaybills = new IntegratedWaybillStatus();

        GenericResponse<List<LogoInvoiceJson>> jsonInvoices = new GenericResponse<List<LogoInvoiceJson>>();
        GenericResponse<List<LogoWaybillJson>> jsonWaybills = new GenericResponse<List<LogoWaybillJson>>();
        LogoDataReader reader = new LogoDataReader();
        UnityApplication unity = LogoApplication.getApplication();
        List<Discount> discounts = new List<Discount>();
        Helper helper = new Helper();

        bool isLoggedIn = false;
        string filePath = "";
        InvoiceType invoiceType;

        private void Form1_Load(object sender, EventArgs e)
        { 
            cmbInvoice.SelectedIndex = 0;
            lblLogoConnectionInfo.Text = "";
            startDate.Value = DateTime.Now.AddDays(-1);
            if (useDispatch)
                chkDispatch.Visible = true;

            if(XMLTransferInfo)
            {
                btnCheckLogoConnection.Visible = false;
                btnSendToLogo.Visible = false;
                btnXML.Visible = true;
            }
        }

        void createXML(LogoInvoice invoice)
        {
            XmlDocument output = null;
            XmlNode outputInvoiceDbop = null;
            XmlNode outputInvoiceSales = null;
            XmlNode outputTransactions = null;
            XmlNode outputDispatches = null;
            XmlNode outputDispatch = null;
            XmlNode outputTransaction = null;

            string date = "";

            output = new XmlDocument();
            outputInvoiceSales = null;

            XmlDeclaration xmlDeclaration = output.CreateXmlDeclaration("1.0", "ISO-8859-9", null);
            output.InsertBefore(xmlDeclaration, output.DocumentElement);

            //8 satış , 3 Satış iade  
            if (invoice.type == 8 || invoice.type == 3)
            {
                outputInvoiceSales = output.CreateNode(XmlNodeType.Element, "SALES_INVOICES", "");
            }
            else
            {
                //alış faturalarında 
                outputInvoiceSales = output.CreateNode(XmlNodeType.Element, "PURCHASE_INVOICES", "");
            }
            output.AppendChild(outputInvoiceSales);

            outputInvoiceDbop = output.CreateNode(XmlNodeType.Element, "INVOICE", "");
            XmlAttribute newAttr = output.CreateAttribute("DBOP");
            newAttr.Value = "INS";
            outputInvoiceDbop.Attributes.Append(newAttr);
            outputInvoiceSales.AppendChild(outputInvoiceDbop);

            if(invoice.type == 8)
                helper.AddNode(output, outputInvoiceDbop, "TYPE", "1");  //satış fatularında

            if (invoice.type == 3)
                 helper.AddNode(output, outputInvoiceDbop, "TYPE", "3");  //iade fatruralarında

            if (useShortDate)
            {
                date =(invoice.date.ToShortDateString());
            }
            else
            {
                date = (invoice.date.ToString("dd-MM-yyyy"));
            }

            helper.AddNode(output, outputInvoiceDbop, "DATE", date);

            helper.AddNode(output, outputInvoiceDbop, "NUMBER", invoice.number);
            helper.AddNode(output, outputInvoiceDbop, "DOC_NUMBER", invoice.documentNumber);
            helper.AddNode(output, outputInvoiceDbop, "ARP_CODE", invoice.customerCode);
            //zorunlu değil
            helper.AddNode(output, outputInvoiceDbop, "DIVISION", invoice.distributorBranchCode);
            helper.AddNode(output, outputInvoiceDbop, "DEPARTMENT", helper.getDepartment());

            if (useCypheCode)
            {
                helper.AddNode(output, outputInvoiceDbop, "AUTH_CODE", cypheCode);
            }

            helper.AddNode(output, outputInvoiceDbop, "AUXIL_CODE", invoice.salesmanCode);
            helper.AddNode(output, outputInvoiceDbop, "SOURCE_WH", invoice.wareHouseCode);
            helper.AddNode(output, outputInvoiceDbop, "SOURCE_COST_GRP", invoice.wareHouseCode);
            //
            helper.AddNode(output, outputInvoiceDbop, "EINVOICE", reader.getEInvoiceByCustomerCode(invoice.customerCode).ToString());
            helper.AddNode(output, outputInvoiceDbop, "PROFILE_ID", reader.getProfileIDByCustomerCode(invoice.customerCode).ToString());

            helper.AddNode(output, outputInvoiceDbop, "TOTAL_DISCOUNTS", invoice.discountTotal.ToString());  // indirim toplamı
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_DISCOUNTED", (invoice.netTotal - invoice.discountTotal).ToString());  // toplam tutar
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_GROSS", invoice.grossTotal.ToString()); // brüt tutar
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_NET", invoice.netTotal.ToString());  // Kdv hariç tutar
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_VAT", invoice.vatTotal.ToString());  // Toplam Kdv
            helper.AddNode(output, outputInvoiceDbop, "PAYMENT_CODE", invoice.paymentCode);  // ödeme planı
            helper.AddNode(output, outputInvoiceDbop, "SALESMAN_CODE", invoice.salesmanCode);  // salesman 

            outputTransactions = output.CreateNode(XmlNodeType.Element, "TRANSACTIONS", "");
            outputInvoiceDbop.AppendChild(outputTransactions);

            for (int i = 0; i < invoice.details.Count; i++)
            {

                outputTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
                outputTransactions.AppendChild(outputTransaction);

                if (invoice.details[i].type == 2)
                {
                    double discountRate = Convert.ToDouble(Math.Round(Convert.ToDecimal((100 * Convert.ToDouble(invoice.details[i].discountTotal)) / Convert.ToDouble(invoice.details[i].grossTotal)), 2)); 
                    //1discounts 
                    if (discountRate > 0 && discountRate < 100)
                    {
                        outputTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
                        outputTransactions.AppendChild(outputTransaction);
                        helper.AddNode(output, outputTransaction, "DETAIL_LEVEL", "0");
                        helper.AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                        helper.AddNode(output, outputTransaction, "QUANTITY", "0");
                        helper.AddNode(output, outputTransaction, "BILLED", "0");
                        helper.AddNode(output, outputTransaction, "TOTAL", Convert.ToDouble(invoice.details[i].discountTotal).ToString().Replace(",", "."));
                        helper.AddNode(output, outputTransaction, "DISCOUNT_RATE", discountRate.ToString().Replace(",", "."));
                        helper.AddNode(output, outputTransaction, "DISPATCH_NUMBER", invoice.number);
                        if (invoice.type == 3)  // iade faturaları için
                        {
                            helper.AddNode(output, outputTransaction, "RET_COST_TYPE", "1");
                        }
                    }
                } else
                {
                    helper.AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                    helper.AddNode(output, outputTransaction, "MASTER_CODE", invoice.details[i].code);
                    helper.AddNode(output, outputTransaction, "QUANTITY", invoice.details[i].quantity.ToString());
                    helper.AddNode(output, outputTransaction, "PRICE", invoice.details[i].price.ToString());
                    helper.AddNode(output, outputTransaction, "TOTAL", invoice.details[i].total.ToString());
                    helper.AddNode(output, outputTransaction, "BILLED", "0");
                    helper.AddNode(output, outputTransaction, "DISPATCH_NUMBER", invoice.number);
                    helper.AddNode(output, outputTransaction, "VAT_RATE", invoice.details[i].vatRate.ToString());
                    helper.AddNode(output, outputTransaction, "SOURCEINDEX", invoice.wareHouseCode);
                    helper.AddNode(output, outputTransaction, "UNIT_CODE", helper.getUnit(invoice.details[i].unitCode));
                    //efaturalarda istiyor olabilri
                    helper.AddNode(output, outputTransaction, "UNIT_GLOBAL_CODE", "NIU");

                    if (invoice.type == 3)  // iade faturaları için
                    {
                        helper.AddNode(output, outputTransaction, "RET_COST_TYPE", "1");
                    }
                } 
            }

            string fileName = invoice.number + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xml";

            string saveFilePath = filePath+"\\" + fileName ;
            output.Save(saveFilePath);

        }

        public List<LogoInvoice> GetSelectedInvoices()
        {
            List<LogoInvoice> invoices = new List<LogoInvoice>();

            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    string number = row.Cells["number"].Value.ToString();
                    LogoInvoiceJson selectedInvoice = jsonInvoices.data.Where(inv => inv.number == number).FirstOrDefault();

                    LogoInvoice invoice = new LogoInvoice();
                    invoice.type = (int)selectedInvoice.invoiceType;
                    invoice.number = selectedInvoice.number;
                    invoice.documentNumber = selectedInvoice.documentNumber;
                    invoice.wareHouseCode = selectedInvoice.wareHouseCode;
                    invoice.customerCode = selectedInvoice.customerCode;
                    invoice.customerName = selectedInvoice.customerName;
                    invoice.date = selectedInvoice.date;
                    invoice.documentDate = selectedInvoice.documentDate;
                    invoice.deliveryDate = selectedInvoice.deliveryDate;
                    invoice.discountTotal = selectedInvoice.discountTotal;
                    invoice.preVatIncludedTotal = selectedInvoice.preVatIncludedTotal;
                    invoice.vatTotal = selectedInvoice.vatTotal;
                    invoice.grossTotal = selectedInvoice.grossTotal;
                    invoice.netTotal = selectedInvoice.netTotal;
                    invoice.paymentCode = selectedInvoice.paymentCode;
                    invoice.note = selectedInvoice.note;
                    invoice.salesmanCode = selectedInvoice.salesmanCode;
                    invoice.distributorBranchCode = selectedInvoice.distributorBranchCode;
                    invoice.customerBranchCode = selectedInvoice.customerBranchCode;
                    invoice.customerBranchName = selectedInvoice.customerBranchName;

                    List<InvoiceDetail> invoiceDetails = new List<model.InvoiceDetail>();
                    foreach (var selectedInvoiceDetail in selectedInvoice.details)
                    {
                        InvoiceDetail invDetail = new InvoiceDetail();
                        invDetail.type = 0;
                        invDetail.code = selectedInvoiceDetail.code;
                        invDetail.quantity = selectedInvoiceDetail.quantity;
                        invDetail.price = selectedInvoiceDetail.price;
                        invDetail.total = selectedInvoiceDetail.total;
                        invDetail.discountTotal = selectedInvoiceDetail.discountTotal;
                        invDetail.unitCode = selectedInvoiceDetail.unitCode;
                        invDetail.vatIncluded = selectedInvoiceDetail.vatIncluded;
                        invDetail.vatRate = selectedInvoiceDetail.vatRate;
                        invDetail.vatAmount = selectedInvoiceDetail.vatAmount;
                        invDetail.netTotal = selectedInvoiceDetail.netTotal;
                        invDetail.barcode = selectedInvoiceDetail.barcode;
                        invDetail.invoiceDetailLineOrder = selectedInvoiceDetail.invoiceDetailLineOrder;
                        invDetail.grossTotal = selectedInvoiceDetail.grossTotal;

                        List<InvoiceDetail> invoiceDetailDiscountDetails = new List<InvoiceDetail>();
                        foreach (var discount in selectedInvoiceDetail.discounts)
                        {
                            InvoiceDetail invDetailDiscountDetail = new InvoiceDetail();

                            invDetailDiscountDetail.type = 2;
                            invDetailDiscountDetail.rate = discount.rate;
                            invDetailDiscountDetail.discountTotal = discount.discountTotal;
                            invDetailDiscountDetail.price = invDetail.price;
                            invDetailDiscountDetail.grossTotal = invDetail.grossTotal;

                            invoiceDetailDiscountDetails.Add(invDetailDiscountDetail);
                        }

                        invoiceDetails.Add(invDetail);

                        if (invoiceDetailDiscountDetails.Count > 0)// discountlar da bir detay olarak eklendi ve bu detaylar invoice detail e eklendi
                        {
                            foreach (var invoiceDetailDiscountDetail in invoiceDetailDiscountDetails)
                            {
                                invoiceDetails.Add(invoiceDetailDiscountDetail);
                            }
                        }
                    }
                    invoice.details = invoiceDetails;
                    invoices.Add(invoice);
                }
            }
            return invoices;
        }
        public List<LogoWaybill> GetSelectedWaybills()
        {
            List<LogoWaybill> waybills = new List<LogoWaybill>();

            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    string number = row.Cells["number"].Value.ToString();
                    LogoWaybillJson selectedWaybill = jsonWaybills.data.Where(inv => inv.number == number).FirstOrDefault();

                    LogoWaybill waybill = new LogoWaybill();
                    waybill.type = (int)selectedWaybill.waybillType;
                    waybill.number = selectedWaybill.number;
                    waybill.documentNumber = selectedWaybill.documentNumber;
                    waybill.wareHouseCode = selectedWaybill.wareHouseCode;
                    waybill.customerCode = selectedWaybill.customerCode;
                    waybill.customerName = selectedWaybill.customerName;
                    waybill.date = selectedWaybill.date;
                    waybill.documentDate = selectedWaybill.documentDate;
                    waybill.deliveryDate = selectedWaybill.deliveryDate;
                    waybill.discountTotal = selectedWaybill.discountTotal;
                    waybill.preVatIncludedTotal = selectedWaybill.preVatIncludedTotal;
                    waybill.vatTotal = selectedWaybill.vatTotal;
                    waybill.grossTotal = selectedWaybill.grossTotal;
                    waybill.netTotal = selectedWaybill.netTotal;
                    waybill.paymentCode = selectedWaybill.paymentCode;
                    waybill.note = selectedWaybill.note;
                    waybill.salesmanCode = selectedWaybill.salesmanCode;
                    waybill.distributorBranchCode = selectedWaybill.distributorBranchCode;
                    waybill.customerBranchCode = selectedWaybill.customerBranchCode;
                    waybill.customerBranchName = selectedWaybill.customerBranchName;

                    List<WaybillDetail> waybillDetails = new List<WaybillDetail>();
                    foreach (var selectedWaybillDetail in selectedWaybill.details)
                    {
                        WaybillDetail waybillDetail = new WaybillDetail();
                        waybillDetail.type = 0;
                        waybillDetail.code = selectedWaybillDetail.code;
                        waybillDetail.quantity = selectedWaybillDetail.quantity;
                        waybillDetail.price = selectedWaybillDetail.price;
                        waybillDetail.total = selectedWaybillDetail.total;
                        waybillDetail.discountTotal = selectedWaybillDetail.discountTotal;
                        waybillDetail.unitCode = selectedWaybillDetail.unitCode;
                        waybillDetail.vatIncluded = selectedWaybillDetail.vatIncluded;
                        waybillDetail.vatRate = selectedWaybillDetail.vatRate;
                        waybillDetail.vatAmount = selectedWaybillDetail.vatAmount;
                        waybillDetail.netTotal = selectedWaybillDetail.netTotal;
                        waybillDetail.barcode = selectedWaybillDetail.barcode;
                        waybillDetail.waybillDetailLineOrder = selectedWaybillDetail.waybillDetailLineOrder;
                        waybillDetail.grossTotal = selectedWaybillDetail.grossTotal;

                        List<WaybillDetail> waybillDetailDiscountDetails = new List<WaybillDetail>();
                        foreach (var discount in selectedWaybillDetail.discounts)
                        {
                            WaybillDetail waybillDetailDiscountDetail = new WaybillDetail();

                            waybillDetailDiscountDetail.type = 2;
                            waybillDetailDiscountDetail.rate = discount.rate;
                            waybillDetailDiscountDetail.discountTotal = discount.discountTotal;
                            waybillDetailDiscountDetail.price = waybillDetail.price;
                            waybillDetailDiscountDetail.grossTotal = waybillDetail.grossTotal;

                            waybillDetailDiscountDetails.Add(waybillDetailDiscountDetail);
                        }

                        waybillDetails.Add(waybillDetail);

                        if (waybillDetailDiscountDetails.Count > 0)// discountlar da bir detay olarak eklendi ve bu detaylar invoice detail e eklendi
                        {
                            foreach (var waybillDetailDiscountDetail in waybillDetailDiscountDetails)
                            {
                                waybillDetails.Add(waybillDetailDiscountDetail);
                            }
                        }
                    }
                    waybill.details = waybillDetails;
                    waybills.Add(waybill);
                }
            }
            return waybills;
        }
        void Test()
        {

            string url = "http://172.16.40.17:9002";
            RestClient restClient = new RestClient(url);
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
            var con = requestResponse.Content;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            GenericResponse<List<LogoInvoice>> invResponse = JsonConvert.DeserializeObject<GenericResponse<List<LogoInvoice>>>(requestResponse.Content, settings);

            dataGridInvoice.DataSource = invResponse.data;

            List<LogoInvoice> testInvoices = new List<LogoInvoice>();
        }
        void CheckLogin()
        {
            Cursor.Current = Cursors.WaitCursor;
            isLoggedIn = unity.Login(logoUserName, logoPassword, int.Parse(companyCode), int.Parse(season));
            if (isLoggedIn)
            {
                lblLogoConnectionInfo.BackColor = System.Drawing.Color.Green;
                lblLogoConnectionInfo.Text = "Logo Bağlantısı BAŞARILI.";
                btnSendToLogo.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn && chkDispatch.Checked != true) ? true : false;
                btnCheckLogoConnection.Enabled = false;
            }
            else
            {
                lblLogoConnectionInfo.BackColor = System.Drawing.Color.Red;
                lblLogoConnectionInfo.Text = "Logo Bağlantısı BAŞARISIZ.";
                btnSendToLogo.Enabled = false;
                btnCheckLogoConnection.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
            //return isLoggedIn;
        }
        void FillGrid(List<LogoInvoiceJson> jsonData)
        {
            dataGridInvoice.Rows.Clear();
            foreach (var data in jsonData)
            {
                int n = dataGridInvoice.Rows.Add();
                dataGridInvoice.Rows[n].Cells[0].Value = "false";
                dataGridInvoice.Rows[n].Cells[1].Value = (helper.getInvoiceType((int)data.invoiceType)).ToString();
                dataGridInvoice.Rows[n].Cells[2].Value = data.number;
                dataGridInvoice.Rows[n].Cells[3].Value = data.date.ToShortDateString();
                dataGridInvoice.Rows[n].Cells[4].Value = data.documentNumber;
                dataGridInvoice.Rows[n].Cells[5].Value = data.customerCode;
                dataGridInvoice.Rows[n].Cells[6].Value = data.customerName;
                dataGridInvoice.Rows[n].Cells[7].Value = data.discountTotal.ToString();
                dataGridInvoice.Rows[n].Cells[8].Value = data.vatTotal.ToString();
                dataGridInvoice.Rows[n].Cells[9].Value = data.grossTotal.ToString();
            }
        }
        void FillGrid(List<LogoWaybillJson> jsonData)
        {
            dataGridInvoice.Rows.Clear();
            foreach (var data in jsonData)
            {
                int n = dataGridInvoice.Rows.Add();
                dataGridInvoice.Rows[n].Cells[0].Value = "false";
                dataGridInvoice.Rows[n].Cells[1].Value = (helper.getInvoiceTypeForWaybill((int)data.waybillType)).ToString();
                dataGridInvoice.Rows[n].Cells[2].Value = data.number;
                dataGridInvoice.Rows[n].Cells[3].Value = data.date.ToShortDateString();
                dataGridInvoice.Rows[n].Cells[4].Value = data.documentNumber;
                dataGridInvoice.Rows[n].Cells[6].Value = data.customerName;
                dataGridInvoice.Rows[n].Cells[7].Value = data.discountTotal.ToString();
                dataGridInvoice.Rows[n].Cells[8].Value = data.vatTotal.ToString();
                dataGridInvoice.Rows[n].Cells[9].Value = data.grossTotal.ToString();
            }
        }
        class GenericResponse<T>
        {
            public T data { get; set; }
            public int responseStatus { get; set; }
        }
        public void GetInvoices()
        {
            RestClient restClient = new RestClient(url);
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
            var con = requestResponse.Content;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            jsonInvoices = JsonConvert.DeserializeObject<GenericResponse<List<LogoInvoiceJson>>>(requestResponse.Content, settings);

            FillGrid(jsonInvoices.data);
        }
        public void GetWaybills()
        {
            RestClient restClient = new RestClient(url);
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
            var con = requestResponse.Content;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            jsonWaybills = JsonConvert.DeserializeObject<GenericResponse<List<LogoWaybillJson>>>(requestResponse.Content, settings);

            FillGrid(jsonWaybills.data);
        }
        void SendResponse(IntegratedInvoiceStatus integratedInvoices)
        {
            try
            {
                RestClient restClient = new RestClient(url);
                RestRequest restRequest = new RestRequest("/integration/invoices/sync-statuses?", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };

                restRequest.AddJsonBody(integratedInvoices);

                var requestResponse = restClient.Execute<StatusResponse>(restRequest);
                var con = requestResponse.Content;

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                GenericResponse<List<StatusResponse>> invResponse = JsonConvert.DeserializeObject<GenericResponse<List<StatusResponse>>>(requestResponse.Content, settings);

            }
            catch
            {
                MessageBox.Show("Fatura Logoya Aktarıldı Fakat salesArt ' taki durumu güncellenemedi.. ", "Fatura Statüsünün Gücellenememesi", MessageBoxButtons.OK);
            }

        }
        void SendResponse(IntegratedWaybillStatus integratedWaybills)
        {
            try
            {
                RestClient restClient = new RestClient(url);
                RestRequest restRequest = new RestRequest("/integration/waybills/sync-statuses?", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };

                restRequest.AddJsonBody(integratedWaybills);

                var requestResponse = restClient.Execute<StatusResponse>(restRequest);
                var con = requestResponse.Content;

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                GenericResponse<List<StatusResponse>> invResponse = JsonConvert.DeserializeObject<GenericResponse<List<StatusResponse>>>(requestResponse.Content, settings);

            }
            catch
            {
                MessageBox.Show("İrsaliye Logoya Aktarıldı Fakat salesArt ' taki durumu güncellenemedi.. ", "İrsaliye Statüsünün Gücellenememesi", MessageBoxButtons.OK);
            }

        }
        public IntegratedInvoiceStatus sendMultipleInvoice(List<LogoInvoice> invoices)
        {
            //1-Invoice listesi boş mu kontrol edilmeli 
            string remoteInvoiceNumber = "";
            string message = "";
            string remoteInvoiceStatus = "";

            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();

            // invoices.Where(inv => inv.number.Contains() )

            try
            {
                if (isLoggedIn)
                {
                    foreach (var invoice in invoices)
                    {  // boolean dön 
                        Data newInvoice = unity.NewDataObject(DataObjectType.doSalesInvoice);                       

                        remoteInvoiceNumber = reader.getInvoiceNumberByDocumentNumber(invoice.number);  // salesArttaki invoice number , logoda documentNumber alanına yazılıyor.
                        if (remoteInvoiceNumber != "")
                        {
                            IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(invoice.number + " belge numaralı fatura, sistemde zaten mevcut. Kontrol Ediniz" , invoice.number, remoteInvoiceNumber, true);
                            receivedInvoices.Add(recievedInvoice);
                        }
                        else
                        {
                            //8 satış , 3 Satış iade ,9 verilen hizmet
                            if (invoice.type == 8 || invoice.type == 3 || invoice.type == 9)
                            {
                                newInvoice = unity.NewDataObject(DataObjectType.doSalesInvoice);
                            }
                            else
                            {
                                newInvoice = unity.NewDataObject(DataObjectType.doPurchInvoice);
                            }
                            newInvoice.New();
                            newInvoice.DataFields.FieldByName("TYPE").Value = invoice.type; 

                            if(useDefaultNumber)
                            {
                                newInvoice.DataFields.FieldByName("NUMBER").Value = "~";
                            }
                            else
                            {
                                newInvoice.DataFields.FieldByName("NUMBER").Value = invoice.number; // düzenlecek
                            }
                            
                            newInvoice.DataFields.FieldByName("DOC_NUMBER").Value =  invoice.documentNumber;

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
                            }

                            if (useCypheCode)
                            {
                                newInvoice.DataFields.FieldByName("AUTH_CODE").Value = cypheCode; 
                            }


                            //newInvoice.DataFields.FieldByName("POST_FLAGS").Value = 241;
                            newInvoice.DataFields.FieldByName("DIVISION").Value = invoice.distributorBranchCode;
                            newInvoice.DataFields.FieldByName("TOTAL_DISCOUNTS").Value = invoice.discountTotal;
                            newInvoice.DataFields.FieldByName("TOTAL_DISCOUNTED").Value = invoice.netTotal - invoice.discountTotal;
                            newInvoice.DataFields.FieldByName("ADD_DISCOUNTS").Value = invoice.discountTotal;

                            newInvoice.DataFields.FieldByName("TOTAL_VAT").Value = invoice.vatTotal;
                            newInvoice.DataFields.FieldByName("TOTAL_GROSS").Value = invoice.grossTotal;
                            newInvoice.DataFields.FieldByName("TOTAL_NET").Value = invoice.netTotal;
                            newInvoice.DataFields.FieldByName("NOTES1").Value = "ST Notu: "+ invoice.note + " Sevk :"+ invoice.customerBranchCode +"_"+ invoice.customerBranchName ;
                            newInvoice.DataFields.FieldByName("TC_NET").Value = invoice.netTotal;
                            newInvoice.DataFields.FieldByName("SINGLE_PAYMENT").Value = invoice.netTotal;
                            newInvoice.DataFields.FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;
                            newInvoice.DataFields.FieldByName("SALESMAN_CODE").Value = invoice.salesmanCode;


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
                                //dispatches_lines[1].FieldByName("ADD_DISCOUNTS").Value = 160.09;

                                dispatches_lines[0].FieldByName("TOTAL_DISCOUNTS").Value = invoice.discountTotal;
                                dispatches_lines[0].FieldByName("TOTAL_DISCOUNTED").Value = invoice.netTotal - invoice.discountTotal;
                                dispatches_lines[0].FieldByName("ADD_DISCOUNTS").Value = invoice.discountTotal;

                                dispatches_lines[0].FieldByName("TOTAL_VAT").Value = invoice.vatTotal;
                                dispatches_lines[0].FieldByName("TOTAL_GROSS").Value = invoice.grossTotal;
                                dispatches_lines[0].FieldByName("TOTAL_NET").Value = invoice.netTotal;
                                dispatches_lines[0].FieldByName("NOTES1").Value = "ST Notu: " + invoice.note + " Sevk :" + invoice.customerBranchCode + "_" + invoice.customerBranchName;
                                //dispatches_lines[0].FieldByName("TC_NET").Value = invoice.netTotal;
                                //dispatches_lines[0].FieldByName("SINGLE_PAYMENT").Value = invoice.netTotal;
                                //dispatches_lines[0].FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;
                                //dispatches_lines[0].FieldByName("SALESMAN_CODE").Value = invoice.salesmanCode;
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
                                        //newInvoiceLines[i].FieldByName("MASTER_CODE").Value = "";
                                        //newInvoiceLines[i].FieldByName("DETAIL_LEVEL").Value = 1;
                                        newInvoiceLines[i].FieldByName("QUANTITY").Value = 0;
                                        newInvoiceLines[i].FieldByName("TOTAL").Value = Convert.ToDouble(detail.discountTotal); 
                                        newInvoiceLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble(Math.Round(Convert.ToDecimal((100 * Convert.ToDouble(detail.discountTotal)) / Convert.ToDouble(detail.grossTotal)),2));
                                        newInvoiceLines[i].FieldByName("BASE_AMOUNT").Value = Convert.ToDouble(invoice.netTotal);
                                        //newInvoiceLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.price);

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
                                                string productDetailMessage =  detail.code + " Kodlu Ürün Logoda Bulunamadı "; 
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
                                                string productDetailMessage = detail.barcode + " Barkodlu Ürün Logoda bulunamadı .." ; 
                                                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(productDetailMessage, invoice.number, remoteInvoiceNumber, false);
                                                receivedInvoices.Add(recievedInvoice); 
                                                integratedInvoices.integratedInvoices = receivedInvoices;
                                                integratedInvoices.distributorId = distributorId;
                                                return integratedInvoices;
                                            }
                                        } 
                                        else
                                            newInvoiceLines[i].FieldByName("MASTER_CODE").Value = detail.code;


                                        newInvoiceLines[i].FieldByName("SOURCEINDEX").Value = invoice.wareHouseCode;
                                        newInvoiceLines[i].FieldByName("SOURCECOSTGRP").Value = invoice.wareHouseCode;
                                        newInvoiceLines[i].FieldByName("QUANTITY").Value = detail.quantity;
                                        newInvoiceLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.price);
                                        newInvoiceLines[i].FieldByName("TOTAL").Value = detail.total;
                                        newInvoiceLines[i].FieldByName("CURR_PRICE").Value = 160;  // currency TL
                                        //newInvoiceLines[i].FieldByName("UNIT_CODE").Value = "AD";
                                        newInvoiceLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit(detail.unitCode);
                                        newInvoiceLines[i].FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;

                                        //newInvoiceLines[i].FieldByName("UNIT_CONV1").Value = 1; //adet carpanı
                                        //newInvoiceLines[i].FieldByName("UNIT_CONV2").Value = 12;  // koli carpanı
                                        newInvoiceLines[i].FieldByName("VAT_RATE").Value = Convert.ToInt32(detail.vatRate);
                                        newInvoiceLines[i].FieldByName("VAT_AMOUNT").Value = detail.vatAmount;
                                        newInvoiceLines[i].FieldByName("VAT_BASE").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                        newInvoiceLines[i].FieldByName("TOTAL_NET").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                        newInvoiceLines[i].FieldByName("SALEMANCODE").Value = invoice.salesmanCode;
                                        newInvoiceLines[i].FieldByName("MONTH").Value = DateTime.Now.Month;
                                        newInvoiceLines[i].FieldByName("YEAR").Value = DateTime.Now.Year;
                                        //newInvoiceLines[i].FieldByName("EDT_CURR").Value = 1;
                                        //newInvoiceLines[i].FieldByName("UNIT_GLOBAL_CODE").Value = "NIU";
                                        newInvoiceLines[i].FieldByName("BARCODE").Value = detail.barcode;

                                        if (invoice.type == 3 || invoice.type == 8 || invoice.type == 9) // satış , satış iade ve verilen hizmet ise satış fiyatı üzerinden çalışsın denildi
                                        {

                                            newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 2;
                                        }
                                        else
                                        {
                                            newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 1;
                                        }

                                        if (invoice.type == 3)  // iade faturaları 
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
                            
                            //newInvoice.DataFields.FieldByName("EARCHIVEDETR_INTPAYMENTTYPE").Value = 0;
                            //newInvoice.DataFields.FieldByName("EBOOK_DOCDATE").Value = "06.07.2015";
                            //newInvoice.DataFields.FieldByName("EBOOK_DOCNR").Value = "1234";
                            //newInvoice.DataFields.FieldByName("EBOOK_DOCTYPE").Value = 5;
                            //newInvoice.DataFields.FieldByName("EBOOK_PAYTYPE").Value = "COKSECMELI";
                            //newInvoice.DataFields.FieldByName("EBOOK_NOPAY").Value = 1;

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
            string remoteDespatcheStatus = "";

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
                                        //newInvoiceLines[i].FieldByName("MASTER_CODE").Value = "";
                                        //newInvoiceLines[i].FieldByName("DETAIL_LEVEL").Value = 1;
                                        newWaybillLines[i].FieldByName("QUANTITY").Value = 0;
                                        newWaybillLines[i].FieldByName("TOTAL").Value = Convert.ToDouble(detail.discountTotal);
                                        newWaybillLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble((100 * Convert.ToDouble(detail.discountTotal)) / Convert.ToDouble(detail.grossTotal));
                                        newWaybillLines[i].FieldByName("BASE_AMOUNT").Value = Convert.ToDouble(despatch.netTotal);
                                        //newInvoiceLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.price);

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
        public IntegratedInvoiceStatus xmlExportWithLObjects(List<LogoInvoice> invoices)
        {
            string remoteInvoiceNumber = "";
            string message = "";
            string remoteInvoiceStatus = "";

            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();

            unity.Login(logoUserName, logoPassword, int.Parse(companyCode), int.Parse(season));

            try
            {
                foreach (var invoice in invoices)
                {
                    Data newInvoice = new Data();

                    //8 satış , 3 Satış iade ,9 verilen hizmet
                    if (invoice.type == 8 || invoice.type == 3 || invoice.type == 9)
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
                        newInvoice.DataFields.FieldByName("NUMBER").Value = invoice.number;
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
                    }

                    if (useCypheCode)
                    {
                        newInvoice.DataFields.FieldByName("AUTH_CODE").Value = cypheCode;
                    }

                    //newInvoice.DataFields.FieldByName("POST_FLAGS").Value = 241;
                    newInvoice.DataFields.FieldByName("DIVISION").Value = invoice.distributorBranchCode;
                    newInvoice.DataFields.FieldByName("TOTAL_DISCOUNTS").Value = invoice.discountTotal;
                    newInvoice.DataFields.FieldByName("TOTAL_DISCOUNTED").Value = invoice.netTotal - invoice.discountTotal;
                    newInvoice.DataFields.FieldByName("ADD_DISCOUNTS").Value = invoice.discountTotal;

                    newInvoice.DataFields.FieldByName("TOTAL_VAT").Value = invoice.vatTotal;
                    newInvoice.DataFields.FieldByName("TOTAL_GROSS").Value = invoice.grossTotal;
                    newInvoice.DataFields.FieldByName("TOTAL_NET").Value = invoice.netTotal;
                    newInvoice.DataFields.FieldByName("NOTES1").Value = "ST Notu: " + invoice.note + " Sevk :" + invoice.customerBranchCode + "_" + invoice.customerBranchName;
                    newInvoice.DataFields.FieldByName("TC_NET").Value = invoice.netTotal;
                    newInvoice.DataFields.FieldByName("SINGLE_PAYMENT").Value = invoice.netTotal;
                    newInvoice.DataFields.FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;
                    newInvoice.DataFields.FieldByName("SALESMAN_CODE").Value = invoice.salesmanCode;


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
                        //dispatches_lines[1].FieldByName("ADD_DISCOUNTS").Value = 160.09;

                        dispatches_lines[0].FieldByName("TOTAL_DISCOUNTS").Value = invoice.discountTotal;
                        dispatches_lines[0].FieldByName("TOTAL_DISCOUNTED").Value = invoice.netTotal - invoice.discountTotal;
                        dispatches_lines[0].FieldByName("ADD_DISCOUNTS").Value = invoice.discountTotal;

                        dispatches_lines[0].FieldByName("TOTAL_VAT").Value = invoice.vatTotal;
                        dispatches_lines[0].FieldByName("TOTAL_GROSS").Value = invoice.grossTotal;
                        dispatches_lines[0].FieldByName("TOTAL_NET").Value = invoice.netTotal;
                        dispatches_lines[0].FieldByName("NOTES1").Value = "ST Notu: " + invoice.note + " Sevk :" + invoice.customerBranchCode + "_" + invoice.customerBranchName;
                        //dispatches_lines[0].FieldByName("TC_NET").Value = invoice.netTotal;
                        //dispatches_lines[0].FieldByName("SINGLE_PAYMENT").Value = invoice.netTotal;
                        //dispatches_lines[0].FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;
                        //dispatches_lines[0].FieldByName("SALESMAN_CODE").Value = invoice.salesmanCode;
                    }
                    Lines newInvoiceLines = newInvoice.DataFields.FieldByName("TRANSACTIONS").Lines;

                    for (int i = 0; i < invoice.details.Count; i++)
                    {
                        if (newInvoiceLines.AppendLine())
                        {
                            InvoiceDetail detail = invoice.details[i];
                            if (detail.type == 2)  // indirim
                            {
                                newInvoiceLines[i].FieldByName("TYPE").Value = detail.type;
                                //newInvoiceLines[i].FieldByName("MASTER_CODE").Value = "";
                                //newInvoiceLines[i].FieldByName("DETAIL_LEVEL").Value = 1;
                                newInvoiceLines[i].FieldByName("QUANTITY").Value = 0;
                                newInvoiceLines[i].FieldByName("TOTAL").Value = Convert.ToDouble(detail.discountTotal);
                                newInvoiceLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble(Math.Round(Convert.ToDecimal((100 * Convert.ToDouble(detail.discountTotal)) / Convert.ToDouble(detail.grossTotal)), 2));
                                newInvoiceLines[i].FieldByName("BASE_AMOUNT").Value = Convert.ToDouble(invoice.netTotal);
                                //newInvoiceLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.price);

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
                                        IntegratedInvoiceDto unRecievedInvoice = new IntegratedInvoiceDto(productDetailMessage, invoice.number, remoteInvoiceNumber, false);
                                        receivedInvoices.Add(unRecievedInvoice);
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
                                        IntegratedInvoiceDto unRecievedInvoice = new IntegratedInvoiceDto(productDetailMessage, invoice.number, remoteInvoiceNumber, false);
                                        receivedInvoices.Add(unRecievedInvoice);
                                        integratedInvoices.integratedInvoices = receivedInvoices;
                                        integratedInvoices.distributorId = distributorId;
                                        return integratedInvoices;
                                    }
                                }
                                else
                                    newInvoiceLines[i].FieldByName("MASTER_CODE").Value = detail.code;


                                newInvoiceLines[i].FieldByName("SOURCEINDEX").Value = invoice.wareHouseCode;
                                newInvoiceLines[i].FieldByName("SOURCECOSTGRP").Value = invoice.wareHouseCode;
                                newInvoiceLines[i].FieldByName("QUANTITY").Value = detail.quantity;
                                newInvoiceLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.price);
                                newInvoiceLines[i].FieldByName("TOTAL").Value = detail.total;
                                newInvoiceLines[i].FieldByName("CURR_PRICE").Value = 160;  // currency TL
                                                                                           //newInvoiceLines[i].FieldByName("UNIT_CODE").Value = "AD";
                                newInvoiceLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit(detail.unitCode);
                                newInvoiceLines[i].FieldByName("PAYMENT_CODE").Value = invoice.paymentCode;

                                //newInvoiceLines[i].FieldByName("UNIT_CONV1").Value = 1; //adet carpanı
                                //newInvoiceLines[i].FieldByName("UNIT_CONV2").Value = 12;  // koli carpanı
                                newInvoiceLines[i].FieldByName("VAT_RATE").Value = Convert.ToInt32(detail.vatRate);
                                newInvoiceLines[i].FieldByName("VAT_AMOUNT").Value = detail.vatAmount;
                                newInvoiceLines[i].FieldByName("VAT_BASE").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                newInvoiceLines[i].FieldByName("TOTAL_NET").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                newInvoiceLines[i].FieldByName("SALEMANCODE").Value = invoice.salesmanCode;
                                newInvoiceLines[i].FieldByName("MONTH").Value = DateTime.Now.Month;
                                newInvoiceLines[i].FieldByName("YEAR").Value = DateTime.Now.Year;
                                //newInvoiceLines[i].FieldByName("EDT_CURR").Value = 1;
                                //newInvoiceLines[i].FieldByName("UNIT_GLOBAL_CODE").Value = "NIU";
                                newInvoiceLines[i].FieldByName("BARCODE").Value = detail.barcode;

                                if (invoice.type == 3 || invoice.type == 8 || invoice.type == 9) // satış , satış iade ve verilen hizmet ise satış fiyatı üzerinden çalışsın denildi
                                {

                                    newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 2;
                                }
                                else
                                {
                                    newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 1;
                                }

                                if (invoice.type == 3)  // iade faturaları 
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

                    //newInvoice.DataFields.FieldByName("EARCHIVEDETR_INTPAYMENTTYPE").Value = 0;
                    //newInvoice.DataFields.FieldByName("EBOOK_DOCDATE").Value = "06.07.2015";
                    //newInvoice.DataFields.FieldByName("EBOOK_DOCNR").Value = "1234";
                    //newInvoice.DataFields.FieldByName("EBOOK_DOCTYPE").Value = 5;
                    //newInvoice.DataFields.FieldByName("EBOOK_PAYTYPE").Value = "COKSECMELI";
                    //newInvoice.DataFields.FieldByName("EBOOK_NOPAY").Value = 1;

                    newInvoice.FillAccCodes();
                    newInvoice.CreateCompositeLines();
                    newInvoice.ReCalculate();


                    ValidateErrors err = newInvoice.ValidateErrors;

                    helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");

                    string fileName = invoice.number + "_" + DateTime.Now.ToShortDateString() + ".xml";

                    if (invoice.type == 8 || invoice.type == 3 || invoice.type == 9)
                        newInvoice.ExportToXML("SALES_INVOICES", filePath + "\\" + fileName);
                    else
                        newInvoice.ExportToXML("PURCHASE_INVOICES", filePath + "\\" + fileName);

                    IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(message, invoice.number, invoice.number, true);
                    receivedInvoices.Add(recievedInvoice);

                    helper.LogFile("POST Bitti", "-", "-", "-", "-");
                }
            }
            catch (Exception ex)
            {
                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(ex.Message.ToString(), "", "", false);
                receivedInvoices.Add(recievedInvoice);
            } 
            finally
            {
                unity.UserLogout();
                unity.Disconnect();
            }

            integratedInvoices.integratedInvoices = receivedInvoices;
            integratedInvoices.distributorId = distributorId;

            return integratedInvoices;
        }
        public IntegratedInvoiceStatus xmlExport(List<LogoInvoice> invoices)
        {
            string remoteInvoiceNumber = "";
            string message = "";
            string remoteInvoiceStatus = "";

            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();
             

            try
            {
                foreach (var invoice in invoices)
                {
                    createXML(invoice);
                    IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(message, invoice.number, invoice.number, true);
                    receivedInvoices.Add(recievedInvoice);
                }
            }
            catch (Exception ex)
            {
                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(ex.Message.ToString(), "", "", false);
                receivedInvoices.Add(recievedInvoice);
            }

            integratedInvoices.integratedInvoices = receivedInvoices;
            integratedInvoices.distributorId = distributorId;

            return integratedInvoices;
        }
        private void btnCheckLogoConnection_Click(object sender, EventArgs e)
        {
            helper.LogFile("Login Kontolü Basladı", "-", "-", "-", "-");
            CheckLogin();
            helper.LogFile("Login Kontolü Bitti", "-", "-", "-", "-");
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isLoggedIn)
            {
                unity.UserLogout();
                unity.Disconnect();
            }
        }
        private void btnGetInvoices_Click(object sender, EventArgs e)
        { 
            Cursor.Current = Cursors.WaitCursor;
            dataGridInvoice.Rows.Clear();
            chkSelectAll.Checked = false;
            if (chkDispatch.Checked)
                GetWaybills();
            else
                GetInvoices();

            btnSendToLogo.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn) ? true : false;
            btnCheckLogoConnection.Enabled = (dataGridInvoice.Rows.Count > 0 && !isLoggedIn) ? true : false;
            Cursor.Current = Cursors.Default;
        }
        private void btnSendToLogo_Click(object sender, EventArgs e)
        { 
            int selectedInvoiceCount = 0;
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    selectedInvoiceCount += 1;
                }
            }
            if (selectedInvoiceCount > 0)
            {
                List<LogoInvoice> selectedInvoices = GetSelectedInvoices();
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
                //IntegratedInvoiceStatus status = sendMultipleDespatch(selectedInvoices);//sendMultipleInvoice(selectedInvoices);
                IntegratedInvoiceStatus status = sendMultipleInvoice(selectedInvoices);
                SendResponse(status);
                helper.ShowMessages(status);
                helper.LogFile("Fatura Aktarım Bitti", "-", "-", "-", "-");
                dataGridInvoice.Rows.Clear();
                btnSendToLogo.Enabled = false;
                btnCheckLogoConnection.Enabled = false;
                lblLogoConnectionInfo.Text = "";

                Cursor.Current = Cursors.Default;
            }
            else MessageBox.Show("Fatura Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
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
                    invoiceType = InvoiceType.SELLING;
                    break;
                case 1:
                    invoiceType = InvoiceType.DAMAGED_SELLING_RETURN;
                    break;
                case 2:
                    invoiceType = InvoiceType.SELLING_RETURN;
                    break;
                case 3:
                    invoiceType = InvoiceType.SELLING_SERVICE;
                    break;
                case 4:
                    invoiceType = InvoiceType.BUYING_SERVICE;
                    break;
                case 5:
                    invoiceType = InvoiceType.BUYING;
                    break;
                case 6:
                    invoiceType = InvoiceType.DAMAGED_BUYING_RETURN;
                    break;
                case 7:
                    invoiceType = InvoiceType.BUYING_RETURN;
                    break;
            }
        }
        private void cmbDispatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbDispatch.SelectedIndex)
            {
                case 0:
                    invoiceType = InvoiceType.SELLING;
                    break;
                case 1:
                    invoiceType = InvoiceType.DAMAGED_SELLING_RETURN;
                    break;
                case 2:
                    invoiceType = InvoiceType.SELLING_RETURN;
                    break;
                case 3:
                    invoiceType = InvoiceType.BUYING;
                    break;
                case 4:
                    invoiceType = InvoiceType.DAMAGED_BUYING_RETURN;
                    break;
                case 5:
                    invoiceType = InvoiceType.BUYING_RETURN;
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
                lblDispatch.Visible = true;
                cmbDispatch.Enabled = true;
                lblDispatch.Enabled = true;
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
                lblDispatch.Enabled = false;
                btnWaybill.Enabled = false;
                cmbInvoice.SelectedIndex = 0;
                cmbInvoice_SelectedIndexChanged(0, EventArgs.Empty);
            }
        }
        private void btnWaybill_Click(object sender, EventArgs e)
        {
            int selectedWaybillCount = 0;
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    selectedWaybillCount += 1;
                }
            }
            if (selectedWaybillCount > 0)
            {
                List<LogoWaybill> selectedWaybills = GetSelectedWaybills();
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("İrsaliye Aktarım Basladı", "-", "-", "-", "-");
                IntegratedWaybillStatus status = sendMultipleDespatch(selectedWaybills);//sendMultipleInvoice(selectedInvoices);
                //IntegratedInvoiceStatus status = sendMultipleInvoice(selectedInvoices);
                SendResponse(status);
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

        private void btnXML_Click(object sender, EventArgs e)
        { 
            int selectedInvoiceCount = 0;
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    selectedInvoiceCount += 1;
                }
            }
            if (selectedInvoiceCount > 0)
            {
                SaveFileDialog fd = new SaveFileDialog();
                fd.Title = "Faturaların Kaydedileceği Yeri Seçin";
                fd.FileName = "_";
                fd.ShowDialog();
                filePath = Path.GetDirectoryName(fd.FileName);

                List<LogoInvoice> selectedInvoices = GetSelectedInvoices();
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
                //IntegratedInvoiceStatus status = xmlExportWithLObjects(selectedInvoices);
                IntegratedInvoiceStatus status = xmlExport(selectedInvoices);
                SendResponse(status);
                helper.ShowMessages(status);
                helper.LogFile("Fatura Aktarım Bitti", "-", "-", "-", "-");
                dataGridInvoice.Rows.Clear();
                Cursor.Current = Cursors.Default;
            }
            else MessageBox.Show("Fatura Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
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
    }

}


