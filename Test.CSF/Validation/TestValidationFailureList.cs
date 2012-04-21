using System;
using NUnit.Framework;
using CSF.Validation;
using Test.CSF.Mocks;
using CSF.Reflection;

namespace Test.CSF.Validation
{
  [TestFixture]
  public class TestValidationTestList
  {
    #region tests
    
    [Test]
    public void TestByMember()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByMember(x => x.PropertyOne);
      
      Assert.AreEqual(2, subset.Count, "Correct count");
      Assert.AreEqual("PropertyOne", subset[0].Member.Name, "Correct member name 1");
      Assert.AreEqual("PropertyOne", subset[1].Member.Name, "Correct member name 2");
    }
    
    [Test]
    public void TestByIdentifier()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByIdentifier("Bar");
      
      Assert.AreEqual(2, subset.Count, "Correct count");
      Assert.AreEqual("Bar", subset[0].Identifier, "Correct identifier 1");
      Assert.AreEqual("Bar", subset[1].Identifier, "Correct identifier 2");
    }
    
    [Test]
    public void TestByIdentifierAndMember()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByIdentifier("Bar", x => x.PropertyOne);
      
      Assert.AreEqual(1, subset.Count, "Correct count");
      Assert.AreEqual("Bar", subset[0].Identifier, "Correct identifier");
      Assert.AreEqual("PropertyOne", subset[0].Member.Name, "Correct member name");
    }
    
    [Test]
    public void TestByIdentifierEmpty()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByIdentifier("Spong");
      
      Assert.AreEqual(0, subset.Count, "Correct count");
    }
    
    #endregion
  }
}

