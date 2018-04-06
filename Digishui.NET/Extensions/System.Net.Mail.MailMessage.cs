using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.Net.Mail.MailMessage Extensions for use with SendGrid via SMTP.
  /// </summary>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    private static SmtpClient SmtpClient
    {
      get
      {
        SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", 465);

        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("apikey", Configuration.SendGridApiKey);

        return smtpClient;
      }
    }

    //-------------------------------------------------------------------------------------------------------------------------
    private static void Prepare(this MailMessage mailMessage, bool supportBCC = false)
    {
      if (mailMessage.From == null)
      {
        mailMessage.From = new MailAddress(Configuration.SendGridDefaultFromAddress, Configuration.SendGridDefaultFromName);
      }

      if (supportBCC == true)
      {
        mailMessage.Bcc.Add(Configuration.SendGridDefaultBCC);
      }
    }

    ////-------------------------------------------------------------------------------------------------------------------------
    //private static void EmbedImages(this MailMessage mailMessage, string templatePath, string imageExtension, string imageMimeType)
    //{
    //  string[] ImageFiles = Directory.GetFiles(templatePath, $"*.{imageExtension}");

    //  foreach (string ImageFile in ImageFiles)
    //  {
    //    if (Path.GetFileName(ImageFile).StartsWith(".") == false)
    //    {
    //      if (mailMessage.HtmlContent.Contains(Path.GetFileName(ImageFile)) == true)
    //      {
    //        byte[] FileBytes = File.ReadAllBytes(ImageFile);
    //        string Base64FileContent = Convert.ToBase64String(FileBytes);
    //        mailMessage.AddAttachment(Path.GetFileName(ImageFile), Base64FileContent, imageMimeType, "inline", Path.GetFileName(ImageFile));
    //      }
    //    }
    //  }
    //}

    ////-------------------------------------------------------------------------------------------------------------------------
    //public static void LoadTemplate(this MailMessage mailMessage, string templateName)
    //{
    //  string TemplatePath = System.Web.HttpContext.Current.Server.MapPath($"/EmailTemplates/{templateName}");

    //  if (File.Exists(Path.Combine(TemplatePath, "body.html")) == true)
    //  {
    //    mailMessage.HtmlContent = File.ReadAllText(Path.Combine(TemplatePath, "body.html"));
    //    mailMessage.HtmlContent = mailMessage.HtmlContent.Replace("<img src=\"", "<img src=\"cid:");

    //    mailMessage.EmbedImages(TemplatePath, "gif", "image/gif");
    //    mailMessage.EmbedImages(TemplatePath, "jpg", "image/jpeg");
    //    mailMessage.EmbedImages(TemplatePath, "png", "image/png");
    //  }

    //  if (File.Exists(Path.Combine(TemplatePath, "body.txt")) == true)
    //  {
    //    mailMessage.PlainTextContent = File.ReadAllText(Path.Combine(TemplatePath, "body.txt")); ;
    //  }
    //}

    //-------------------------------------------------------------------------------------------------------------------------
    public static void AddAttachment(this MailMessage mailMessage, string content, string fileName, string mimeType)
    {
      mailMessage.Attachments.Add(new Attachment(content.ToStream(), fileName, mimeType));
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static void AddAttachment(this MailMessage mailMessage, Stream contentStream, string fileName, string mimeType)
    {
      mailMessage.Attachments.Add(new Attachment(contentStream, fileName, mimeType));
    }

    ////-------------------------------------------------------------------------------------------------------------------------
    //public static void SetTemplateVariable(this MailMessage mailMessage, string variableName, string variableValue)
    //{
    //  if (mailMessage.HtmlContent != null)
    //  {
    //    mailMessage.HtmlContent = mailMessage.HtmlContent.Replace($"{{[{variableName}]}}", variableValue);
    //  }

    //  if (mailMessage.PlainTextContent != null)
    //  {
    //    mailMessage.PlainTextContent = mailMessage.PlainTextContent.Replace($"{{[{variableName}]}}", variableValue);
    //  }
    //}

    //-------------------------------------------------------------------------------------------------------------------------
    public static async Task<bool> SendAsync(this MailMessage mailMessage, bool supportBCC = false)
    {
      mailMessage.Prepare(supportBCC);

      try
      {
        await SmtpClient.SendMailAsync(mailMessage);
      }
      catch
      {
        return false;
      }

      return true;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static bool Send(this MailMessage mailMessage, bool supportBCC = false)
    {
      mailMessage.Prepare(supportBCC);

      try
      {
        SmtpClient.Send(mailMessage);
      }
      catch
      {
        return false;
      }

      return true;
    }
  }
}
