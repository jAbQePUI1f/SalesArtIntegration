namespace invoiceIntegration.model
{
    public class DiscountJson
    {
        public string rewardType { get; set; }
        //public int type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public decimal rate { get; set; }
        public decimal discountTotal { get; set; }
        public int discountLineOrder { get; set; }
    }
}
