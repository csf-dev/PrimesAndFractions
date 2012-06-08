using System;
using NUnit.Framework;
using CSF.Reflection;
using System.Collections.Generic;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestTypeExtensions
  {
    #region tests
    
    [Test]
    public void TestGetSubclasses()
    {
      IList<Type> types = typeof(Foo).GetSubclasses();
      
      Assert.AreEqual(2, types.Count, "Correct count");
      
      Assert.IsTrue(types.Contains(typeof(Bar)), "Contains 'bar'");
      Assert.IsTrue(types.Contains(typeof(Baz)), "Contains 'baz'");
    }
    
    #endregion
    
    #region contained classes
    
    class Foo {}
    
    class Bar : Foo {}
    
    class Baz : Bar {}
    
    #endregion
  }
}

