using System;
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestDateTimeEqualityComparer
  {
    [Test]
    public void TestAreEqual()
    {
      DateTimeEqualityComparer comparer = new DateTimeEqualityComparer(TimeSpan.FromSeconds(1));

      DateTime now = DateTime.Now;

      Assert.IsTrue(comparer.AreEqual(now, now.AddMilliseconds(5)), "Dates are equal");
      Assert.IsFalse(comparer.AreEqual(now, now.AddMilliseconds(-1001)), "Dates are not equal");
    }
  }
}

