using System;
using NUnit.Framework;
using CSF.Collections;
using Test.CSF.Mocks;
using CSF.Reflection;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestSimplePropertyKeyMapping
  {
    #region tests

    [Test]
    public void TestConstructor()
    {
      SimplePropertyKeyMapping<SampleObject> association;
      
      association = new SimplePropertyKeyMapping<SampleObject>(StaticReflectionUtility.GetProperty<SampleObject>(x => x.PropertyOne));
      Assert.IsNotNull(association.Property, "Property not null");
      Assert.AreEqual("PropertyOne", association.Property.Name, "Correct property name");
      Assert.AreEqual("PropertyOne", association.Key, "Correct key");
      
      association = new SimplePropertyKeyMapping<SampleObject>(StaticReflectionUtility.GetProperty<SampleObject>(x => x.PropertyTwo));
      Assert.IsNotNull(association.Property, "Property not null");
      Assert.AreEqual("PropertyTwo", association.Property.Name, "Correct property name");
      Assert.AreEqual("PropertyTwo", association.Key, "Correct key");
    }
    
    #endregion
  }
}

