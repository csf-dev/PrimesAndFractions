using System;
using NUnit.Framework;
using CSF.Collections.Serialization.MappingModel;
using Moq;

namespace Test.CSF.Collections.Serialization.MappingModel
{
  [TestFixture]
  public class TestClassMapping
  {
    #region tests

    [Test]
    public void TestConstructor()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      Assert.IsNotNull(mapping);
    }

    [Test]
    public void TestValidateMapAs()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();

      mapping.MapAs = new Mock<IMapping>().Object;

      mapping.Validate();
    }

    [Test]
    public void TestValidateMappings()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();

      mapping.Mappings.Add(new Mock<IMapping>().Object);
      mapping.Mappings.Add(new Mock<IMapping>().Object);

      mapping.Validate();
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestValidateMappingsAndMapAs()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();

      mapping.Mappings.Add(new Mock<IMapping>().Object);
      mapping.Mappings.Add(new Mock<IMapping>().Object);
      mapping.MapAs = new Mock<IMapping>().Object;

      mapping.Validate();
    }

    [Test]
    public void TestFactoryMethodOK()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      Foo testOutput = mapping.FactoryMethod();
      Assert.IsNotNull(testOutput);
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestFactoryMethodNotOK()
    {
      ClassMapping<Bar> mapping = new ClassMapping<Bar>();
      Bar testOutput = mapping.FactoryMethod();

      Assert.Fail("Test should not reach this point");
      Assert.IsNull(testOutput);
    }

    #endregion
  }
}

