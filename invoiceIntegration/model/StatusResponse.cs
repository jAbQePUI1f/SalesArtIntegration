using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
    public class StatusResponse
    {
        public Message message { get; set; }
        public int responseStatus { get; set; }
    }
}
