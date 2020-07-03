using invoiceIntegration.config;
using invoiceIntegration.helper;
using invoiceIntegration.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityObjects;

namespace invoiceIntegration.repository
{
    public class LogoDataReader
    {

        String season = Configuration.getSeason();
        String companyCode = Configuration.getCompanyCode();

        string conString = Configuration.getLogoConnection();

        Helper helper = new Helper(); 
         
        UnityApplication unity = LogoApplication.getApplication();

        //string conString = "Server=172.16.40.20;Database=AYK2008;Persist Security Info=True;User ID=PG;Password=PG2007;";

        //public virtual QueryModel GetQueryDesc(string[] fields, TableID tableId, int firmNr, int perNr, int topCount = 1)
        //{
        //    QueryModel queryModel = new QueryModel();
        //    List<string> values = new List<string>();
        //    string tableName = UnityApplication.GetTableName((int)tableId, firmNr, perNr);
        //    Query Qry = UnityApplication.NewQuery();
        //    var QueryString =
        //        $"SELECT top({topCount}) {string.Join(",", fields)} FROM {tableName} ORDER BY LOGICALREF DESC";
        //    Qry.Statement = QueryString;
        //    if (Qry.OpenDirect())
        //    {
        //        bool res = Qry.First();
        //        while (res)
        //        {
        //            for (int i = 0; i < fields.Length; i++)
        //            {
        //                values.Add(Qry.QueryFields[i].Value.ToString());
        //            }
        //            res = Qry.Next();
        //        }

        //        queryModel.IsComplete = true;
        //        queryModel.Values = values.ToArray();
        //    }
        //    else
        //    {
        //        queryModel.IsComplete = false;
        //        queryModel.Content = Qry.DBErrorDesc;
        //    }
        //    Qry.Close();

        //    return queryModel;
        //}

