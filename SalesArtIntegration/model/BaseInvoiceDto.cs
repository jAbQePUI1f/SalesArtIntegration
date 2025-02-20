namespace invoiceIntegration.model
{
    public class BaseInvoiceDto
    {
        public int code { get; set; }
        public string remoteInvoiceNumber { get; set; }
        public string invoiceNumber { get; set; }
        public string message { get; set; }
        public BaseInvoiceDto(int _code, string _remoteInvoiceNumber, string _invoiceNumber, string _message)
        {
            code = _code;
            remoteInvoiceNumber = _remoteInvoiceNumber;
            invoiceNumber = _invoiceNumber;
            message = _message;
        }
        public BaseInvoiceDto()
        {

        }
    }
}
