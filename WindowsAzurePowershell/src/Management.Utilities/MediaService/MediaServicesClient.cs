using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Management.Utilities.CloudService;
using Microsoft.WindowsAzure.Management.Utilities.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services;
using Microsoft.WindowsAzure.ServiceManagement;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService
{
    public class MediaServicesClient: IMediaServicesClient
    {
        public List<string> GetMediaServiceAccounts()
        {
            throw new System.NotImplementedException();
        }


        private string subscriptionId;

        private CloudServiceClient cloudServiceClient;

        public const string WebsitesServiceVersion = "2012-12-01";

        public IMediaServiceManagement MediaServiceChannel { get; internal set; }

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
            HeadersInspector.RequestHeaders.Add(ServiceManagement.Constants.VersionHeaderName, WebsitesServiceVersion);
            HeadersInspector.RequestHeaders.Add(ApiConstants.UserAgentHeaderName, ApiConstants.UserAgentHeaderValue);
            HeadersInspector.RemoveHeaders.Add(ApiConstants.VSDebuggerCausalityDataHeaderName);
            MediaServiceChannel = ChannelHelper.CreateChannel<IMediaServiceManagement>(
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
    }
}