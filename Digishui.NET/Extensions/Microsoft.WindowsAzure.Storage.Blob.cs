using Microsoft.WindowsAzure.Storage.Blob;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   Microsoft.WindowsAzure.Storage.Blob Extensions
  /// </summary>
  public static partial class Extensions
  {
    public static string GetName(this IListBlobItem value)
    {
      return value.Uri.ToString().Substring(value.Container.Uri.ToString().Length + 1);
    }
  }
}
