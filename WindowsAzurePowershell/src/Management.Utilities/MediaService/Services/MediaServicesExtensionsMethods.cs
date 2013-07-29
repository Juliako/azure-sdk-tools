using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService.Services
{
   

    public static class MediaServicesExtensionsMethods
    {
        public static MediaServiceAccounts GetMediaServices(this IMediaServiceManagement proxy, string subscriptionName)
        {
            return proxy.EndGetMediaServices(proxy.BeginGetMediaServices(subscriptionName,null,null));
        }

        public static void DeleteMediaServicesAccount(this IMediaServiceManagement proxy, string subscriptionName, string accountName)
        {
            proxy.EndDeleteMediaServicesAccount(
                proxy.BeginDeleteMediaServicesAccount(subscriptionName, accountName, null, null));
        }

        public static void RegenerateMediaServicesAccount(this IMediaServiceManagement proxy, string subscriptionName, string accountName, string keyType)
        {
            proxy.EndRegenerateMediaServicesAccount(
                proxy.BeginRegenerateMediaServicesAccount(subscriptionName, accountName, keyType, null, null));
        }
    }
}