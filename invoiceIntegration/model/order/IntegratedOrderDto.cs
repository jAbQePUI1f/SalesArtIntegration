using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
    public class IntegratedOrderDto
    {
        public string message { get; set; }
        public string remoteOrderId { get; set; }
        public long orderId { get; set; }
        public bool synced { get; set; }

        public IntegratedOrderDto(string _message, string _remoteOrderId, long _orderId, bool _synced)
        {
            message = _message;
            remoteOrderId = _remoteOrderId;
            orderId = _orderId;
            synced = _synced;
        }
    }
}
