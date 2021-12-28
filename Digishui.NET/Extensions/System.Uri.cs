using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

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

      httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_2_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36";

      httpWebRequest.ReadWriteTimeout = 60000;
      httpWebRequest.Timeout = 60000;

      return httpWebRequest;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<HttpWebResponse> WebRequestGetAsync(this Uri uri,
                                                                 CookieContainer cookieContainer,
                                                                 Uri refererUri = null)
    {
      HttpWebRequest httpWebRequest = CreateRequest(uri, cookieContainer, refererUri);
      HttpWebResponse httpWebResponse = await httpWebRequest.GetAsync();
      return httpWebResponse;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<HttpWebResponse> WebRequestPostAsync(this Uri uri,
                                                                  CookieContainer cookieContainer,
                                                                  NameValueCollection formData,
                                                                  Uri refererUri = null)
    {
      HttpWebRequest httpWebRequest = CreateRequest(uri, cookieContainer, refererUri);
      HttpWebResponse httpWebResponse = await httpWebRequest.PostAsync(formData);
      return httpWebResponse;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <remarks>
    ///   https://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
    /// </remarks>
    public static async Task<HttpWebResponse> WebRequestPostAsync(this Uri uri,
                                                                  CookieContainer cookieContainer,
                                                                  NameValueCollection formData,
                                                                  List<FormFile> formFiles,
                                                                  Uri refererUri = null)
    {
      string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

      HttpWebRequest httpWebRequest = CreateRequest(uri, cookieContainer, refererUri);
      httpWebRequest.ContentType = $"multipart/form-data; boundary={boundary}";
      httpWebRequest.Method = "POST";
      httpWebRequest.KeepAlive = true;

      Stream requestMemoryStream = await httpWebRequest.GetRequestStreamAsync();

      if (formData != null)
      {
        foreach (string key in formData.Keys)
        {
          //If the key contains "[]//", we're dealing with the desire to submit an unindexed array based on the order
          //that fields bearing the same key preceding the "[]//" we added to the formData NameValueCollection. This is
          //handled just by submitting the same form field over and over again, and the server parses it based on order
          //of inclusion. So, the part following "[]//" is a descriptor to differentiate values in the collection, and
          //it's not sent in the POST payload. Technically, NameValueCollection halfway handles this scenario, in that
          //if you try to add entries with the same key multiple times it accepts tham and comma separates them, but this
          //becomes problematic if one of the values contains commas (it doesn't quote comma-containing values, so you
          //can't figure out if it's a new field with the same name or a value containing a comma). This is a hack around
          //the implementation of the NameValueCollection as it pertains to duplication submission of unindexed formField
          //arrays.
          string formFieldName = key;
          if (formFieldName.Contains("[]//") == true) { formFieldName = formFieldName.Substring(0, key.IndexOf("[]//") + 2); }

          string formItem = $"\r\n--{boundary}\r\nContent-Disposition: form-data; name=\"{formFieldName}\";\r\n\r\n{formData[key]}";
          byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(formItem);
          await requestMemoryStream.WriteAsync(formItemBytes, 0, formItemBytes.Length);
        }
      }

      foreach (FormFile formFile in formFiles)
      {
        string header = $"\r\n--{boundary}\r\nContent-Disposition: form-data; name=\"{formFile.FormFieldName}\"; filename=\"{formFile.FileName}\"\r\nContent-Type: {formFile.ContentType ?? "application/octet-stream"}\r\n\r\n";
        byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);
        await requestMemoryStream.WriteAsync(headerBytes, 0, headerBytes.Length);
        await formFile.Stream.CopyToAsync(requestMemoryStream, true);
      }

      byte[] endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes($"\r\n--{boundary}--");
      await requestMemoryStream.WriteAsync(endBoundaryBytes, 0, endBoundaryBytes.Length);

      //await requestMemoryStream.CopyToAsync(await httpWebRequest.GetRequestStreamAsync(), true);

      //httpWebRequest.ContentLength = requestMemoryStream.Length;

      //using (Stream requestStream = httpWebRequest.GetRequestStream())
      //{
      //  requestMemoryStream.Position = 0;
      //  byte[] tempBuffer = new byte[requestMemoryStream.Length];
      //  requestMemoryStream.Read(tempBuffer, 0, tempBuffer.Length);
      //  requestMemoryStream.Close();
      //  requestStream.Write(tempBuffer, 0, tempBuffer.Length);
      //}

      return (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
    }
  }
}
