using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model.waybill
{
    public class LogoWaybillJson
    {
        public BillingTypeEnum waybillType { get; set; }   // 8 satış , 3 Satış iade ,9 verilen hizmet ;  4 alınan hizmet ,1 alım ,  6 alım iade ,  
        public string number { get; set; }
        public DateTime date { get; set; }
        public string documentNumber { get; set; }
        public string wareHouseCode { get; set; }
        //public decimal vatRate { get; set; }
        //public string shipLocCode { get; set; } // şube codu        
        public string customerCode { get; set; }
        public string customerName { get; set; }
        public string customerBranchCode { get; set; }
        public string customerBranchName { get; set; }
        public bool ebillCustomer { get; set; }
        public DateTime documentDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public decimal discountTotal { get; set; }  // yapılan toplam indirim tutarı
        public decimal preVatIncludedTotal { get; set; }  // vergi eklenmeden önceki tutar
        public decimal vatTotal { get; set; }  // toplam kdv
        public decimal grossTotal { get; set; }  // indirim uygulanmadan önceki tutar (KDV HARİC)
        public decimal netTotal { get; set; }  // indirim düşülmüş , kdv eklenmiş tutar , son fatura tutarı yani
        public string paymentCode { get; set; } // ödeme planı kodu
        public string note { get; set; }
        //public int createdBy { get; set; }
        public string salesmanCode { get; set; }
        public List<WaybillDetailJson> details { get; set; }
        //public string customerTradingGroup { get; set; }
        public string distributorBranchCode { get; set; }
    }
}
