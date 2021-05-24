using invoiceIntegration.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityObjects;
using invoiceIntegration.repository;
using Configuration = invoiceIntegration.config.Configuration;
using invoiceIntegration.config;
using invoiceIntegration.model.waybill;
using invoiceIntegration.model.order;

namespace invoiceIntegration.helper
{
    public class IntegratedHelper
    {
        bool isProducerCode = Configuration.getIsProducerCode();
        bool useCypheCode = Configuration.getUseCypheCode();
        string cypheCode = Configuration.getCypheCode();
        bool useDefaultNumber = Configuration.getUseDefaultNumber();
        bool useShortDate = Configuration.getUseShortDate();
        int distributorId = Configuration.getDistributorId();
        bool useDispatch = Configuration.getUseDispatch();
        bool integrationForMikroERP = Configuration.getIntegrationForMikroERP();
        string shipAgentCode = Configuration.getShipAgentCode();
        string campaignLineNo = Configuration.getCampaignLineNo();
        UnityApplication unity = LogoApplication.getApplication();
        LogoDataReader reader = new LogoDataReader();
        Helper helper = new Helper();
        IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
        IntegratedWaybillStatus integratedWaybills = new IntegratedWaybillStatus();
        IntegratedOrderStatus integratedOrders = new IntegratedOrderStatus();               
        public IntegratedInvoiceStatus sendMultipleInvoice(List<LogoInvoice> invoices, bool isLoggedIn)
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
                            if (Configuration.getUseShipCode())
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
                                        else if (Configuration.getIsBarcode())   //sümerde ürün kodları barkoda göre getirilecek
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
        public IntegratedWaybillStatus sendMultipleDespatch(List<LogoWaybill> despatches, bool isLoggedIn)
        {
            string remoteDespatchNumber = "";
            string message = "";
            List<IntegratedWaybillDto> receivedWaybills = new List<IntegratedWaybillDto>();
            try
            {
                if (isLoggedIn)
                {
                    foreach (var despatch in despatches)
                    { 
                        Data newDespatch = unity.NewDataObject(DataObjectType.doSalesDispatch);
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
                                }
                                else
                                {
                                    newWaybillLines[i].FieldByName("TYPE").Value = detail.type;
                                    if (isProducerCode) // bazı distlerde ürün kodları producerCode'a yazılı
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
                                    newWaybillLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit(detail.unitCode);
                                    newWaybillLines[i].FieldByName("PAYMENT_CODE").Value = despatch.paymentCode;
                                    newWaybillLines[i].FieldByName("VAT_RATE").Value = Convert.ToInt32(detail.vatRate);
                                    newWaybillLines[i].FieldByName("VAT_AMOUNT").Value = detail.vatAmount;
                                    newWaybillLines[i].FieldByName("VAT_BASE").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                    newWaybillLines[i].FieldByName("TOTAL_NET").Value = Convert.ToDouble(detail.price) * detail.quantity;
                                    newWaybillLines[i].FieldByName("SALEMANCODE").Value = despatch.salesmanCode;
                                    newWaybillLines[i].FieldByName("MONTH").Value = DateTime.Now.Month;
                                    newWaybillLines[i].FieldByName("YEAR").Value = DateTime.Now.Year;
                                    newWaybillLines[i].FieldByName("BARCODE").Value = detail.barcode;
                                    if (despatch.type == (int)InvoiceType.SELLING_RETURN || despatch.type == (int)InvoiceType.SELLING || despatch.type == (int)InvoiceType.SELLING_SERVICE) // satış , satış iade ve verilen hizmet ise satış fiyatı üzerinden çalışsın denildi
                                    {

                                        newWaybillLines[i].FieldByName("PRCLISTTYPE").Value = 2;
                                    }
                                    else
                                    {
                                        newWaybillLines[i].FieldByName("PRCLISTTYPE").Value = 1;
                                    }

                                    if (despatch.type == (int) InvoiceType.SELLING_RETURN)  // iade faturaları 
                                    {
                                        newWaybillLines[i].FieldByName("RET_COST_TYPE").Value = 1;
                                    }
                                }
                            }
                        }
                        newDespatch.DataFields.FieldByName("EINVOICE").Value = reader.getEInvoiceByCustomerCode(despatch.customerCode);
                        newDespatch.DataFields.FieldByName("AFFECT_RISK").Value = 0;
                        newDespatch.DataFields.FieldByName("DOC_DATE").Value = despatch.documentDate.ToShortDateString();
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
        public IntegratedOrderStatus sendMultipleOrder(List<Order> orders, bool isLoggedIn)
        {
            string message = "";
            long remoteOrderId = 0;
            List<IntegratedOrderDto> receivedOrders = new List<IntegratedOrderDto>();
            try
            {
                if (isLoggedIn)
                {
                    foreach (var order in orders)
                    {
                        Data newOrder = unity.NewDataObject(DataObjectType.doSalesOrderSlip);
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
                        if (useCypheCode)
                        {
                            newOrder.DataFields.FieldByName("AUTH_CODE").Value = order.customer.code.Substring(2, 2);
                        }
                        newOrder.DataFields.FieldByName("AUXIL_CODE").Value = order.customer.code.Substring(4, 2);
                        newOrder.DataFields.FieldByName("DIVISION").Value = order.customerBranch.code;
                        newOrder.DataFields.FieldByName("TOTAL_VAT").Value = order.vatTotal;
                        newOrder.DataFields.FieldByName("TOTAL_GROSS").Value = order.grossTotal;
                        newOrder.DataFields.FieldByName("TOTAL_NET").Value = order.preVatNetTotal;
                        newOrder.DataFields.FieldByName("NOTES1").Value = order.salesmanNote;
                        newOrder.DataFields.FieldByName("PAYMENT_CODE").Value = order.paymentType.code;
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
                                    newOrderLines[i].FieldByName("QUANTITY").Value = detail.quantity;
                                    newOrderLines[i].FieldByName("PRICE").Value = Convert.ToDouble(detail.orderItemPrice);
                                    newOrderLines[i].FieldByName("TOTAL").Value = detail.grossTotal;
                                    newOrderLines[i].FieldByName("CURR_PRICE").Value = 160;  // currency TL
                                    newOrderLines[i].FieldByName("UNIT_CODE").Value = helper.getUnit(detail.unitCode);
                                    newOrderLines[i].FieldByName("PAYMENT_CODE").Value = order.paymentType.code;
                                    newOrderLines[i].FieldByName("DUE_DATE").Value = Convert.ToDateTime(order.orderDate.ToString("dd-MM-yyyy"));
                                    newOrderLines[i].FieldByName("VAT_RATE").Value = Convert.ToInt32(detail.vatRate);
                                    newOrderLines[i].FieldByName("VAT_AMOUNT").Value = detail.vatTotal;
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
                            IntegratedOrderDto recievedOrder = new IntegratedOrderDto(message, integratedOrderRef, order.orderId, true);
                            receivedOrders.Add(recievedOrder);
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
    }
}
