using invoiceIntegration.config;
using invoiceIntegration.helper;
using invoiceIntegration.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using static invoiceIntegration.frmMain;

namespace invoiceIntegration
{
    public class XmlHelper : Configuration
    {
        Helper helper = new Helper();
        int distributorId = Configuration.getDistributorId();
        public IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
        public XmlNode AddOrderDbopNode(XmlDocument output, XmlNode outputOrderNode, LogoInvoice invoice)
        {
            string date;
            if (getUseShortDate())
                date = (invoice.date.ToShortDateString());
            else
                date = (invoice.date.ToString("dd.MM.yyyy"));
            AddNode(output, outputOrderNode, "NUMBER", "JW" + invoice.number);
            AddNode(output, outputOrderNode, "DATE", date);
            AddNode(output, outputOrderNode, "TIME", helper.Hour(invoice.date).ToString());
            AddNode(output, outputOrderNode, "DOC_NUMBER", invoice.number);
            AddNode(output, outputOrderNode, "ORDER_STATUS", "4");
            AddNode(output, outputOrderNode, "DIVISION", "1");
            AddNode(output, outputOrderNode, "SOURCE_WH", invoice.wareHouseCode);
            AddNode(output, outputOrderNode, "SOURCE_COST_GRP", invoice.wareHouseCode);
            AddNode(output, outputOrderNode, "AUXIL_CODE", invoice.salesmanCode);
            AddNode(output, outputOrderNode, "ARP_CODE", invoice.customerCode);
            AddNode(output, outputOrderNode, "TOTAL_DISCOUNTED", (invoice.netTotal - invoice.discountTotal).ToString().Replace(",", "."));  // toplam tutar
            AddNode(output, outputOrderNode, "TOTAL_GROSS", invoice.grossTotal.ToString().Replace(",", ".")); // brüt tutar
            AddNode(output, outputOrderNode, "TOTAL_NET", invoice.netTotal.ToString().Replace(",", "."));  // Kdv hariç tutar
            AddNode(output, outputOrderNode, "TOTAL_VAT", invoice.vatTotal.ToString().Replace(",", "."));  // Toplam Kdv
            AddNode(output, outputOrderNode, "PAYMENT_CODE", invoice.paymentCode);
            AddNode(output, outputOrderNode, "NOTES1", " " + invoice.note);
            AddNode(output, outputOrderNode, "SHIPPING_AGENT", getShipAgentCode());
            AddNode(output, outputOrderNode, "AFFECT_RISK", "1");
            if (getUseShipCode())
                AddNode(output, outputOrderNode, "SHIPLOC_CODE", invoice.customerBranchCode);
            if (invoice.type == (int)InvoiceType.SELLING || invoice.type == (int)InvoiceType.SELLING_RETURN)
                AddNode(output, outputOrderNode, "SALESMAN_CODE", invoice.salesmanCode);
            if (getUseCypheCode())
                AddNode(output, outputOrderNode, "AUTH_CODE", getCypheCode());
            return outputOrderNode;
        }
        public XmlNode AddOrderTransactionNode(XmlDocument output, XmlNode outputTransactions, XmlNode outputTransaction, LogoInvoice invoice)
        {
            for (int i = 0; i < invoice.details.Count; i++)
            {
                outputTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
                outputTransactions.AppendChild(outputTransaction);
                if (invoice.details[i].type == 2)
                {
                    //1discounts 
                    if (invoice.details[i].rate > Convert.ToDecimal(0) && invoice.details[i].rate < Convert.ToDecimal(100))
                    {
                        AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                        AddNode(output, outputTransaction, "DISCOUNT_RATE", Convert.ToDouble(Math.Round(invoice.details[i].rate, 2)).ToString());
                        //AddNode(output, outputTransaction, "DESCRIPTION", invoice.details[i].name);
                    }
                }
                else
                {
                    AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                    AddNode(output, outputTransaction, "MASTER_CODE", "JW" + invoice.details[i].code);
                    AddNode(output, outputTransaction, "DIVISION", "1");
                    if (invoice.type == 8 || invoice.type == 3)
                    {
                        AddNode(output, outputTransaction, "GL_CODE1", "600.10.20.J01");
                        AddNode(output, outputTransaction, "GL_CODE2", "391.01.18");
                    }
                    else
                    {
                        AddNode(output, outputTransaction, "GL_CODE1", "153.10.20.J01");
                    }
                    AddNode(output, outputTransaction, "QUANTITY", invoice.details[i].quantity.ToString());
                    AddNode(output, outputTransaction, "PRICE", Math.Round(invoice.details[i].price, 2).ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "TOTAL", Math.Round(invoice.details[i].total, 2).ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "TOTAL_NET", invoice.details[i].netTotal.ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "VAT_RATE", invoice.details[i].vatRate.ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "VAT_AMOUNT", invoice.details[i].vatAmount.ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "UNIT_CODE", helper.getUnit(invoice.details[i].unitCode));
                    AddNode(output, outputTransaction, "PAYMENT_CODE", invoice.paymentCode);
                    AddNode(output, outputTransaction, "SOURCE_WH", invoice.wareHouseCode);
                    AddNode(output, outputTransaction, "SOURCE_COST_GRP", invoice.wareHouseCode);
                    AddNode(output, outputTransaction, "AFFECT_RISK", "1");
                    if (invoice.type == (int)InvoiceType.SELLING || invoice.type == (int)InvoiceType.SELLING_RETURN)
                        AddNode(output, outputTransaction, "SALESMAN_CODE", invoice.salesmanCode);
                }
            }
            return outputTransaction;
        }
        public IntegratedInvoiceStatus OrderListExportToXml(List<LogoInvoice> invoices)
        {
            XmlDocument output = new XmlDocument();
            XmlNode outputOrderDbop = null;
            XmlNode outputOrderSales = null;
            XmlNode outputTransactions = null;
            XmlNode outputTransaction = null;
            XmlDeclaration xmlDeclaration = output.CreateXmlDeclaration("1.0", "ISO-8859-9", null);
            output.InsertBefore(xmlDeclaration, output.DocumentElement);
            List<IntegratedInvoiceDto> receivedOrders = new List<IntegratedInvoiceDto>();
            if (invoices.FirstOrDefault().type == (int)InvoiceType.SELLING || invoices.FirstOrDefault().type == (int)InvoiceType.SELLING_RETURN)
            {
                outputOrderSales = output.CreateNode(XmlNodeType.Element, "SALES_ORDERS", "");
            }
            else
            {
                outputOrderSales = output.CreateNode(XmlNodeType.Element, "PURCHASE_ORDERS", "");
            }
            output.AppendChild(outputOrderSales);
            foreach (var invoice in invoices)
            {
                try
                {
                    IntegratedInvoiceDto recievedOrder = new IntegratedInvoiceDto("", invoice.number, invoice.number, true);
                    receivedOrders.Add(recievedOrder);
                    outputOrderDbop = output.CreateNode(XmlNodeType.Element, "ORDER_SLIP", "");
                    XmlAttribute newAttr = output.CreateAttribute("DBOP");
                    newAttr.Value = "INS";
                    outputOrderDbop.Attributes.Append(newAttr);
                    outputOrderSales.AppendChild(outputOrderDbop);
                    outputOrderDbop = AddOrderDbopNode(output, outputOrderDbop, invoice);
                    outputTransactions = output.CreateNode(XmlNodeType.Element, "TRANSACTIONS", "");
                    outputOrderDbop.AppendChild(outputTransactions);
                    outputTransaction = AddOrderTransactionNode(output, outputTransactions, outputTransaction, invoice);
                }
                catch (Exception ex)
                {
                    IntegratedInvoiceDto recievedOrder = new IntegratedInvoiceDto(ex.Message.ToString(), "", "", false);
                    receivedOrders.Add(recievedOrder);
                }
            }
            string fileName = DateTime.Now.ToString("dd-MM-yyyy") + ".xml";
            //string saveFilePath = filePath + "\\" + fileName;
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            output.Save(filePath);
            integratedInvoices.integratedInvoices = receivedOrders;
            integratedInvoices.distributorId = getDistributorId();
            return integratedInvoices;
        }
        public XmlNode AddInvoiceDbopNode(XmlDocument output, XmlNode outputInvoiceNode, LogoInvoice invoice, string outputNodeType)
        {
            string date;
            if (getUseShortDate())
                date = (invoice.date.ToShortDateString());
            else
                date = (invoice.date.ToString("dd.MM.yyyy"));
            if (outputNodeType == constants.NodeType.OutputInvoiceDbop)
            {
                AddNode(output, outputInvoiceNode, "TYPE", invoice.type.ToString());
                if (getUseDefaultNumber())
                    AddNode(output, outputInvoiceNode, "NUMBER", "~");
                else
                    AddNode(output, outputInvoiceNode, "NUMBER", invoice.number);
                AddNode(output, outputInvoiceNode, "DATE", date);
                AddNode(output, outputInvoiceNode, "TIME", helper.Hour(invoice.date).ToString());
                AddNode(output, outputInvoiceNode, "DOC_NUMBER", invoice.number);
                AddNode(output, outputInvoiceNode, "DOC_DATE", invoice.documentDate.ToString("dd.MM.yyyy"));
                AddNode(output, outputInvoiceNode, "ARP_CODE", invoice.customerCode);
                AddNode(output, outputInvoiceNode, "SHIPPING_AGENT", getShipAgentCode());
                if (getUseCypheCode())
                    AddNode(output, outputInvoiceNode, "AUTH_CODE", getCypheCode());
                if (getUseShipCode())
                {
                    AddNode(output, outputInvoiceNode, "SHIPLOC_CODE", invoice.customerBranchCode);
                    AddNode(output, outputInvoiceNode, "SHIPLOC_DEF", invoice.customerBranchName);
                }
                AddNode(output, outputInvoiceNode, "TOTAL_DISCOUNTS", invoice.discountTotal.ToString().Replace(",", "."));  // indirim toplamı
                AddNode(output, outputInvoiceNode, "TOTAL_DISCOUNTED", (invoice.grossTotal).ToString().Replace(",", "."));  // toplam tutar
                AddNode(output, outputInvoiceNode, "TOTAL_VAT", invoice.vatTotal.ToString().Replace(",", "."));  // Toplam Kdv
                AddNode(output, outputInvoiceNode, "TOTAL_GROSS", invoice.grossTotal.ToString().Replace(",", ".")); // brüt tutar
                AddNode(output, outputInvoiceNode, "TOTAL_NET", invoice.netTotal.ToString().Replace(",", "."));  // Kdv hariç
                // /if (distributorId == 47)                                                    
                //invoice.note = invoice.customerBranchName + " - " + invoice.note;  // gülpa distributoru, şubelerin açıklamaya eklenmesini istedi
                AddNode(output, outputInvoiceNode, "NOTES1", invoice.note);
                AddNode(output, outputInvoiceNode, "DIVISION", invoice.distributorBranchCode);
                AddNode(output, outputInvoiceNode, "DEPARTMENT", helper.getDepartment());
                AddNode(output, outputInvoiceNode, "DIVISION", helper.getDivision());
                AddNode(output, outputInvoiceNode, "AUXIL_CODE", invoice.salesmanCode);
                AddNode(output, outputInvoiceNode, "SOURCE_WH", invoice.wareHouseCode);
                AddNode(output, outputInvoiceNode, "SOURCE_COST_GRP", invoice.wareHouseCode);
                AddNode(output, outputInvoiceNode, "SALESMAN_CODE", invoice.salesmanCode);
                AddNode(output, outputInvoiceNode, "EINVOICE", invoice.ebillCustomer ? "1" : "2");
                AddNode(output, outputInvoiceNode, "AFFECT_RISK", "1");
                return outputInvoiceNode;
            }
            else
            {
                AddNode(output, outputInvoiceNode, "TYPE", invoice.type.ToString());
                AddNode(output, outputInvoiceNode, "NUMBER", invoice.number);
                AddNode(output, outputInvoiceNode, "DATE", date);
                AddNode(output, outputInvoiceNode, "TIME", helper.Hour(invoice.date).ToString());
                AddNode(output, outputInvoiceNode, "DOC_NUMBER", invoice.number);
                AddNode(output, outputInvoiceNode, "DOC_DATE", invoice.documentDate.ToString("dd.MM.yyyy"));
                AddNode(output, outputInvoiceNode, "SHIPPING_AGENT", getShipAgentCode());
                AddNode(output, outputInvoiceNode, "SHIP_DATE", invoice.date.AddDays(2).ToString("dd.MM.yyyy"));
                AddNode(output, outputInvoiceNode, "SHIP_TIME", helper.Hour(invoice.date.AddDays(2)).ToString());
                AddNode(output, outputInvoiceNode, "DISP_STATUS", "1");
                if (getUseCypheCode())
                    AddNode(output, outputInvoiceNode, "AUTH_CODE", getCypheCode());
                AddNode(output, outputInvoiceNode, "ARP_CODE", invoice.customerCode);
                AddNode(output, outputInvoiceNode, "TOTAL_DISCOUNTS", invoice.discountTotal.ToString().Replace(",", "."));  // indirim toplamı
                AddNode(output, outputInvoiceNode, "TOTAL_DISCOUNTED", (invoice.grossTotal).ToString().Replace(",", "."));  // toplam tutar
                AddNode(output, outputInvoiceNode, "TOTAL_GROSS", invoice.grossTotal.ToString().Replace(",", ".")); // brüt tutar
                AddNode(output, outputInvoiceNode, "TOTAL_NET", invoice.netTotal.ToString().Replace(",", "."));  // Kdv hariç tutar
                AddNode(output, outputInvoiceNode, "TOTAL_VAT", invoice.vatTotal.ToString().Replace(",", "."));  // Toplam Kdv
                AddNode(output, outputInvoiceNode, "NOTES1", invoice.note);
                AddNode(output, outputInvoiceNode, "ORIG_NUMBER", invoice.number);
                AddNode(output, outputInvoiceNode, "DOC_DATE", invoice.documentDate.ToString("dd.MM.yyyy"));
                AddNode(output, outputInvoiceNode, "DOC_TIME", helper.Hour(invoice.documentDate).ToString());
                //AddNode(output, outputInvoiceDbop, "PROFILE_ID", invoice.isElectronicInvoiceCustomer ? "1" : "0");
                return outputInvoiceNode;
            }
        }
        public XmlNode AddInvoiceTransactionNode(XmlDocument output, XmlNode outputTransactions, XmlNode outputTransaction, LogoInvoice invoice)
        {
            for (int i = 0; i < invoice.details.Count; i++)
            {
                outputTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
                outputTransactions.AppendChild(outputTransaction);
                if (invoice.details[i].type == 2)
                {
                    if (invoice.details[i].rate > Convert.ToDecimal(0) && invoice.details[i].rate < Convert.ToDecimal(100))
                    {
                        AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                        AddNode(output, outputTransaction, "BILLED", "1");
                        AddNode(output, outputTransaction, "DISCOUNT_RATE", Convert.ToDouble(Math.Round(invoice.details[i].rate, 2)).ToString().Replace(",", "."));
                        AddNode(output, outputTransaction, "DISPATCH_NUMBER", invoice.number);
                        AddNode(output, outputTransaction, "DESCRIPTION", invoice.details[i].name);
                        AddNode(output, outputTransaction, "SOURCEINDEX", invoice.wareHouseCode);
                        AddNode(output, outputTransaction, "SOURCECOSTGRP", invoice.wareHouseCode);
                        if (invoice.type == (int)InvoiceType.SELLING_RETURN)  // iade faturaları için
                        {
                            AddNode(output, outputTransaction, "RET_COST_TYPE", "1");
                        }
                    }
                }
                else if (invoice.details[i].type == 1)//Promosyon 
                {
                    AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                    AddNode(output, outputTransaction, "MASTER_CODE", invoice.details[i].code);
                    AddNode(output, outputTransaction, "QUANTITY", invoice.details[i].quantity.ToString());
                    AddNode(output, outputTransaction, "PRICE", Math.Round(invoice.details[i].price, 2).ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "TOTAL", Math.Round(invoice.details[i].grossTotal, 2).ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "VAT_RATE", invoice.details[i].vatRate.ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "UNIT_CODE", helper.getUnit(invoice.details[i].unitCode));
                    AddNode(output, outputTransaction, "BILLED", "1");
                    AddNode(output, outputTransaction, "DISCOUNT_RATE", Convert.ToDouble(Math.Round(invoice.details[i].rate, 2)).ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "DISPATCH_NUMBER", invoice.number);
                    AddNode(output, outputTransaction, "DESCRIPTION", invoice.details[i].name);
                    AddNode(output, outputTransaction, "SOURCEINDEX", invoice.wareHouseCode);
                    AddNode(output, outputTransaction, "SOURCECOSTGRP", invoice.wareHouseCode);

                }
                else
                {
                   
                    AddNode(output, outputTransaction, "TYPE", invoice.details[i].type.ToString());
                    AddNode(output, outputTransaction, "MASTER_CODE", invoice.details[i].code);
                    AddNode(output, outputTransaction, "QUANTITY", invoice.details[i].quantity.ToString());
                    AddNode(output, outputTransaction, "PRICE", Math.Round(invoice.details[i].price, 2).ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "TOTAL", Math.Round(invoice.details[i].grossTotal, 2).ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "BILLED", "1");
                    AddNode(output, outputTransaction, "DISPATCH_NUMBER", invoice.number);
                    AddNode(output, outputTransaction, "VAT_RATE", invoice.details[i].vatRate.ToString().Replace(",", "."));
                    AddNode(output, outputTransaction, "SOURCEINDEX", invoice.wareHouseCode);
                    AddNode(output, outputTransaction, "PAYMENT_CODE", invoice.paymentCode);
                    AddNode(output, outputTransaction, "UNIT_CODE", helper.getUnit(invoice.details[i].unitCode));
                    AddNode(output, outputTransaction, "AFFECT_RISK", "1");
                    // efaturalarda istiyor olabilri
                    // AddNode(output, outputTransaction, "UNIT_GLOBAL_CODE", "NIU");
                    if (invoice.type == (int)InvoiceType.SELLING_RETURN)
                    {
                        AddNode(output, outputTransaction, "RET_COST_TYPE", "1");
                    }
                }
            }
            return outputTransaction;
        }
        public IntegratedInvoiceStatus InvoiceListExportToXml(List<LogoInvoice> invoices)
        {
            XmlDocument output = new XmlDocument();
            XmlNode outputInvoiceDbop = null;
            XmlNode outputInvoiceSales = null;
            XmlNode outputTransactions = null;
            XmlNode outputDispatches = null;
            XmlNode outputDispatch = null;
            XmlNode outputTransaction = null;
            XmlDeclaration xmlDeclaration = output.CreateXmlDeclaration("1.0", "ISO-8859-9", null);
            output.InsertBefore(xmlDeclaration, output.DocumentElement);
            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();
            if (invoices.FirstOrDefault().type == (int)InvoiceType.SELLING || invoices.FirstOrDefault().type == (int)InvoiceType.SELLING_RETURN)
            {
                outputInvoiceSales = output.CreateNode(XmlNodeType.Element, "SALES_INVOICES", "");
            }
            else if (invoices.FirstOrDefault().type == (int)InvoiceType.BUYING || invoices.FirstOrDefault().type == (int)InvoiceType.BUYING_RETURN)
            {
                outputInvoiceSales = output.CreateNode(XmlNodeType.Element, "PURCHASE_INVOICES", "");
            }
            else
            {
                // Başka Bir fatura tipi gelirse diye tasarlandı.
            }
            output.AppendChild(outputInvoiceSales);
            foreach (var invoice in invoices)
            {
                try
                {
                    IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto("", invoice.number, invoice.number, true);
                    receivedInvoices.Add(recievedInvoice);
                    outputInvoiceDbop = output.CreateNode(XmlNodeType.Element, "INVOICE", "");
                    XmlAttribute newAttr = output.CreateAttribute("DBOP");
                    newAttr.Value = "INS";
                    outputInvoiceDbop.Attributes.Append(newAttr);
                    outputInvoiceSales.AppendChild(outputInvoiceDbop);
                    outputInvoiceDbop = AddInvoiceDbopNode(output, outputInvoiceDbop, invoice, constants.NodeType.OutputInvoiceDbop);

                    outputDispatches = output.CreateNode(XmlNodeType.Element, "DISPATCHES", "");
                    outputInvoiceDbop.AppendChild(outputDispatches);
                    outputDispatch = output.CreateNode(XmlNodeType.Element, "DISPATCH", "");
                    outputDispatches.AppendChild(outputDispatch);
                    outputDispatch = AddInvoiceDbopNode(output, outputDispatch, invoice, constants.NodeType.OutputInvoiceDispatch);

                    outputTransactions = output.CreateNode(XmlNodeType.Element, "TRANSACTIONS", "");
                    outputInvoiceDbop.AppendChild(outputTransactions);
                    outputTransaction = AddInvoiceTransactionNode(output, outputTransactions, outputTransaction, invoice);
                    //AddNode(output, outputInvoiceDbop, "EINVOICE", invoice.ebillCustomer ? "1" : "2");
                }
                catch (Exception ex)
                {
                    IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(ex.Message.ToString(), "", "", false);
                    receivedInvoices.Add(recievedInvoice);
                }
            }
            string fileName = DateTime.Now.ToString("dd-MM-yyyy") + ".xml";
            //string saveFilePath = filePath + "\\" + fileName;
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            output.Save(filePath);
            integratedInvoices.integratedInvoices = receivedInvoices;
            integratedInvoices.distributorId = getDistributorId();
            return integratedInvoices;
        }
        public void AddNode(XmlDocument Document, XmlNode Node, string Tag, string InnerText)
        {
            XmlNode tempNode = Document.CreateNode(XmlNodeType.Element, Tag, "");
            tempNode.InnerText = InnerText;
            Node.AppendChild(tempNode);
        }
        public void SaveToXml(DataGridView dataGridInvoice, GenericResponse<List<LogoInvoiceJson>> jsonInvoices)
        {
            SelectionHelper selectionHelper = new SelectionHelper();
            var selectedInvoices = selectionHelper.GetSelectedInvoices(dataGridInvoice, jsonInvoices);
            if (selectedInvoices.Count > 0)
            {
                helper.LogFile("Fatura Aktarım Basladı", "-", "-", "-", "-");
                if (Configuration.getXMLTransferForOrder())
                    integratedInvoices = OrderListExportToXml(selectedInvoices);
                else
                    integratedInvoices = InvoiceListExportToXml(selectedInvoices);
                helper.ShowMessages(integratedInvoices);
                helper.LogFile("Fatura Aktarım Bitti", "-", "-", "-", "-");
            }
            else
            {
                MessageBox.Show("Fatura Seçmelisiniz..", "Fatura Seçim", MessageBoxButtons.OK);
            }
        }
        #region -- paymentCodeStartLine
        //public IntegratedCollectionStatus ExportPaymentsToXml(List<LogoCollectionModel> logoCollections)
        //{
        //    List<IntegratedCollectionDto> ıntegratedCollectionDtos = new List<IntegratedCollectionDto>();
        //    IntegratedCollectionStatus IntegratedCollectionStatus = new IntegratedCollectionStatus();
        //    List<LogoCollectionModel> otherPaymentList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> otherPaymentDetail = new List<LogoCollectionModelDetail>();

