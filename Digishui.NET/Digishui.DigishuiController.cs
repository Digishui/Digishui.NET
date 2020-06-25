using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui
{
  //===========================================================================================================================
  public class DigishuiController : Controller
  {
    //-------------------------------------------------------------------------------------------------------------------------
    protected AjaxErrorsResult AjaxErrors(Dictionary<string, string> errors)
    {
      return new AjaxErrorsResult(errors);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    protected AjaxErrorsResult AjaxError(string key, string value)
    {
      return new AjaxErrorsResult(new Dictionary<string, string> { { key, value } });
    }
  }

  //===========================================================================================================================
  public class AjaxErrorsResult : ActionResult
  {
    //-------------------------------------------------------------------------------------------------------------------------
    public Dictionary<string, string> Errors { get; private set; }

    //-------------------------------------------------------------------------------------------------------------------------
    public AjaxErrorsResult(Dictionary<string, string> errors)
    {
      Errors = errors;
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      HttpResponseBase response = context.HttpContext.Response;

      response.TrySkipIisCustomErrors = true;

      response.StatusCode = 400;

      response.StatusDescription = "AjaxErrors";

      if ((Errors?.Count ?? 0) != 0)
      {
        response.Write(JsonConvert.SerializeObject(Errors));
      }
    }
  }
}