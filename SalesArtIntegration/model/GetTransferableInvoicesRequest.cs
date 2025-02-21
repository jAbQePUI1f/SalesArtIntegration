using System.Collections.Generic;

namespace invoiceIntegration.model
{
    class GetTransferableInvoicesRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> invoiceTypes { get; set; }
    }  
}
