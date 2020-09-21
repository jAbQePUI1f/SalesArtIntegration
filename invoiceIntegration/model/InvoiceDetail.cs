using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.model
{
    public class InvoiceDetail
    {
        public int type { get; set; }  // 0 malzeme , 1 promosyon , 2 indirim , 3 masraf , 4 hizmet
        public string code { get; set; }
        public string name { get; set; }
        //public string wareHouseCode { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
        public decimal discountTotal { get; set; }
        public decimal grossTotal { get; set; }
        public string unitCode { get; set; }  // ADET , KOLİ
        //public decimal unitConversationFactor { get; set; }
        public bool vatIncluded { get; set; }  // 1, 0
        public decimal rate { get; set; }  // indriim detayı için yazıldı
        public decimal vatRate { get; set; }
        public decimal vatAmount { get; set; }
        public decimal netTotal { get; set; }
        public string barcode { get; set; }
        public int invoiceDetailLineOrder { get; set; }
        public List<Discount> discounts { get; set; }
    }
}
