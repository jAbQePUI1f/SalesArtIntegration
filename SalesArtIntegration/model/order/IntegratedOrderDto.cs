namespace invoiceIntegration.model.order
{
    public class IntegratedOrderDto
    {
        public string message { get; set; }
        public long remoteOrderId { get; set; }
        public long orderId { get; set; }
        public bool synced { get; set; }

        public IntegratedOrderDto(string _message, long _remoteOrderId, long _orderId, bool _synced)
        {
            message = _message;
            remoteOrderId = _remoteOrderId;
            orderId = _orderId;
            synced = _synced;
        }
    }
}
