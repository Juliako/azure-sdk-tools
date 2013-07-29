using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService.Services
{
   

    public static class MediaServicesExtensionsMethods
    {
        public static MediaServiceAccounts GetMediaServices(this IMediaServiceManagementAzureNamespace proxy, string subscriptionName)
        {
            return proxy.EndGetMediaServices(proxy.BeginGetMediaServices(subscriptionName,null,null));
        }

        public static MediaServiceAccountDetails GetMediaService(this IMediaServiceManagementAzureNamespace proxy, string subscriptionName, string name)
        {
            return proxy.EndGetMediaService(proxy.BeginGetMediaService(subscriptionName,name, null, null));
        }
        public static MediaServiceAccountDetails GetMediaService(this IMediaServiceManagementResourceProviderNamespace proxy, string subscriptionName, string name)
        {
            return proxy.EndGetMediaService(proxy.BeginGetMediaService(subscriptionName,name, null, null));
        }
    }
}