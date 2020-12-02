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
using invoiceIntegration.model.order;

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
        bool XMLTransferForOrder = Configuration.getXMLTransferForOrder(); 
        int distributorId = Configuration.getDistributorId();
        bool useDispatch = Configuration.getUseDispatch();
        bool integrationForMikroERP = Configuration.getIntegrationForMikroERP();
        string shipAgentCode = Configuration.getShipAgentCode();
        string url = Configuration.getUrl();
        string campaignLineNo = Configuration.getCampaignLineNo();
        bool orderTransferToLogoInfo = Configuration.getOrderTransferToLogoInfo();

        IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
        IntegratedWaybillStatus integratedWaybills = new IntegratedWaybillStatus();
        IntegratedOrderStatus integratedOrders = new IntegratedOrderStatus();
        IntegratedOrderStatusForMessage integratedOrderStatusForMessage = new IntegratedOrderStatusForMessage();

        GenericResponse<List<LogoInvoiceJson>> jsonInvoices = new GenericResponse<List<LogoInvoiceJson>>();
        GenericResponse<List<LogoWaybillJson>> jsonWaybills = new GenericResponse<List<LogoWaybillJson>>();
        GenericResponse<OrderResponse> jsonOrders = new GenericResponse<OrderResponse>();
        LogoDataReader reader = new LogoDataReader();
        UnityApplication unity = LogoApplication.getApplication();
        List<Discount> discounts = new List<Discount>();
        Helper helper = new Helper();

        bool isLoggedIn = false;
        string filePath = "";
        string invoiceType;

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
                btnSendOrderToLogo.Visible = false;
                btnXML.Visible = true;
            }

            if(integrationForMikroERP)
            {
                btnSendToLogo.Text = "Faturaları Mikroya Aktar";
                isLoggedIn = true;
            }

            if(orderTransferToLogoInfo)
            {
                btnSendOrderToLogo.Visible = true;
                btnSendToLogo.Visible = false;
                btnXML.Visible = false;
                cmbInvoice.Items.Clear();
                cmbInvoice.Items.Add("Satış Siparişleri");
                cmbInvoice.SelectedIndex = 0;
            }
        }

        void createXMLforInvoice(LogoInvoice invoice)
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
            
            
            helper.AddNode(output, outputInvoiceDbop, "TYPE", invoice.type.ToString());

            if (useDefaultNumber)
            {
                helper.AddNode(output, outputInvoiceDbop, "NUMBER", "~");
            }
            else
            {
                helper.AddNode(output, outputInvoiceDbop, "NUMBER", invoice.number); 
            }
            

            if (useShortDate)
            {
                date =(invoice.date.ToShortDateString());
            }
            else
            {
                date = (invoice.date.ToString("dd.MM.yyyy"));
            }

            helper.AddNode(output, outputInvoiceDbop, "DATE", date);
            helper.AddNode(output, outputInvoiceDbop, "TIME", helper.Hour(invoice.date).ToString());
            helper.AddNode(output, outputInvoiceDbop, "DOC_NUMBER", invoice.documentNumber);
            helper.AddNode(output, outputInvoiceDbop, "DOC_DATE", invoice.documentDate.ToString("dd.MM.yyyy"));
            helper.AddNode(output, outputInvoiceDbop, "ARP_CODE", invoice.customerCode);
            helper.AddNode(output, outputInvoiceDbop, "SHIPPING_AGENT", shipAgentCode);

            if (useCypheCode)
                helper.AddNode(output, outputInvoiceDbop, "AUTH_CODE", cypheCode);
              
            if (useShipCode)
            {
                helper.AddNode(output, outputInvoiceDbop, "SHIPLOC_CODE", invoice.customerBranchCode);
                helper.AddNode(output, outputInvoiceDbop, "SHIPLOC_DEF", invoice.customerBranchName);
            }

            helper.AddNode(output, outputInvoiceDbop, "TOTAL_DISCOUNTS", invoice.discountTotal.ToString().Replace(",","."));  // indirim toplamı
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_DISCOUNTED", (invoice.grossTotal).ToString().Replace(",", "."));  // toplam tutar
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_VAT", invoice.vatTotal.ToString().Replace(",", "."));  // Toplam Kdv
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_GROSS", invoice.grossTotal.ToString().Replace(",", ".")); // brüt tutar
            helper.AddNode(output, outputInvoiceDbop, "TOTAL_NET", invoice.netTotal.ToString().Replace(",", "."));  // Kdv hariç tutar
            helper.AddNode(output, outputInvoiceDbop, "NOTES1", invoice.note);  

            //zorunlu değil
            helper.AddNode(output, outputInvoiceDbop, "DIVISION", invoice.distributorBranchCode);
            helper.AddNode(output, outputInvoiceDbop, "DEPARTMENT", helper.getDepartment());
            helper.AddNode(output, outputInvoiceDbop, "AUXIL_CODE", invoice.salesmanCode);
            helper.AddNode(output, outputInvoiceDbop, "SOURCE_WH", invoice.wareHouseCode);
            helper.AddNode(output, outputInvoiceDbop, "SOURCE_COST_GRP", invoice.wareHouseCode);
            // 
             
            helper.AddNode(output, outputInvoiceDbop, "SALESMAN_CODE", invoice.salesmanCode);  // salesman 

            #region dispatch
             
            outputDispatches = output.CreateNode(XmlNodeType.Element, "DISPATCHES", "");
            outputInvoiceDbop.AppendChild(outputDispatches);
            outputDispatch = output.CreateNode(XmlNodeType.Element, "DISPATCH", "");
            outputDispatches.AppendChild(outputDispatch);

            helper.AddNode(output, outputDispatch, "TYPE", invoice.type.ToString());
            helper.AddNode(output, outputDispatch, "NUMBER", invoice.number);

            if (useShortDate)
            {
                date = (invoice.date.ToShortDateString());
            }
            else
            {
                date = (invoice.date.ToString("dd.MM.yyyy"));
            }
            helper.AddNode(output, outputDispatch, "DATE", date);
            helper.AddNode(output, outputDispatch, "TIME", helper.Hour(invoice.date).ToString());
            helper.AddNode(output, outputDispatch, "DOC_NUMBER", invoice.number);
            helper.AddNode(output, outputDispatch, "DOC_DATE", invoice.documentDate.ToString("dd.MM.yyyy"));
            helper.AddNode(output, outputDispatch, "SHIPPING_AGENT", shipAgentCode); 
            helper.AddNode(output, outputDispatch, "SHIP_DATE", invoice.date.AddDays(2).ToString("dd.MM.yyyy")); 
            helper.AddNode(output, outputDispatch, "SHIP_TIME", helper.Hour(invoice.date.AddDays(2)).ToString());
            helper.AddNode(output, outputDispatch, "DISP_STATUS", "1");

            if (useCypheCode)
                helper.AddNode(output, outputDispatch, "AUTH_CODE", cypheCode);

            helper.AddNode(output, outputDispatch, "ARP_CODE", invoice.customerCode);

            helper.AddNode(output, outputDispatch, "TOTAL_DISCOUNTS", invoice.discountTotal.ToString().Replace(",", "."));  // indirim toplamı
            helper.AddNode(output, outputDispatch, "TOTAL_DISCOUNTED", (invoice.grossTotal).ToString().Replace(",", "."));  // toplam tutar
            helper.AddNode(output, outputDispatch, "TOTAL_GROSS", invoice.grossTotal.ToString().Replace(",", ".")); // brüt tutar
            helper.AddNode(output, outputDispatch, "TOTAL_NET", invoice.netTotal.ToString().Replace(",", "."));  // Kdv hariç tutar
            helper.AddNode(output, outputDispatch, "TOTAL_VAT", invoice.vatTotal.ToString().Replace(",", "."));  // Toplam Kdv
            helper.AddNode(output, outputDispatch, "NOTES1", invoice.note); 
            helper.AddNode(output, outputDispatch, "ORIG_NUMBER", invoice.number); 
            helper.AddNode(output, outputDispatch, "DOC_DATE", invoice.documentDate.ToString("dd.MM.yyyy")); 
            helper.AddNode(output, outputDispatch, "DOC_TIME", helper.Hour(invoice.documentDate).ToString());
            #endregion

            outputTransactions = output.CreateNode(XmlNodeType.Element, "TRANSACTIONS", "");
            outputInvoiceDbop.AppendChild(outputTransactions);

            for (int i = 0; i < invoice.details.Count; i++)
            {

                outputTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
                outputTransactions.AppendChild(outputTransaction);

                if (invoice.details[i].type == 2)
                {
                     //1discounts 
                    if (invoice.details[i].rate > Convert.ToDecimal(0) && invoice.details[i].rate < Convert.ToDecimal(100))
                    {
                        helper.AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                        helper.AddNode(output, outputTransaction, "BILLED", "1"); 
                        helper.AddNode(output, outputTransaction, "DISCOUNT_RATE", Convert.ToDouble(Math.Round(invoice.details[i].rate, 2)).ToString().Replace(",", "."));
                        helper.AddNode(output, outputTransaction, "DISPATCH_NUMBER", invoice.number);
                        helper.AddNode(output, outputTransaction, "DESCRIPTION", invoice.details[i].name);
                        helper.AddNode(output, outputTransaction, "SOURCEINDEX", invoice.wareHouseCode);
                        helper.AddNode(output, outputTransaction, "SOURCECOSTGRP", invoice.wareHouseCode);
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
                    helper.AddNode(output, outputTransaction, "PRICE", Math.Round(invoice.details[i].price,2).ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "TOTAL", Math.Round(invoice.details[i].grossTotal,2).ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "BILLED", "1");
                    helper.AddNode(output, outputTransaction, "DISPATCH_NUMBER", invoice.number);
                    helper.AddNode(output, outputTransaction, "VAT_RATE", invoice.details[i].vatRate.ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "SOURCEINDEX", invoice.wareHouseCode);
                    helper.AddNode(output, outputTransaction, "PAYMENT_CODE", invoice.paymentCode);
                    helper.AddNode(output, outputTransaction, "UNIT_CODE", helper.getUnit(invoice.details[i].unitCode));
                    // efaturalarda istiyor olabilri
                    // helper.AddNode(output, outputTransaction, "UNIT_GLOBAL_CODE", "NIU");

                    if (invoice.type == 3)  // iade faturaları için
                    {
                        helper.AddNode(output, outputTransaction, "RET_COST_TYPE", "1");
                    } 
                    
                } 
            } 

            helper.AddNode(output, outputInvoiceDbop, "EINVOICE", invoice.ebillCustomer ? "1" : "0" );
            //helper.AddNode(output, outputInvoiceDbop, "PROFILE_ID", invoice.isElectronicInvoiceCustomer ? "1" : "0");

            string fileName = invoice.number + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xml";

            string saveFilePath = filePath+"\\" + fileName ;
            output.Save(saveFilePath);

        }

        void createXMLforOrder(LogoInvoice invoice)
        {
            XmlDocument output = null;
            XmlNode outputOrderDbop = null;
            XmlNode outputOrderSales = null;
            XmlNode outputTransactions = null;
            XmlNode outputTransaction = null;

            string date = "";

            output = new XmlDocument();
            outputOrderSales = null;

            XmlDeclaration xmlDeclaration = output.CreateXmlDeclaration("1.0", "ISO-8859-9", null);
            output.InsertBefore(xmlDeclaration, output.DocumentElement);

            //8 satış , 3 Satış iade  
            if (invoice.type == 8 || invoice.type == 3)
            {
                outputOrderSales = output.CreateNode(XmlNodeType.Element, "SALES_ORDERS", "");
            }
            else
            {
                outputOrderSales = output.CreateNode(XmlNodeType.Element, "PURCHASE_ORDERS", "");
            }
            output.AppendChild(outputOrderSales);

            outputOrderDbop = output.CreateNode(XmlNodeType.Element, "ORDER_SLIP", "");
            XmlAttribute newAttr = output.CreateAttribute("DBOP");
            newAttr.Value = "INS";
            outputOrderDbop.Attributes.Append(newAttr);
            outputOrderSales.AppendChild(outputOrderDbop);
            
            helper.AddNode(output, outputOrderDbop, "NUMBER", "JW" + invoice.number);

            if (useShortDate)
            {
                date = (invoice.date.ToShortDateString());
            }
            else
            {
                date = (invoice.date.ToString("dd.MM.yyyy"));
            }

            helper.AddNode(output, outputOrderDbop, "DATE", date);
            helper.AddNode(output, outputOrderDbop, "TIME", helper.Hour(invoice.date).ToString());
            helper.AddNode(output, outputOrderDbop, "DOC_NUMBER", invoice.number);
            helper.AddNode(output, outputOrderDbop, "ORDER_STATUS", "4");
            helper.AddNode(output, outputOrderDbop, "DIVISION", "1");
            helper.AddNode(output, outputOrderDbop, "SOURCE_WH", invoice.wareHouseCode);
            helper.AddNode(output, outputOrderDbop, "SOURCE_COST_GRP", invoice.wareHouseCode);
            helper.AddNode(output, outputOrderDbop, "AUXIL_CODE", invoice.salesmanCode);
            helper.AddNode(output, outputOrderDbop, "ARP_CODE", invoice.customerCode);
            helper.AddNode(output, outputOrderDbop, "TOTAL_DISCOUNTED", (invoice.netTotal - invoice.discountTotal).ToString().Replace(",", "."));  // toplam tutar
            helper.AddNode(output, outputOrderDbop, "TOTAL_GROSS", invoice.grossTotal.ToString().Replace(",", ".")); // brüt tutar
            helper.AddNode(output, outputOrderDbop, "TOTAL_NET", invoice.netTotal.ToString().Replace(",", "."));  // Kdv hariç tutar
            helper.AddNode(output, outputOrderDbop, "TOTAL_VAT", invoice.vatTotal.ToString().Replace(",", "."));  // Toplam Kdv
            helper.AddNode(output, outputOrderDbop, "PAYMENT_CODE", invoice.paymentCode);
            helper.AddNode(output, outputOrderDbop, "NOTES1", " "+invoice.note); 
            helper.AddNode(output, outputOrderDbop, "SHIPPING_AGENT", shipAgentCode); 



            if (invoice.type == 8 || invoice.type == 3)
                helper.AddNode(output, outputOrderDbop, "SALESMAN_CODE", invoice.salesmanCode); 

            if (useCypheCode)
                helper.AddNode(output, outputOrderDbop, "AUTH_CODE", cypheCode);
               
            outputTransactions = output.CreateNode(XmlNodeType.Element, "TRANSACTIONS", "");
            outputOrderDbop.AppendChild(outputTransactions);

            for (int i = 0; i < invoice.details.Count; i++)
            {

                outputTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
                outputTransactions.AppendChild(outputTransaction);

                if (invoice.details[i].type == 2)
                {
                     //1discounts 
                    if (invoice.details[i].rate > Convert.ToDecimal(0) && invoice.details[i].rate < Convert.ToDecimal(100))
                    {
                        helper.AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString()); 
                        helper.AddNode(output, outputTransaction, "DISCOUNT_RATE", Convert.ToDouble(Math.Round(invoice.details[i].rate, 2)).ToString());
                        helper.AddNode(output, outputTransaction, "DESCRIPTION", invoice.details[i].name);
                    }
                }
                else
                {
                    helper.AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                    helper.AddNode(output, outputTransaction, "MASTER_CODE", "JW" + invoice.details[i].code);
                    helper.AddNode(output, outputTransaction, "DIVISION", "1");
                    if (invoice.type == 8 || invoice.type == 3)
                    {
                        helper.AddNode(output, outputTransaction, "GL_CODE1", "600.10.20.J01");
                        helper.AddNode(output, outputTransaction, "GL_CODE2", "391.01.18");
                    }
                    else
                    {
                        helper.AddNode(output, outputTransaction, "GL_CODE1", "153.10.20.J01");
                    }
                    helper.AddNode(output, outputTransaction, "QUANTITY", invoice.details[i].quantity.ToString());
                    helper.AddNode(output, outputTransaction, "PRICE", Math.Round(invoice.details[i].price, 2).ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "TOTAL", Math.Round(invoice.details[i].total, 2).ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "TOTAL_NET", invoice.details[i].netTotal.ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "VAT_RATE", invoice.details[i].vatRate.ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "VAT_AMOUNT", invoice.details[i].vatAmount.ToString().Replace(",", "."));
                    helper.AddNode(output, outputTransaction, "UNIT_CODE", helper.getUnit(invoice.details[i].unitCode));
                    helper.AddNode(output, outputTransaction, "PAYMENT_CODE", invoice.paymentCode);
                    helper.AddNode(output, outputTransaction, "SOURCE_WH", invoice.wareHouseCode);
                    helper.AddNode(output, outputTransaction, "SOURCE_COST_GRP", invoice.wareHouseCode);

                    if (invoice.type == 8 || invoice.type == 3)
                        helper.AddNode(output, outputTransaction, "SALESMAN_CODE", invoice.salesmanCode);
                }
            }

            string fileName = invoice.number + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xml";

            string saveFilePath = filePath + "\\" + fileName;
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
                    invoice.ebillCustomer = selectedInvoice.ebillCustomer;
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
                        if(selectedInvoice.invoiceType == InvoiceType.BUYING_SERVICE || selectedInvoice.invoiceType == InvoiceType.SELLING_SERVICE)
                        {
                            invDetail.type = (int)selectedInvoice.invoiceType;
                        }
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
                            invDetailDiscountDetail.name = discount.name;

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
                            waybillDetailDiscountDetail.name = discount.name;

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
        public List<Order> GetSelectedOrders()
        {
            List<Order> orders = new List<Order>();

            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    string number = row.Cells["number"].Value.ToString();
                    Order selectedOrder = jsonOrders.data.orders.Where(inv => inv.receiptNumber == number).FirstOrDefault();
                      
                    Order order = new Order();
                    order.type = 3;
                    order.receiptNumber = selectedOrder.receiptNumber;
                    order.warehouse = selectedOrder.warehouse;
                    order.customer = selectedOrder.customer;
                    order.orderDate = selectedOrder.orderDate;
                    order.deliveryDate = selectedOrder.deliveryDate;
                    order.discountTotal = selectedOrder.discountTotal;
                    order.preVatNetTotal = selectedOrder.preVatNetTotal;
                    order.vatTotal = selectedOrder.vatTotal;
                    order.grossTotal = selectedOrder.grossTotal;
                    order.paymentType = selectedOrder.paymentType;
                    order.salesmanNote = selectedOrder.salesmanNote;
                    order.salesman = selectedOrder.salesman;
                    order.customerBranch = selectedOrder.customerBranch;
                    order.orderId = selectedOrder.orderId;

                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                    foreach (var selectedOrderDetail in selectedOrder.details)
                    {
                        OrderDetail ordDetail = new OrderDetail();

                        ordDetail.productCode = selectedOrderDetail.productCode;
                        ordDetail.quantity = selectedOrderDetail.quantity;
                        ordDetail.orderItemPrice = selectedOrderDetail.orderItemPrice;
                        ordDetail.grossTotal = selectedOrderDetail.grossTotal;
                        ordDetail.discountAmount = selectedOrderDetail.discountAmount;
                        //ordDetail.unitCode = selectedOrderDetail.unitCode;
                        //ordDetail.vatIncluded = selectedOrderDetail.vatIncluded;
                        //ordDetail.vatRate = selectedOrderDetail.vatRate;
                        ordDetail.vatTotal = selectedOrderDetail.vatTotal;
                        ordDetail.preVatNetTotal = selectedOrderDetail.preVatNetTotal;
                        ordDetail.productBarcode = selectedOrderDetail.productBarcode;
                        ordDetail.lineOrder = selectedOrderDetail.lineOrder;
                        ordDetail.grossTotal = selectedOrderDetail.grossTotal;
                        ordDetail.vatRate = selectedOrderDetail.vatRate;
                        ordDetail.unitCode = selectedOrderDetail.unitCode;

                        orderDetails.Add(ordDetail);
                         
                    }
                    order.details = orderDetails;
                    orders.Add(order);
                }
            }
            return orders;
        }
        public List<LogoInvoiceJson> GetSelectedInvoicesForMikro()
        {
            List<LogoInvoiceJson> invoices = new List<LogoInvoiceJson>();

            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    string number = row.Cells["number"].Value.ToString();
                    LogoInvoiceJson selectedInvoice = jsonInvoices.data.Where(inv => inv.number == number).FirstOrDefault();
                      
                    invoices.Add(selectedInvoice);
                }
            }
            return invoices;
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
            if (jsonData != null)
            {
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
        void FillGrid(OrderResponse jsonData)
        {
            dataGridInvoice.Rows.Clear();
            foreach (var data in jsonData.orders)
            {
                int n = dataGridInvoice.Rows.Add();
                dataGridInvoice.Rows[n].Cells[0].Value = "false";
                dataGridInvoice.Rows[n].Cells[1].Value = "Satış Siparişi";
                dataGridInvoice.Rows[n].Cells[2].Value = data.receiptNumber;
                dataGridInvoice.Rows[n].Cells[3].Value = data.orderDate.ToShortDateString();
                dataGridInvoice.Rows[n].Cells[4].Value = data.receiptNumber;
                dataGridInvoice.Rows[n].Cells[6].Value = data.customer.code;
                dataGridInvoice.Rows[n].Cells[7].Value = data.discountTotal.ToString();
                dataGridInvoice.Rows[n].Cells[8].Value = data.vatTotal.ToString();
                dataGridInvoice.Rows[n].Cells[9].Value = data.grossTotal.ToString();
            }
        }
        class GenericResponse<T>
        {
            public T data { get; set; }
            public int responseStatus { get; set; }
            public model.order.Message message { get; set; }
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
        public void GetOrders()
        {
            RestClient restClient = new RestClient(url);
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
            var con = requestResponse.Content;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            jsonOrders = JsonConvert.DeserializeObject<GenericResponse<OrderResponse>>(requestResponse.Content, settings);
            
            FillGrid(jsonOrders.data);
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
        void SendResponse(IntegratedOrderStatus integratedOrders)
        {
            try
            {
                RestClient restClient = new RestClient(url);
                RestRequest restRequest = new RestRequest("/integration/orders/sync", Method.PUT)
                {
                    RequestFormat = DataFormat.Json
                };

                restRequest.AddJsonBody(integratedOrders);

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
                MessageBox.Show("Sipariş Logoya Aktarıldı Fakat salesArt ' taki durumu güncellenemedi.. ", "Fatura Statüsünün Gücellenememesi", MessageBoxButtons.OK);
            }

        }
        public IntegratedInvoiceStatus sendMultipleInvoice(List<LogoInvoice> invoices)
        {
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
                                newInvoice.DataFields.FieldByName("SHIPLOC_DEF").Value = invoice.customerBranchName;
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
                            newInvoice.DataFields.FieldByName("SHIPPING_AGENT").Value = shipAgentCode;


                            if (invoice.type != (int)InvoiceType.BUYING_SERVICE && invoice.type != (int)InvoiceType.SELLING_SERVICE) // hizmet faturaları için irsaliye alanları doldurulmadı
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
                                        newInvoiceLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble(Math.Round(detail.rate,2));
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
                                        else if (invoice.type == (int)InvoiceType.SELLING_SERVICE || invoice.type == (int)InvoiceType.BUYING_SERVICE)
                                        {
                                            newInvoiceLines[i].FieldByName("MASTER_CODE").Value = reader.getServiceCodeBySalesArtServiceCode(detail.code);
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
            string remoteNumber = "";
            string message = "";
            string remoteOrderStatus = "";
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
                        if ( order.type == 3)
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

                        newOrder.DataFields.FieldByName("TIME").Value = helper.Hour(order.orderDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
                        newOrder.DataFields.FieldByName("SALESMAN_CODE").Value = order.salesman.code;
                        newOrder.DataFields.FieldByName("ARP_CODE").Value = order.customer.code;
                        newOrder.DataFields.FieldByName("SOURCE_WH").Value = order.warehouse.code;
                        newOrder.DataFields.FieldByName("SOURCE_COST_GRP").Value = order.warehouse.code;
                        newOrder.DataFields.FieldByName("ORDER_STATUS").Value = 1;
                        //newOrder.DataFields.FieldByName("DEPARTMENT").Value = helper.getDepartment();

                        string asdfg = order.customer.code.Substring(2, 2);

                        if (useCypheCode)
                        {
                            newOrder.DataFields.FieldByName("AUTH_CODE").Value = order.customer.code.Substring(2,2);
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
                IntegratedOrderDto recievedOrder = new IntegratedOrderDto(ex.Message.ToString(), remoteOrderId, 0 , false);
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
                    newInvoice.DataFields.FieldByName("SHIPPING_AGENT").Value = shipAgentCode;

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
                        dispatches_lines[0].FieldByName("SHIPPING_AGENT").Value = shipAgentCode;
                        dispatches_lines[0].FieldByName("SHIP_DATE").Value = invoice.date.AddDays(2).ToString("dd.MM.yyyy");
                        dispatches_lines[0].FieldByName("SHIP_TIME").Value = helper.Hour(invoice.date.AddDays(2)).ToString();
                        dispatches_lines[0].FieldByName("DISP_STATUS").Value = 1;

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
                                newInvoiceLines[i].FieldByName("DISCOUNT_RATE").Value = Convert.ToDouble(Math.Round(detail.rate, 2));
                                newInvoiceLines[i].FieldByName("DESCRIPTION").Value = detail.name;

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
            string remoteNumber = "";
            string message = "";
            string remoteInvoiceStatus = "";

            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();
             

            try
            {
                foreach (var invoice in invoices)
                {
                    if (XMLTransferForOrder)
                        createXMLforOrder(invoice);
                    else
                        createXMLforInvoice(invoice);
                    
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
        public IntegratedInvoiceStatus sendMultipleInvoicesForMikro(List<LogoInvoiceJson> invoices)
        {
            string remoteNumber = "";
            string message = "";
            string remoteInvoiceStatus = "";

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
            else if (orderTransferToLogoInfo)
                GetOrders();
            else GetInvoices();

            btnSendToLogo.Enabled = (dataGridInvoice.Rows.Count > 0 && isLoggedIn) ? true : false;
            btnCheckLogoConnection.Enabled = (dataGridInvoice.Rows.Count > 0 && !isLoggedIn) ? true : false;
            Cursor.Current = Cursors.Default;
        }
        private void btnSendToLogo_Click(object sender, EventArgs e)
        { 
            int selectedInvoiceCount = 0;
            IntegratedInvoiceStatus status = new IntegratedInvoiceStatus();
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    selectedInvoiceCount += 1;
                }
            }
            if (selectedInvoiceCount > 0)
            {
                List<LogoInvoiceJson> selectedInvoicesForMikro = new List<LogoInvoiceJson>();
                List<LogoInvoice> selectedInvoices = new List<LogoInvoice>();
                if (integrationForMikroERP)
                    selectedInvoicesForMikro = GetSelectedInvoicesForMikro();
                else
                    selectedInvoices = GetSelectedInvoices();
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
                if(integrationForMikroERP)
                    status = sendMultipleInvoicesForMikro(selectedInvoicesForMikro);
                else
                    status = sendMultipleInvoice(selectedInvoices);
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

        private void btnSendOrderToLogo_Click(object sender, EventArgs e)
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
                List<Order> selectedOrders = GetSelectedOrders();
                Cursor.Current = Cursors.WaitCursor;
                helper.LogFile("Sipariş Aktarım Basladı", "-", "-", "-", "-");
                IntegratedOrderStatus status = sendMultipleOrder(selectedOrders);
                SendResponse(status);
                helper.ShowMessages(status);
                helper.LogFile("Sipariş Aktarım Bitti", "-", "-", "-", "-");
                dataGridInvoice.Rows.Clear();
                btnSendToLogo.Enabled = false;
                btnCheckLogoConnection.Enabled = false;
                lblLogoConnectionInfo.Text = "";
                Cursor.Current = Cursors.Default;
            }
            else MessageBox.Show("Sipariş Seçmelisiniz..", "Sipariş Seçim", MessageBoxButtons.OK);
        }
    }

}


