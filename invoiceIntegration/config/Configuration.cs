using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoiceIntegration.config
{
    public class Configuration
    {
        public static string getLogoUserName()
        {
            return ConfigurationManager.AppSettings["LogoUserName"];
        }

        public static String getLogoPassword()
        {
            return ConfigurationManager.AppSettings["LogoPassword"];
        }

        public static String getCompanyCode()
        {
            return ConfigurationManager.AppSettings["CompanyCode"];
        }

        public static String getSeason()
        {
            return ConfigurationManager.AppSettings["Season"];
        }

        public static String getLogoConnection()
        {
            return ConfigurationManager.AppSettings["logoConnection"];
        }

        public static Int32 getDistributorId()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["DistributorId"]);
        }

        public static bool getUseCypheCode()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useCypheCode"]);
        }
        public static bool getUseDispatch()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["useDispatch"]);
        }
        public static string getUnit(string unit)
        {
            return ConfigurationManager.AppSettings[unit];
        }

        public static string getDepartment()
        {
            return ConfigurationManager.AppSettings["Department"];
        }
        public static bool getIsProducerCode()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["isProducerCode"]);
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

        public static string getCypheCode()
        {
            return ConfigurationManager.AppSettings["YetkiKodu"];
        }

        public static string getUrl()
        {
            return ConfigurationManager.AppSettings["URL"];
        }
        
    }
}
