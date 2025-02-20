using System.Collections.Generic;

namespace invoiceIntegration.model
{
    public class IntegratedInvoiceStatus
    {
        public int distributorId { get; set; }
        public List<IntegratedInvoiceDto> integratedInvoices { get; set; }
         
    }
}
