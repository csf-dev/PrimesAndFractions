using System;
using NUnit.Framework;
using CSF.Reflection;
using System.Reflection;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestReflectionHelper
  {
    #region tests
    
    [Test]
    public void TestGetProperty()
    {
      PropertyInfo property = ReflectionHelper.GetProperty<SampleObject>(x => x.PropertyOne);
      Assert.IsNotNull(property, "Not null");
      Assert.AreEqual("PropertyOne", property.Name, "Correct name");
    }
    
    #endregion
    
    #region contained object
    
    class SampleObject
    {
      public string PropertyOne
      {
        get;
        set;
      }
      
      public int PropertyTwo
      {
        get;
        set;
      }
    }
    
    #endregion
  }
}

