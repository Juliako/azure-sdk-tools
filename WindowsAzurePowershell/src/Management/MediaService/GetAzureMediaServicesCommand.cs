using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.WindowsAzure.Management.Utilities.MediaService;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Microsoft.WindowsAzure.Management.Utilities.Websites.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services;

namespace Microsoft.WindowsAzure.Management.MediaService
{
     
    
   

      
    /// <summary>
    /// Gets an azure website.
    /// </summary>
   [Cmdlet(VerbsCommon.Get, "AzureMediaServices"), OutputType(typeof(MediaServiceAccount), typeof(IEnumerable<MediaServiceAccount>))]
    public class GetAzureMediaServicesCommand : MediaServiceBaseCmdlet
    {
       [Parameter(Position = 0, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The media service account name.")]
       [ValidateNotNullOrEmpty]
       public string Name
       {
           get;
           set;
       }

       /// <summary>
        /// Initializes a new instance of the GetAzureWebsiteCommand class.
        /// </summary>
        public GetAzureMediaServicesCommand()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GetAzureWebsiteCommand class.
        /// </summary>
        /// <param name="channel">
        /// Channel used for communication with Azure's service management APIs.
        /// </param>
        public GetAzureMediaServicesCommand(IMediaServiceManagement channel)
        {
            Channel = channel;
        }

       protected virtual void WriteMediaAccounts(IEnumerable<MediaServiceAccount> mediaServiceAccounts)
       {
           WriteObject(mediaServiceAccounts, true);
       }
        public IMediaServicesClient MediaServicesClient { get; set; }

        public override void ExecuteCmdlet()
        {
            
            List<MediaServiceAccount> accounts = new List<MediaServiceAccount>();
           
                InvokeInOperationContext(() =>
                {
                    accounts.AddRange(RetryCall(s => Channel.GetMediaServices(s)));
                    WaitForOperation(CommandRuntime.ToString());
                });

            // Output results
                WriteMediaAccounts(accounts);
        }
    }
    }