        //    List<LogoCollectionModel> cashPaymentList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> cashPaymentDetail = new List<LogoCollectionModelDetail>();

        //    List<LogoCollectionModel> creditPaymentList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> creditPaymentDetail = new List<LogoCollectionModelDetail>();

        //    List<LogoCollectionModel> iadePaymentList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> iadePaymentDetail = new List<LogoCollectionModelDetail>();

        //    List<LogoCollectionModel> senetPaymentList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> senetPaymentDetail = new List<LogoCollectionModelDetail>();

        //    List<LogoCollectionModel> bankHavaleList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> bankHavaleDetail = new List<LogoCollectionModelDetail>();

        //    List<LogoCollectionModel> alacakDekontList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> alacakDekontDetail = new List<LogoCollectionModelDetail>();

        //    List<LogoCollectionModel> borcDekontList = new List<LogoCollectionModel>();
        //    List<LogoCollectionModelDetail> borcDekontDetail = new List<LogoCollectionModelDetail>();

        //    otherPaymentDetail.Clear();
        //    cashPaymentDetail.Clear();
        //    creditPaymentDetail.Clear();
        //    senetPaymentDetail.Clear();
        //    iadePaymentDetail.Clear();
        //    otherPaymentList.Clear();
        //    cashPaymentList.Clear();
        //    creditPaymentList.Clear();
        //    senetPaymentList.Clear();
        //    iadePaymentList.Clear();
        //    bankHavaleList.Clear();
        //    bankHavaleDetail.Clear();
        //    alacakDekontList.Clear();
        //    alacakDekontDetail.Clear();
        //    borcDekontList.Clear();
        //    borcDekontDetail.Clear();


