using System;
using NUnit.Framework;
using CSF.Collections;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestPropertyKeyAssociation
  {
    #region tests

    [Test]
    public void TestConstructor()
    {
      PropertyKeyAssociation<SampleObject> association;
      
      association = new PropertyKeyAssociation<SampleObject>(x => x.PropertyOne);
      Assert.IsNotNull(association.Property, "Property not null");
      Assert.AreEqual("PropertyOne", association.Property.Name, "Correct property name");
      Assert.AreEqual("PropertyOne", association.Key, "Correct key");
      
      association = new PropertyKeyAssociation<SampleObject>(x => x.PropertyTwo);
      Assert.IsNotNull(association.Property, "Property not null");
      Assert.AreEqual("PropertyTwo", association.Property.Name, "Correct property name");
      Assert.AreEqual("PropertyTwo", association.Key, "Correct key");
    }
    
    #endregion
    
    #region test object
    
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

