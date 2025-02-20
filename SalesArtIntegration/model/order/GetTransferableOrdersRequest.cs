namespace invoiceIntegration.model.order
{
    public class GetTransferableOrdersRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int distributorId { get; set; }
    }
}
