using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   SendGrid.SendGridMessage Extensions
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    private static SendGridClient SendGridClient
    {
      get
      {
        return new SendGridClient(Configuration.SendGridApiKey);
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private static WebClient SendGridWebClient
    {
      get
      {
        WebClient webClient = new WebClient
        {
          BaseAddress = "https://api.sendgrid.com/v3/",
          UseDefaultCredentials = false
        };

        webClient.Headers.Add("Authorization", $"Bearer {Configuration.SendGridApiKey}");
        webClient.Headers.Add("Content-Type", "application/json");
        webClient.Headers.Add("User-Agent", $"digishui/{Assembly.GetAssembly(typeof(Extensions)).GetName().Version}");
        webClient.Headers.Add("Accept", "application/json");

        return webClient;
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Prepares SendGrid message for sending.
    /// </summary>
    /// <param name="sendGridMessage">The SendGrid message to prepare.</param>
    /// <param name="addDefaultBcc">If true, adds the default bcc address from application configuration setting (Digishui.SendGridDefaultBCC)</param>
    /// <param name="renderPlainTextOnlyAlsoAsPreformattedHtml">If true and message only has plain text content, adds html content equal to plain text content, wrapped in pre tags to preserve fixed-width font rendering in most mail clients.</param>
    private static void Prepare(this SendGridMessage sendGridMessage, bool addDefaultBcc = false, bool renderPlainTextOnlyAlsoAsPreformattedHtml = true)
    {
      if (sendGridMessage.From == null)
      {
        sendGridMessage.From = new EmailAddress(Configuration.SendGridDefaultFromAddress, Configuration.SendGridDefaultFromName);
      }

      if (addDefaultBcc == true)
      {
        sendGridMessage.AddBcc(Configuration.SendGridDefaultBCC);
      }

      //If plain text content is supplied but html content is not supplied, set htmlcontent to same as plain text content
      //wrapped in <pre> tags. This ensures fixed width font rendering across most mail clients, which is preferable since
      //plain text content is composed with that assumption. Also, if only plain text content is supplied, disable click
      //tracking and open tracking because these features mangle the plain text composition, and SendGrid doesn't apply
      //the same mangling to the html composition when wrapped in <pre> tags.
      if ((sendGridMessage.PlainTextContent != null) && (sendGridMessage.HtmlContent == null) && (renderPlainTextOnlyAlsoAsPreformattedHtml == true))
      {
        sendGridMessage.HtmlContent = $"<pre style=\"white-space: pre-wrap; white-space: -moz-pre-wrap; white-space: -pre-wrap; white-space: -o-pre-wrap; white-space: break-word\">{sendGridMessage.PlainTextContent}</pre>";

        sendGridMessage.SetClickTracking(false, false);
        sendGridMessage.SetOpenTracking(false, null);
      }

      //Sending a message through the SendGrid API fails if you have the same email address listed as a recipient more than
      //once across the To, CC, and BCC fields. The following code deduplicates addresses, with the most consequential instance
      //of the address being preserved.

      List<string> recipients = new List<string>();

      for (int i = (sendGridMessage.Personalizations[0]?.Tos?.Count ?? 0) - 1; i >= 0; i--)
      {
        EmailAddress emailAddress = sendGridMessage.Personalizations[0].Tos[i];

        if (recipients.Contains(emailAddress.Email.Trim().ToLower()) == false)
        {
          recipients.Add(emailAddress.Email.Trim().ToLower());
        }
        else
        {
          sendGridMessage.Personalizations[0].Tos.RemoveAt(i);
        }
      }

      for (int i = (sendGridMessage.Personalizations[0]?.Ccs?.Count ?? 0) - 1; i >= 0; i--)
      {
        EmailAddress emailAddress = sendGridMessage.Personalizations[0].Ccs[i];

        if (recipients.Contains(emailAddress.Email.Trim().ToLower()) == false)
        {
          recipients.Add(emailAddress.Email.Trim().ToLower());
        }
        else
        {
          sendGridMessage.Personalizations[0].Ccs.RemoveAt(i);
        }
      }

      for (int i = (sendGridMessage.Personalizations[0]?.Bccs?.Count ?? 0) - 1; i >= 0; i--)
      {
        EmailAddress emailAddress = sendGridMessage.Personalizations[0].Bccs[i];

        if (recipients.Contains(emailAddress.Email.Trim().ToLower()) == false)
        {
          recipients.Add(emailAddress.Email.Trim().ToLower());
        }
        else
        {
          sendGridMessage.Personalizations[0].Bccs.RemoveAt(i);
        }
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private static void EmbedImages(this SendGridMessage TheSendGridMessage, string TemplatePath, string ImageExtension, string ImageMimeType)
    {
      string[] ImageFiles = Directory.GetFiles(TemplatePath, $"*.{ImageExtension}");

      foreach (string ImageFile in ImageFiles)
      {
        if (Path.GetFileName(ImageFile).StartsWith(".") == false)
        {
          if (TheSendGridMessage.HtmlContent.Contains(Path.GetFileName(ImageFile)) == true)
          {
            byte[] FileBytes = File.ReadAllBytes(ImageFile);
            string Base64FileContent = Convert.ToBase64String(FileBytes);
            TheSendGridMessage.AddAttachment(Path.GetFileName(ImageFile), Base64FileContent, ImageMimeType, "inline", Path.GetFileName(ImageFile));
          }
        }
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void LoadTemplate(this SendGridMessage TheSendGridMessage, string TemplateName)
    {
      string TemplatePath = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\EmailTemplates\\{TemplateName}";

      if (File.Exists(Path.Combine(TemplatePath, "body.html")) == true)
      {
        TheSendGridMessage.HtmlContent = File.ReadAllText(Path.Combine(TemplatePath, "body.html"));
        TheSendGridMessage.HtmlContent = TheSendGridMessage.HtmlContent.Replace("<img src=\"", "<img src=\"cid:");

        TheSendGridMessage.EmbedImages(TemplatePath, "gif", "image/gif");
        TheSendGridMessage.EmbedImages(TemplatePath, "jpg", "image/jpeg");
        TheSendGridMessage.EmbedImages(TemplatePath, "png", "image/png");
      }

      if (File.Exists(Path.Combine(TemplatePath, "body.txt")) == true)
      {
        TheSendGridMessage.PlainTextContent = File.ReadAllText(Path.Combine(TemplatePath, "body.txt")); ;
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void AddAttachment(this SendGridMessage TheSendGridMessage, string Content, string Filename, string MimeType)
    {
      string Base64FileContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(Content));

      TheSendGridMessage.AddAttachment(Path.GetFileName(Filename), Base64FileContent, MimeType);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void AddAttachment(this SendGridMessage TheSendGridMessage, Stream ContentStream, string Filename, string MimeType)
    {
      byte[] FileBytes = new byte[ContentStream.Length];
      ContentStream.Read(FileBytes, 0, FileBytes.Length);

      string Base64FileContent = Convert.ToBase64String(FileBytes);

      TheSendGridMessage.AddAttachment(Path.GetFileName(Filename), Base64FileContent, MimeType);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void SetTemplateVariable(this SendGridMessage TheSendGridMessage, string VariableName, string Value)
    {
      if (TheSendGridMessage.HtmlContent != null)
      {
        TheSendGridMessage.HtmlContent = TheSendGridMessage.HtmlContent.Replace($"{{[{VariableName}]}}", Value);
      }

      if (TheSendGridMessage.PlainTextContent != null)
      {
        TheSendGridMessage.PlainTextContent = TheSendGridMessage.PlainTextContent.Replace($"{{[{VariableName}]}}", Value);
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Sends SendGrid message asynchronously.
    /// </summary>
    /// <param name="sendGridMessage">The SendGrid message to send.</param>
    /// <param name="addDefaultBcc">If true, adds the default bcc address from application configuration setting (Digishui.SendGridDefaultBCC)</param>
    /// <param name="renderPlainTextOnlyAlsoAsPreformattedHtml">If true and message only has plain text content, adds html content equal to plain text content, wrapped in pre tags to preserve fixed-width font rendering in most mail clients.</param>
    /// <returns></returns>
    public static async Task<bool> SendAsync(this SendGridMessage sendGridMessage, bool addDefaultBcc = false, bool renderPlainTextOnlyAlsoAsPreformattedHtml = true)
    {
      sendGridMessage.Prepare(addDefaultBcc, renderPlainTextOnlyAlsoAsPreformattedHtml);

      try
      {
        Response response = await SendGridClient.SendEmailAsync(sendGridMessage);
      }
      catch (Exception exception)
      {
        System.Diagnostics.Debug.Print(exception.Message);

        return false;
      }

      return true;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///   Sends SendGrid message synchronously.
    /// </summary>
    /// <param name="sendGridMessage">The SendGrid message to send.</param>
    /// <param name="addDefaultBcc">If true, adds the default bcc address from application configuration setting (Digishui.SendGridDefaultBCC)</param>
    /// <param name="renderPlainTextOnlyAlsoAsPreformattedHtml">If true and message only has plain text content, adds html content equal to plain text content, wrapped in pre tags to preserve fixed-width font rendering in most mail clients.</param>
    /// <returns></returns>
    public static bool Send(this SendGridMessage sendGridMessage, bool addDefaultBcc = false, bool renderPlainTextOnlyAlsoAsPreformattedHtml = true)
    {
      sendGridMessage.Prepare(addDefaultBcc, renderPlainTextOnlyAlsoAsPreformattedHtml);

      try
      {
        System.Diagnostics.Debug.Print(SendGridWebClient.UploadString("mail/send", sendGridMessage.Serialize()));
      }
      catch (Exception exception)
      {
        System.Diagnostics.Debug.Print(exception.Message);

        return false;
      }

      return true;
    }
  }
}
