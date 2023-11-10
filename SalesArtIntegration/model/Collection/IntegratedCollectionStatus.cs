using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.Collection
{
    public class IntegratedCollectionStatus
    {
        public int distributorId { get; set; }
        public List<IntegratedCollectionDto> collections { get; set; }
    }
}