        public int getLogicalRef(String ficheNo)
        {
            int LogicalRef = 0;

            try
            {
                String Qry = "SELECT ORF.LOGICALREF";
                Qry += " FROM LG_" + companyCode + "_" + season + "_ORFICHE ORF WITH (NOLOCK) ";
                Qry += " WHERE ORF.FICHENO = '" + ficheNo + "'";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();

                if (dr.Read())
                {
                    LogicalRef = int.Parse(dr["LOGICALREF"].ToString());
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return LogicalRef;
        }
        public string getItemCardType(long logicalRef)
        {
            string cardType = "";

            try
            {
                String Qry = "SELECT ITM.CARDTYPE";
                Qry += " FROM LG_" + companyCode + "_ITEMS ITM WITH (NOLOCK) ";
                Qry += " WHERE ITM.LOGICALREF = '" + logicalRef + "'";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    cardType = dr["CARDTYPE"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return cardType;
        }
        public String getTraidingGroup(long logicalRef)
        {
            String traidingGroup = "";

            try
            {
                String Qry = "SELECT TRADINGGRP ";
                Qry += " FROM LG_" + companyCode + "_CLCARD WITH (NOLOCK) ";
                Qry += " WHERE LOGICALREF = '" + logicalRef + "'";

                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();

                if (dr.Read())
                {
                    traidingGroup = dr["TRADINGGRP"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return traidingGroup;
        }
        public int getLogicalRefByCustomerCode(String custCode)
        {
            int LogicalRef = 0;

            try
            {
                String Qry = "SELECT CLC.LOGICALREF";
                Qry += " FROM LG_" + companyCode + "_CLCARD CLC WITH (NOLOCK) ";
                Qry += " WHERE CLC.CODE = '" + custCode + "'";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();

                if (dr.Read())
                {
                    LogicalRef = int.Parse(dr["LOGICALREF"].ToString());
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return LogicalRef;
        }
        public int getProfileIDByCustomerCode(string custCode)
        {
            int profileID = 0;

            try
            {
                String Qry = "SELECT CLC.PROFILEID";
                Qry += " FROM LG_" + companyCode + "_CLCARD CLC WITH (NOLOCK) ";
                Qry += " WHERE CLC.CODE = '" + custCode + "'";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();

                if (dr.Read())
                {
                    profileID = int.Parse(dr["PROFILEID"].ToString());
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return profileID;
        }
        public int getEInvoiceByCustomerCode(string custCode)
        {
            int acceptEInv = 0;

            try
            {
                String Qry = "SELECT CLC.ACCEPTEINV";
                Qry += " FROM LG_" + companyCode + "_CLCARD CLC WITH (NOLOCK) ";
                Qry += " WHERE CLC.CODE = '" + custCode + "'";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();

                if (dr.Read())
                {
                    acceptEInv = int.Parse(dr["ACCEPTEINV"].ToString());
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return acceptEInv;
        }
        public string getCodeByProductLogicalRef(long logicalRef)
        {
            string code = "";

            try
            {
                String Qry = "SELECT CODE";
                Qry += " FROM LG_" + companyCode + "_ITEMS WITH (NOLOCK) ";
                Qry += " WHERE LOGICALREF = '" + code + "'";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();

                if (dr.Read())
                {
                    code = dr["CODE"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return code;
        }
        public string getInvoiceNumberByDocumentNumber(string documentNumber)
        {
            string ficheNumber = "";

            try
            {
                String Qry = "SELECT TOP 1 FICHENO";
                Qry += " FROM LG_" + companyCode + "_" + season + "_INVOICE WITH (NOLOCK) ";
                Qry += " WHERE DOCODE  = '" + documentNumber + "' ORDER BY LOGICALREF DESC ";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    ficheNumber = dr["FICHENO"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return ficheNumber;
        }

        public string getProductCodeByProducerCode(string producerCode)
        {
            string productCode = "";

            try
            {
                String Qry = "SELECT TOP 1 CODE";
                Qry += " FROM LG_" + companyCode + "_ITEMS WITH (NOLOCK) ";
                Qry += " WHERE ( PRODUCERCODE  = '" + producerCode + "' OR CODE LIKE '%" + producerCode + "%' ) AND ACTIVE =  0 ";

                helper.LogFile("SQL Query", producerCode, "", "", Qry);

                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    productCode = dr["CODE"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                helper.LogFile("SQL Query", producerCode, "", "", e.Message +"__"+producerCode);
            }
            return productCode;
        }

        public string getProductCodeByBarcode(string barcode)
        {
            string productCode = "";

            try
            {  
                String Qry = "SELECT I.CODE ";
                Qry += " FROM LG_" + companyCode + "_ITEMS I WITH (NOLOCK) INNER JOIN LG_" + companyCode + "_UNITSETF US WITH (NOLOCK) ON I.UNITSETREF = US.LOGICALREF ";
                Qry += " INNER JOIN LG_" + companyCode + "_UNITSETL U WITH (NOLOCK) ON US.LOGICALREF = U.UNITSETREF ";
                Qry += " INNER JOIN LG_" + companyCode + "_UNITBARCODE B WITH (NOLOCK) ON B.ITEMREF = I.LOGICALREF AND U.LOGICALREF = B.UNITLINEREF ";
                Qry += " WHERE B.BARCODE  = '" + barcode + "' AND I.ACTIVE =  0 ";


                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    productCode = dr["CODE"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return productCode;
        }

        public string createInvoice(LogoInvoiceJson invoice)
        {
            string remoteRef = "";
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand(); // new SqlCommand("SP_InsertInvoice_Peros", conn);

                SqlTransaction transaction;
                transaction = conn.BeginTransaction("InvoiceTransaction");

                cmd.Connection = conn;
                cmd.Transaction = transaction;

                try
                {
                    string profileID = getProfileIDFromCustomerCodeMikro(invoice.customerCode).ToString();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_InsertInvoice_Peros";

                    cmd.Parameters.AddWithValue("@ERP_CARI_KOD", invoice.customerCode);
                    cmd.Parameters.AddWithValue("@ERP_CARI_SUBE_ADI", invoice.customerBranchName);
                    cmd.Parameters.AddWithValue("@PROFILE_ID", profileID);   // 0:Ticari Fatura 1:Temel Fatura
                    cmd.Parameters.AddWithValue("@INVOICE_TYPE_CODE", ((invoice.invoiceType == BillingTypeEnum.BUYING_RETURN || invoice.invoiceType == BillingTypeEnum.DAMAGED_BUYING_RETURN || invoice.invoiceType == BillingTypeEnum.DAMAGED_SELLING_RETURN || invoice.invoiceType == BillingTypeEnum.SELLING_RETURN) ? 1 : 0).ToString());  //0:Normal 1:İade
                    cmd.Parameters.AddWithValue("@INVOICE_NUMBER", invoice.number);
                    cmd.Parameters.AddWithValue("@ISSUE_DATE", invoice.date); //fat. tarihi
                    cmd.Parameters.AddWithValue("@PAYMENT_DUE_DATE", invoice.date);  // vade tarihi
                    cmd.Parameters.AddWithValue("@TAX_EXCLUSIVE_AMOUNT", invoice.grossTotal);  // toplam tutarı kdv siz
                    cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT1", invoice.discountTotal);  // indirim tutarı
                    cmd.Parameters.AddWithValue("@TAX_PERCENTAGE", Convert.ToDecimal(0.00));  // 0 gelecek hep ?? 
                    cmd.Parameters.AddWithValue("@TAX_AMOUNT1", invoice.vatTotal);  // vergi tutarı
                    cmd.Parameters.AddWithValue("@PAYABLE_AMOUNT", invoice.netTotal);  // vergi dahil toplam tutar
                    cmd.Parameters.AddWithValue("@DOCUMENT_CURRENCY_CODE", "0");  // 0: TL  , 1: Dolar
                    cmd.Parameters.AddWithValue("@NOTE1", "" + invoice.note);
                    cmd.Parameters.AddWithValue("@TAX_EXEMPTION_REASON", "" + invoice.note);  // bedelsiz fatura notu(fatura notuun aynısı olablilr)
                    cmd.Parameters.AddWithValue("@KASAHIZKOD", "");  // Bos gönderileiblir.
                    cmd.Parameters.AddWithValue("@KASAHIZMET", SqlDbType.TinyInt).SqlValue = 0;  // (0 gelecek -- 0:Carimiz 1:Cari Personelimiz 2:Bankamız 3:Hizmetimiz 4:Kasamız 5:Giderimiz 6:Muhasebe Hesabımız 7:Personelimiz 8:Demirbaşımız 9:İthalat Dosyamız 10:Finansal Sözleşmemiz 11:Kredi Sözleşmemiz 12:Dönemsel Hizmetimiz 13:Kredi Kartımız
                    cmd.Parameters.AddWithValue("@CINSI", SqlDbType.TinyInt).SqlValue = 6;  // satış faturası için 6  --> 	0:Nakit 1:Müşteri Çeki 2:Müşteri Senedi 3:Firma Çeki 4:Firma Senedi 5:Dekont 6:Toptan Fatura 7:Perakende Faturası 8:Hizmet Faturası 9:Serbest Meslek Makbuzu 10:Vade Farkı Faturası 11:Kur Farkı Faturası 12:Fason Faturası 13:Dış Ticaret Faturası 14:Demirbaş Faturası 15:Değer Farkı Faturası 16:Cari Açılış 17:Müşteri Havale Sözü 18:Müşteri Ödeme Sözü 19:Müşteri Kredi Kartı 20:Firma Havale Emri 21:Firma Ödeme Emri 22:Firma Kredi Kartı 23:Vade Farkı Sıfırlama 24:Hal Faturası 25:Müstahsil Fatura 26:Stok Gider Pusulası 27:Gider Makbuzu 28:İthalat Masraf Faturası 29:Gümrük Beyannamesi 30:Finansal Kiralama Sözleşmesi 31:Finansal Kira Faturası 32:FUTURE_2 33:Avans Makbuzu 34:Müstahsil Değer Farkı Faturası 35:Kabzımal Faturası 36:Hediye Çeki Faturası 37:Müşteri Teminat Mektubu 38:Firma Teminat Mektubu 39:Depozito Çeki 40:Depozito Senedi 41:Firma Reel Kredi Kartı
                    cmd.Parameters.AddWithValue("@CHECK_UUID", SqlDbType.Bit).SqlValue = 1;  // fatura numarası içeeride daha önce kaydedilmiş mi kontro ledilsin mi ? 1 ise evet
                    cmd.Parameters.AddWithValue("@SATIRNO", 0);  // 0 olacak , cünkü her bir fsturanın tek bir header'ı olacak
                    cmd.Parameters.AddWithValue("@EVRAKTIP", (invoice.invoiceType == BillingTypeEnum.DAMAGED_SELLING_RETURN || invoice.invoiceType == BillingTypeEnum.SELLING || invoice.invoiceType == BillingTypeEnum.SELLING_RETURN || invoice.invoiceType == BillingTypeEnum.SELLING_SERVICE) ? 63 : 0);  //  0:Alış Faturası , 63:Satış Faturası 
                    
                    SqlParameter sqlParam = new SqlParameter("@id", SqlDbType.UniqueIdentifier);
                    sqlParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sqlParam);
                    cmd.ExecuteNonQuery();
                      
                    remoteRef = sqlParam.Value.ToString();

                    cmd.CommandText = "SP_InsertInvoiceDetail_Peros";
                    
                    foreach (var detail in invoice.details)
                    {
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@INVOICE_TYPE_CODE", (invoice.invoiceType == BillingTypeEnum.SELLING || invoice.invoiceType == BillingTypeEnum.SELLING_SERVICE) ? "SATIŞ" : "IADE");  //0:Normal 1:İade
                        cmd.Parameters.AddWithValue("@INVOICE_NUMBER", invoice.number);
                        cmd.Parameters.AddWithValue("@ISSUE_DATE", invoice.date); //fat. tarihi
                        cmd.Parameters.AddWithValue("@ERP_CARI_KOD", invoice.customerCode);
                        cmd.Parameters.AddWithValue("@ERP_PRODUCT_STOK_KOD", getProductCodeFromProviderCode(detail.code));  // stok kodu
                        cmd.Parameters.AddWithValue("@QUANTITY", SqlDbType.Decimal).SqlValue = detail.quantity;  // miktar
                        cmd.Parameters.AddWithValue("@QUANTITY_AMOUNT", SqlDbType.Decimal).SqlValue = detail.price;  // birim fiyat 
                        cmd.Parameters.AddWithValue("@TAX_PERCENTAGE", detail.vatRate);
                        cmd.Parameters.AddWithValue("@TAX_AMOUNT", detail.vatAmount);  // vergi tutarı
                        cmd.Parameters.AddWithValue("@ITEM_NOTE", "");
                        cmd.Parameters.AddWithValue("@PROFILE_ID", profileID);

                        if(detail.discounts != null && detail.discounts.Count > 0)
                        {
                            for (int i = 0; i < detail.discounts.Count; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT1", detail.discounts[i].discountTotal);
                                        break;
                                    case 1:
                                        cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT2", detail.discounts[i].discountTotal);
                                        break;
                                    case 2:
                                        cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT3", detail.discounts[i].discountTotal);
                                        break;
                                    case 3:
                                        cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT4", detail.discounts[i].discountTotal);
                                        break;
                                    case 4:
                                        cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT5", detail.discounts[i].discountTotal);
                                        break;
                                    case 5:
                                        cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT6", detail.discounts[i].discountTotal);
                                        break;
                                } 
                            }
                        }
                        else  // indirim yok ise , indirim satırına 0 yazıldı , procedure bu alanları bekliyor , doldurmalıyız.
                        {
                            cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT1", SqlDbType.Decimal).SqlValue = 0;
                            cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT2", SqlDbType.Decimal).SqlValue = 0;
                            cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT3", SqlDbType.Decimal).SqlValue = 0;
                            cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT4", SqlDbType.Decimal).SqlValue = 0;
                            cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT5", SqlDbType.Decimal).SqlValue = 0;
                            cmd.Parameters.AddWithValue("@DISCOUNT_AMOUNT6", SqlDbType.Decimal).SqlValue = 0;
                        }

                        cmd.ExecuteNonQuery(); 
                    }
                     transaction.Commit();
                }
                catch (Exception ex )
                {
                    MessageBox.Show("Tip:" + ex.GetType().ToString() + ",  Hata Mesajı:" + ex.Message, "Fatura Kaydetme Hatası", MessageBoxButtons.OK);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show( "Tip:" + ex2.GetType().ToString() + ",  Hata Mesajı:" + ex2.Message, "Rolll Back Hatası", MessageBoxButtons.OK);
                    }
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            } 
            return remoteRef;
        }
        
        public string getProductCodeFromProviderCode(string code)
        {
            string productCode = "";

            try
            {
                String Qry = "SELECT sto_kod ";
                Qry += " FROM STOKLAR WITH (NOLOCK) ";
                Qry += " WHERE sto_uretici_kodu = '" + code + "'";
                 
                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    productCode = dr["sto_kod"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(code + " Kod'lu Ürünün Bilgileri Mikrodan Alınamadı.  Bu Ürünün Tanımlı Olduğundan Emin Olunuz..  Hata:" + e.Message, "Ürün Kodu Hatası", MessageBoxButtons.OK);
            }
            return productCode;
        }

        public int getProfileIDFromCustomerCodeMikro(string code)
        {
            int profileID = 0 ;

            try
            {
                String Qry = "SELECT cari_efatura_fl ";
                Qry += " FROM CARI_HESAPLAR WITH (NOLOCK) ";
                Qry += " WHERE cari_kod = '" + code + "'";
                 


               SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    profileID = Convert.ToInt32(dr["cari_efatura_fl"]);
                }
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(code + " Kod'lu Müşterinin Bilgileri Mikrodan Alınamadı.  Bu müşterinin Tanımlı Olduğundan Emin Olunuz..  Hata:" + e.Message , "Cari Kart Hatası", MessageBoxButtons.OK);
            }
            return profileID;
        }

        public string checkInvoiceNumber(string invoiceNumber, string customerCode)
        {
            string invoiceGuid = "";

            try
            {
                string profileId = getProfileIDFromCustomerCodeMikro(customerCode).ToString();

                String Qry = "SELECT top 1 cha_Guid ";
                Qry += " FROM CARI_HESAP_HAREKETLERI WITH (NOLOCK) ";
                Qry += " WHERE cha_evrakno_seri = " ;
                Qry += " (case when '" + profileId + "' = '1' then 'KZL'  else 'KZA' end) and cha_diger_belge_adi =  '" + invoiceNumber +"' ";
                  
                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    invoiceGuid = (dr["cha_Guid"]).ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Beklenmeyen Bir Hata Oluştu", MessageBoxButtons.OK);
            }
            return invoiceGuid;
        }
    }
}
