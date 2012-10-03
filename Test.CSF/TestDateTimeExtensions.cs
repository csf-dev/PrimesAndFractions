using System;
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestDateTimeExtensions
  {
    [Test]
    public void TestAsAgeInYears()
    {
      DateTime birth, reference;

      birth = new DateTime(1976, 09, 18);
      reference = new DateTime(2012, 10, 03);

      Assert.AreEqual(36, birth.AsAgeInYears(reference), reference.ToShortDateString());

      reference = new DateTime(2012, 10, 20);
      Assert.AreEqual(36, birth.AsAgeInYears(reference), reference.ToShortDateString());

      reference = new DateTime(2012, 09, 03);
      Assert.AreEqual(35, birth.AsAgeInYears(reference), reference.ToShortDateString());

      reference = new DateTime(2012, 09, 20);
      Assert.AreEqual(36, birth.AsAgeInYears(reference), reference.ToShortDateString());

      reference = new DateTime(2012, 08, 03);
      Assert.AreEqual(35, birth.AsAgeInYears(reference), reference.ToShortDateString());

      reference = new DateTime(2012, 08, 20);
      Assert.AreEqual(35, birth.AsAgeInYears(reference), reference.ToShortDateString());
    }
  }
}

