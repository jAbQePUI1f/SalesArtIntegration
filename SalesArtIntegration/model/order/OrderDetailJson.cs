using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.order
{
    public class OrderDetailJson
    {
        public string productCode { get; set; }
        public string productBarcode { get; set; }
        public decimal orderItemPrice { get; set; }
        public string productName { get; set; }
        public string unitCode { get; set; }
        public int quantity { get; set; }
        public decimal grossTotal { get; set; }
        public decimal preVatNetTotal { get; set; }
        public decimal vatRate { get; set; }
        public decimal vatTotal { get; set; }
        public int lineOrder { get; set; }
        public decimal vatIncludedPrice { get; set; }
        public decimal discountAmount { get; set; }
        public decimal? discountRate { get; set; }
        public decimal manualDiscountAmount { get; set; }
        public decimal manualDiscountRate { get; set; }
    }
}
