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

    [Test]
    public void TestImplementsInterface()
    {
      Type testType = typeof(Bar);

      Assert.IsTrue(testType.ImplementsInterface<IMarker>(), "Implements IMarker");
      Assert.IsFalse(testType.ImplementsInterface<IDisposable>(), "Does not implement IDisposable");
    }

    [Test]
    public void TestImplementsInterfaceGeneric()
    {
      Type testType = typeof(Baz);

      Assert.IsTrue(testType.ImplementsInterface<IMarker<int>>(), "Implements IMarker<int>");
      Assert.IsFalse(testType.ImplementsInterface<IDisposable>(), "Does not implement IDisposable");
    }

    #endregion
    
    #region contained classes
    
    class Foo {}
    
    class Bar : Foo, IMarker {}
    
    class Baz : Bar, IMarker<int> {}

    interface IMarker {}

    interface IMarker<T> {}
    
    #endregion
  }
}

