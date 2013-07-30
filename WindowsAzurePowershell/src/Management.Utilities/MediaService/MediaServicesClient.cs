using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Management.Utilities.CloudService;
using Microsoft.WindowsAzure.Management.Utilities.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;
using Microsoft.WindowsAzure.ServiceManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceError = Microsoft.WindowsAzure.Management.Utilities.Websites.Services.ServiceError;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService
{
    public class MediaServicesClient: IMediaServicesClient
    {
        public List<MediaServiceAccount> GetMediaServiceAccounts()
        {
            List<MediaServiceAccount> mediaServiceAccounts = new List<MediaServiceAccount>();

            using (HttpClient client = CreateIMediaServicesHttpClient())
            {
               mediaServiceAccounts = client.GetJson<List<MediaServiceAccount>>(UriElements.Accounts, Logger);
            }

            return mediaServiceAccounts;
        }

        public MediaServiceAccountDetails GetMediaService(string name)
        {
            MediaServiceAccountDetails details =null;
            using (HttpClient client = CreateIMediaServicesHttpClient())
            {
                details = client.GetJson<MediaServiceAccountDetails>(String.Format("{0}/{1}", UriElements.Accounts, name), Logger);
            }
            return details;
        }

        public AccountCreationResult NewAzureMediaService(AccountCreationRequest request)
        {
            AccountCreationResult result = null;
            using (HttpClient client = CreateIMediaServicesHttpClient())
            {
                HttpResponseMessage message = client.PostAsJsonAsyncWithoutEnsureSuccessCode(UriElements.Accounts, JObject.FromObject(request), Logger);
                string content =message.Content.ReadAsStringAsync().Result;
                if (message.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject(content, typeof (AccountCreationResult)) as AccountCreationResult;
                }
                else
                {

                    XmlDocument  doc =new XmlDocument();
                    doc.LoadXml(content);
                    content = doc.InnerText;
                    var serviceError = JsonConvert.DeserializeObject(content, typeof(ServiceError)) as ServiceError;
                    //Websites.Services.ServiceError serviceError = (ServiceError) serializer.Deserialize(message.Content.ReadAsStreamAsync().Result);
                    throw new ServiceManagementClientException(message.StatusCode, new ServiceManagementError() { Code = message.StatusCode.ToString(), Message = serviceError.Message}, string.Empty);
                }
            }
            return result;
        }


        private string subscriptionId;

        private CloudServiceClient cloudServiceClient;

        public const string MediaServiceVersion = "2013-03-01";

        public IMediaServiceManagementAzureNamespace MediaServiceChannel { get; internal set; }

        public IServiceManagement ServiceManagementChannel { get; internal set; }

        public SubscriptionData Subscription { get; set; }

        public Action<string> Logger { get; set; }

        public HeadersInspector HeadersInspector { get; set; }
        /// <summary>
        /// Creates new WebsitesClient.
        /// </summary>
        /// <param name="subscription">The Windows Azure subscription data object</param>
        /// <param name="logger">The logger action</param>
        public MediaServicesClient(SubscriptionData subscription, Action<string> logger)
        {
            subscriptionId = subscription.SubscriptionId;
            Subscription = subscription;
            Logger = logger;
            HeadersInspector = new HeadersInspector();
            HeadersInspector.RequestHeaders.Add(Constants.VersionHeaderName, MediaServiceVersion);
            HeadersInspector.RequestHeaders.Add(ApiConstants.UserAgentHeaderName, ApiConstants.UserAgentHeaderValue);
            HeadersInspector.RemoveHeaders.Add(ApiConstants.VSDebuggerCausalityDataHeaderName);
            MediaServiceChannel = ChannelHelper.CreateChannel<IMediaServiceManagementAzureNamespace>(
                ConfigurationConstants.WebHttpBinding(),
                new Uri(subscription.ServiceEndpoint),
                subscription.Certificate,
                HeadersInspector,
                new HttpRestMessageInspector(logger));

            ServiceManagementChannel = ChannelHelper.CreateServiceManagementChannel<IServiceManagement>(
                ConfigurationConstants.WebHttpBinding(),
                new Uri(subscription.ServiceEndpoint),
                subscription.Certificate,
                new HttpRestMessageInspector(logger));

            cloudServiceClient = new CloudServiceClient(subscription, debugStream: logger);
        }

        private HttpClient CreateIMediaServicesHttpClient()
        {
            WebRequestHandler requestHandler = new WebRequestHandler();
            requestHandler.ClientCertificates.Add(Subscription.Certificate);
            StringBuilder endpoint = new StringBuilder(General.EnsureTrailingSlash(Subscription.ServiceEndpoint));
            endpoint.Append(subscriptionId);
            endpoint.Append("/services/mediaservices/");
            HttpClient client = HttpClientHelper.CreateClient(endpoint.ToString(), handler: requestHandler);
            client.DefaultRequestHeaders.Add(ServiceManagement.Constants.VersionHeaderName, MediaServiceVersion);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

           
            return client;
        }
    }
}