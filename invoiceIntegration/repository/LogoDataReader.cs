using invoiceIntegration.config;
using invoiceIntegration.helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
