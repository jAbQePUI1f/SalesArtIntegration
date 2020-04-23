using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.waybill
{
    public class GetTransferableWaybillsRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> waybillTypes { get; set; }
    }
}
