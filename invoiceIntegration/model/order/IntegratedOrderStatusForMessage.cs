using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.order
{
    public class IntegratedOrderStatusForMessage
    {
        public int distributorId { get; set; }
        public List<IntegratedOrderForMessageDto> orders { get; set; }
         
    }
}
