using System;
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestEnumExtensions
  {
    #region tests
    
    [Test]
    public void TestGetDescription()
    {
      Assert.AreEqual("First", SampleEnum.One.GetDescription(), "Correct description");
    }
    
    [Test]
    public void TestGetDescriptionNull()
    {
      Assert.IsNull(SampleEnum.Three.GetDescription(), "Null description");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentException))]
    public void TestGetDescriptionInvalid()
    {
      ((SampleEnum) 5).GetDescription();
      Assert.Fail("Test should not reach this point");
    }
    
    #endregion
    
    #region test enumeration
    
    enum SampleEnum : int
    {
      [System.ComponentModel.Description("First")]
      One   = 1,
      
      [System.ComponentModel.Description("Second")]
      Two   = 2,
      
      Three = 3
    }
    
    #endregion
  }
}

