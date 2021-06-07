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
        string season = Configuration.getSeason();
        string companyCode = Configuration.getCompanyCode();
        string conString = Configuration.getLogoConnection();
        Helper helper = new Helper(); 
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
                SqlCommand cmd = conn.CreateCommand();
                SqlTransaction transaction;
                transaction = conn.BeginTransaction("InvoiceTransaction");
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                try
                {
                    string profileID = getProfileIDFromCustomerCodeMikro(invoice.customerCode).ToString();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_InsertInvoice_SCJ";
                    cmd.Parameters.AddWithValue("@ERP_CARI_KOD", invoice.customerCode);
                    if (invoice.customerBranchName != null)
                    {
                        cmd.Parameters.AddWithValue("@ERP_CARI_SUBE_ADI", invoice.customerBranchName);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ERP_CARI_SUBE_ADI", "Şube Adı Boş");
                    }

                    if (invoice.salesmanCode != null)
                    {
                        cmd.Parameters.AddWithValue("@SALESMAN_CODE", invoice.salesmanCode);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SALESMAN_CODE", "Plasiyer Kod Boş");
                    }
                    if (invoice.invoiceType == InvoiceType.BUYING || invoice.invoiceType == InvoiceType.SELLING_RETURN || invoice.invoiceType == InvoiceType.DAMAGED_SELLING_RETURN)
                    {
                        cmd.Parameters.AddWithValue("@CHA_TIP", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CHA_TIP", 0);
                    }
                    if (invoice.invoiceType == InvoiceType.SELLING_RETURN || invoice.invoiceType == InvoiceType.BUYING_RETURN || invoice.invoiceType == InvoiceType.DAMAGED_BUYING_RETURN || invoice.invoiceType == InvoiceType.DAMAGED_SELLING_RETURN)
                    {
                        cmd.Parameters.AddWithValue("@CHA_NORMAL_IADE", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CHA_NORMAL_IADE", 0);
                    }

                    cmd.Parameters.AddWithValue("@PROFILE_ID", profileID);   // 0:Ticari Fatura 1:Temel Fatura
                    cmd.Parameters.AddWithValue("@INVOICE_TYPE_CODE", ((invoice.invoiceType == InvoiceType.BUYING_RETURN || invoice.invoiceType == InvoiceType.DAMAGED_BUYING_RETURN || invoice.invoiceType == InvoiceType.DAMAGED_SELLING_RETURN || invoice.invoiceType == InvoiceType.SELLING_RETURN) ? 1 : 0).ToString());  //0:Normal 1:İade
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
                    cmd.Parameters.AddWithValue("@KASAHIZMET", SqlDbType.TinyInt).SqlValue = 0;
                    // (0 gelecek -- 0:Carimiz 1:Cari Personelimiz 2:Bankamız 3:Hizmetimiz 4:Kasamız 5:Giderimiz 6:Muhasebe                     
                    cmd.Parameters.AddWithValue("@CINSI", SqlDbType.TinyInt).SqlValue = 6;
                    cmd.Parameters.AddWithValue("@CHECK_UUID", SqlDbType.Bit).SqlValue = 1;  // fatura numarası içeeride daha önce kaydedilmiş mi kontro ledilsin mi ? 1 ise evet
                    cmd.Parameters.AddWithValue("@SATIRNO", 0);  // 0 olacak , cünkü her bir fsturanın tek bir header'ı olacak

                    if (invoice.invoiceType == InvoiceType.SELLING || invoice.invoiceType == InvoiceType.DAMAGED_BUYING_RETURN || invoice.invoiceType == InvoiceType.BUYING_RETURN)
                    {
                        cmd.Parameters.AddWithValue("@EVRAKTIP", 63);  // 63:Satış Faturası                     
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EVRAKTIP", 0);  // 0:Alış Faturası 
                    }

                    SqlParameter sqlParam = new SqlParameter("@id", SqlDbType.UniqueIdentifier);
                    sqlParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sqlParam);
                    cmd.ExecuteNonQuery();
                    remoteRef = sqlParam.Value.ToString();
                    cmd.CommandText = "SP_InsertInvoiceDetail_SCJ";
                    foreach (var detail in invoice.details)
                    {
                        cmd.Parameters.Clear();
                        if (invoice.invoiceType == InvoiceType.SELLING || invoice.invoiceType == InvoiceType.BUYING)
                        {
                            cmd.Parameters.AddWithValue("@INVOICE_TYPE_CODE", "NORMAL");  //0:Normal 1:İade
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@INVOICE_TYPE_CODE", "IADE");
                        }
                        cmd.Parameters.AddWithValue("@INVOICE_NUMBER", invoice.number);
                        if (invoice.salesmanCode != null)
                        {
                            cmd.Parameters.AddWithValue("@SALESMAN_CODE", invoice.salesmanCode);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@SALESMAN_CODE", "Plasiyer Kod Boş");
                        }
                        if (invoice.invoiceType == InvoiceType.DAMAGED_SELLING_RETURN || invoice.invoiceType == InvoiceType.BUYING || invoice.invoiceType == InvoiceType.SELLING_RETURN)
                        {
                            if (invoice.ebillCustomer)
                            {
                                cmd.Parameters.AddWithValue("@ST_EVRAK_TIP", 13);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ST_EVRAK_TIP", 3);
                            }
                            cmd.Parameters.AddWithValue("@ST_TIP", 0);
                        }
                        else
                        {
                            if (invoice.ebillCustomer)
                            {
                                cmd.Parameters.AddWithValue("@ST_EVRAK_TIP", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ST_EVRAK_TIP", 4);
                            }
                            cmd.Parameters.AddWithValue("@ST_TIP", 1);
                        }
                        if (invoice.ebillCustomer)
                        {
                            cmd.Parameters.AddWithValue("@EbillCustomer", "EFatura");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@EbillCustomer", "");
                        }
                        cmd.Parameters.AddWithValue("@WAREHOUSE_CODE", invoice.wareHouseCode); //Depo Kod
                        cmd.Parameters.AddWithValue("@ISSUE_DATE", invoice.date);
                        cmd.Parameters.AddWithValue("@ERP_CARI_KOD", invoice.customerCode);
                        cmd.Parameters.AddWithValue("@ERP_PRODUCT_STOK_KOD", detail.code);
                        cmd.Parameters.AddWithValue("@QUANTITY_AMOUNT", SqlDbType.Decimal).SqlValue = detail.grossTotal; // Quantity * Price
                        if (detail.unitCode == constants.UnitCodeType.KOLI || detail.unitCode == constants.UnitCodeType.KL)
                        {
                            cmd.Parameters.AddWithValue("@QUANTITY", SqlDbType.Decimal).SqlValue = detail.quantity;
                            cmd.Parameters.AddWithValue("@UNIT_CODE", SqlDbType.Decimal).SqlValue = 1;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@QUANTITY", SqlDbType.Decimal).SqlValue = (detail.quantity / getProductConversionFactor(detail.code));
                            cmd.Parameters.AddWithValue("@UNIT_CODE", SqlDbType.Decimal).SqlValue = 2;
                        }
                        cmd.Parameters.AddWithValue("@TAX_PERCENTAGE", detail.vatRate);
                        cmd.Parameters.AddWithValue("@TAX_AMOUNT", detail.vatAmount);  // vergi tutarı
                        cmd.Parameters.AddWithValue("@ITEM_NOTE", "");
                        cmd.Parameters.AddWithValue("@PROFILE_ID", profileID);
                        if (detail.discounts != null && detail.discounts.Count > 0)
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
                catch (Exception ex)
                {
                    MessageBox.Show("Tip:" + ex.GetType().ToString() + ",  Hata Mesajı:" + ex.Message, "Fatura Kaydetme Hatası", MessageBoxButtons.OK);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Tip:" + ex2.GetType().ToString() + ",  Hata Mesajı:" + ex2.Message, "Rolll Back Hatası", MessageBoxButtons.OK);
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

        public decimal getProductConversionFactor(string code)
        {
            decimal conversionFactor = 0;
            try
            {
                String Qry = "SELECT sto_birim2_katsayi ";
                Qry += " FROM STOKLAR WITH (NOLOCK) ";
                Qry += " WHERE sto_kod = '" + code + "'";
                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;
                conn.Open();
                SqlDataReader dr = sqlCmd.ExecuteReader();
                if (dr.Read())
                {
                    conversionFactor = Convert.ToDecimal(dr["sto_birim2_katsayi"]);
                }
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(code + " Kod'lu Ürünün Bilgileri Mikrodan Alınamadı.  Bu Ürünün Tanımlı Olduğundan Emin Olunuz..  Hata:" + e.Message, "Ürün Kodu Hatası", MessageBoxButtons.OK);
            }
            return conversionFactor;
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
        
        public string getServiceCodeBySalesArtServiceCode(string serviceCode, int serviceType)
        {
            string erpServiceCode = "";

            try
            {
                String Qry = "SELECT CODE";
                Qry += " FROM LG_" + companyCode + "_SRVCARD WITH (NOLOCK) ";
                Qry += " WHERE   SPECODE5  = '" + serviceCode + "' AND CARDTYPE = '" + serviceType + "'  AND ACTIVE =  0 ";

                helper.LogFile("SQL Query", serviceCode, "", "", Qry);

                SqlConnection conn = new SqlConnection(conString);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = Qry;
                sqlCmd.Connection = conn;

                conn.Open();

                SqlDataReader dr = sqlCmd.ExecuteReader();


                if (dr.Read())
                {
                    erpServiceCode = dr["CODE"].ToString();
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                helper.LogFile("SQL Query", serviceCode, "", "", e.Message + "__" + serviceCode);
            }
            return erpServiceCode;
        }
    }
}
