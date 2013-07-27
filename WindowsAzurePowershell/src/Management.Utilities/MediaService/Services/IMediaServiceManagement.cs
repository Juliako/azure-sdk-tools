// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;
using Microsoft.WindowsAzure.Management.Utilities.Websites;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService.Services
{
    [XmlRoot(ElementName = "Error", Namespace = UriElements.ServiceNamespace)]
    public class ServiceError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string ExtendedCode { get; set; }
        public string MessageTemplate { get; set; }

        [XmlArray("Parameters")]
        [XmlArrayItem(typeof(string), Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
        public List<string> Parameters { get; set; }
    }

    /// <summary>
    /// Provides the Windows Azure Service Management Api for Windows Azure Websites. 
    /// </summary>
    [ServiceContract(Namespace = UriElements.ServiceNamespace)]
    [ServiceKnownType(typeof(MediaServiceAccount))]
    public interface IMediaServiceManagement
    {

        #region Site CRUD

        [Description("Returns all the sites for a given subscription and webspace.")]
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "GET", UriTemplate = UriElements.MediaServiceRoot)]
        IAsyncResult BeginGetMediaServices(string subscriptionName, AsyncCallback callback, object state);
        MediaServiceAccounts EndGetMediaServices(IAsyncResult asyncResult);

        [Description("Deletes the account for a given subscription.")]
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(Method = "DELETE", UriTemplate = UriElements.MediaServiceRoot + "/{accountName}")]
        IAsyncResult BeginDeleteMediaServicesAccount(
            string subscriptionName,
            string accountName,
            AsyncCallback callback,
            object state);
        void EndDeleteMediaServicesAccount(IAsyncResult asyncResult);
        #endregion        
    }
}
