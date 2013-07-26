using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.Management.Utilities.Websites;
using Microsoft.WindowsAzure.Management.Utilities.Websites.Services.WebEntities;

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