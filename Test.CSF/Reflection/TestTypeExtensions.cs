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
    public void TestGetInterface()
    {
      Type testType = typeof(Bar);

      Assert.AreEqual(typeof(IMarker), testType.GetInterface<IMarker>(), "Implements IMarker");
      Assert.IsNull(testType.GetInterface<IDisposable>(), "Does not implement IDisposable");
    }

    [Test]
    public void TestImplementsInterface()
    {
      Type testType = typeof(Bar);

      Assert.IsTrue(testType.ImplementsInterface<IMarker>(), "Implements IMarker");
      Assert.IsFalse(testType.ImplementsInterface<IDisposable>(), "Does not implement IDisposable");
    }

    [Test]
    public void TestGetInterfaceGeneric()
    {
      Type testType = typeof(Baz);

      Assert.AreEqual(typeof(IMarker<int>), testType.GetInterface<IMarker<int>>(), "Implements IMarker<int>");
      Assert.IsNull(testType.GetInterface<IDisposable>(), "Does not implement IDisposable");
    }

    [Test]
    public void TestImplementsInterfaceGeneric()
    {
      Type testType = typeof(Baz);

      Assert.IsTrue(testType.ImplementsInterface<IMarker<int>>(), "Implements IMarker<int>");
      Assert.IsFalse(testType.ImplementsInterface<IDisposable>(), "Does not implement IDisposable");
    }

    [Test]
    public void TestGetMangledName()
    {
      Assert.AreEqual("Test.CSF.Reflection.TestTypeExtensions+Baz",
                      typeof(Baz).GetMangledName(),
                      "Correct for non-generic type");
      Assert.AreEqual("Test.CSF.Reflection.TestTypeExtensions+IMarker`1",
                      typeof(IMarker<int>).GetMangledName(),
                      "Correct for generic type");
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

