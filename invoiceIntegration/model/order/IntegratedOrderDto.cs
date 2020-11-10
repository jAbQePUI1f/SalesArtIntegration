using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
    public class IntegratedOrderDto
    {
        public string errorMessage { get; set; }
        public string invoiceNumber { get; set; }
        public string remoteOrderNumber { get; set; }
        public bool successfullyIntegrated { get; set; }

        public IntegratedOrderDto(string _errorMessage, string _orderNumber, string _remoteOrderNumber, bool _successfullyIntegrated)
        {
            errorMessage = _errorMessage;
            invoiceNumber = _orderNumber;
            remoteOrderNumber = _remoteOrderNumber;
            successfullyIntegrated = _successfullyIntegrated;
        }
    }
}
