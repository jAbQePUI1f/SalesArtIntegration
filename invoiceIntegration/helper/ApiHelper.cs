using invoiceIntegration.config;
using invoiceIntegration.model;
using invoiceIntegration.model.order;
using invoiceIntegration.model.waybill;
using MetroFramework.Controls;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static invoiceIntegration.frmMain;

namespace invoiceIntegration.helper
{
    public class ApiHelper
    {
        int distributorId = Configuration.getDistributorId();
        GenericResponse<List<LogoInvoiceJson>> jsonInvoices = new GenericResponse<List<LogoInvoiceJson>>();
        GenericResponse<List<LogoWaybillJson>> jsonWaybills = new GenericResponse<List<LogoWaybillJson>>();
        GenericResponse<OrderResponse> jsonOrders = new GenericResponse<OrderResponse>();
        public GenericResponse<List<LogoInvoiceJson>> GetInvoices(MetroDateTime startDate, MetroDateTime endDate, string invoiceType, DataGridView dataGridInvoice)
        {
            GridHelper gridHelper = new GridHelper();
            RestClient restClient = new RestClient(Configuration.getUrl());
            RestRequest restRequest = new RestRequest("/integration/invoices?", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            GetTransferableInvoicesRequest req = new GetTransferableInvoicesRequest()
            {
                startDate = startDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                endDate = endDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                invoiceTypes = new List<string>()
            };
            req.invoiceTypes.Add(invoiceType.ToString());
            restRequest.AddParameter("distributorId", distributorId, ParameterType.QueryString);
            restRequest.AddJsonBody(req);
            var requestResponse = restClient.Execute<LogoInvoice>(restRequest);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            jsonInvoices = JsonConvert.DeserializeObject<GenericResponse<List<LogoInvoiceJson>>>(requestResponse.Content, settings);
            gridHelper.FillGrid(jsonInvoices.data.Cast<dynamic>().ToList(), dataGridInvoice, constants.ListType.LogoInvoiceJson);
            return jsonInvoices;
        }
        public GenericResponse<List<LogoWaybillJson>> GetWaybills(MetroDateTime startDate, MetroDateTime endDate, string invoiceType, DataGridView dataGridInvoice)
        {
            GridHelper gridHelper = new GridHelper();
            RestClient restClient = new RestClient(Configuration.getUrl());
            RestRequest restRequest = new RestRequest("/integration/waybills?", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            GetTransferableWaybillsRequest req = new GetTransferableWaybillsRequest()
            {
                startDate = startDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                endDate = endDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                waybillTypes = new List<string>()
            };
            req.waybillTypes.Add(invoiceType.ToString());
            restRequest.AddParameter("distributorId", distributorId, ParameterType.QueryString);
            restRequest.AddJsonBody(req);
            var requestResponse = restClient.Execute<LogoWaybill>(restRequest);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            jsonWaybills = JsonConvert.DeserializeObject<GenericResponse<List<LogoWaybillJson>>>(requestResponse.Content, settings);
            gridHelper.FillGrid(jsonWaybills.data.Cast<dynamic>().ToList(), dataGridInvoice, constants.ListType.LogoWaybillJson);
            return jsonWaybills;
        }
        public GenericResponse<OrderResponse> GetOrders(MetroDateTime startDate, MetroDateTime endDate, DataGridView dataGridInvoice)
        {
            GridHelper gridHelper = new GridHelper();
            RestClient restClient = new RestClient(Configuration.getUrl());
            RestRequest restRequest = new RestRequest("/integration/orders/", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            GetTransferableOrdersRequest req = new GetTransferableOrdersRequest()
            {
                startDate = startDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                endDate = endDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                distributorId = distributorId
            };
            restRequest.AddJsonBody(req);
            var requestResponse = restClient.Execute<OrderResponse>(restRequest);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            jsonOrders = JsonConvert.DeserializeObject<GenericResponse<OrderResponse>>(requestResponse.Content, settings);
            gridHelper.FillOrdersToGrid(jsonOrders.data, dataGridInvoice);
            return jsonOrders;
        }
    }
}
