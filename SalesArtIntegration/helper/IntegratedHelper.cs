using invoiceIntegration.config;
using invoiceIntegration.model;
using invoiceIntegration.model.Collection;
using invoiceIntegration.model.order;
using invoiceIntegration.model.waybill;
using invoiceIntegration.repository;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityObjects;
using Configuration = invoiceIntegration.config.Configuration;

namespace invoiceIntegration.helper
{
    public class IntegratedHelper
    {
        bool isProducerCode = Configuration.getIsProducerCode();
        bool useCypheCode = Configuration.getUseCypheCode();
        string cypheCode = Configuration.getCypheCode();
        string cashCode = Configuration.getCashCode();
        bool useDefaultNumber = Configuration.getUseDefaultNumber();
        bool useShortDate = Configuration.getUseShortDate();
        int distributorId = Configuration.getDistributorId();
        bool useDispatch = Configuration.getUseDispatch();
        bool integrationForMikroERP = Configuration.getIntegrationForMikroERP();
        string shipAgentCode = Configuration.getShipAgentCode();
        string campaignLineNo = Configuration.getCampaignLineNo();
        string affectRisk = Configuration.getAffectRisk();
        LogoDataReader reader = new LogoDataReader();
        Helper helper = new Helper();
        IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
        IntegratedWaybillStatus integratedWaybills = new IntegratedWaybillStatus();
        IntegratedOrderStatus integratedOrders = new IntegratedOrderStatus();
        IntegratedCollectionStatus IntegratedCollectionStatus = new IntegratedCollectionStatus();
        public IntegratedInvoiceStatus sendMultipleInvoice(List<LogoInvoice> invoices)
        {
            UnityApplication unity = LogoApplication.getApplication();

            string remoteInvoiceNumber = "";
            string message = "";
            List<IntegratedInvoiceDto> receivedInvoices = new List<IntegratedInvoiceDto>();
            try
            {
                foreach (var invoice in invoices)
                {
                    Data newInvoice = unity.NewDataObject(DataObjectType.doSalesInvoice);
                    remoteInvoiceNumber = reader.getInvoiceNumberByDocumentNumber(invoice.number);

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
                        newInvoice.DataFields.FieldByName("DIVISION").Value = helper.getDivision();
                        newInvoice.DataFields.FieldByName("AFFECT_RISK").Value = affectRisk;
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
                                    dispatches_lines[0].FieldByName("NUMBER").Value = invoice.number;
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
                                dispatches_lines[0].FieldByName("AFFECT_RISK").Value = affectRisk;
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
                                    newInvoiceLines[i].FieldByName("AFFECT_RISK").Value = affectRisk;
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
                                    if (isProducerCode)
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
                                    else if (Configuration.getIsBarcode())
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

                                    if (invoice.type == (int)InvoiceType.SELLING || invoice.type == (int)InvoiceType.SELLING_RETURN || invoice.type == (int)InvoiceType.SELLING_SERVICE)
                                    {
                                        newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 2;
                                    }
                                    else
                                    {
                                        newInvoiceLines[i].FieldByName("PRCLISTTYPE").Value = 1;
                                    }
                                    if (invoice.type == (int)InvoiceType.SELLING_RETURN)
                                    {
                                        newInvoiceLines[i].FieldByName("RET_COST_TYPE").Value = 1;
                                    }
                                }
                            }
                        }
                        Lines paymentList = newInvoice.DataFields.FieldByName("PAYMENT_LIST").Lines;
                        newInvoice.DataFields.FieldByName("EINVOICE").Value = reader.getEInvoiceByCustomerCode(invoice.customerCode);
                        newInvoice.DataFields.FieldByName("PROFILE_ID").Value = reader.getProfileIDByCustomerCode(invoice.customerCode);
                        newInvoice.DataFields.FieldByName("AFFECT_RISK").Value = affectRisk;
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
            catch (Exception ex)
            {
                IntegratedInvoiceDto recievedInvoice = new IntegratedInvoiceDto(ex.Message.ToString(), "", remoteInvoiceNumber, false);
                receivedInvoices.Add(recievedInvoice);
            }
            finally
            {
                unity.UserLogout();
                unity.Disconnect();
                message = "";
            }
            integratedInvoices.integratedInvoices = receivedInvoices;
            integratedInvoices.distributorId = distributorId;
            return integratedInvoices;
        }
        public IntegratedWaybillStatus sendMultipleDespatch(List<LogoWaybill> despatches)
        {
            UnityApplication unity = LogoApplication.getApplication();

            string remoteDespatchNumber = "";
            string message = "";
            List<IntegratedWaybillDto> receivedWaybills = new List<IntegratedWaybillDto>();
            try
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
                    newDespatch.DataFields.FieldByName("NUMBER").Value = despatch.number;
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
                    newDespatch.DataFields.FieldByName("DIVISION").Value = helper.getDivision();
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

                                if (despatch.type == (int)InvoiceType.SELLING_RETURN)
                                {
                                    newWaybillLines[i].FieldByName("RET_COST_TYPE").Value = 1;
                                }
                            }
                        }
                    }
                    newDespatch.DataFields.FieldByName("EINVOICE").Value = reader.getEInvoiceByCustomerCode(despatch.customerCode);
                    newDespatch.DataFields.FieldByName("AFFECT_RISK").Value = affectRisk;
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
            catch (Exception ex)
            {
                IntegratedWaybillDto recievedWaybill = new IntegratedWaybillDto(ex.Message.ToString(), "", remoteDespatchNumber, false);
                receivedWaybills.Add(recievedWaybill);
            }
            finally
            {
                unity.UserLogout();
                unity.Disconnect();
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
            UnityApplication unity = LogoApplication.getApplication();

            string message = "";
            long remoteOrderId = 0;
            List<IntegratedOrderDto> receivedOrders = new List<IntegratedOrderDto>();
            try
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
            catch (Exception ex)
            {
                IntegratedOrderDto recievedOrder = new IntegratedOrderDto(ex.Message.ToString(), remoteOrderId, 0, false);
                receivedOrders.Add(recievedOrder);
            }
            finally
            {
                unity.UserLogout();
                unity.Disconnect();
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
        #region -- paymentCodeStartLine
        //    public IntegratedCollectionStatus sendMultipleCollections(List<LogoCollectionModel> logoCollections)
        //    {
        //        UnityApplication unity = LogoApplication.getApplication();
        //        string remoteNumber = "";
        //        string message = "";
        //        List<IntegratedCollectionDto> ıntegratedCollectionDtos = new List<IntegratedCollectionDto>();
        //        List<LogoCollectionModel> otherPaymentList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> otherPaymentDetail = new List<LogoCollectionModelDetail>();

        //        List<LogoCollectionModel> cashPaymentList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> cashPaymentDetail = new List<LogoCollectionModelDetail>();

        //        List<LogoCollectionModel> creditPaymentList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> creditPaymentDetail = new List<LogoCollectionModelDetail>();

        //        List<LogoCollectionModel> iadePaymentList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> iadePaymentDetail = new List<LogoCollectionModelDetail>();

        //        List<LogoCollectionModel> senetPaymentList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> senetPaymentDetail = new List<LogoCollectionModelDetail>();

        //        List<LogoCollectionModel> bankHavaleList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> bankHavaleDetail = new List<LogoCollectionModelDetail>();

        //        List<LogoCollectionModel> alacakDekontList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> alacakDekontDetail = new List<LogoCollectionModelDetail>();

        //        List<LogoCollectionModel> borcDekontList = new List<LogoCollectionModel>();
        //        List<LogoCollectionModelDetail> borcDekontDetail = new List<LogoCollectionModelDetail>();
        //        int control = 0;
        //        try
        //        {
        //            foreach (var item in logoCollections)
        //            {
        //                IntegratedCollectionDto trCash = null;
        //                IntegratedCollectionDto trCredit = null;
        //                IntegratedCollectionDto trSenet = null;
        //                IntegratedCollectionDto trOther = null;
        //                IntegratedCollectionDto trIade = null;
        //                IntegratedCollectionDto trAlacak = null;
        //                IntegratedCollectionDto trBorc = null;

        //                otherPaymentDetail.Clear();
        //                cashPaymentDetail.Clear();
        //                creditPaymentDetail.Clear();
        //                senetPaymentDetail.Clear();
        //                iadePaymentDetail.Clear();
        //                otherPaymentList.Clear();
        //                cashPaymentList.Clear();
        //                creditPaymentList.Clear();
        //                senetPaymentList.Clear();
        //                iadePaymentList.Clear();
        //                bankHavaleList.Clear();
        //                bankHavaleDetail.Clear();
        //                alacakDekontList.Clear();
        //                alacakDekontDetail.Clear();
        //                borcDekontList.Clear();
        //                borcDekontDetail.Clear();


        //                foreach (LogoCollectionModelDetail paymentDetail in item.collectionModelDetail)
        //                {
        //                    if (paymentDetail.PaymentType == "2")
        //                        creditPaymentDetail.Add(paymentDetail);
        //                    else if (paymentDetail.PaymentType == "5")
        //                    {
        //                        senetPaymentDetail.Add(paymentDetail);
        //                    }
        //                    else if (paymentDetail.PaymentType == "3")
        //                    {
        //                        otherPaymentDetail.Add(paymentDetail);
        //                    }
        //                    else if (paymentDetail.PaymentType == "6")
        //                    {
        //                        iadePaymentDetail.Add(paymentDetail);
        //                    }
        //                    else if (paymentDetail.PaymentType == "4")
        //                    {
        //                        bankHavaleDetail.Add(paymentDetail);
        //                    }
        //                    else if (paymentDetail.PaymentType == "10") 
        //                    {
        //                        alacakDekontDetail.Add(paymentDetail);
        //                    }
        //                    else if (paymentDetail.PaymentType == "11")
        //                    {
        //                        borcDekontDetail.Add(paymentDetail);
        //                    }
        //                    else
        //                        cashPaymentDetail.Add(paymentDetail);
        //                }

        //                if (control == 1)
        //                {
        //                    control = 0;
        //                    continue;
        //                }


        //                if (otherPaymentDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in otherPaymentDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = otherPaymentDetail;
        //                    otherPaymentList.Add(item);
        //                }
        //                if (otherPaymentList.Count > 0)
        //                {
        //                    trOther = CreateCekSenetPayment(otherPaymentList, "1");
        //                }

        //                if (cashPaymentDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in cashPaymentDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = cashPaymentDetail;
        //                    cashPaymentList.Add(item);
        //                }
        //                if (cashPaymentList.Count > 0)
        //                {
        //                    trCash = CreatePaymentsKasaHar(cashPaymentList);
        //                }

        //                if (bankHavaleDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in bankHavaleDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = bankHavaleDetail;
        //                    bankHavaleList.Add(item);
        //                }
        //                if (bankHavaleList.Count > 0)
        //                {
        //                    trCash = CreateBankHavales(bankHavaleList);
        //                }

        //                if (creditPaymentDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in creditPaymentDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = creditPaymentDetail;
        //                    creditPaymentList.Add(item);

        //                }
        //                if (creditPaymentList.Count > 0)
        //                {
        //                    trCredit = CreatePaymentsOld(creditPaymentList, "70");
        //                }

        //                if (alacakDekontDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in alacakDekontDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = alacakDekontDetail;
        //                    alacakDekontList.Add(item);
        //                }
        //                if (alacakDekontList.Count > 0)
        //                {
        //                    trAlacak = CreateBorcAlacakDekont(alacakDekontList, "4");
        //                }

        //                if (borcDekontDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in borcDekontDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = borcDekontDetail;
        //                    borcDekontList.Add(item);
        //                }
        //                if (alacakDekontList.Count > 0)
        //                {
        //                    trBorc = CreateBorcAlacakDekont(borcDekontList, "3");
        //                }
        //                //end new
        //                if (iadePaymentDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in iadePaymentDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = iadePaymentDetail;
        //                    iadePaymentList.Add(item);
        //                }
        //                if (iadePaymentList.Count > 0)
        //                {
        //                    trIade = CreatePaymentsOld(iadePaymentList, "1");
        //                }

        //                if (senetPaymentDetail.Count > 0)
        //                {
        //                    item.collectionModelHeader.Amount = 0;
        //                    foreach (LogoCollectionModelDetail paymentDetail in senetPaymentDetail)
        //                        item.collectionModelHeader.Amount += paymentDetail.Amount;
        //                    item.collectionModelDetail = senetPaymentDetail;
        //                    senetPaymentList.Add(item);
        //                }
        //                if (senetPaymentList.Count > 0)
        //                {
        //                    trSenet = CreateCekSenetPayment(senetPaymentList, "2");
        //                }

        //                string ErrorMessage = "";

        //                if (trCash.errorMessage != null && trCash.errorMessage.Length > 0)
        //                {
        //                    ErrorMessage += "Nakit Error:" + trCash.errorMessage;
        //                }
        //                if (trSenet.errorMessage != null && trSenet.errorMessage.Length > 0)
        //                {
        //                    ErrorMessage += "Senet Error:" + trSenet.errorMessage;
        //                }
        //                if (trOther.errorMessage != null && trOther.errorMessage.Length > 0)
        //                {
        //                    ErrorMessage += "Cek Error:" + trOther.errorMessage;
        //                }
        //                if (trCredit.errorMessage != null && trCredit.errorMessage.Length > 0)
        //                {
        //                    ErrorMessage += "KrediKart Error:" + trCredit.errorMessage;
        //                }
        //                if (trIade.errorMessage != null && trIade.errorMessage.Length > 0)
        //                {
        //                    ErrorMessage += "IadeTahsilat Error:" + trIade.errorMessage;
        //                }
        //                if (trBorc.errorMessage != null && trBorc.errorMessage.Length > 0)
        //                {
        //                    ErrorMessage += "Borc Dekont Error:" + trBorc.errorMessage;
        //                }
        //                if (trAlacak.errorMessage != null && trAlacak.errorMessage.Length > 0)
        //                {
        //                    ErrorMessage += "Alacak Dekont Error:" + trAlacak.errorMessage;
        //                }

        //                try
        //                {


        //                    if (ErrorMessage.Trim().Length > 0)
        //                    {
        //                        IntegratedCollectionDto recievedInvoice = new IntegratedCollectionDto(item.collectionModelHeader.Number + " belge numaralı tahsilat, sistemde zaten mevcut. Kontrol Ediniz", item.collectionModelHeader.Number, remoteNumber, false);
        //                        ıntegratedCollectionDtos.Add(recievedInvoice);
        //                    }
        //                    else
        //                    {
        //                        IntegratedCollectionDto recievedInvoice = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, item.collectionModelHeader.Number, true);
        //                        ıntegratedCollectionDtos.Add(recievedInvoice);
        //                    }


        //                }
        //                catch (Exception ex)
        //                {

        //                }

        //            }
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //        IntegratedCollectionStatus.collections = ıntegratedCollectionDtos;
        //        IntegratedCollectionStatus.distributorId = distributorId;
        //        return IntegratedCollectionStatus;
        //    }

        //    public IntegratedCollectionDto CreateCekSenetPayment(List<LogoCollectionModel> logoCollectionModels, string belgeTipi)
        //    {
        //        UnityApplication unity = LogoApplication.getApplication();

        //        string remoteDespatchNumber = "";
        //        string message = "";
        //        IntegratedCollectionDto collectionDtoResult = null;
        //        try
        //        {
        //            foreach (var item in logoCollectionModels)
        //            {
        //                Data paymentData = unity.NewDataObject(DataObjectType.doCQPnRoll);
        //                paymentData.New();
        //                paymentData.DataFields.FieldByName("MASTER_MODULE").Value = "5";
        //                paymentData.DataFields.FieldByName("MASTER_CODE").Value = item.collectionModelHeader.CustomerCode;
        //                paymentData.DataFields.FieldByName("TYPE").Value = belgeTipi;

        //                if (useShortDate)
        //                {
        //                    paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToShortDateString());
        //                }
        //                else
        //                {
        //                    paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToString("dd-MM-yyyy"));
        //                }
        //                paymentData.DataFields.FieldByName("NUMBER").Value = item.collectionModelHeader.Number;

        //                Lines tranLines = paymentData.DataFields.FieldByName("TRANSACTIONS").Lines;

        //                int a = 0;
        //                foreach (LogoCollectionModelDetail detail in item.collectionModelDetail)
        //                {
        //                    tranLines.AppendLine();
        //                    if (detail.PaymentType == "3")
        //                    {
        //                        tranLines[a].FieldByName("TYPE").Value = "1";

        //                        if (detail.BankName != null)
        //                            tranLines[a].FieldByName("BANK_TITLE").Value = detail.BankName;
        //                        else
        //                            tranLines[a].FieldByName("BANK_TITLE").Value = "BANKA";


        //                        if (detail.SerialNo != null)
        //                            tranLines[a].FieldByName("SERIAL_NR").Value = detail.SerialNo;
        //                        else
        //                            tranLines[a].FieldByName("SERIAL_NR").Value = "000000";

        //                        tranLines[a].FieldByName("DIVISION_NO").Value = detail.DivisionNo;

        //                        tranLines[a].FieldByName("ACCOUNT_NO").Value = detail.AccountNo;

        //                        tranLines[a].FieldByName("CS_IBAN").Value = detail.Iban;

        //                        tranLines[a].FieldByName("CITY").Value = detail.CityName;

        //                        tranLines[a].FieldByName("INFORMANT").Value = detail.BankBranch;
        //                    }
        //                    else if (detail.PaymentType == "5")
        //                    {
        //                        tranLines[a].FieldByName("TYPE").Value = "2";



        //                        tranLines[a].FieldByName("CITY").Value = detail.CityName;
        //                        tranLines[a].FieldByName("GUARANTOR").Value = detail.GuarantorName;
        //                    }

        //                    tranLines[a].FieldByName("NUMBER").Value = detail.DocNumber;
        //                    tranLines[a].FieldByName("TAX_NR").Value = detail.TaxNr;

        //                    tranLines[a].FieldByName("OWING").Value = detail.Owing;

        //                    tranLines[a].FieldByName("DUE_DATE").Value = detail.OrderDate.ToString("dd.MM.yyyy");
        //                    tranLines[a].FieldByName("DATE").Value = detail.PaymentDate.ToString("dd.MM.yyyy");

        //                    tranLines[a].FieldByName("AMOUNT").Value = detail.Amount;
        //                    tranLines[a].FieldByName("SALESMAN_CODE").Value = item.collectionModelHeader.SalesmanCode;
        //                    tranLines[a].FieldByName("AFFECT_RISK").Value = affectRisk;

        //                    a++;
        //                }
        //                paymentData.DataFields.FieldByName("AFFECT_RISK").Value = affectRisk;

        //                ValidateErrors err = paymentData.ValidateErrors;
        //                helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
        //                if (paymentData.Post())
        //                {
        //                    var integratedWaybillRef = paymentData.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
        //                    paymentData.Read(integratedWaybillRef);
        //                    remoteDespatchNumber = paymentData.DataFields.FieldByName("NUMBER").Value;
        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, true);
        //                    return recievedCollection;
        //                }
        //                else
        //                {
        //                    if (paymentData.ErrorCode != 0)
        //                    {
        //                        message = "DBError(" + paymentData.ErrorCode.ToString() + ")-" + paymentData.ErrorDesc + paymentData.DBErrorDesc;
        //                        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                        return recievedCollection;
        //                    }
        //                    else if (paymentData.ValidateErrors.Count > 0)
        //                    {
        //                        for (int i = 0; i < err.Count; i++)
        //                        {
        //                            message += err[i].Error;
        //                        }

        //                        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                        return recievedCollection;
        //                    }
        //                }
        //                helper.LogFile("POST Bitti", "-", "-", "-", "-");

        //            }
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //        return collectionDtoResult;
        //    }

        //    public IntegratedCollectionDto CreatePaymentsKasaHar(List<LogoCollectionModel> logoCollectionModels)
        //    {
        //        UnityApplication unity = LogoApplication.getApplication();
        //        string remoteDespatchNumber = "";
        //        string message = "";
        //        IntegratedCollectionDto collectionDtoResult = null;

        //        foreach (var item in logoCollectionModels)
        //        {

        //            Data paymentData = unity.NewDataObject(DataObjectType.doSafeDepositTrans);

        //            paymentData.New();

        //            paymentData.DataFields.FieldByName("TYPE").Value = "11";

        //            paymentData.DataFields.FieldByName("SD_CODE").Value = cashCode;

        //            if (useShortDate)
        //            {
        //                paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToShortDateString());
        //            }
        //            else
        //            {
        //                paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToString("dd-MM-yyyy"));
        //            }
        //            paymentData.DataFields.FieldByName("NUMBER").Value = item.collectionModelHeader.Number;
        //            paymentData.DataFields.FieldByName("AUTH_CODE").Value = cypheCode;

        //            paymentData.DataFields.FieldByName("MASTER_TITLE").Value = item.collectionModelHeader.CustomerName;
        //            paymentData.DataFields.FieldByName("DESCRIPTION").Value = item.collectionModelHeader.Desc + " - TAHSILAT";
        //            paymentData.DataFields.FieldByName("AMOUNT").Value = item.collectionModelHeader.Amount.ToString().Replace(",", ".");
        //            paymentData.DataFields.FieldByName("TC_AMOUNT").Value = item.collectionModelHeader.TCAmount.ToString().Replace(",", ".");

        //            Lines paymentLines = paymentData.DataFields.FieldByName("ATTACHMENT_ARP").Lines;

        //            double totalCash = 0;
        //            int a = 0;
        //            foreach (LogoCollectionModelDetail detail in item.collectionModelDetail)
        //            {
        //                paymentLines.AppendLine();

        //                paymentLines[a].FieldByName("ARP_CODE").Value = item.collectionModelHeader.CustomerCode;
        //                paymentLines[a].FieldByName("TRANNO").Value = detail.TranNo;
        //                paymentLines[a].FieldByName("DOC_NUMBER").Value = detail.DocNumber;

        //                DateTime payDate = Convert.ToDateTime(detail.PaymentDate);
        //                paymentLines[a].FieldByName("MONTH").Value = payDate.Month.ToString();
        //                paymentLines[a].FieldByName("YEAR").Value = payDate.Year.ToString();

        //                DateTime Tarih = DateTime.Parse(detail.PaymentDate.ToString());
        //                string tarih1 = Tarih.ToString("dd.MM.yyyy");
        //                paymentLines[a].FieldByName("PROCDATE").Value = tarih1;
        //                paymentLines[a].FieldByName("DISCOUNT_DUEDATE").Value = tarih1;
        //                paymentLines[a].FieldByName("DESCRIPTION").Value = detail.PaymentTypeName + " - Tahsilat";

        //                paymentLines[a].FieldByName("AFFECT_RISK").Value = affectRisk;
        //                paymentLines[a].FieldByName("SALESMAN_CODE").Value = item.collectionModelHeader.SalesmanCode;

        //                a++;
        //            }


        //            paymentLines[0].FieldByName("CREDIT").Value = item.collectionModelHeader.Amount.ToString().Replace(",", ".");
        //            paymentLines[0].FieldByName("TC_AMOUNT").Value = item.collectionModelHeader.Amount.ToString().Replace(",", ".");

        //            ValidateErrors err = paymentData.ValidateErrors;
        //            helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
        //            if (paymentData.Post())
        //            {
        //                var integratedRef = paymentData.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
        //                paymentData.Read(integratedRef);
        //                remoteDespatchNumber = paymentData.DataFields.FieldByName("NUMBER").Value;
        //                IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, true);
        //                return recievedCollection;
        //            }
        //            else
        //            {
        //                if (paymentData.ErrorCode != 0)
        //                {
        //                    message = "DBError(" + paymentData.ErrorCode.ToString() + ")-" + paymentData.ErrorDesc + paymentData.DBErrorDesc;
        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                    return recievedCollection;
        //                }
        //                else if (paymentData.ValidateErrors.Count > 0)
        //                {
        //                    for (int i = 0; i < err.Count; i++)
        //                    {
        //                        message += err[i].Error;
        //                    }

        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                    return recievedCollection;
        //                }
        //            }
        //            helper.LogFile("POST Bitti", "-", "-", "-", "-");
        //        }
        //        return collectionDtoResult;
        //    }

        //    public IntegratedCollectionDto CreateBankHavales(List<LogoCollectionModel> logoCollectionModels)
        //    {
        //        UnityApplication unity = LogoApplication.getApplication();

        //        string remoteDespatchNumber = "";
        //        string message = "";
        //        IntegratedCollectionDto collectionDtoResult = null;

        //        foreach (var item in logoCollectionModels)
        //        {

        //            IData paymentData = unity.NewDataObject(DataObjectType.doBankVoucher);
        //            paymentData.New();

        //            paymentData.DataFields.FieldByName("TYPE").Value = "3";

        //            if (useShortDate)
        //            {
        //                paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToShortDateString());
        //            }
        //            else
        //            {
        //                paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToString("dd-MM-yyyy"));
        //            }

        //            paymentData.DataFields.FieldByName("NUMBER").Value = item.collectionModelHeader.Number;

        //            paymentData.DataFields.FieldByName("NOTES1").Value = item.collectionModelHeader.Notes1;

        //            Lines paymentLines = paymentData.DataFields.FieldByName("TRANSACTIONS").Lines;
        //            int a = 0;
        //            foreach (LogoCollectionModelDetail detail in item.collectionModelDetail)
        //            {
        //                paymentLines.AppendLine();

        //                paymentLines[a].FieldByName("TYPE").Value = 1;

        //                paymentLines[a].FieldByName("BANKACC_CODE").Value = detail.BankAccCode;


        //                paymentLines[a].FieldByName("AMOUNT").Value = detail.Amount.ToString().Replace(",", ".");
        //                paymentLines[a].FieldByName("TC_AMOUNT").Value = detail.Amount.ToString().Replace(",", ".");
        //                paymentLines[a].FieldByName("DEBIT").Value = detail.Amount.ToString().Replace(",", ".");

        //                paymentLines[a].FieldByName("DOC_NUMBER").Value = detail.DocNumber;

        //                DateTime tarih = DateTime.Parse(detail.PaymentDate.ToString());
        //                string tarihs = tarih.ToString("dd.MM.yyyy");
        //                paymentLines[a].FieldByName("DATE").Value = tarihs;
        //                paymentLines[a].FieldByName("DUE_DATE").Value = tarihs;

        //                paymentLines[a].FieldByName("BANK_PROC_TYPE").Value = 2;

        //                paymentLines[a].FieldByName("AFFECT_RISK").Value = affectRisk;
        //                paymentData.DataFields.FieldByName("SALESMAN_CODE").Value = item.collectionModelHeader.SalesmanCode;
        //            }

        //            ValidateErrors err = paymentData.ValidateErrors;
        //            helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
        //            if (paymentData.Post())
        //            {
        //                var integratedRef = paymentData.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
        //                paymentData.Read(integratedRef);
        //                remoteDespatchNumber = paymentData.DataFields.FieldByName("NUMBER").Value;
        //                IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, true);
        //                return recievedCollection;
        //            }
        //            else
        //            {
        //                if (paymentData.ErrorCode != 0)
        //                {
        //                    message = "DBError(" + paymentData.ErrorCode.ToString() + ")-" + paymentData.ErrorDesc + paymentData.DBErrorDesc;
        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                    return recievedCollection;
        //                }
        //                else if (paymentData.ValidateErrors.Count > 0)
        //                {
        //                    for (int i = 0; i < err.Count; i++)
        //                    {
        //                        message += err[i].Error;
        //                    }

        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                    return recievedCollection;
        //                }
        //            }
        //            helper.LogFile("POST Bitti", "-", "-", "-", "-");

        //        }
        //        return collectionDtoResult;
        //    }

        //    public IntegratedCollectionDto CreatePaymentsOld(List<LogoCollectionModel> logoCollectionModels, string belgeTipi)
        //    {
        //        UnityApplication unity = LogoApplication.getApplication();

        //        string remoteDespatchNumber = "";
        //        string message = "";
        //        IntegratedCollectionDto collectionDtoResult = null;

        //        foreach (var item in logoCollectionModels)
        //        {
        //            if (!Configuration.getTransferCreditCartToCase())
        //            {
        //                IData paymentData = unity.NewDataObject(DataObjectType.doARAPVoucher);
        //                paymentData.New();


        //                paymentData.DataFields.FieldByName("TYPE").Value = belgeTipi;

        //                paymentData.DataFields.FieldByName("BANKACC_CODE").Value = item.collectionModelHeader.BankAccCode;

        //                paymentData.DataFields.FieldByName("TIME").Value = helper.Hour(item.collectionModelHeader.PaymentDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
        //                if (useShortDate)
        //                {
        //                    paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToShortDateString());
        //                }
        //                else
        //                {
        //                    paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToString("dd-MM-yyyy"));
        //                }

        //                paymentData.DataFields.FieldByName("NUMBER").Value = item.collectionModelHeader.Number;

        //                paymentData.DataFields.FieldByName("TOTAL_CREDIT").Value = item.collectionModelHeader.Amount.ToString().Replace(",", ".");

        //                paymentData.DataFields.FieldByName("AFFECT_RISK").Value = affectRisk;

        //                Lines paymentLines = paymentData.DataFields.FieldByName("TRANSACTIONS").Lines;

        //                int a = 0;
        //                foreach (LogoCollectionModelDetail detail in item.collectionModelDetail)
        //                {
        //                    paymentLines.AppendLine();

        //                    paymentLines[a].FieldByName("DOC_NUMBER").Value = detail.DocNumber;
        //                    paymentLines[a].FieldByName("ARP_CODE").Value = detail.CustomerCode;
        //                    paymentLines[a].FieldByName("TRANNO").Value = detail.TranNo;

        //                    paymentLines[a].FieldByName("BANKACC_CODE").Value = detail.BankAccCode;

        //                    paymentLines[a].FieldByName("CREDIT").Value = detail.Amount.ToString().Replace(",", ".");
        //                    paymentLines[a].FieldByName("TC_AMOUNT").Value = detail.Amount.ToString().Replace(",", ".");
        //                    paymentLines[a].FieldByName("AFFECT_RISK").Value = affectRisk;
        //                    paymentData.DataFields.FieldByName("SALESMAN_CODE").Value = item.collectionModelHeader.SalesmanCode;
        //                }

        //                ValidateErrors err = paymentData.ValidateErrors;
        //                helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
        //                if (paymentData.Post())
        //                {
        //                    var integratedRef = paymentData.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
        //                    paymentData.Read(integratedRef);
        //                    remoteDespatchNumber = paymentData.DataFields.FieldByName("NUMBER").Value;
        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, true);
        //                    return recievedCollection;
        //                }
        //                else
        //                {
        //                    if (paymentData.ErrorCode != 0)
        //                    {
        //                        message = "DBError(" + paymentData.ErrorCode.ToString() + ")-" + paymentData.ErrorDesc + paymentData.DBErrorDesc;
        //                        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                        return recievedCollection;
        //                    }
        //                    else if (paymentData.ValidateErrors.Count > 0)
        //                    {
        //                        for (int i = 0; i < err.Count; i++)
        //                        {
        //                            message += err[i].Error;
        //                        }

        //                        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                        return recievedCollection;
        //                    }
        //                }
        //                helper.LogFile("POST Bitti", "-", "-", "-", "-");
        //            }
        //            else
        //            {
        //                string paymentCode = string.Empty;

        //                IData paymentData = unity.NewDataObject(UnityObjects.DataObjectType.doSafeDepositTrans);

        //                paymentData.New();

        //                paymentData.DataFields.FieldByName("TYPE").Value = "11";
        //                paymentData.DataFields.FieldByName("SD_CODE").Value = item.collectionModelHeader.CashCode;

        //                if (useShortDate)
        //                {
        //                    paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToShortDateString());
        //                }
        //                else
        //                {
        //                    paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToString("dd-MM-yyyy"));
        //                }

        //                paymentData.DataFields.FieldByName("NUMBER").Value = item.collectionModelHeader.Number;

        //                paymentData.DataFields.FieldByName("MASTER_TITLE").Value = item.collectionModelHeader.CustomerName;
        //                paymentData.DataFields.FieldByName("DESCRIPTION").Value = item.collectionModelHeader.Desc + " - TAHSILAT";
        //                paymentData.DataFields.FieldByName("AMOUNT").Value = item.collectionModelHeader.Amount.ToString().Replace(",", ".");
        //                paymentData.DataFields.FieldByName("TC_AMOUNT").Value = item.collectionModelHeader.TCAmount.ToString().Replace(",", ".");

        //                Lines paymentLines = paymentData.DataFields.FieldByName("ATTACHMENT_ARP").Lines;

        //                int a = 0;
        //                foreach (LogoCollectionModelDetail detail in item.collectionModelDetail)
        //                {
        //                    paymentLines.AppendLine();

        //                    paymentLines[a].FieldByName("ARP_CODE").Value = item.collectionModelHeader.CustomerCode;
        //                    paymentLines[a].FieldByName("TRANNO").Value = detail.TranNo;
        //                    paymentLines[a].FieldByName("DOC_NUMBER").Value = detail.DocNumber;
        //                    paymentLines[a].FieldByName("CREDIT").Value = detail.Amount.ToString().Replace(",", ".");
        //                    paymentLines[a].FieldByName("TC_AMOUNT").Value = detail.Amount.ToString().Replace(",", ".");
        //                    DateTime payDate = Convert.ToDateTime(detail.PaymentDate);
        //                    paymentLines[a].FieldByName("MONTH").Value = payDate.Month.ToString();
        //                    paymentLines[a].FieldByName("YEAR").Value = payDate.Year.ToString();

        //                    paymentLines[a].FieldByName("DESCRIPTION").Value = detail.PaymentTypeName + " - Tahsilat";

        //                    paymentLines[a].FieldByName("AFFECT_RISK").Value = affectRisk;
        //                    paymentData.DataFields.FieldByName("SALESMAN_CODE").Value = item.collectionModelHeader.SalesmanCode;

        //                    a++;
        //                }

        //                ValidateErrors err = paymentData.ValidateErrors;
        //                helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
        //                if (paymentData.Post())
        //                {
        //                    var integratedRef = paymentData.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
        //                    paymentData.Read(integratedRef);
        //                    remoteDespatchNumber = paymentData.DataFields.FieldByName("NUMBER").Value;
        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, true);
        //                    return recievedCollection;
        //                }
        //                else
        //                {
        //                    if (paymentData.ErrorCode != 0)
        //                    {
        //                        message = "DBError(" + paymentData.ErrorCode.ToString() + ")-" + paymentData.ErrorDesc + paymentData.DBErrorDesc;
        //                        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                        return recievedCollection;
        //                    }
        //                    else if (paymentData.ValidateErrors.Count > 0)
        //                    {
        //                        for (int i = 0; i < err.Count; i++)
        //                        {
        //                            message += err[i].Error;
        //                        }

        //                        IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                        return recievedCollection;
        //                    }
        //                }
        //                helper.LogFile("POST Bitti", "-", "-", "-", "-");
        //            }
        //        }
        //        return collectionDtoResult;

        //    }

        //    public IntegratedCollectionDto CreateBorcAlacakDekont(List<LogoCollectionModel> logoCollectionModels, string belgeTipi)
        //    {
        //        UnityApplication unity = LogoApplication.getApplication();

        //        string remoteDespatchNumber = "";
        //        string message = "";
        //        IntegratedCollectionDto collectionDtoResult = null;

        //        foreach (var item in logoCollectionModels)
        //        {
        //            IData paymentData = unity.NewDataObject(UnityObjects.DataObjectType.doARAPVoucher);
        //            paymentData.New();

        //            paymentData.DataFields.FieldByName("TYPE").Value = belgeTipi;

        //            if (useShortDate)
        //            {
        //                paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToShortDateString());
        //            }
        //            else
        //            {
        //                paymentData.DataFields.FieldByName("DATE").Value = Convert.ToDateTime(item.collectionModelHeader.PaymentDate.ToString("dd-MM-yyyy"));
        //            }

        //            paymentData.DataFields.FieldByName("NUMBER").Value = item.collectionModelHeader.Number;

        //            Lines paymentLines = paymentData.DataFields.FieldByName("TRANSACTIONS").Lines;

        //            int a = 0;
        //            foreach (LogoCollectionModelDetail detail in item.collectionModelDetail)
        //            {
        //                paymentLines.AppendLine();
        //                paymentLines[a].FieldByName("ARP_CODE").Value = item.collectionModelHeader.CustomerName;
        //                paymentLines[a].FieldByName("TRADING_GRP").Value = item.collectionModelHeader.TradingGroup;
        //                paymentLines[a].FieldByName("TC_AMOUNT").Value = detail.Amount.ToString().Replace(".", ",");
        //                paymentLines[a].FieldByName("DEBIT").Value = detail.Amount.ToString().Replace(".", ",");
        //                paymentLines[a].FieldByName("TRANNO").Value = detail.DocNumber;
        //                paymentLines[a].FieldByName("AFFECT_RISK").Value = affectRisk;
        //                paymentLines[a].FieldByName("SALESMAN_CODE").Value = item.collectionModelHeader.SalesmanCode;

        //                a++;
        //            }

        //            ValidateErrors err = paymentData.ValidateErrors;
        //            helper.LogFile("Post İşlemi Basladı", "-", "-", "-", "-");
        //            if (paymentData.Post())
        //            {
        //                var integratedRef = paymentData.DataFields.FieldByName("INTERNAL_REFERENCE").Value;
        //                paymentData.Read(integratedRef);
        //                remoteDespatchNumber = paymentData.DataFields.FieldByName("NUMBER").Value;
        //                IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, true);
        //                return recievedCollection;
        //            }
        //            else
        //            {
        //                if (paymentData.ErrorCode != 0)
        //                {
        //                    message = "DBError(" + paymentData.ErrorCode.ToString() + ")-" + paymentData.ErrorDesc + paymentData.DBErrorDesc;
        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                    return recievedCollection;
        //                }
        //                else if (paymentData.ValidateErrors.Count > 0)
        //                {
        //                    for (int i = 0; i < err.Count; i++)
        //                    {
        //                        message += err[i].Error;
        //                    }

        //                    IntegratedCollectionDto recievedCollection = new IntegratedCollectionDto(message, item.collectionModelHeader.Number, remoteDespatchNumber, false);
        //                    return recievedCollection;
        //                }
        //            }
        //            helper.LogFile("POST Bitti", "-", "-", "-", "-");
        //        }
        //        return collectionDtoResult;
        //    }
        //}
    //}
        #endregion

