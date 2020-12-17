using System;
using System.Collections.Specialized;
using System.Net;
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
    private static HttpWebRequest CreateRequest(Uri uri,
                                                CookieContainer cookieContainer,
                                                Uri refererUri = null,
                                                bool ignoreCertificateValidationErrors = false)
    {
      HttpWebRequest httpWebRequest = WebRequest.CreateHttp(uri);

      httpWebRequest.CookieContainer = cookieContainer;

      if (ignoreCertificateValidationErrors == true) { httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; }; }

      httpWebRequest.AllowAutoRedirect = true;

      httpWebRequest.Referer = refererUri?.ToString() ?? "";

      httpWebRequest.Headers.Add("DNT", "1");

      httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";

      httpWebRequest.ReadWriteTimeout = 60000;
      httpWebRequest.Timeout = 60000;

      return httpWebRequest;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<HttpWebResponse> GetAsync(this Uri uri,
                                                       CookieContainer cookieContainer,
                                                       Uri refererUri = null)
    {
      HttpWebRequest httpWebRequest = CreateRequest(uri, cookieContainer, refererUri);
      HttpWebResponse httpWebResponse = await httpWebRequest.GetAsync();
      return httpWebResponse;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<HttpWebResponse> PostAsync(this Uri uri,
                                                        CookieContainer cookieContainer,
                                                        NameValueCollection formData,
                                                        Uri refererUri = null)
    {
      HttpWebRequest httpWebRequest = CreateRequest(uri, cookieContainer, refererUri);
      HttpWebResponse httpWebResponse = await httpWebRequest.PostAsync(formData);
      return httpWebResponse;
    }
  }
}