        //    foreach (var item in logoCollections)
        //    {
        //        otherPaymentDetail.Clear();
        //        cashPaymentDetail.Clear();
        //        creditPaymentDetail.Clear();
        //        senetPaymentDetail.Clear();
        //        iadePaymentDetail.Clear();
        //        bankHavaleDetail.Clear();

        //        IntegratedCollectionDto trCash = null;
        //        IntegratedCollectionDto trCredit = null;
        //        IntegratedCollectionDto trSenet = null;
        //        IntegratedCollectionDto trOther = null;
        //        IntegratedCollectionDto trIade = null;
        //        IntegratedCollectionDto trAlacak = null;
        //        IntegratedCollectionDto trBorc = null;

        //        foreach (LogoCollectionModelDetail paymentDetail in item.collectionModelDetail)
        //        {
        //            if (paymentDetail.PaymentType == "2")
        //                creditPaymentDetail.Add(paymentDetail);
        //            else if (paymentDetail.PaymentType == "5")
        //            {
        //                senetPaymentDetail.Add(paymentDetail);
        //            }
        //            else if (paymentDetail.PaymentType == "3")
        //            {
        //                otherPaymentDetail.Add(paymentDetail);
        //            }
        //            else if (paymentDetail.PaymentType == "6")
        //            {
        //                iadePaymentDetail.Add(paymentDetail);
        //            }
        //            else if (paymentDetail.PaymentType == "4")
        //            {
        //                bankHavaleDetail.Add(paymentDetail);
        //            }
        //            else if (paymentDetail.PaymentType == "10") 
        //            {
        //                alacakDekontDetail.Add(paymentDetail);
        //            }
        //            else if (paymentDetail.PaymentType == "11")
        //            {
        //                borcDekontDetail.Add(paymentDetail);
        //            }
        //            else
        //                cashPaymentDetail.Add(paymentDetail);
        //        }

