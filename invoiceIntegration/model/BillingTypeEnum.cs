using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
     
    public enum BillingTypeEnum
    {
        // 8 satış , 3 Satış iade ,9 verilen hizmet ;  4 alınan hizmet ,1 alım ,  6 alım iade

        SELLING = 8,
        BUYING = 1,
        SELLING_RETURN = 3,
        BUYING_RETURN = 6,
        DAMAGED_SELLING_RETURN = 3,
        DAMAGED_BUYING_RETURN = 6 ,
        SELLING_SERVICE = 9,
        BUYING_SERVICE = 4 
    }
}
