using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.order
{
    public class GetTransferableOrdersRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int distributorId { get; set; }
    }
}
