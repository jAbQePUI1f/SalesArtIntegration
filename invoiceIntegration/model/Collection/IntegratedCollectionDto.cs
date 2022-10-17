using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.Collection
{
    public class IntegratedCollectionDto
    {
        public string errorMessage { get; set; }
        public string Number { get; set; }
        public string remoteNumber { get; set; }
        public bool successfullyIntegrated { get; set; }

        public IntegratedCollectionDto(string _errorMessage, string _invoiceNumber, string _remoteInvoiceNumber, bool _successfullyIntegrated)
        {
            errorMessage = _errorMessage;
            Number = _invoiceNumber;
            remoteNumber = _remoteInvoiceNumber;
            successfullyIntegrated = _successfullyIntegrated;
        }
    }
}
