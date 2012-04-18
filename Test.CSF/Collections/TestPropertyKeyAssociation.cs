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
    public void TestSetProperty()
    {
      PropertyKeyAssociation<SampleObject> association = new PropertyKeyAssociation<SampleObject>();
      
      association.SetProperty(x => x.PropertyOne);
      
      Assert.IsNotNull(association.Property, "Property is not null");
      Assert.AreEqual("PropertyOne", association.Property.Name, "Correct name");
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

