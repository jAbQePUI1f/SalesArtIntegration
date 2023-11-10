using System;
using System.Collections.Generic;

namespace invoiceIntegration.model.Collection
{
    public class LogoCollectionModel
    {
        public LogoCollectionModelHeader collectionModelHeader { get; set; }
        public List<LogoCollectionModelDetail> collectionModelDetail { get; set; }
    }
    public class LogoCollectionModelHeader
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Number { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Type { get; set; }
        public string CashCode { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Desc { get; set; }
        public decimal Amount { get; set; }
        public decimal TCAmount { get; set; }
        public decimal RCAmount { get; set; }
        public string MasterModelu { get; set; }
        public string Notes1 { get; set; }
        public string BankAccCode { get; set; }
        public string TotalCredit { get; set; }
        public string TradingGroup { get; set; }
        public string SalesmanCode { get; set; }

    }
    public class LogoCollectionModelDetail
    {
        public string Type { get; set; }
        public string BankTitle { get; set; }
        public string BankName { get; set; }
        public string SerialNo { get; set; }
        public string Owing { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeName { get; set; }
        public string TrCode { get; set; }
        public string CustomerCode { get; set; }
        public string TranNo { get; set; }
        public string Credit { get; set; }
        public decimal TCAmount { get; set; }
        public decimal RCAmount { get; set; }
        public string DocNumber { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Desc { get; set; }
        public string DivisionNo { get; set; }
        public string AccountNo { get; set; }
        public string Iban { get; set; }
        public string BankBranch { get; set; }
        public string CityName { get; set; }
        public string GuarantorName { get; set; }
        public string TaxNr { get; set; }
        public string BankAccCode { get; set; }

    }
}
