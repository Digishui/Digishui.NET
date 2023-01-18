using Digishui.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digishui.NET.Test
{
  [TestClass]
  public class TrimaxionTests
  {
    [TestMethod]
    public async Task Trimaxion_GetAsync()
    {
      Trimaxion trimaxion = new Trimaxion();

      await trimaxion.GetAsync(new Uri("https://www.google.com", UriKind.Absolute));

      //if ("30-MAY-19 12.00.00.000 AM".IsDateTime() == false) { Assert.Fail("System.string.IsDateTime extension should return True for value '30-MAY-19 12.00.00.000 AM'"); }
      //if ("43615".IsDateTime() == false) { Assert.Fail("System.string.IsDateTime extension should return True for value '43615'"); }
    }
  }
}