        //        if (otherPaymentDetail.Count > 0)
        //        {
        //            item.collectionModelHeader.Amount = 0;
        //            foreach (LogoCollectionModelDetail paymentDetail in otherPaymentDetail)
        //                item.collectionModelHeader.Amount += paymentDetail.Amount;
        //            item.collectionModelDetail = otherPaymentDetail;
        //            otherPaymentList.Add(item);
        //        }
        //        if (otherPaymentList.Count > 0)
        //        {
        //            trOther = ExportCekSenetToXml(otherPaymentList, "1");
        //        }

        //        if (cashPaymentDetail.Count > 0)
        //        {
        //            item.collectionModelHeader.Amount = 0;
        //            foreach (LogoCollectionModelDetail paymentDetail in cashPaymentDetail)
        //                item.collectionModelHeader.Amount += paymentDetail.Amount;
        //            item.collectionModelDetail = cashPaymentDetail;
        //            cashPaymentList.Add(item);
        //        }
        //        if (cashPaymentList.Count > 0)
        //        {
        //            trCash = ExportPaymentsKasaHarToXml(cashPaymentList);
        //        }

        //        string ErrorMessage = "";

        //        if (!trCash.successfullyIntegrated)
        //        {
        //            ErrorMessage += "Nakit Error:" + trCash.errorMessage;
        //        }
        //        if (!trOther.successfullyIntegrated)
        //        {
        //            ErrorMessage += "Cek Error:" + trOther.errorMessage;
        //        }

