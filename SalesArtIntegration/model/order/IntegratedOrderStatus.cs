using System.Collections.Generic;

namespace invoiceIntegration.model.order
{
    public class IntegratedOrderStatus
    {
        public int distributorId { get; set; }
        public List<IntegratedOrderDto> orders { get; set; }
         
    }
}
