using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
    public class InvoicePostResponse
    {
        public List<BaseInvoiceDto> invoices { get; set; }
    }
}