        //        try
        //        {
        //            if (ErrorMessage.Trim().Length > 0)
        //            {
        //                IntegratedCollectionDto recievedInvoice = new IntegratedCollectionDto(item.collectionModelHeader.Number + " belge numaralı tahsilat, sistemde zaten mevcut. Kontrol Ediniz", item.collectionModelHeader.Number, "", false);
        //                ıntegratedCollectionDtos.Add(recievedInvoice);
        //            }
        //            else
        //            {
        //                IntegratedCollectionDto recievedInvoice = new IntegratedCollectionDto(ErrorMessage, item.collectionModelHeader.Number, item.collectionModelHeader.Number, true);
        //                ıntegratedCollectionDtos.Add(recievedInvoice);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    IntegratedCollectionStatus.collections = ıntegratedCollectionDtos;
        //    IntegratedCollectionStatus.distributorId = distributorId;
        //    return IntegratedCollectionStatus;

        //    return null;
        //}

        //public IntegratedCollectionDto ExportCekSenetToXml(List<LogoCollectionModel> paymentList, string belgeTipi)
        //{
        //    XmlDocument output = new XmlDocument();
        //    XmlNode outputSD = null;
        //    XmlNode outputSDDbop = null;
        //    XmlNode outputAtachmentArp = null;
        //    XmlNode outputCHQPNTransaction = null;
        //    XmlNode outputTransaction = null;
        //    XmlNode outputTransaction1 = null;

