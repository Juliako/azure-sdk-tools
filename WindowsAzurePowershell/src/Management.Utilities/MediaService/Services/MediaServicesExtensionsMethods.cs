using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService.Services
{
   

    public static class MediaServicesExtensionsMethods
    {
        public static MediaServiceAccounts GetMediaServices(this IMediaServiceManagement proxy, string subscriptionName)
        {
            return proxy.EndGetMediaServices(proxy.BeginGetMediaServices(subscriptionName,null,null));
        }
    }
}