using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections;
using System.Collections.Generic;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestOrderNeutralEqualityComparer
  {
    [Test]
    public void TestEquals()
    {
      IEqualityComparer comparer = new OrderNeutralEqualityComparer<string>();

      IList<string> listOne = new string[] { "foo", "bar", "baz" };
      IList<string> listTwo = new string[] { "bar", "foo", "baz" };
      IList<string> listThree = new string[] { "bar", "foo", "quux" };

      Assert.IsTrue(comparer.Equals(listOne, listTwo), "Lists are equal");
      Assert.IsFalse(comparer.Equals(listOne, listThree), "Lists are not equal");
    }
  }
}

