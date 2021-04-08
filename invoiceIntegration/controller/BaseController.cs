using invoiceIntegration.config;
using invoiceIntegration.helper;
using invoiceIntegration.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.controller
{
    public class BaseController
    {
       public Helper helper = new Helper();
       public IntegratedInvoiceStatus integratedInvoices = new IntegratedInvoiceStatus();
        public int distributorId { get; set; }
        public string cypheCode { get; set; }
        public string shipAgentCode { get; set; }
        public bool useDefaultNumber { get; set; }
        public bool useCypheCode { get; set; }
        public bool useShipCode { get; set; }
        public bool XMLTransferForOrder { get; set; }
        public bool useShortDate { get; set; }
        public string filePath { get; set; }
        public BaseController()
        {
            distributorId = Configuration.getDistributorId();
            cypheCode = Configuration.getCypheCode();
            shipAgentCode = Configuration.getShipAgentCode();
            useDefaultNumber = Configuration.getUseDefaultNumber();
            useCypheCode = Configuration.getUseCypheCode();
            useShipCode = Configuration.getUseShipCode();
            XMLTransferForOrder = Configuration.getXMLTransferForOrder();
            useShortDate = Configuration.getUseShortDate();        
        }
    }
}
