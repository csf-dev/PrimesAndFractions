using System;
using NUnit.Framework;
using CSF.Reflection;
using System.Reflection;
using Test.CSF.Mocks;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestStaticReflectionUtility
  {
    #region tests
    
    [Test]
    public void TestGetMember()
    {
      PropertyInfo property;
      
      property = (PropertyInfo) StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyOne);
      Assert.IsNotNull(property, "Not null");
      Assert.AreEqual("PropertyOne", property.Name, "Correct name");
      
      property = (PropertyInfo) StaticReflectionUtility.GetMember<SampleObject>(x => x.PropertyTwo);
      Assert.IsNotNull(property, "2 not null");
      Assert.AreEqual("PropertyTwo", property.Name, "2 correct name");
    }
    
    [Test]
    public void TestGetProperty()
    {
      PropertyInfo property;
      
      property = StaticReflectionUtility.GetProperty<SampleObject>(x => x.PropertyOne);
      Assert.IsNotNull(property, "Not null");
      Assert.AreEqual("PropertyOne", property.Name, "Correct name");
      
      property = StaticReflectionUtility.GetProperty<SampleObject>(x => x.PropertyTwo);
      Assert.IsNotNull(property, "2 not null");
      Assert.AreEqual("PropertyTwo", property.Name, "2 correct name");
    }

    [Test]
    public void TestGetTypeFromAppDomain()
    {
      Type targetType = StaticReflectionUtility.GetTypeFromAppDomain("CSF.Testing.Mocks.SampleClass");
      Assert.IsNotNull(targetType);
      Assert.AreEqual(typeof(global::CSF.Testing.Mocks.SampleClass), targetType, "Correct type");
    }

    [Test]
    public void TestGetMethod()
    {
      MethodInfo method = StaticReflectionUtility.GetMethod<SampleClass>(x => x.GetString());
      Assert.IsNotNull(method, "Method is not null");
      string output = (string) method.Invoke(new SampleClass(), null);
      Assert.AreEqual("I am a sample string", output, "Correct output");
    }
    
    #endregion

    #region mocks

    public class SampleClass
    {
      public string GetString()
      {
        return "I am a sample string";
      }
    }

    #endregion
  }
}