        //    DateTime Tarih;
        //    string docNumber = "";
        //    XmlDeclaration xmlDeclaration = output.CreateXmlDeclaration("1.0", "ISO-8859-9", null);
        //    output.InsertBefore(xmlDeclaration, output.DocumentElement);

        //    outputSD = output.CreateNode(XmlNodeType.Element, "CQPN_ROLLS", "");
        //    output.AppendChild(outputSD);

        //    try
        //    {

        //        foreach (var item in paymentList)
        //        {
        //            docNumber = item.collectionModelHeader.Number;
        //            outputSDDbop = output.CreateNode(XmlNodeType.Element, "CHQPN_ROLL", "");
        //            XmlAttribute newAttr = output.CreateAttribute("DBOP");
        //            newAttr.Value = "INS";
        //            outputSDDbop.Attributes.Append(newAttr);
        //            outputSD.AppendChild(outputSDDbop);
        //            AddNode(output, outputSDDbop, "TYPE", belgeTipi);
        //            AddNode(output, outputSDDbop, "MASTER_MODULE", "5");

        //            AddNode(output, outputSDDbop, "MASTER_CODE", item.collectionModelHeader.CustomerCode);
        //            AddNode(output, outputSDDbop, "NUMBER", item.collectionModelHeader.Number);

