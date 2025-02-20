using System.Collections.Generic;

namespace invoiceIntegration.model
{
    public class InvoicePostResponse
    {
        public List<BaseInvoiceDto> invoices { get; set; }
    }
}
