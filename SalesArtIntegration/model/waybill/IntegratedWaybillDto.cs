namespace invoiceIntegration.model.waybill
{
    public class IntegratedWaybillDto
    {
        public string errorMessage { get; set; }
        public string invoiceNumber { get; set; }
        public string remoteInvoiceNumber { get; set; }
        public bool successfullyIntegrated { get; set; }

        public IntegratedWaybillDto(string _errorMessage, string _invoiceNumber, string _remoteInvoiceNumber, bool _successfullyIntegrated)
        {
            errorMessage = _errorMessage;
            invoiceNumber = _invoiceNumber;
            remoteInvoiceNumber = _remoteInvoiceNumber;
            successfullyIntegrated = _successfullyIntegrated;
        }
    }
}
