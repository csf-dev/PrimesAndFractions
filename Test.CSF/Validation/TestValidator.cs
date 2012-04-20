using System;
using NUnit.Framework;
using CSF.Validation;

namespace Test.CSF.Validation
{
  [TestFixture]
  public class TestValidator
  {
    #region tests
    
    [Test]
    public void TestAddTest()
    {
      IValidator<SampleObject> validator = new Validator<SampleObject>();
      SampleObject target = new SampleObject() {
        PropertyTwo = 3
      };
      
      validator.AddTest(x => x.PropertyTwo > 5);
      
      Assert.AreEqual(1, validator.Tests.Count, "Correct count of rules");
      Assert.IsNull(validator.Tests[0].Member, "Member not null");
    }
    
    [Test]
    public void TestAddTestGeneric()
    {
      IValidator<SampleObject> validator = new Validator<SampleObject>();
      SampleObject target = new SampleObject() {
        PropertyOne = "Hi"
      };
      
      validator.AddTest<string>(x => x.PropertyOne,
                                y => y.Length > 4);
      
      Assert.AreEqual(1, validator.Tests.Count, "Correct count of rules");
      Assert.IsNotNull(validator.Tests[0].Member, "Member is not null");
      Assert.AreEqual("PropertyOne", validator.Tests[0].Member.Name, "Correct member name");
    }
    
    #endregion
    
    #region contained type
    
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

