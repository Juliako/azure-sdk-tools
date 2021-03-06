﻿using Microsoft.WindowsAzure.Management.Utilities.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services;
using ServiceError = Microsoft.WindowsAzure.Management.Utilities.Websites.Services.ServiceError;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService
{
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

    namespace Microsoft.WindowsAzure.Management.Utilities.Websites.Common
    {
        using System;
        using System.IO;
        using System.Linq;
        using System.Net;
        using System.ServiceModel;
        using System.Xml.Serialization;
        using Properties;
        using ServiceManagement;

        public abstract class MediaServiceBaseCmdlet : CloudBaseCmdlet<IMediaServiceManagementAzureNamespace>
        {
            protected override Operation WaitForOperation(string opdesc)
            {
                string operationId = RetrieveOperationId();
                Operation operation = new Operation();
                operation.OperationTrackingId = operationId;
                operation.Status = "Success";
                return operation;
            }

            protected string ProcessException(Exception ex)
            {
                return ProcessException(ex, true);
            }

            protected string ProcessException(Exception ex, bool showError)
            {
                if (ex.InnerException is WebException)
                {
                    WebException webException = ex.InnerException as WebException;
                    if (webException != null && webException.Response != null)
                    {
                        using (StreamReader streamReader = new StreamReader(webException.Response.GetResponseStream()))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ServiceError));
                            ServiceError serviceError = (ServiceError)serializer.Deserialize(streamReader);

                            if (showError)
                            {
                                WriteExceptionError(new Exception(serviceError.Message));
                            }

                            return serviceError.Message;
                        }
                    }
                }

                if (showError)
                {
                    WriteExceptionError(ex);
                }

                return ex.Message;
            }

            protected override void ProcessRecord()
            {
                try
                {
                    base.ProcessRecord();
                }
                catch (EndpointNotFoundException ex)
                {
                    ProcessException(ex);
                }
                catch (ProtocolException ex)
                {
                    ProcessException(ex);
                }
            }
        }
    }

}