//using Digishui.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;
//using Microsoft.WindowsAzure.Storage.RetryPolicies;

//namespace Digishui.NET.Test
//{
//  [TestClass]
//  public partial class ExtensionTests
//  {
//    [TestMethod]
//    public async Task Microsoft_WindowsAzure_Storage_Blob_CloudBlobContainer_ListAsync()
//    {
//      string connectionString = "";
//      string containerName = "";
//      string prefix = "";

//      CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

//      CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
//      IRetryPolicy retryPolicy = new LinearRetry(TimeSpan.FromSeconds(2), 10);
//      cloudBlobClient.DefaultRequestOptions.RetryPolicy = retryPolicy;

//      CloudBlobContainer coudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

//      if (coudBlobContainer.Exists() == false)
//      {
//        Assert.Fail();
//      }
//      else
//      {
//        List<string> blobItems = coudBlobContainer.ListBlobs(prefix, false).OrderBy(ob => ob.Uri.AbsoluteUri).Select(s => s.Uri.AbsoluteUri).ToList();
//        List<string> asyncBlobItems = (await coudBlobContainer.ListBlobsAsync(prefix, false)).OrderBy(ob => ob.Uri.AbsoluteUri).Select(s => s.Uri.AbsoluteUri).ToList();

//        if ((blobItems.Count == 0) || (asyncBlobItems.Count == 0)) { Assert.Fail(); }

//        CollectionAssert.AreEqual(blobItems, asyncBlobItems);
//      }
//    }
//  }
//}
