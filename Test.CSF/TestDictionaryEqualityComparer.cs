using System;
using NUnit.Framework;
using CSF;
using System.Collections.Generic;
using System.Collections;

namespace Test.CSF
{
  [TestFixture]
  public class TestDictionaryEqualityComparer
  {
    [Test]
    public void TestAreEqualSuccess()
    {
      Dictionary<int, string>
        dictionary1 = new Dictionary<int, string>(),
        dictionary2 = new Dictionary<int, string>();
      IEqualityComparer comparer = new DictionaryEqualityComparer<int, string>();

      dictionary1.Add(1, "One");
      dictionary1.Add(2, "Two");
      dictionary1.Add(3, "Three");
      dictionary1.Add(4, "Four");

      dictionary2.Add(3, "Three");
      dictionary2.Add(1, "One");
      dictionary2.Add(4, "Four");
      dictionary2.Add(2, "Two");

      Assert.IsTrue(comparer.Equals(dictionary1, dictionary1), "Reference equality");
      Assert.IsTrue(comparer.Equals(dictionary1, dictionary2), "Dictionaries are equal");
    }

    [Test]
    public void TestAreEqualFailure()
    {
      Dictionary<int, string>
        dictionary1 = new Dictionary<int, string>(),
        dictionary2 = new Dictionary<int, string>();
      IEqualityComparer comparer = new DictionaryEqualityComparer<int, string>();

      dictionary1.Add(1, "One");
      dictionary1.Add(2, "Two");
      dictionary1.Add(3, "Three");
      dictionary1.Add(4, "Four");

      dictionary2.Add(3, "Three");
      dictionary2.Add(1, "One");
      dictionary2.Add(5, "Five");
      dictionary2.Add(2, "Two");

      Assert.IsFalse(comparer.Equals(dictionary1, dictionary2), "Dictionaries are not equal");
    }
  }
}

