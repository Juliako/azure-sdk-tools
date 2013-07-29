using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.Management.Utilities.Websites.Services.WebEntities;
using Newtonsoft.Json;

namespace Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities
{
    public interface IMediaServiceAccount
    {
        string Name { get; set; }
        Guid AccountId { get; set; }
        Uri ParentLink { get; set; }
        Uri SelfLink { get; set; }
        string State { get; set; }
        string Type { get; set; }

    }

    [DataContract(Namespace = UriElements.ServiceNamespace, Name = "ServiceResource")]
    [Newtonsoft.Json.JsonObject(Title = "ServiceResource")]
    public class MediaServiceAccount : IMediaServiceAccount
    {
        [DataMember(EmitDefaultValue = false, Order = 5)]
        public Guid AccountId { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 0)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 4)]
        public Uri ParentLink { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public Uri SelfLink { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string State { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 1)]
        public string Type { get; set; }
    }
    [Newtonsoft.Json.JsonObject(Title = "AccountDetails")]
    [DataContract(Namespace = UriElements.AccountDetailsNamespace, Name = "AccountDetails")]
    public class MediaServiceAccountDetails
    {
        
        [DataMember]
        internal string AccountKey { get; set; }

        [DataMember]
        internal AccountKeys AccountKeys { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AccountRegion { get; set; }

        [DataMember]
        public string StorageAccountName { get; set; }

        public string PrimaryAccountKey { get { return AccountKeys.Primary; } }

        public string SecondaryAccountKey { get { return AccountKeys.Secondary; } }
    }

    [DataContract(Namespace = UriElements.AccountDetailsNamespace)]
    public class AccountKeys
    {
        [DataMember]
        public string Primary { get; set; }

        [DataMember]
        public string Secondary { get; set; }
    }

     /// <summary>
    /// Collection of sites
    /// </summary>
    [CollectionDataContract(Namespace = UriElements.ServiceNamespace,Name = "ServiceResources", ItemName = "ServiceResource")]
    public class MediaServiceAccounts : List<MediaServiceAccount>
    {

        /// <summary>
        /// Empty collection
        /// </summary>
        public MediaServiceAccounts() { }

        /// <summary>
        /// Initialize collection
        /// </summary>
        /// <param name="mediaAccounts"></param>
        public MediaServiceAccounts(List<MediaServiceAccount> mediaAccounts) : base(mediaAccounts) { }
    }

    /// <summary>
    /// Paged collection of sites
    /// </summary>
    [DataContract(Namespace = UriElements.ServiceNamespace)]
    public class PagedSites : PagedSet<MediaServiceAccount>
    {
    }


   
}