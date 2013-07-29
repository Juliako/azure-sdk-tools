using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.WindowsAzure.Management.Utilities.MediaService;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Microsoft.WindowsAzure.Management.Utilities.Websites.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services;
using System;
using Microsoft.WindowsAzure.Management.Utilities.Properties;

namespace Microsoft.WindowsAzure.Management.MediaService
{
    public enum KeyType { Primary, Secondary }

    /// <summary>
    /// Gets an azure website.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AzureMediaServicesAccountKey", SupportsShouldProcess = true), OutputType(typeof(string))]
    public class NewAzureMediaServicesKeyCommand : MediaServiceBaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The media services account name.")]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get;
            set;
        }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The media services key type <Primary|Secondary>.")]
        [ValidateNotNullOrEmpty]
        public KeyType KeyType
        {
            get;
            set;
        }

        [Parameter(Position = 2, HelpMessage = "Do not confirm regeneration of the key.")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// Initializes a new instance of the NewAzureMediaServicesKeyCommand class.
        /// </summary>
        public NewAzureMediaServicesKeyCommand()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NewAzureMediaServicesKeyCommand class.
        /// </summary>
        /// <param name="channel">
        /// Channel used for communication with Azure's service management APIs.
        /// </param>
        public NewAzureMediaServicesKeyCommand(IMediaServiceManagement channel)
        {
            Channel = channel;
        }

        public IMediaServicesClient MediaServicesClient { get; set; }

        public override void ExecuteCmdlet()
        {
            ConfirmAction(
                Force.IsPresent,
                string.Format(Resources.RegenerateKeyWarning),
                Resources.RegenerateKeyWhatIfMessage,
                string.Empty,
                () =>
                {
                    InvokeInOperationContext(() =>
                    {
                        RetryCall(s =>
                        {
                            var service = Channel.GetMediaServices(s).Find(acc => acc.Name == this.Name);
                            if (service == null)
                            {
                                throw new Exception(string.Format(Resources.InvalidMediaServicesAccount, Name));
                            }

                            Channel.RegenerateMediaServicesAccount(s, this.Name, KeyType.ToString());

                            WriteObject("newkey");
                        });
                    });
                });
        }
    }
}
