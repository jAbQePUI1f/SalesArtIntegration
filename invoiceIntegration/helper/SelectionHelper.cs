
using invoiceIntegration.model;
using invoiceIntegration.model.order;
using invoiceIntegration.model.waybill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static invoiceIntegration.frmMain;

namespace invoiceIntegration
{
    public class SelectionHelper
    {      
        public List<LogoInvoice> GetSelectedInvoices(DataGridView dataGridInvoice, GenericResponse<List<LogoInvoiceJson>> jsonInvoices)
        {
            List<LogoInvoice> invoices = new List<LogoInvoice>();
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value))
                {
                    string number = row.Cells["number"].Value.ToString();
                    LogoInvoiceJson selectedInvoice = jsonInvoices.data.FirstOrDefault(inv => inv.number == number);
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
                        //if (selectedInvoice.invoiceType == InvoiceType.BUYING_SERVICE || selectedInvoice.invoiceType == InvoiceType.SELLING_SERVICE)
                        //{
                        //    invDetail.type = (int)selectedInvoice.invoiceType;
                        //}
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
        public List<LogoWaybill> GetSelectedWaybills(DataGridView dataGridInvoice, GenericResponse<List<LogoWaybillJson>> jsonWaybills)
        {
            List<LogoWaybill> waybills = new List<LogoWaybill>();
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    string number = row.Cells["number"].Value.ToString();
                    LogoWaybillJson selectedWaybill = jsonWaybills.data.FirstOrDefault(inv => inv.number == number);
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
        public List<Order> GetSelectedOrders(DataGridView dataGridInvoice, GenericResponse<OrderResponse> jsonOrders)
        {
            List<Order> orders = new List<Order>();
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value))
                {
                    string number = row.Cells["number"].Value.ToString();
                    Order selectedOrder = jsonOrders.data.orders.FirstOrDefault(inv => inv.receiptNumber == number);
                    Order order = new Order();
                    order.type =(int) InvoiceType.SELLING_RETURN;
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
                        //ordDetail.vatIncluded = selectedOrderDetail.vatIncluded;
                        ordDetail.vatTotal = selectedOrderDetail.vatTotal;
                        ordDetail.preVatNetTotal = selectedOrderDetail.preVatNetTotal;
                        ordDetail.productBarcode = selectedOrderDetail.productBarcode;
                        ordDetail.lineOrder = selectedOrderDetail.lineOrder;
                        ordDetail.grossTotal = selectedOrderDetail.grossTotal;
                        ordDetail.vatRate = selectedOrderDetail.vatRate;
                        ordDetail.unitCode = selectedOrderDetail.unitCode;
                        List<OrderDetail> orderDetailDiscountDetails = new List<OrderDetail>();
                        if (selectedOrderDetail.discountDetails != null)
                        {
                            foreach (var discount in selectedOrderDetail.discountDetails)
                            {
                                OrderDetail ordDetailDiscountDetail = new OrderDetail();
                                ordDetailDiscountDetail.type = 2;
                                ordDetailDiscountDetail.rate = discount.rate;
                                ordDetailDiscountDetail.discountTotal = discount.discountTotal;
                                ordDetailDiscountDetail.price = ordDetail.price;
                                ordDetailDiscountDetail.grossTotal = ordDetail.grossTotal;
                                ordDetailDiscountDetail.productName = discount.name;
                                orderDetailDiscountDetails.Add(ordDetailDiscountDetail);
                            }
                        }
                        orderDetails.Add(ordDetail);
                        if (orderDetailDiscountDetails.Count > 0)// discountlar da bir detay olarak eklendi ve bu detaylar order detail e eklendi
                        {
                            foreach (var orderDetailDiscountDetail in orderDetailDiscountDetails)
                            {
                                orderDetails.Add(orderDetailDiscountDetail);
                            }
                        }
                    }
                    order.details = orderDetails;
                    orders.Add(order);
                }
            }
            return orders;
        }
        public List<LogoInvoiceJson> GetSelectedInvoicesForMikro(DataGridView dataGridInvoice, GenericResponse<List<LogoInvoiceJson>> dataList)
        {
            List<LogoInvoiceJson> invoices = new List<LogoInvoiceJson>();
            foreach (DataGridViewRow row in dataGridInvoice.Rows)
            {
                if (Convert.ToBoolean(row.Cells["chk"].Value) == true)
                {
                    string number = row.Cells["number"].Value.ToString();
                    LogoInvoiceJson selectedInvoice = dataList.data.FirstOrDefault(inv => inv.number == number);
                    invoices.Add(selectedInvoice);
                }
            }
            return invoices;
        }
    }
}
