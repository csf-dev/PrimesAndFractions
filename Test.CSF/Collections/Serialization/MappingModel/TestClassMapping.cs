using System;
using Moq;
using NUnit.Framework;
using CSF.Collections.Serialization.MappingModel;
using CSF.Reflection;

namespace Test.CSF.Collections.Serialization.MappingModel
{
  [TestFixture]
  public class TestClassMapping
  {
    #region validation tests

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
    [ExpectedException(ExceptionType = typeof(InvalidMappingException))]
    public void TestValidateMappingsAndMapAs()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();

      mapping.Mappings.Add(new Mock<IMapping>().Object);
      mapping.Mappings.Add(new Mock<IMapping>().Object);
      mapping.MapAs = new Mock<IMapping>().Object;

      mapping.Validate();
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException))]
    public void TestValidateInvalid()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      mapping.Validate();
    }

    #endregion

    #region general tests

    [Test]
    public void TestConstructor()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      Assert.IsNotNull(mapping);
    }

    [Test]
    [Description("This tests the default factory method somewhat indirectly - by invoking it after construction")]
    public void TestDefaultFactoryMethodOK()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      Foo testOutput = mapping.FactoryMethod();
      Assert.IsNotNull(testOutput);
    }

    [Test]
    [Description(@"This tests the default factory method somewhat indirectly - by invoking it after construction.
The test should fail because the 'Bar' type does not have a parameterless constructor.")]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestDefaultFactoryMethodNotOK()
    {
      ClassMapping<Bar> mapping = new ClassMapping<Bar>();
      Bar testOutput = mapping.FactoryMethod();

      Assert.Fail("Test should not reach this point");
      Assert.IsNull(testOutput);
    }

    [Test]
    public void TestGetMapping()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var child = new Mock<IMapping>();

      child.Setup(x => x.Property).Returns(StaticReflectionUtility.GetProperty<Foo>(f => f.TestInteger));

      mapping.Mappings.Add(child.Object);
      IMapping retrieved = mapping.GetMapping<Foo>(x => x.TestInteger);

      Assert.AreSame(retrieved, child.Object);
    }

    #endregion

    #region serialization tests

    [Test]
    [Ignore("This is a placeholder for more tests to come!")]
    public void TestDeserialize()
    {
      /* TODO: Write this implementation
       * There are multiple tests that need writing here covering many things:
       * 
       * * Mandatory failures on child properties
       * * A flag failure
       * * A flag failure on a child property
       * * Successful deserialization of properties
       * * Partially missing data (IE: one property missing)
       * * Missing/no data (IE: all properties missing)
       * * Successful deserialization of map-as
       * * Missing data for map-as
       */
      throw new NotImplementedException();
    }

    [Test]
    [Ignore("This is a placeholder for more tests to come!")]
    public void TestSerialize()
    {
      /* TODO: Write this implementation
       * There are multiple tests that need writing here covering many things:
       * 
       * * Mandatory failures on child properties (nothing to serialize)
       * * Serialization writing a flag value
       * * Successful serialization of properties
       * * Partially missing data (IE: one property null)
       * * Missing/no data (IE: pass a null reference in)
       * * Successful serialization of map-as
       * * Missing data for map-as (IE: pass a null reference in)
       */
      throw new NotImplementedException();
    }

    #endregion
  }
}

