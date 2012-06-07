using System;
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestIEnumerableExtensions
  {
    [Test]
    public void TestToSeparatedString()
    {
      int[] foo = {1, 2, 3, 4, 5, 10, 20};
      Assert.AreEqual("1, 2, 3, 4, 5, 10, 20", foo.ToSeparatedString(", "), "Correct with ', ' separator");
      Assert.AreEqual("123451020", foo.ToSeparatedString(String.Empty), "Correct with empty separator");
    }
  }
}

