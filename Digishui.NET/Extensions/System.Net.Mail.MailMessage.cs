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
