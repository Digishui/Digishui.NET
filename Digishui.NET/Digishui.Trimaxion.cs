using Digishui.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui
{
  //===========================================================================================================================
  public class Trimaxion
  {
    //-------------------------------------------------------------------------------------------------------------------------
    public Uri CurrentUri { get; private set; } = new Uri("https://www.google.com", UriKind.Absolute);
    public MemoryStream ResponseStream { get; private set; } = null;

    //-------------------------------------------------------------------------------------------------------------------------
    private CookieContainer CookieContainer { get; } = new CookieContainer();
    private HttpWebResponse HttpWebResponse { get; set; } = null;
    private Encoding ResponseEncoding { get; set; } = null;
    private string PageSourceCache { get; set; } = String.Empty;

    //-------------------------------------------------------------------------------------------------------------------------
    public async Task GetAsync(Uri uri)
    {
      HttpWebResponse = await uri.WebRequestGetAsync(CookieContainer, CurrentUri);
      await ProcessResponse();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public async Task PostAsync(Uri uri, NameValueCollection formData)
    {
      HttpWebResponse = await uri.WebRequestPostAsync(CookieContainer, formData, CurrentUri);
      await ProcessResponse();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public async Task PostAsync(Uri uri, NameValueCollection formData, List<FormFile> formFiles)
    {
      HttpWebResponse = await uri.WebRequestPostAsync(CookieContainer, formData, formFiles, CurrentUri);
      await ProcessResponse();
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private async Task ProcessResponse()
    {
      CurrentUri = HttpWebResponse.ResponseUri;
      ResponseEncoding = (HttpWebResponse.CharacterSet == "") ? Encoding.UTF8 : Encoding.GetEncoding(HttpWebResponse.CharacterSet);

      if (ResponseStream != null) { ResponseStream.Dispose(); }
      if (PageSourceCache != String.Empty) { PageSourceCache = String.Empty; }

      ResponseStream = new MemoryStream();
      await HttpWebResponse.GetResponseStream().CopyToAsync(ResponseStream);
      ResponseStream.Position = 0;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public string PageSource
    {
      get
      {
        if (PageSourceCache == String.Empty)
        {
          if ((ResponseStream?.Length ?? 0) <= 0)
          {
            PageSourceCache = String.Empty;
          }
          else
          {
            StreamReader streamReader = new StreamReader(ResponseStream, ResponseEncoding);
            PageSourceCache = streamReader.ReadToEnd();
            ResponseStream.Position = 0;
          }
        }

        return PageSourceCache;
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public HttpStatusCode StatusCode
    {
      get
      {
        return HttpWebResponse.StatusCode;
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public HtmlNode GetFormNodeById(string formId = null)
    {
      HtmlNode.ElementsFlags.Remove("form");
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(PageSource);

      HtmlNode formNode = null;

      if (formId == null)
      {
        formNode = htmlDocument.DocumentNode.SelectNodes("//form")[0];
      }
      else
      {
        foreach (HtmlNode htmlNode in htmlDocument.DocumentNode.SelectNodes("//form"))
        {
          if (htmlNode.Attributes["id"]?.Value == formId)
          {
            formNode = htmlNode;
          }
        }
      }

      return formNode;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public string GetHrefStartingWith(string hrefStart)
    {
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(PageSource);

      foreach (HtmlNode htmlNode in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
      {
        string href = htmlNode.Attributes["href"].Value;

        if (href.StartsWith(hrefStart) == true) { return href; }
      }

      return null;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public Uri GetFormActionUri(string baseUrl = null, string formName = null)
    {
      HtmlNode formNode = GetFormNodeById(formName);

      if (baseUrl == null)
      {
        return new Uri(formNode.Attributes["action"]?.Value, UriKind.Absolute);
      }
      else
      {
        return new Uri(new Uri(baseUrl, UriKind.Absolute), formNode.Attributes["action"]?.Value);
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public NameValueCollection GetFormData(string formName = null)
    {
      NameValueCollection formData = HttpUtility.ParseQueryString(String.Empty);

      HtmlNode formNode = GetFormNodeById(formName);

      foreach (HtmlNode inputNode in formNode.Descendants("input"))
      {
        string elementType = inputNode.Attributes["type"]?.Value;

        if (elementType == "hidden")
        {
          string elementName = inputNode.Attributes["name"]?.Value;
          string elementValue = (inputNode.Attributes["value"] != null) ? inputNode.Attributes["value"].Value : "";

          if (elementName != null)
          {
            formData.Add(elementName, elementValue);
          }
        }
      }

      return formData;
    }
  }
}
