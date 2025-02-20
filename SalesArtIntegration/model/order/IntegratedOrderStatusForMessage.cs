using System.Collections.Generic;

namespace invoiceIntegration.model.order
{
    public class IntegratedOrderStatusForMessage
    {
        public int distributorId { get; set; }
        public List<IntegratedOrderForMessageDto> orders { get; set; }
         
    }
}
