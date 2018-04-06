using System.Configuration;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui
{
  //===========================================================================================================================
  internal static class Configuration
  {
    //-------------------------------------------------------------------------------------------------------------------------
    public static string SendGridDefaultFromAddress { get { return ConfigurationManager.AppSettings["Digishui.SendGridDefaultFromAddress"]; } }
    
    //-------------------------------------------------------------------------------------------------------------------------
    public static string SendGridDefaultFromName { get { return ConfigurationManager.AppSettings["Digishui.SendGridDefaultFromName"]; } }

    //-------------------------------------------------------------------------------------------------------------------------
    public static string SendGridDefaultBCC { get { return ConfigurationManager.AppSettings["Digishui.SendGridDefaultBCC"]; } }

    //-------------------------------------------------------------------------------------------------------------------------
    public static string SendGridApiKey { get { return ConfigurationManager.AppSettings["Digishui.SendGridApiKey"]; } }
  }
}
