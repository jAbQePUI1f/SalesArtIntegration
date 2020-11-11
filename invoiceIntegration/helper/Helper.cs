using invoiceIntegration.config;
using invoiceIntegration.model;
using invoiceIntegration.model.order;
using invoiceIntegration.model.waybill;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace invoiceIntegration.helper
{
    public class Helper
    {
        public void LogFile(string logCaption, string invoiceNumber, string remoteInvoiceNumber, string isSuccess, string message)
        {
            StreamWriter log;
            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }
            log.WriteLine("------------------------");
            log.WriteLine("Hata Mesajı:" + message);
            log.WriteLine("salesArt Fatura No:" + invoiceNumber + " -> Logo Fatura No : " + remoteInvoiceNumber);
            log.WriteLine("Başarılı mı ? :" + isSuccess);
            log.WriteLine("Log Adı:" + logCaption);
            log.WriteLine("Log Zamanı:" + DateTime.Now);

            log.Close();
        }
        public string getUnit(string unitInfo)
        {
            return Configuration.getUnit(unitInfo);
        }
        public string getDepartment()
        {
            return Configuration.getDepartment();
        }

        bool integrationForMikroERP = Configuration.getIntegrationForMikroERP();

        public string getInvoiceType(int type)
        {
            string invoiceType = "";
            switch (type)
            {
                // 8 satış , 3 Satış iade ,9 verilen hizmet ;  4 alınan hizmet ,1 alım ,  6 alım iade ,
                case 8:
                    invoiceType = "Toptan Satış Faturası";
                    break;
                case 3:
                    invoiceType = "Toptan Satış İade Faturası";
                    break;
                case 9:
                    invoiceType = "Verilen Hizmet Faturası";
                    break;
                case 4:
                    invoiceType = "Alınan Hizmet Faturası";
                    break;
                case 1:
                    invoiceType = "Alım Faturası";
                    break;
                case 6:
                    invoiceType = "Alım İade Faturası";
                    break;
            }
            return invoiceType;
        }
        public string getInvoiceTypeForWaybill(int type)
        {
            string invoiceType = "";
            switch (type)
            {
                // 8 satış , 3 Satış iade ,9 verilen hizmet ;  4 alınan hizmet ,1 alım ,  6 alım iade ,
                case 8:
                    invoiceType = "Toptan Satış İrsaliyesi";
                    break;
                case 3:
                    invoiceType = "Toptan Satış İade İrsaliyesi";
                    break;
                case 9:
                    invoiceType = "Verilen Hizmet İrsaliyesi";
                    break;
                case 4:
                    invoiceType = "Alınan Hizmet İrsaliyesi";
                    break;
                case 1:
                    invoiceType = "Alım İrsaliyesi";
                    break;
                case 6:
                    invoiceType = "Alım İade İrsaliyesi";
                    break;
            }
            return invoiceType;
        }
        public int Hour(DateTime invoiceDate)
        {
            int time;
            int hour, minute, second;
            hour = invoiceDate.Hour;
            minute = invoiceDate.Minute;
            second = invoiceDate.Second;
            time = 16777216 * hour + 65536 * minute + 256 * second + 1;
            return time;
        }
        public int Hour(string invoiceDate)  // "2020-04-21T16:37:14Z"
        {
            int time = 0;
            int hour, minute, second;
            hour = Convert.ToInt32(invoiceDate.Substring(11, 2));
            minute = Convert.ToInt32(invoiceDate.Substring(14, 2));
            second = Convert.ToInt32(invoiceDate.Substring(17, 2));
            time = 16777216 * hour + 65536 * minute + 256 * second + 1;
            return time;
        }
        public void ShowMessages(IntegratedInvoiceStatus integratedInvoices)
        {
            string basarili = "";
            string basarisiz = "";
            foreach (var item in integratedInvoices.integratedInvoices)
            {
                if (item.successfullyIntegrated)
                {
                    LogFile("Aktarım Bilgisi", item.invoiceNumber, item.remoteInvoiceNumber, "AKTARIM BAŞARILI", item.errorMessage);
                    basarili += " Fatura Numarası   : " ;
                    basarili += integrationForMikroERP ? item.invoiceNumber : item.remoteInvoiceNumber;
                }
                else
                {
                    LogFile("Aktarım Bilgisi", item.invoiceNumber, item.remoteInvoiceNumber, "AKTARIM BAŞARISIZ..!!!", item.errorMessage);
                    basarisiz += item.invoiceNumber + " numaralı fatura için : " + item.errorMessage; 
                }
            }
            string msj = "Başarılı : " + basarili + "    Başarısız : " + basarisiz;
            if (MessageBox.Show(msj, "Aktarılan/Aktarılamayan Fatura Bilgileri", MessageBoxButtons.OK) == DialogResult.OK)
            { Clipboard.SetText(msj); }
        }
        public void ShowMessages(IntegratedWaybillStatus integratedWaybills)
        {
            string basarili = "";
            string basarisiz = "";
            foreach (var item in integratedWaybills.integratedWaybills)
            {
                if (item.successfullyIntegrated)
                {
                    LogFile("İrsaliye Aktarım Bilgisi", item.invoiceNumber, item.remoteInvoiceNumber, "AKTARIM BAŞARILI", item.errorMessage);
                    basarili += " Logo İrsaliye Numarası   : " + item.remoteInvoiceNumber;
                }
                else
                {
                    LogFile("İrsaliye Aktarım Bilgisi", item.invoiceNumber, item.remoteInvoiceNumber, "AKTARIM BAŞARISIZ..!!!", item.errorMessage);
                    basarisiz += item.invoiceNumber + " numaralı irsaliye için : " + item.errorMessage;
                }
            }
            string msj = "Başarılı : " + basarili + "    Başarısız : " + basarisiz;
            if (MessageBox.Show(msj, "Aktarılan/Aktarılamayan İrsaliye Bilgileri", MessageBoxButtons.OK) == DialogResult.OK)
            { Clipboard.SetText(msj); }
        }
        public void ShowMessages(IntegratedOrderStatus integratedOrders)
        {
            string basarili = "";
            string basarisiz = "";
            foreach (var item in integratedOrders.orders)
            {
                if (item.synced)
                {
                    LogFile("Aktarım Bilgisi", item.orderId.ToString(), item.remoteOrderId.ToString(), "AKTARIM BAŞARILI", item.message);
                    basarili += " Sipariş Numarası   : ";
                    basarili += integrationForMikroERP ? item.orderId.ToString() : item.remoteOrderId.ToString();
                }
                else
                {
                    LogFile("Aktarım Bilgisi", item.orderId.ToString(), item.remoteOrderId.ToString(), "AKTARIM BAŞARISIZ..!!!", item.message);
                    basarisiz += item.orderId + " numaralı sipariş için : " + item.message;
                }
            }
            string msj = "Başarılı : " + basarili + "    Başarısız : " + basarisiz;
            if (MessageBox.Show(msj, "Aktarılan/Aktarılamayan Sipariş Bilgileri", MessageBoxButtons.OK) == DialogResult.OK)
            { Clipboard.SetText(msj); }
        }
        public  void AddNode(XmlDocument Document, XmlNode Node, string Tag, string InnerText)
        {
            XmlNode tempNode = Document.CreateNode(XmlNodeType.Element, Tag, "");
            tempNode.InnerText = InnerText;
            Node.AppendChild(tempNode);
        }
        public  void AddCDataNode(XmlDocument Document, XmlNode Node, string Tag, string InnerText)
        {
            XmlNode tempNode = Document.CreateNode(XmlNodeType.Element, Tag, "");
            XmlCDataSection cdata = Document.CreateCDataSection(InnerText);
            tempNode.AppendChild(cdata);
            Node.AppendChild(tempNode);
        }

        //public static int GetConv1(string unitname, string productcode)
        //{
        //    int conv1 = 1;
        //    SqlConnection Connection = new SqlConnection(Settings.DataSourceErpSQLConnectionString(ApplicationProcessOwnerName: "frmInvoiceSeller"));
        //    Connection.Open();
        //    SqlCommand cmdConv1 = Connection.CreateCommand();
        //    cmdConv1.CommandType = CommandType.Text;


        //    cmdConv1.CommandText = "SELECT IT.CONVFACT1 FROM LG_" + Settings.DBFirmNo + "_ITMUNITA IT INNER JOIN LG_" + Settings.DBFirmNo + "_UNITSETL UNIT ON UNIT.LOGICALREF=IT.UNITLINEREF INNER JOIN LG_" + Settings.DBFirmNo + "_ITEMS STOK ON IT.ITEMREF=STOK.LOGICALREF WHERE UNIT.NAME='" + unitname + "' AND STOK.CODE='" + productcode + "'";

        //    conv1 = Convert.ToInt32(cmdConv1.ExecuteScalar());
        //    Connection.Close();
        //    return conv1;
        //}



        //public static int GetConv2(string unitname, string productcode)
        //{
        //    int conv2 = 1;
        //    SqlConnection Connection = new SqlConnection(Settings.DataSourceErpSQLConnectionString(ApplicationProcessOwnerName: "frmInvoiceSeller"));
        //    Connection.Open();
        //    Logger.WriteLineHistory("GetConv2:");
        //    SqlCommand cmdConv2 = Connection.CreateCommand();
        //    cmdConv2.CommandType = CommandType.Text;


        //    cmdConv2.CommandText = "SELECT IT.CONVFACT2 FROM LG_" + Settings.DBFirmNo + "_ITMUNITA IT INNER JOIN LG_" + Settings.DBFirmNo + "_UNITSETL UNIT ON UNIT.LOGICALREF=IT.UNITLINEREF INNER JOIN LG_" + Settings.DBFirmNo + "_ITEMS STOK ON IT.ITEMREF=STOK.LOGICALREF WHERE UNIT.NAME='" + unitname + "' AND STOK.CODE='" + productcode + "'";

        //    conv2 = Convert.ToInt32(cmdConv2.ExecuteScalar());
        //    Connection.Close();
        //    return conv2;
        //}

        //public static int GetCurrencyType(string curcode)
        //{
        //    int curtype = 1;
        //    Logger.WriteLineHistory("GetConv1:");
        //    SqlConnection Connection = new SqlConnection(Settings.DataSourceErpSQLConnectionString(ApplicationProcessOwnerName: "frmInvoiceSeller"));
        //    Connection.Open();
        //    SqlCommand cmdcurtype = Connection.CreateCommand();
        //    cmdcurtype.CommandType = CommandType.Text;


        //    cmdcurtype.CommandText = "SELECT CURTYPE FROM L_CURRENCYLIST   WHERE  CURCODE='" + curcode + "'";

        //    curtype = Convert.ToInt32(cmdcurtype.ExecuteScalar());
        //    Connection.Close();
        //    return curtype;
        //}
    }
}
