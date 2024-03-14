using System;
using System.Collections.Generic;

namespace invoiceIntegration.model.order
{
    public class OrderJson
    {
        public long orderId { get; set; }
        public string receiptNumber { get; set; }
        public BaseIdNameCodeDto customer { get; set; }
        public BaseIdNameCodeDto customerBranch { get; set; }
        public BaseIdNameCodeDto salesman { get; set; }
        public BaseIdNameCodeDto paymentType { get; set; }
        public BaseIdNameCodeDto warehouse { get; set; }
        public DateTime deliveryDate { get; set; }
        public decimal? manualDiscountRate { get; set; }
        public decimal manualDiscountAmount { get; set; }
        public decimal discountTotal { get; set; }
        public decimal grossTotal { get; set; }        
        public DateTime orderDate { get; set; }
        public decimal preVatNetTotal { get; set; }
        public string salesmanNote { get; set; }
        public decimal vatIncludedTotal { get; set; }
        public decimal vatTotal { get; set; }
        public List<OrderDetailJson> details { get; set; }
    }
}
