using invoiceIntegration.config;
using invoiceIntegration.model;
using invoiceIntegration.model.waybill;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    basarili += " Fatura Numarası   : " + item.remoteInvoiceNumber;
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
    }
}
