using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.WindowsAzure.Management.Utilities.MediaService;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Microsoft.WindowsAzure.Management.Utilities.Websites.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services;
using System;
using Microsoft.WindowsAzure.Management.Utilities.Properties;
using System.Net;

namespace Microsoft.WindowsAzure.Management.MediaService
{
    /// <summary>
    /// Gets an azure website.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AzureMediaServicesAccount", SupportsShouldProcess = true), OutputType(typeof(bool))]
    public class RemoveAzureMediaServicesAccountCommand : MediaServiceBaseCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The media services account name.")]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            get;
            set;
        }

        [Parameter(Position = 1, HelpMessage = "Do not confirm deletion of account.")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// Initializes a new instance of the GetAzureWebsiteCommand class.
        /// </summary>
        public RemoveAzureMediaServicesAccountCommand()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GetAzureWebsiteCommand class.
        /// </summary>
        /// <param name="channel">
        /// Channel used for communication with Azure's service management APIs.
        /// </param>
        public RemoveAzureMediaServicesAccountCommand(IMediaServiceManagementAzureNamespace channel)
        {
            Channel = channel;
        }

        public IMediaServicesClient MediaServicesClient { get; set; }

        public override void ExecuteCmdlet()
        {

            ConfirmAction(
                Force.IsPresent,
                string.Format(Resources.RemoveMediaAccountWarning),
                Resources.RemoveMediaAccountWhatIfMessage,
                string.Empty,
                () =>
                {
                    InvokeInOperationContext(() =>
                    {
                        RetryCall(s =>
                        {
                            try
                            {
                                Channel.DeleteMediaServicesAccount(s, this.Name);
                            }
                            catch (Exception x)
                            {
                                var webx = x.InnerException as WebException;
                                if (webx != null && ((HttpWebResponse)webx.Response).StatusCode == HttpStatusCode.NotFound)
                                {
                                    throw new Exception(string.Format(Resources.InvalidMediaServicesAccount, Name));
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        });
                    });

                    WriteObject(true);
                });
        }
    }
}
