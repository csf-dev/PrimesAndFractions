using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestNameValueCollectionExtensions
  {
    [Test]
    public void TestToDictionary()
    {
      NameValueCollection collection = new NameValueCollection();

      collection.Add("foo", "bar");
      collection.Add("foo", "baz");
      collection.Add("spong", "wibble");

      IDictionary<string,string> clone = collection.ToDictionary();

      Assert.AreEqual(2, clone.Count, "Count of elements");
      Assert.AreEqual("bar,baz", clone["foo"], "Content for 'foo'");
      Assert.AreEqual("wibble", clone["spong"], "Content for 'spong'");
    }
  }
}

