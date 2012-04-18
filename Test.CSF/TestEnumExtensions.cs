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
    public void TestGetUIText()
    {
      Assert.AreEqual("First", SampleEnum.One.GetUIText(), "Correct description");
    }
    
    [Test]
    public void TestGetUITextNull()
    {
      Assert.IsNull(SampleEnum.Three.GetUIText(), "Null description");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentException))]
    public void TestGetUITextInvalid()
    {
      ((SampleEnum) 5).GetUIText();
      Assert.Fail("Test should not reach this point");
    }
    
    #endregion
    
    #region test enumeration
    
    enum SampleEnum : int
    {
      [UIText("First")]
      One   = 1,
      
      [UIText("Second")]
      Two   = 2,
      
      Three = 3
    }
    
    #endregion
  }
}

