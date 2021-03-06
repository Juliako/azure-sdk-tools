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

namespace Microsoft.WindowsAzure.Management.ServiceManagement.IaaS.Endpoints
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Management.Automation;
    using IaaS;
    using Model;
    using Properties;
    using WindowsAzure.ServiceManagement;

    [Cmdlet(VerbsCommon.Set, "AzureEndpoint"), OutputType(typeof(IPersistentVM))]
    public class SetAzureEndpoint : VirtualMachineConfigurationCmdletBase
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Endpoint name.")]
        [ValidateNotNullOrEmpty]
        public string Name 
        {
            get; 
            set; 
        }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "Endpoint protocol.")]
        [ValidateSet("tcp", "udp", IgnoreCase = true)]
        [ValidateNotNullOrEmpty]
        public string Protocol
        { 
            get; 
            set; 
        }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "Local port.")]
        [ValidateNotNullOrEmpty]
        public int LocalPort
        { 
            get; 
            set; 
        }

        [Parameter(Mandatory = false, HelpMessage = "Public port.")]
        [ValidateNotNullOrEmpty]
        public int? PublicPort 
        { 
            get; 
            set;
        }


        [Parameter(Mandatory = false, HelpMessage = "Enable Direct Server Return")]
        [ValidateNotNull]
        public bool? DirectServerReturn
        {
            get; 
            set;
        }

        [Parameter(Mandatory = false, HelpMessage = "ACL Config for the endpoint.")]
        [ValidateNotNull]
        public NetworkAclObject ACL
        {
            get; 
            set; 
        }

        internal void ExecuteCommand()
        {
            ValidateParameters();

            var endpoints = GetInputEndpoints();
            var endpoint = endpoints.SingleOrDefault(p => p.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));

            if (endpoint == null)
            {
                ThrowTerminatingError(
                    new ErrorRecord(
                            new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Resources.EndpointCanNotBeFoundInVMConfigurationInSetAzureEndpoint, this.Name)),
                            string.Empty,
                            ErrorCategory.InvalidData,
                            null));
            }

            endpoint.Port = PublicPort.HasValue ? PublicPort : null;
            endpoint.LocalPort = LocalPort;
            endpoint.Protocol = Protocol;
            endpoint.EndpointAccessControlList = this.ACL;
            endpoint.EnableDirectServerReturn = this.DirectServerReturn;

            WriteObject(VM, true);
        }

        protected override void ProcessRecord()
        {
            try
            {
                base.ProcessRecord();
                ExecuteCommand();
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
            }
        }

        protected Collection<InputEndpoint> GetInputEndpoints()
        {
            var role = VM.GetInstance();

            var networkConfiguration = role.ConfigurationSets
                                        .OfType<NetworkConfigurationSet>()
                                        .SingleOrDefault();

            if (networkConfiguration == null)
            {
                networkConfiguration = new NetworkConfigurationSet();
                role.ConfigurationSets.Add(networkConfiguration);
            }

            if (networkConfiguration.InputEndpoints == null)
            {
                networkConfiguration.InputEndpoints = new Collection<InputEndpoint>();
            }

            var inputEndpoints = networkConfiguration.InputEndpoints;
            return inputEndpoints;
        }

        private void ValidateParameters()
        {
            if (LocalPort < 0 || LocalPort > 65535)
            {
                throw new ArgumentException(Resources.PortsNotInRangeInSetAzureEndpoint);
            }

            if (PublicPort != null && (PublicPort < 0 || PublicPort > 65535))
            {
                throw new ArgumentException(Resources.PortsNotInRangeInSetAzureEndpoint);
            }
        }
    }
}