using System.Collections.Generic;

namespace invoiceIntegration.model.order
{
    public class OrderPostResponse
    {
        public List<BaseOrderDto> invoices { get; set; }
    }
}
