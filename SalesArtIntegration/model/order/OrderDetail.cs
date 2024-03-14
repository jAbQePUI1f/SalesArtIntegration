using System.Collections.Generic;

namespace invoiceIntegration.model.order
{
    public class OrderDetail
    {
        public int type { get; set; }
        public decimal rate { get; set; }
        public decimal discountTotal { get; set; }
        public decimal price { get; set; }
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
        public List<Discount> discountDetails { get; set; }
    }
}
