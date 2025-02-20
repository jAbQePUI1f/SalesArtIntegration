using System;
using System.Configuration;

namespace invoiceIntegration.config
{
    public class Configuration
    {
        public static string getLogoUserName()
        {
            return ConfigurationManager.AppSettings["LogoUserName"];
        }
        public static string getLogoPassword()
        {
            return ConfigurationManager.AppSettings["LogoPassword"];
        }
        public static string getCompanyCode()
        {
            return ConfigurationManager.AppSettings["CompanyCode"];
        }
        public static string getSeason()
        {
            return ConfigurationManager.AppSettings["Season"];
        }
        public static string getLogoConnection()
        {
            return ConfigurationManager.AppSettings["logoConnection"];
        }
        public static Int32 getDistributorId()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["DistributorId"]);
        }
        public static string getDistributorName()
        {
            return ConfigurationManager.AppSettings["DistributorName"];
        }
        public static bool getTransferCreditCartToCase()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["TransferCreditCartToCase"]);
        }
        public static bool getUseCypheCode()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useCypheCode"]);
        }
        public static bool getUseInvoice()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useInvoice"]);
        }
        public static bool getUseDispatch()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useDispatch"]);
        }
        public static bool getUseCollection()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useCollection"]);
        }
        public static string getUnit(string unit)
        {
            return ConfigurationManager.AppSettings[unit];
        }
        public static string getDepartment()
        {
            return ConfigurationManager.AppSettings["Department"];
        }
        public static string getDivision()
        {
            return ConfigurationManager.AppSettings["Division"];
        }
        public static bool getIsProducerCode()
        {
            return ConfigurationManager.AppSettings["Division"];
        }
        public static bool getUseProducerCode()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useProducerCode"]);
        }
        public static string getUsePersonaProductCode()
        {
            return ConfigurationManager.AppSettings["usePersonaProductCode"];
        }
        public static bool getIsBarcode()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["isBarkod"]);
        }
        public static bool getUseDefaultNumber()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useDefaultNumber"]);
        }
        public static bool getUseShortDate()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useShortDate"]);
        }
        public static bool getUseShipCode()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useShipCode"]);
        }
        public static bool getXMLTransferInfo()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["XMLTransfer"]);
        }
        public static bool getXMLTransferForOrder()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["XMLTransferForOrder"]);
        }
        public static string getCashCode()
        {
            return ConfigurationManager.AppSettings["CashCode"];
        }
        public static string getCypheCode()
        {
            return ConfigurationManager.AppSettings["YetkiKodu"];
        }
        public static string getUrl()
        {
            return ConfigurationManager.AppSettings["URL"];
        }
        public static bool getIntegrationForMikroERP()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["integrationForMikroERP"]);
        }
        public static string getShipAgentCode()
        {
            return ConfigurationManager.AppSettings["shipAgentCode"];
        }
        public static string getCampaignLineNo()
        {
            return ConfigurationManager.AppSettings["campaignLineNo"];
        }
        public static bool getOrderTransferToLogoInfo()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["orderTransferToLogo"]);
        }
        public static string getAffectRisk()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["affectRisk"]);
        }
    }
}