        //            if (item.collectionModelHeader.Number != null)
        //            {
        //                Tarih = DateTime.Parse(item.collectionModelHeader.Number.ToString());
        //                string tarih1 = Tarih.ToString("dd.MM.yyyy");
        //                AddNode(output, outputSDDbop, "DATE", tarih1);
        //            }


        //            AddNode(output, outputSDDbop, "NOTES1", item.collectionModelHeader.Desc.ToString() + " - TAHSİLAT");


        //            AddNode(output, outputSDDbop, "TOTAL", item.collectionModelHeader.Amount.ToString());

        //            if (item.collectionModelDetail != null && item.collectionModelDetail.Count > 0)
        //            {
        //                outputAtachmentArp = output.CreateNode(XmlNodeType.Element, "TRANSACTIONS", "");
        //                outputSDDbop.AppendChild(outputAtachmentArp);
        //            }

        //            foreach (LogoCollectionModelDetail paymentDetail in item.collectionModelDetail)
        //            {
        //                outputCHQPNTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
        //                outputAtachmentArp.AppendChild(outputCHQPNTransaction);

        //                AddNode(output, outputCHQPNTransaction, "TYPE", belgeTipi);
        //                AddNode(output, outputCHQPNTransaction, "CURRENT_STATUS", "1");
        //                if (item.collectionModelHeader.Number != null) AddNode(output, outputCHQPNTransaction, "NUMBER", item.collectionModelHeader.Number.ToString());
        //                if (item.collectionModelHeader.CustomerName != null) AddNode(output, outputCHQPNTransaction, "OWING", item.collectionModelHeader.CustomerName);

        //                if (paymentDetail.OrderDate != null)
        //                {
        //                    Tarih = DateTime.Parse(paymentDetail.OrderDate.ToString());
        //                    string tarih1 = Tarih.ToString("dd.MM.yyyy");
        //                    AddNode(output, outputCHQPNTransaction, "DUE_DATE", tarih1);
        //                }

        //                if (paymentDetail.PaymentDate != null)
        //                {
        //                    Tarih = DateTime.Parse(paymentDetail.PaymentDate.ToString());
        //                    string tarih1 = Tarih.ToString("dd.MM.yyyy");
        //                    AddNode(output, outputCHQPNTransaction, "DATE", tarih1);
        //                }

        //                if (paymentDetail.Amount != null) AddNode(output, outputCHQPNTransaction, "AMOUNT", paymentDetail.Amount.ToString().Replace(",", "."));
        //                AddNode(output, outputCHQPNTransaction, "TC_XRATE", "1");
        //                if (paymentDetail.Amount != null) AddNode(output, outputCHQPNTransaction, "TC_AMOUNT", paymentDetail.Amount.ToString().Replace(",", "."));


        //                    AddNode(output, outputCHQPNTransaction, "CREDIT_FLAG", "1");


        //                AddNode(output, outputCHQPNTransaction, "SERIAL_NR", "99999999999999");
        //            }


        //           AddNode(output, outputSDDbop, "ARP_TRANSACTIONS", "");
        //           AddNode(output, outputSDDbop, "BANK_TRANSACTIONS", "");


        //            outputTransaction = output.CreateNode(XmlNodeType.Element, "PAYMENT_LIST", "");
        //            outputSDDbop.AppendChild(outputTransaction);

        //            foreach (LogoCollectionModelDetail paymentDetail in item.collectionModelDetail)
        //            {
        //                outputTransaction1 = output.CreateNode(XmlNodeType.Element, "PAYMENT", "");
        //                outputTransaction.AppendChild(outputTransaction1);
        //                if (item.collectionModelHeader.PaymentDate != null)
        //                {
        //                    Tarih = DateTime.Parse(item.collectionModelHeader.PaymentDate.ToString());
        //                    string tarih1 = Tarih.ToString("dd.MM.yyyy");
        //                    AddNode(output, outputTransaction1, "DATE", tarih1);

        //                }
        //               AddNode(output, outputTransaction1, "MODULENR", "6");
        //               AddNode(output, outputTransaction1, "SIGN", "1");
        //               AddNode(output, outputTransaction1, "TRCODE", "1");

        //                if (paymentDetail.Amount != null) AddNode(output, outputTransaction1, "TOTAL", paymentDetail.Amount.ToString().Replace(",", "."));
        //                AddNode(output, outputTransaction1, "TRRATE", "1");
        //                AddNode(output, outputTransaction1, "PAYMENT_TYPE", "2");
        //                AddNode(output, outputTransaction1, "DISCTRLIST", "");
        //                AddNode(output, outputTransaction1, "DISCTRDELLIST", "");
        //                if (paymentDetail.Amount != null) AddNode(output, outputTransaction1, "TRNET", paymentDetail.Amount.ToString().Replace(",", "."));

        //                if (item.collectionModelHeader.PaymentDate != null)
        //                {
        //                    Tarih = DateTime.Parse(item.collectionModelHeader.PaymentDate.ToString());
        //                    string tarih1 = Tarih.ToString("dd.MM.yyyy");
        //                    AddNode(output, outputTransaction1, "PROCDATE", tarih1);
        //                }

        //            }

        //        }

