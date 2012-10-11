using System;
using NUnit.Framework;
using System.Globalization;
using CSF.Globalization;
using System.Collections.Generic;

namespace Test.CSF.Globalization
{
  [TestFixture]
  public class TestCultureInfoExtensions
  {
    [Test]
    public void TestGetAllMonths()
    {
      CultureInfo culture = CultureInfo.GetCultureInfo("fr-FR");

      IDictionary<int, string> allMonths = culture.GetAllMonths();
      Assert.AreEqual(12, allMonths.Count, "Correct count of months");
      Assert.AreEqual("avril", allMonths[4], "Correct representation of April");
    }
  }
}

