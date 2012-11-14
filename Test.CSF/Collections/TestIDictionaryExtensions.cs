using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestIDictionaryExtensions
  {
    [Test]
    public void TestToNameValueCollection()
    {
      Dictionary<string,string> test = new Dictionary<string, string>();
      test.Add("foo", "bar");
      test.Add("spong", "wibble");

      NameValueCollection output = test.ToNameValueCollection();

      Assert.IsNotNull(output, "Null");
      Assert.AreEqual(2, output.Count, "Count");
      Assert.AreEqual("bar", output["foo"], "foo");
      Assert.AreEqual("wibble", output["spong"], "spong");
    }
  }
}

