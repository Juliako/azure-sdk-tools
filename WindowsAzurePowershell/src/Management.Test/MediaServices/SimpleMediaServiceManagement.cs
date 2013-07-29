using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Management.Test.Utilities.Common;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services;
using Microsoft.WindowsAzure.Management.Utilities.MediaService.Services.MediaServicesEntities;

namespace Microsoft.WindowsAzure.Management.Test.Utilities.MediaServices
{
    public class SimpleMediaServiceManagementAzureNamespace : IMediaServiceManagementAzureNamespace
    {
        /// <summary>
        /// Gets or sets a value indicating whether the thunk wrappers will
        /// throw an exception if the thunk is not implemented.  This is useful
        /// when debugging a test.
        /// </summary>
        public bool ThrowsIfNotImplemented { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMediaServiceManagementAzureNamespace"/> class.
        /// </summary>
        public SimpleMediaServiceManagementAzureNamespace()
        {
            ThrowsIfNotImplemented = true;
        }

        public Func<SimpleServiceManagementAsyncResult, MediaServiceAccounts> GetMediaServicesThunk { get; set; }

        public IAsyncResult BeginGetMediaServices(string subscriptionName, AsyncCallback callback, object state)
        {
            SimpleServiceManagementAsyncResult result = new SimpleServiceManagementAsyncResult();
            result.Values["subscriptionName"] = subscriptionName;
            result.Values["callback"] = callback;
            result.Values["state"] = state;
            return result;
        }

        public MediaServiceAccounts EndGetMediaServices(IAsyncResult asyncResult)
        {
            if (GetMediaServicesThunk != null)
            {
                SimpleServiceManagementAsyncResult result = asyncResult as SimpleServiceManagementAsyncResult;
                Assert.IsNotNull(result, "asyncResult was not SimpleServiceManagementAsyncResult!");

                return GetMediaServicesThunk(result);
            }
            else if (ThrowsIfNotImplemented)
            {
                throw new NotImplementedException("GetSitesThunk is not implemented!");
            }

            return default(MediaServiceAccounts);
        }

        public IAsyncResult BeginGetMediaService(string subscriptionName, string name, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public MediaServiceAccountDetails EndGetMediaService(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDeleteMediaServicesAccount(string subscriptionName, string accountName, AsyncCallback callback, object state)
        {
            SimpleServiceManagementAsyncResult result = new SimpleServiceManagementAsyncResult();
            result.Values["subscriptionName"] = subscriptionName;
            result.Values["accountName"] = accountName;
            result.Values["callback"] = callback;
            result.Values["state"] = state;
            return result;
        }

        public void EndDeleteMediaServicesAccount(IAsyncResult asyncResult)
        {
            if (GetMediaServicesThunk != null)
            {
                SimpleServiceManagementAsyncResult result = asyncResult as SimpleServiceManagementAsyncResult;
                Assert.IsNotNull(result, "asyncResult was not SimpleServiceManagementAsyncResult!");
            }
            else if (ThrowsIfNotImplemented)
            {
                throw new NotImplementedException("GetSitesThunk is not implemented!");
            }
        }

        #region IMediaServiceManagementAzureNamespace Members


        public IAsyncResult BeginRegenerateMediaServicesAccount(string subscriptionName, string accountName, string keyType, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public void EndRegenerateMediaServicesAccount(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}