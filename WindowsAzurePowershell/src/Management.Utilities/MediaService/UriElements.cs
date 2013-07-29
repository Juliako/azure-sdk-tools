﻿// ----------------------------------------------------------------------------------
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

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService
{

    public class UriElements
    {
        public const string AccountDetailsNamespace = "http://schemas.datacontract.org/2004/07/Microsoft.Cloud.Media.Management.ResourceProvider.Models";
        public const string ServiceNamespace = "http://schemas.microsoft.com/windowsazure";
        // Parameters
       

        // Service resources
        public const string MediaServiceRoot = "{subscriptionName}/services/mediaservices/Accounts";
        public const string MediaServiceAccountDetails = "{subscriptionName}/services/mediaservices/Accounts/{name}";

        public const string Accounts = "Accounts";
    }
}