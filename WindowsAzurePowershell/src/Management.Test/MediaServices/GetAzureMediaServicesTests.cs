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
using Microsoft.WindowsAzure.Management.MediaService;
using Microsoft.WindowsAzure.Management.Test.Utilities.MediaServices;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;

namespace Microsoft.WindowsAzure.Management.Test.MediaServices
{
    using System.Collections.Generic;
    using System.Linq;
    using Utilities.Common;
    using Utilities.Websites;
    using Management.Utilities.Common;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetAzureMediaServicesTests : WebsitesTestBase
    {
        [TestMethod]
        public void ProcessGetMediaServicesTest()
        {
            // Setup
            SimpleMediaServiceManagementAzureNamespace channel = new SimpleMediaServiceManagementAzureNamespace();
          
            channel.GetMediaServicesThunk = ar =>
            {
                return new MediaServiceAccounts(new List<MediaServiceAccount> { new MediaServiceAccount { Name = "media" } });
            };

            // Test
            GetAzureMediaServicesCommand getAzureWebsiteCommand = new GetAzureMediaServicesCommand(channel)
            {
                ShareChannel = true,
                CommandRuntime = new MockCommandRuntime(),
                CurrentSubscription = new SubscriptionData { SubscriptionId = base.subscriptionId }
            };

            getAzureWebsiteCommand.ExecuteCmdlet();
            Assert.AreEqual(1, ((MockCommandRuntime)getAzureWebsiteCommand.CommandRuntime).OutputPipeline.Count);
            var accounts = (IEnumerable<MediaServiceAccount>)((MockCommandRuntime)getAzureWebsiteCommand.CommandRuntime).OutputPipeline.FirstOrDefault();
            Assert.IsNotNull(accounts);
           
        }

        [TestMethod]
        public void ProcessGetMediaServiceByNameShouldReturnOneMatchingEntry()
        {
            // Setup
            SimpleMediaServiceManagementAzureNamespace channel = new SimpleMediaServiceManagementAzureNamespace();

            const string expectedName = "media1";
            channel.GetMediaServicesThunk = ar =>
            {
                return new MediaServiceAccounts(new List<MediaServiceAccount> { new MediaServiceAccount { Name = expectedName }, new MediaServiceAccount { Name = "media2" } });
            };

            // Test
            GetAzureMediaServicesCommand getAzureWebsiteCommand = new GetAzureMediaServicesCommand(channel)
            {
                ShareChannel = true,
                CommandRuntime = new MockCommandRuntime(),
                CurrentSubscription = new SubscriptionData { SubscriptionId = base.subscriptionId }
            };
            getAzureWebsiteCommand.Name = expectedName;
            getAzureWebsiteCommand.ExecuteCmdlet();
            Assert.AreEqual(1, ((MockCommandRuntime)getAzureWebsiteCommand.CommandRuntime).OutputPipeline.Count);
            var accounts = (IEnumerable<MediaServiceAccount>)((MockCommandRuntime)getAzureWebsiteCommand.CommandRuntime).OutputPipeline.FirstOrDefault();
            Assert.IsNotNull(accounts);
            Assert.AreEqual(expectedName,accounts.FirstOrDefault().Name);

        }

        [TestMethod]
        public void ProcessGetMediaServiceByNameShouldNotReturnEntriesForNoneMatchingName()
        {
            // Setup
            SimpleMediaServiceManagementAzureNamespace channel = new SimpleMediaServiceManagementAzureNamespace();

            const string expectedName = "media1";
            channel.GetMediaServicesThunk = ar =>
            {
                return new MediaServiceAccounts(new List<MediaServiceAccount> { new MediaServiceAccount { Name = expectedName }, new MediaServiceAccount { Name = "media2" } });
            };

            // Test
            GetAzureMediaServicesCommand getAzureWebsiteCommand = new GetAzureMediaServicesCommand(channel)
            {
                ShareChannel = true,
                CommandRuntime = new MockCommandRuntime(),
                CurrentSubscription = new SubscriptionData { SubscriptionId = base.subscriptionId }
            };
            getAzureWebsiteCommand.Name = Guid.NewGuid().ToString();
            getAzureWebsiteCommand.ExecuteCmdlet();
            Assert.AreEqual(0, ((MockCommandRuntime)getAzureWebsiteCommand.CommandRuntime).OutputPipeline.Count);
            

        }
         
    }
}