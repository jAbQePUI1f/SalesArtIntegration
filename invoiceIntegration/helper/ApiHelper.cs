using invoiceIntegration.config;
using invoiceIntegration.model;
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
    }
}
