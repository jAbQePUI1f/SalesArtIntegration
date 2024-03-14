using System.Collections.Generic;

namespace invoiceIntegration.model.waybill
{
    public class IntegratedWaybillStatus
    {
        public int distributorId { get; set; }
        public List<IntegratedWaybillDto> integratedWaybills { get; set; }
    }
}
