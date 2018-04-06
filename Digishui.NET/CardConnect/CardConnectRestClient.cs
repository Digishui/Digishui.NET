using Digishui.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui
{
  //===========================================================================================================================
  public class CardConnectRestClient
  {
    //-------------------------------------------------------------------------------------------------------------------------
    // API Connectivity Properties
    private string Url { get; set; }

    //-------------------------------------------------------------------------------------------------------------------------
    // Endpoints
    private static string ENDPOINT_AUTH { get; } = "auth";
    private static string ENDPOINT_CAPTURE { get; } = "capture";
    private static string ENDPOINT_VOID { get; } = "void";
    private static string ENDPOINT_REFUND { get; } = "refund";
    private static string ENDPOINT_INQUIRE { get; } = "inquire";
    private static string ENDPOINT_SETTLESTAT { get; } = "settlestat";
    private static string ENDPOINT_FUNDING { get; } = "funding";
    private static string ENDPOINT_PROFILE { get; } = "profile";

    //-------------------------------------------------------------------------------------------------------------------------
    // HttpClient
    private HttpClient HttpClient { get; set; }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   CardConnect Client Constructor
    /// </summary>
    /// <param name="Url">CardConnect API Url</param>
    /// <param name="Username">CardConnect Username</param>
    /// <param name="Password">CardConnect Password</param>
    public CardConnectRestClient(String Url, String Username, String Password)
    {
      if (Url.IsEmpty() == true) throw new ArgumentException("Url parameter is required");
      if (Username.IsEmpty() == true) throw new ArgumentException("Username parameter is required");
      if (Password.IsEmpty() == true) throw new ArgumentException("Password parameter is required");

      if (Url.EndsWith("/") == true) Url = Url.ReplaceLast("/", "");

      this.Url = Url;

      HttpClient = new HttpClient();
      byte[] AuthenticationData = new UTF8Encoding().GetBytes($"{Username}:{Password}");
      HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(AuthenticationData));
      HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Authorize Transaction
    /// </summary>
    /// <param name="Request">JObject representing an Authorization transaction request</param>
    /// <returns>JObject representing an Authorization transaction response</returns>
    public async Task<JObject> authorizeTransactionAsync(JObject Request)
    {
      return (JObject)await SendAsync(ENDPOINT_AUTH, HttpMethod.Put, Request);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Capture Transaction
    /// </summary>
    /// <param name="Request">JObject representing a Capture transaction request</param>
    /// <returns>JObject representing a Capture transaction response</returns>
    public async Task<JObject> captureTransactionAsync(JObject Request)
    {
      return (JObject)await SendAsync(ENDPOINT_CAPTURE, HttpMethod.Put, Request);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Void Transaction
    /// 
    /// </summary>
    /// <param name="Request">JObject representing a Void transaction request</param>
    /// <returns>JObject representing a Void transaction response</returns>
    public async Task<JObject> voidTransactionAsync(JObject Request)
    {
      return (JObject)await SendAsync(ENDPOINT_VOID, HttpMethod.Put, Request);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Refund Transaction
    /// </summary>
    /// <param name="Request">JObject representing a Refund transaction request</param>
    /// <returns>JObject representing a Refund transaction response</returns>
    public async Task<JObject> refundTransactionAsync(JObject Request)
    {
      return (JObject)await SendAsync(ENDPOINT_REFUND, HttpMethod.Put, Request);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Inquire Transaction
    /// </summary>
    /// <param name="MerchantId">Merchant ID</param>
    /// <param name="retref">RetRef to inquire</param>
    /// <returns>JObject representing the request transaction</returns>
    public async Task<JObject> inquireTransactionAsync(String MerchantId, String retref)
    {
      if (MerchantId.IsEmpty() == true) { throw new ArgumentException("Missing required parameter: MerchantId"); }
      if (retref.IsEmpty() == true) { throw new ArgumentException("Missing required parameter: retref"); }

      String url = ENDPOINT_INQUIRE + "/" + retref + "/" + MerchantId;

      return (JObject)await SendAsync(url, HttpMethod.Get, null);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Gets the settlement status for transactions
    /// </summary>
    /// <param name="MerchantID">Merchant ID</param>
    /// <param name="Date">Date in MMDD format</param>
    /// <returns>
    ///   JArray of JObjects representing Settlement batches, each batch containing a JArray of JObjects representing the 
    ///   settlement status of each transaction
    /// </returns>
    public async Task<JArray> settlementStatusAsync(String MerchantID, String Date)
    {
      if (MerchantID.IsEmpty() != Date.IsEmpty()) { throw new ArgumentException("Both MerchantID and Date parameters are required, or neither"); }

      String url = null;

      if ((MerchantID.IsEmpty() == true) || (Date.IsEmpty() == true))
      {
        url = ENDPOINT_SETTLESTAT;
      }
      else
      {
        url = ENDPOINT_SETTLESTAT + "?merchid=" + MerchantID + "&date=" + Date;
      }

      return (JArray)await SendAsync(url, HttpMethod.Get, null);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Retrieves funding status information for the given merchant and date
    /// </summary>
    /// <param name="MerchantID">Merchant ID</param>
    /// <param name="Date">Date in MMDD format</param>
    /// <returns></returns>
    public async Task<JObject> fundingStatusAsync(String MerchantID, String Date)
    {
      if (MerchantID.IsEmpty() != Date.IsEmpty()) { throw new ArgumentException("Both MerchantID and Date parameters are required, or neither"); }

      String url = null;

      if ((MerchantID.IsEmpty() == true) || (Date.IsEmpty() == true))
      {
        url = ENDPOINT_FUNDING;
      }
      else
      {
        url = ENDPOINT_FUNDING + "?merchid=" + MerchantID + "&date=" + Date;
      }

      return (JObject)await SendAsync(url, HttpMethod.Get, null);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Retrieves a profile
    /// </summary>
    /// <param name="ProfileID">ProfileID to retreive</param>
    /// <param name="AccountID">Optional accountID within the profile</param>
    /// <param name="MerchantID">Merchant ID</param>
    /// <returns>JArray of JObjects each represeting a profile</returns>
    public async Task<JObject> profileGetAsync(String ProfileID, String AccountID, String MerchantID)
    {
      if (ProfileID.IsEmpty() == true) { throw new ArgumentException("Missing required parameter: ProfileID"); }
      if (MerchantID.IsEmpty() == true) { throw new ArgumentException("Missing required parameter: MerchantID"); }
      if (AccountID == null) AccountID = "";

      String url = ENDPOINT_PROFILE + "/" + ProfileID + "/" + AccountID + "/" + MerchantID;

      return (JObject)await SendAsync(url, HttpMethod.Get, null);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Deletes a profile
    /// </summary>
    /// <param name="ProfileID">ProfileID to delete</param>
    /// <param name="AccountID">Optional accountID within the profile</param>
    /// <param name="MerchantID">Merchant ID</param>
    /// <returns></returns>
    public async Task<JObject> profileDeleteAsync(String ProfileID, String AccountID, String MerchantID)
    {
      if (ProfileID.IsEmpty() == true) { throw new ArgumentException("Missing required parameter: ProfileID"); }
      if (MerchantID.IsEmpty() == true) { throw new ArgumentException("Missing required parameter: MerchantID"); }
      if (AccountID == null) AccountID = "";

      String url = ENDPOINT_PROFILE + "/" + ProfileID + "/" + AccountID + "/" + MerchantID;

      return (JObject)await SendAsync(url, HttpMethod.Delete, null);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Creates a New Profile
    /// </summary>
    /// <param name="Request">JObject representing Profile Creation request</param>
    /// <returns>JObject representing the newly created Profile</returns>
    public async Task<JObject> profileCreateAsync(JObject Request)
    {
      return (JObject)await SendAsync(ENDPOINT_PROFILE, HttpMethod.Put, Request);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Updates an Existing Profile
    /// </summary>
    /// <param name="Request">JObject representing Profile Update request</param>
    /// <returns>JObject representing the updated Profile</returns>
    public async Task<JObject> profileUpdateAsync(JObject Request)
    {
      return await profileCreateAsync(Request);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private async Task<Object> SendAsync(String Endpoint, HttpMethod Operation, JObject Request)
    {
      HttpRequestMessage MyHttpRequestMessage = new HttpRequestMessage(Operation, $"{Url}/{Endpoint}");

      if (Request != null)
      {
        MyHttpRequestMessage.Content = new StringContent(Request.ToString(), Encoding.UTF8, "application/json");
      }

      try
      {
        HttpResponseMessage MyHttpResponseMessage = await HttpClient.SendAsync(MyHttpRequestMessage);

        string ResponseContent = await MyHttpResponseMessage.Content.ReadAsStringAsync();

        JsonTextReader jsreader = new JsonTextReader(new StringReader(ResponseContent));

        return new JsonSerializer().Deserialize(jsreader);
      }
      catch
      {
        return null;
      }
    }
  }
}