        //        string fileName = "cekSenet_"+DateTime.Now.ToString("dd-MM-yyyy") + ".xml";
        //        //string saveFilePath = filePath + "\\" + fileName;
        //        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        //        output.Save(filePath);
        //        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto("", docNumber, "", true);
        //        return recievedCollection;


        //    }
        //    catch (Exception ex)
        //    {
        //        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(ex.Message.ToString(), docNumber, "", false);
        //        return recievedCollection;
        //    }
        //}

        //public IntegratedCollectionDto ExportPaymentsKasaHarToXml(List<LogoCollectionModel> paymentList)
        //{

        //    XmlDocument output = new XmlDocument();
        //    XmlNode outputSD = null;
        //    XmlNode outputSDDbop = null;
        //    XmlNode outputAtachmentArp = null;
        //    XmlNode outputCHQPNTransaction = null;
        //    XmlNode outputTransaction = null;
        //    XmlNode outputTransaction1 = null;
        //    string payNote = "";
        //    DateTime Tarih;

        //    XmlDeclaration xmlDeclaration = output.CreateXmlDeclaration("1.0", "ISO-8859-9", null);
        //    output.InsertBefore(xmlDeclaration, output.DocumentElement);

        //    outputSD = output.CreateNode(XmlNodeType.Element, "SD_TRANSACTIONS", "");
        //    output.AppendChild(outputSD);
        //    string docNumber = "";
        //    try
        //    {

        //        foreach (var item in paymentList)
        //        {
        //            payNote = "";

        //            docNumber = item.collectionModelHeader.Number;
        //            foreach (LogoCollectionModelDetail paymentDetail in item.collectionModelDetail)
        //            {
        //                payNote = " - " + paymentDetail.PaymentTypeName;

        //                outputSDDbop = output.CreateNode(XmlNodeType.Element, "SD_TRANSACTION", "");
        //                XmlAttribute newAttr = output.CreateAttribute("DBOP");
        //                newAttr.Value = "INS";
        //                outputSDDbop.Attributes.Append(newAttr);
        //                outputSD.AppendChild(outputSDDbop);
        //                AddNode(output, outputSDDbop, "TYPE", "11");


        //                        AddNode(output, outputSDDbop, "SD_CODE", item.collectionModelHeader.SalesmanCode);


        //                if (item.collectionModelHeader.PaymentDate != null)
        //                {
        //                    Tarih = DateTime.Parse(item.collectionModelHeader.PaymentDate.ToString());
        //                    string tarih1 = Tarih.ToString("dd.MM.yyyy");
        //                    AddNode(output, outputSDDbop, "DATE", tarih1);
        //                }

        //                AddNode(output, outputSDDbop, "NUMBER", item.collectionModelHeader.Number.ToString());
        //                if (item.collectionModelHeader.CustomerName != null) AddNode(output, outputSDDbop, "MASTER_TITLE", item.collectionModelHeader.CustomerName.ToString());

        //                if (paymentDetail.Amount != null) AddNode(output, outputSDDbop, "AMOUNT", paymentDetail.Amount.ToString().Replace(",", "."));
        //                if (paymentDetail.Amount != null) AddNode(output, outputSDDbop, "RC_AMOUNT", paymentDetail.Amount.ToString().Replace(",", "."));
        //                if (paymentDetail.Amount != null) AddNode(output, outputSDDbop, "TC_AMOUNT", paymentDetail.Amount.ToString().Replace(",", "."));

        //                    outputAtachmentArp = output.CreateNode(XmlNodeType.Element, "ATTACHMENT_ARP", "");
        //                    outputSDDbop.AppendChild(outputAtachmentArp);


        //                outputCHQPNTransaction = output.CreateNode(XmlNodeType.Element, "TRANSACTION", "");
        //                outputAtachmentArp.AppendChild(outputCHQPNTransaction);

        //                if (item.collectionModelHeader.CustomerCode != null) AddNode(output, outputCHQPNTransaction, "ARP_CODE", item.collectionModelHeader.CustomerCode);
        //                if (item.collectionModelHeader.Number != null) AddNode(output, outputCHQPNTransaction, "TRANNO", item.collectionModelHeader.Number);


        //               AddNode(output, outputCHQPNTransaction, "DESCRIPTION", item.collectionModelHeader.Desc+" - " + paymentDetail.PaymentTypeName);

        //                if (paymentDetail.Amount != null) AddNode(output, outputCHQPNTransaction, "CREDIT", paymentDetail.Amount.ToString().Replace(",", "."));
        //                if (paymentDetail.Amount != null) AddNode(output, outputCHQPNTransaction, "TC_AMOUNT", paymentDetail.Amount.ToString().Replace(",", "."));
        //                //Common.AddNode(output, outputCHQPNTransaction, "TC_XRATE", "1");


        //                    AddNode(output, outputCHQPNTransaction, "AFFECT_RISK", "1");

        //            }
        //        }
        //        string fileName = "kasaHareketi_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xml";
        //        //string saveFilePath = filePath + "\\" + fileName;
        //        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        //        output.Save(filePath);
        //        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto("", docNumber, "", true);
        //        return recievedCollection;
        //    }
        //    catch (Exception ex)
        //    {
        //        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(ex.Message.ToString(), docNumber, "", false);
        //        return recievedCollection;
        //    }

        //}
        #endregion
    }
}
