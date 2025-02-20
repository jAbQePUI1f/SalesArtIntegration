using System.Collections.Generic;

namespace invoiceIntegration.model.waybill
{
    public class GetTransferableWaybillsRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> waybillTypes { get; set; }
    }
}
