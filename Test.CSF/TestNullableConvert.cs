using System;
using CSF;
using NUnit.Framework;

namespace Test.CSF
{
  [TestFixture]
  public class TestNullableConvert
  {
    [Test]
    public void TestToUlong()
    {
      ulong? val = NullableConvert.To<ulong>("5");

      Assert.IsTrue(val.HasValue);
      Assert.AreEqual(5, val.Value);
    }

    [Test]
    public void TestToDate()
    {
      DateTime? val = NullableConvert.To<DateTime>("2012-10-19");

      Assert.IsTrue(val.HasValue);
      Assert.AreEqual(new DateTime(2012, 10, 19), val.Value);
    }

    [Test]
    public void TestToFailures()
    {
      int? val = NullableConvert.To<int>("foo");

      Assert.IsFalse(val.HasValue);

      val = NullableConvert.To<int>(null);

      Assert.IsFalse(val.HasValue);
    }
  }
}

