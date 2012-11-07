using System;
using NUnit.Framework;
using CSF.Collections.Serialization.MappingModel;
using Moq;
using CSF.Reflection;

namespace Test.CSF.Collections.Serialization.MappingModel
{
  [TestFixture]
  public class TestSimplePropertyMapping
  {
    [Test]
    public void TestDefaultSerializationDeserializationMethods()
    {
      var parent = new Mock<IMapping>();
      var property = StaticReflectionUtility.GetProperty<Foo>(x => x.TestInteger);

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object, property);

      Assert.AreEqual(5, mapping.DeserializationFunction("5"), "Deserialize");
      Assert.AreEqual("-56", mapping.SerializationFunction(-56), "Serialize");
    }

    [Test]
    [Ignore("This test is not written yet")]
    public void TestValidate()
    {

    }

    [Test]
    [Ignore("This test is not written yet")]
    public void TestSerialize()
    {

    }

    [Test]
    [Ignore("This test is not written yet")]
    public void TestDeserialize()
    {

    }
  }
}

