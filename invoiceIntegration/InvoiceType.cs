using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration
{
    public enum InvoiceType
    {
        SELLING  ,
        SELLING_RETURN  ,
        DAMAGED_SELLING_RETURN  ,
        DAMAGED_BUYING_RETURN ,
        BUYING_SERVICE  ,
        BUYING_RETURN,
        SELLING_SERVICE,
        BUYING

        //SELLING_SERVICE,
        //BUYING_SERVICE
    }
}
