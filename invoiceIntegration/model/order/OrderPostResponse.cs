using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.order
{
    public class OrderPostResponse
    {
        public List<BaseOrderDto> invoices { get; set; }
    }
}
