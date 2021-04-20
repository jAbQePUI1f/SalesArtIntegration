using invoiceIntegration.config;
using invoiceIntegration.model;
using invoiceIntegration.model.order;
using invoiceIntegration.model.waybill;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static invoiceIntegration.frmMain;

namespace invoiceIntegration.helper
{
    public class ResponseHelper
    {
        string url = Configuration.getUrl();
        public void SendResponse(IntegratedInvoiceStatus integratedInvoices)
        {
            try
            {
                RestClient restClient = new RestClient(url);
                RestRequest restRequest = new RestRequest("/integration/invoices/sync-statuses?", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                restRequest.AddJsonBody(integratedInvoices);
                var requestResponse = restClient.Execute<StatusResponse>(restRequest);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                GenericResponse<List<StatusResponse>> invResponse =
                    JsonConvert.DeserializeObject<GenericResponse<List<StatusResponse>>>(requestResponse.Content, settings);
            }
            catch
            {
                MessageBox.Show("Fatura Logoya Aktarıldı Fakat salesArt ' taki durumu güncellenemedi.. ", "Fatura Statüsünün Gücellenememesi", MessageBoxButtons.OK);
            }
        }
        public void SendResponse(IntegratedWaybillStatus integratedWaybills)
        {
            try
            {
                RestClient restClient = new RestClient(url);
                RestRequest restRequest = new RestRequest("/integration/waybills/sync-statuses?", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                restRequest.AddJsonBody(integratedWaybills);
                var requestResponse = restClient.Execute<StatusResponse>(restRequest);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                GenericResponse<List<StatusResponse>> invResponse = 
                    JsonConvert.DeserializeObject<GenericResponse<List<StatusResponse>>>(requestResponse.Content, settings);
            }
            catch
            {
                MessageBox.Show("İrsaliye Logoya Aktarıldı Fakat salesArt ' taki durumu güncellenemedi.. ", "İrsaliye Statüsünün Gücellenememesi", MessageBoxButtons.OK);
            }

        }
        public void SendResponse(IntegratedOrderStatus integratedOrders)
        {
            try
            {
                RestClient restClient = new RestClient(url);
                RestRequest restRequest = new RestRequest("/integration/orders/sync", Method.PUT)
                {
                    RequestFormat = DataFormat.Json
                };
                restRequest.AddJsonBody(integratedOrders);
                var requestResponse = restClient.Execute<StatusResponse>(restRequest);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                GenericResponse<List<StatusResponse>> invResponse = 
                    JsonConvert.DeserializeObject<GenericResponse<List<StatusResponse>>>(requestResponse.Content, settings);
            }
            catch
            {
                MessageBox.Show("Sipariş Logoya Aktarıldı Fakat salesArt ' taki durumu güncellenemedi.. ", "Fatura Statüsünün Gücellenememesi", MessageBoxButtons.OK);
            }

        }       
    }
}
