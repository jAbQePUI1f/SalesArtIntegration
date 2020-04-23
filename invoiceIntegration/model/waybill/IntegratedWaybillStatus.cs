using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.waybill
{
    public class IntegratedWaybillStatus
    {
        public int distributorId { get; set; }
        public List<IntegratedWaybillDto> integratedWaybills { get; set; }
    }
}
