using System.Collections.Generic;

namespace invoiceIntegration.model.Collection
{
    public class GetTransferableCollectionRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> collectionTypes { get; set; }
    }
}
