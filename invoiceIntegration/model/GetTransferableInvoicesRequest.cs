using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
    class GetTransferableInvoicesRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> invoiceTypes { get; set; }
    }  
}
