using Digishui.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digishui.NET.Test
{
  [TestClass]
  public partial class ExtensionTests
  {
    [TestMethod]
    public void System_string_IsDateTime()
    {
      if ("30-MAY-19 12.00.00.000 AM".IsDateTime() == false) { Assert.Fail("System.string.IsDateTime extension should return True for value '30-MAY-19 12.00.00.000 AM'"); }
      if ("43615".IsDateTime() == false) { Assert.Fail("System.string.IsDateTime extension should return True for value '43615'"); }
    }

    [TestMethod]
    public void System_string_ToDateTime()
    {
      try
      {
        DateTime dateTime = "30-MAY-19 12.00.00.000 AM".ToDateTime();
        if (dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") != "2019-05-30 00:00:00.000") { Assert.Fail($"System.string.ToDateTime extension incorrectly converted '30-MAY-19 12.00.00.000 AM' to '{dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}'"); }
      }
      catch
      {
        Assert.Fail("System.string.ToDateTime extension dependency on System.string.IsDateTime should return True for value '30-MAY-19 12.00.00.000 AM'");
      }

      try
      {
        DateTime dateTime = "43615".ToDateTime();
        if (dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") != "2019-05-30 00:00:00.000") { Assert.Fail($"System.string.ToDateTime extension incorrectly converted '43615' to '{dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}'"); }
      }
      catch
      {
        Assert.Fail("System.string.ToDateTime extension dependency on System.string.IsDateTime should return True for value '43615'");
      }
    }

    [TestMethod]
    public void System_string_GetNumbers()
    {
      if ("1a2b3c! ".GetNumbers() != "123") { Assert.Fail("System.string.GetNumbers extension should return '123' for value '1a2b3c! '"); }
    }

    [TestMethod]
    public void System_string_ReplaceLast()
    {
      if ("test\a ".ReplaceLast("\a ", "!") != "test!") { Assert.Fail("System.string.ReplaceLast extension should return 'test!' for value 'test\\a '"); }
    }

    [TestMethod]
    public void System_string_IsAbaRoutingNumber()
    {
      if ("101000187".IsAbaRoutingNumber() == false) { Assert.Fail("System.string.IsAbaRoutingNumber extension should return True for value '101000187'"); }
      if ("324179555".IsAbaRoutingNumber() == true) { Assert.Fail("System.string.IsAbaRoutingNumber extension should return False for value '324179555'"); }
    }
  }
}
