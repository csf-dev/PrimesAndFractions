using System;
using NUnit.Framework;
using CSF.Validation;
using Test.CSF.Mocks;
using CSF.Reflection;

namespace Test.CSF.Validation
{
  [TestFixture]
  public class TestValidationTest
  {
    #region tests
    
    [Test]
    public void TestExecute()
    {
      ValidationTest<SampleObject> test;
      test = new ValidationTest<SampleObject>(x => x.PropertyOne == "foo", null);
      
      SampleObject target = new SampleObject() {
        PropertyOne = "bar"
      };
      
      Assert.IsFalse(test.Execute(target), "Execute indicates false");
      
      target.PropertyOne = "foo";
      
      Assert.IsTrue(test.Execute(target), "Execute indicates true");
    }
    
    [Test]
    public void TestExecuteProperty()
    {
      ValidationTest<SampleObject, string> test;
      test = new ValidationTest<SampleObject, string>(x => x == "foo",
                                                      StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                      null);
      
      SampleObject target = new SampleObject() {
        PropertyOne = "bar"
      };
      
      Assert.IsFalse(test.Execute(target), "Execute indicates false");
      
      target.PropertyOne = "foo";
      
      Assert.IsTrue(test.Execute(target), "Execute indicates true");
    }
    
    [Test]
    public void TestExecuteField()
    {
      ValidationTest<SampleObject, int> test;
      test = new ValidationTest<SampleObject, int>(x => x == 4,
                                                   StaticReflectionUtility.GetMember<SampleObject>(x => x.FieldTwo),
                                                   null);
      
      SampleObject target = new SampleObject() {
        FieldTwo = 2
      };
      
      Assert.IsFalse(test.Execute(target), "Execute indicates false");
      
      target.FieldTwo = 4;
      
      Assert.IsTrue(test.Execute(target), "Execute indicates true");
    }
    
    #endregion
  }
}

