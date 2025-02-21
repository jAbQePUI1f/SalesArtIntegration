namespace invoiceIntegration.model.order
{
    public class IntegratedOrderForMessageDto
    {
        public string message { get; set; }
        public string remoteOrderNumber { get; set; }
        public long orderId { get; set; }
        public bool synced { get; set; }

        public IntegratedOrderForMessageDto(string _message, string _remoteOrderNumber, long _orderId, bool _synced)
        {
            message = _message;
            remoteOrderNumber = _remoteOrderNumber;
            orderId = _orderId;
            synced = _synced;
        }
    }
}
