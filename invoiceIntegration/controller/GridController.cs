using invoiceIntegration.helper;
using invoiceIntegration.model;
using invoiceIntegration.model.order;
using invoiceIntegration.model.waybill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invoiceIntegration.controller
{
    public class GridController
    {
        Helper helper = new Helper();
        public void FillGrid(List<dynamic> jsonData, DataGridView dataGridInvoice, string listType)
        {
            dataGridInvoice.Rows.Clear();
            if (jsonData != null)
            {
                foreach (var data in jsonData)
                {
                    int row = dataGridInvoice.Rows.Add();
                    dataGridInvoice.Rows[row].Cells[0].Value = "false";
                    if (listType == constants.ListType.LogoInvoiceJson)
                        dataGridInvoice.Rows[row].Cells[1].Value = (helper.getInvoiceType((int)data.invoiceType)).ToString();
                    else
                        dataGridInvoice.Rows[row].Cells[1].Value = (helper.getInvoiceTypeForWaybill((int)data.waybillType)).ToString();
                    dataGridInvoice.Rows[row].Cells[2].Value = data.number;
                    dataGridInvoice.Rows[row].Cells[3].Value = data.date.ToShortDateString();
                    dataGridInvoice.Rows[row].Cells[4].Value = data.documentNumber;
                    dataGridInvoice.Rows[row].Cells[5].Value = data.customerCode;
                    dataGridInvoice.Rows[row].Cells[6].Value = data.customerName;
                    dataGridInvoice.Rows[row].Cells[7].Value = data.discountTotal.ToString();
                    dataGridInvoice.Rows[row].Cells[8].Value = data.vatTotal.ToString();
                    dataGridInvoice.Rows[row].Cells[9].Value = data.grossTotal.ToString();
                }
            }
        }
        public void FillOrdersToGrid(OrderResponse jsonData, DataGridView dataGridInvoice)
        {
            dataGridInvoice.Rows.Clear();
            foreach (var order in jsonData.orders)
            {
                int row = dataGridInvoice.Rows.Add();
                dataGridInvoice.Rows[row].Cells[0].Value = "false";
                dataGridInvoice.Rows[row].Cells[1].Value = "Satış Siparişi";
                dataGridInvoice.Rows[row].Cells[2].Value = order.receiptNumber;
                dataGridInvoice.Rows[row].Cells[3].Value = order.orderDate.ToShortDateString();
                dataGridInvoice.Rows[row].Cells[4].Value = order.receiptNumber;
                dataGridInvoice.Rows[row].Cells[5].Value = order.customer.code;
                dataGridInvoice.Rows[row].Cells[6].Value = order.customer.name;
                dataGridInvoice.Rows[row].Cells[7].Value = order.discountTotal.ToString();
                dataGridInvoice.Rows[row].Cells[8].Value = order.vatTotal.ToString();
                dataGridInvoice.Rows[row].Cells[9].Value = order.grossTotal.ToString();
            }
        }
    }
}
