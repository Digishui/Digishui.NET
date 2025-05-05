using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.Uri Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<HttpWebResponse> OptionsAsync(this HttpWebRequest httpWebRequest)
    {
      httpWebRequest.Method = "OPTIONS";

      return await httpWebRequest.GetResponseAsyncNoException();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<HttpWebResponse> GetAsync(this HttpWebRequest httpWebRequest)
    {
      httpWebRequest.Method = "GET";

      return await httpWebRequest.GetResponseAsyncNoException();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<HttpWebResponse> PostAsync(this HttpWebRequest httpWebRequest,
                                                        NameValueCollection formData)
    {
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/x-www-form-urlencoded";
      httpWebRequest.AllowWriteStreamBuffering = true;

      using (Stream requestStream = await httpWebRequest.GetRequestStreamAsync())
      {
        ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        byte[] postBytes = asciiEncoding.GetBytes(formData.ToString());
        await requestStream.WriteAsync(postBytes, 0, postBytes.Length);
      }

      return (HttpWebResponse)(await httpWebRequest.GetResponseAsyncNoException());
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <see cref="https://stackoverflow.com/questions/10081726/why-does-httpwebrequest-throw-an-exception-instead-returning-httpstatuscode-notf"/>
    /// <param name="httpWebRequest"></param>
    /// <returns></returns>
    public static async Task<HttpWebResponse> GetResponseAsyncNoException(this HttpWebRequest httpWebRequest)
    {
      try
      {
        return (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
      }
      catch (WebException webException)
      {
        if (!(webException.Response is HttpWebResponse webResponse)) { throw; }

        return webResponse;
      }
    }
  }
}
