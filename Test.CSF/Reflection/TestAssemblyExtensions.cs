using System;
using System.Reflection;
using CSF.Reflection;
using NUnit.Framework;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestAssemblyExtensions
  {
    [Test]
    public void TestGetManifestResourceText()
    {
      string resourceText = Assembly.GetExecutingAssembly().GetManifestResourceText("TestResource.txt");
      Assert.AreEqual("This is a test resource file", resourceText, "Correct resource text");
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestGetManifestResourceTextInvalid()
    {
      try
      {
#pragma warning disable 219
      string resourceText = Assembly.GetExecutingAssembly().GetManifestResourceText("Nonexistent.txt");
#pragma warning restore 219
      }
      catch(InvalidOperationException ex)
      {
        Assert.IsTrue(ex.Message.StartsWith("Manifest resource 'Nonexistent.txt' was not found in the assembly 'Test.CSF, Version="));
        throw;
      }
      Assert.Fail("Test should not reach this point");
    }
  }
}

