using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.order
{
    public class BaseOrderDto
    {
        public int code { get; set; }
        public long remoteOrderId { get; set; }
        public long orderId { get; set; }
        public string message { get; set; }


        public BaseOrderDto(int _code, long _remoteOrderId, long _orderId, string _message)
        {
            code = _code;
            remoteOrderId = _remoteOrderId;
            orderId = _orderId;
            message = _message;
        }

        public BaseOrderDto()
        {

        }
    }
}
