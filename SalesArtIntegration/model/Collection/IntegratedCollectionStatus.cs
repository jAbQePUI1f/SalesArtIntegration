using System.Collections.Generic;

namespace invoiceIntegration.model.Collection
{
    public class IntegratedCollectionStatus
    {
        public int distributorId { get; set; }
        public List<IntegratedCollectionDto> collections { get; set; }
    }
}
