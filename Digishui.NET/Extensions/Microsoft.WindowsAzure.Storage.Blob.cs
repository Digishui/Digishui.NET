using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   Microsoft.WindowsAzure.Storage.Blob Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    public static string GetName(this IListBlobItem value)
    {
      return value.Uri.ToString().Substring(value.Container.Uri.ToString().Length + 1);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Returns an enumerable collection of the blobs in the container that are retrieved asynchronously.
    /// </summary>
    /// <param name="cloudBlobContainer">The cloud blob container for which a listing is desired.</param>
    /// <param name="prefix">A string containing the blob name prefix.</param>
    /// <param name="useFlatBlobListing">A boolean value that specifies whether to list blobs in a flat listing, or whether to list blobs hierarchically, by virtual directory.</param>
    /// <returns></returns>
    public static async Task<IEnumerable<IListBlobItem>> ListBlobsAsync(this CloudBlobContainer cloudBlobContainer, 
                                                                        string prefix, 
                                                                        bool useFlatBlobListing = false)
    {
     List<IListBlobItem> blobItems = new List<IListBlobItem>();

      BlobContinuationToken blobContinuationToken = null;

      do
      {
        BlobResultSegment blobResultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(
          prefix: prefix,
          useFlatBlobListing: useFlatBlobListing,
          blobListingDetails: BlobListingDetails.None,
          currentToken: blobContinuationToken,
          options: null,
          operationContext: null,
          maxResults: null
        );

        blobContinuationToken = blobResultSegment.ContinuationToken;

        blobItems.AddRange(blobResultSegment.Results);
      } while (blobContinuationToken != null);

      return blobItems;
    }
  }
}
