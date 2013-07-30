using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.WindowsAzure.Management.Utilities.MediaService;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Microsoft.WindowsAzure.Management.Utilities.Websites.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;

namespace Microsoft.WindowsAzure.Management.MediaService
{
    [Cmdlet(VerbsCommon.New, "AzureMediaServices"), OutputType(typeof(AccountCreationResult))]
    public class NewAzureMediaServiceCommand : MediaServiceBaseCmdlet
    {

        public IMediaServicesClient MediaServicesClient { get; set; }


        [Parameter(Position = 0, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The media service account name.")]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get;
            set;
        }

        [Parameter(Position = 1, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The media service region.")]
        [ValidateNotNullOrEmpty]
        public string Region { get; set; }

        [Parameter(Position = 2, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Azure blobstorage endpoint uri.")]
        [ValidateNotNullOrEmpty]
        public string BlobStorageEndpointUri { get; set; }

        [Parameter(Position = 3, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Azure storage account name")]
        [ValidateNotNullOrEmpty]
        public string StorageAccountName { get; set; }

        [Parameter(Position = 4, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Azure storage account key")]
        [ValidateNotNullOrEmpty]
        public string StorageAccountKey { get; set; }


        public override void ExecuteCmdlet()
        {

            MediaServicesClient = MediaServicesClient ?? new MediaServicesClient(CurrentSubscription, WriteDebug);


            AccountCreationResult result = null;
            var request = new AccountCreationRequest()
            {
                AccountName = Name,
                BlobStorageEndpointUri = BlobStorageEndpointUri,
                Region = Region,
                StorageAccountKey = StorageAccountKey,
                StorageAccountName = StorageAccountName
            };

            InvokeInOperationContext(() =>
            {
                result = RetryCall(s => MediaServicesClient.NewAzureMediaService(request));

            });
            WriteObject(result, false);


        }
    }

}

