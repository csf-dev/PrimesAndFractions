using System;
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestVersionExtensions
  {
    [Test]
    public void TestToSemanticVersion()
    {
      Version version = new Version(1, 2, 3, 45678);
      Assert.AreEqual("1.2.3", version.ToSemanticVersion(), "Correct version string");
    }
  }
}

