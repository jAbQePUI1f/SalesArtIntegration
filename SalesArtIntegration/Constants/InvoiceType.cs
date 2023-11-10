using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration
{
    public enum InvoiceType
    {
        SELLING = 8,

        SELLING_RETURN = 3,

        DAMAGED_SELLING_RETURN = 3,

        DAMAGED_BUYING_RETURN = 6,

        BUYING_SERVICE = 4,

        BUYING_RETURN = 6,

        SELLING_SERVICE = 9,

        BUYING = 1
    }
}
