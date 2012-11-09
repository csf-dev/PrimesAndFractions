using System;
using CSF;
using NUnit.Framework;

namespace Test.CSF
{
  [TestFixture]
  public class TestNullSafe
  {
    #region tests

    [Test]
    public void TestToUlong()
    {
      ulong? val = NullSafe.ConvertTo<ulong>("5");

      Assert.IsTrue(val.HasValue);
      Assert.AreEqual(5, val.Value);
    }

    [Test]
    public void TestToDate()
    {
      DateTime? val = NullSafe.ConvertTo<DateTime>("2012-10-19");

      Assert.IsTrue(val.HasValue);
      Assert.AreEqual(new DateTime(2012, 10, 19), val.Value);
    }

    [Test]
    public void TestToFailures()
    {
      int? val = NullSafe.ConvertTo<int>("foo");

      Assert.IsFalse(val.HasValue);

      val = NullSafe.ConvertTo<int>(null);

      Assert.IsFalse(val.HasValue);
    }

    [Test]
    public void TestGet()
    {
      var root = new MockOuter() { Inner = new MockInner() { Value = 5 } };
      bool result;
      int val;

      result = NullSafe.Get(root, x => x.Inner.Value, out val);

      Assert.IsTrue(result);
      Assert.AreEqual(5, val);

      root.Inner = null;

      result = NullSafe.Get(root, x => x.Inner.Value, out val);

      Assert.IsFalse(result);
    }

    [Test]
    public void TestToString()
    {
      var root = new MockOuter() { Inner = new MockInner() { Value = 5 } };
      string result;

      result = NullSafe.ToString(root, x => x.Inner.Value);

      Assert.AreEqual("5", result);

      root.Inner = null;

      result = NullSafe.ToString(root, x => x.Inner.Value);

      Assert.IsNull(result);
    }

    #endregion

    #region contained types

    class MockOuter
    {
      public MockInner Inner { get; set; }
    }

    class MockInner
    {
      public int Value { get; set; }
    }

    #endregion
  }
}

