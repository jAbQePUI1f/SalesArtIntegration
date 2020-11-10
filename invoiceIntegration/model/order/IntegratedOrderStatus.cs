using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
    public class IntegratedOrderStatus
    {
        public int distributorId { get; set; }
        public List<IntegratedOrderDto> integratedOrders { get; set; }
         
    }
}
