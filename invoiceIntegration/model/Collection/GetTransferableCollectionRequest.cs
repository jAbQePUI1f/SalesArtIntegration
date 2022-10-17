using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.Collection
{
    public class GetTransferableCollectionRequest
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<string> collectionTypes { get; set; }
    }
}
