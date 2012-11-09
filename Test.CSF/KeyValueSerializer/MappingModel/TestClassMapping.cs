using System;
using Moq;
using NUnit.Framework;
using CSF.KeyValueSerializer.MappingModel;
using CSF.Reflection;
using System.Collections.Generic;

namespace Test.CSF.KeyValueSerializer.MappingModel
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

      child.Setup(x => x.Property).Returns(Reflect.Property<Foo>(f => f.TestInteger));

      mapping.Mappings.Add(child.Object);
      IMapping retrieved = mapping.GetMapping<Foo>(x => x.TestInteger);

      Assert.AreSame(retrieved, child.Object);
    }

    #endregion

    #region serialization tests

    [Test]
    public void TestDeserializeMandatoryChildFailure()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>();
      var childTwo = new Mock<IMapping>();

      object outString = "This is a test";
      object outInt = 0;

      childOne
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outString, new int[0]))
        .Returns(true);
      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childTwo
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outInt, new int[0]))
        .Throws(new MandatorySerializationException(childTwo.Object));
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      Dictionary<string,string> data = new Dictionary<string, string>();
      Foo result;

      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsFalse(success, "Not successful due to mandatory failure");
      Assert.IsNull(result, "Result null due to failure");
    }

    [Test]
    public void TestDeserializeFlagFailure()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>();
      var childTwo = new Mock<IMapping>();

      object outString = "This is a test";
      object outInt = 6;

      childOne
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outString, new int[0]))
        .Returns(true);
      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childTwo
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outInt, new int[0]))
        .Returns(true);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);
      mapping.FlagKey = "key";

      Dictionary<string,string> data = new Dictionary<string, string>();
      Foo result;

      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsFalse(success, "Not successful - flag failure");
      Assert.IsNull(result, "Result is null");
    }

    [Test]
    public void TestDeserializeChildFailure()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>();
      var childTwo = new Mock<IMapping>();

      object outString = "This is a test";
      object outInt = 6;

      childOne
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outString, new int[0]))
        .Returns(true);
      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childTwo
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outInt, new int[0]))
        .Returns(false);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      Dictionary<string,string> data = new Dictionary<string, string>();
      Foo result;

      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsTrue(success, "Successful");
      Assert.IsNotNull(result, "Result not null");
      Assert.AreEqual("This is a test", result.TestProperty, "String property correct");
      Assert.AreEqual(0, result.TestInteger, "Integer property was unset because it failed to deserialize");
    }

    [Test]
    public void TestDeserializeProperties()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>();
      var childTwo = new Mock<IMapping>();

      object outString = "This is a test";
      object outInt = 6;

      childOne
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outString, new int[0]))
        .Returns(true);
      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childTwo
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outInt, new int[0]))
        .Returns(true);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      Dictionary<string,string> data = new Dictionary<string, string>();
      Foo result;

      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsTrue(success, "Successful");
      Assert.IsNotNull(result, "Result not null");
      Assert.AreEqual("This is a test", result.TestProperty, "String property correct");
      Assert.AreEqual(6, result.TestInteger, "Integer property correct");
    }

    [Test]
    public void TestDeserializeAllPropertiesMissing()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>();
      var childTwo = new Mock<IMapping>();

      object outString = "This is a test";
      object outInt = 6;

      childOne
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outString, new int[0]))
        .Returns(false);
      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childTwo
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outInt, new int[0]))
        .Returns(false);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      Dictionary<string,string> data = new Dictionary<string, string>();
      Foo result;

      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsFalse(success, "Not successful");
      Assert.IsNull(result, "Result is null");
    }

    [Test]
    public void TestDeserializeMapAs()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var child = new Mock<IMapping>();

      object outFoo = new Foo() {
        TestDateTime = DateTime.Today,
        TestInteger = 21,
        TestProperty = "Sample string"
      };

      child
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outFoo, new int[0]))
        .Returns(true);

      mapping.MapAs = child.Object;

      Dictionary<string,string> data = new Dictionary<string, string>();
      Foo result;

      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsTrue(success, "Successful");
      Assert.IsNotNull(result, "Result not null");
      Assert.AreSame(outFoo, result, "Correct result returned");
    }

    [Test]
    public void TestDeserializeMissingMapAs()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var child = new Mock<IMapping>();

      object outFoo = new Foo() {
        TestDateTime = DateTime.Today,
        TestInteger = 21,
        TestProperty = "Sample string"
      };

      child
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out outFoo, new int[0]))
        .Returns(false);

      mapping.MapAs = child.Object;

      Dictionary<string,string> data = new Dictionary<string, string>();
      Foo result;

      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsFalse(success, "Not successful");
      Assert.IsNull(result, "Result is null");
    }

    [Test]
    public void TestSerializeProperties()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);
      var childTwo = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string>
        childOneResult = new Dictionary<string, string>(),
        childTwoResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");
      childTwoResult.Add("childTwo", "childTwo result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(true);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));
      childTwo
        .Setup(x => x.Serialize(It.IsAny<object>(), out childTwoResult, new int[0]))
        .Returns(true);

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(new Foo(), out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(2, result.Count, "Result count");
      Assert.AreEqual("childOne result", result["childOne"], "Result one");
      Assert.AreEqual("childTwo result", result["childTwo"], "Result two");
    }

    [Test]
    public void TestSerializeSuccessWithFlag()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);
      var childTwo = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string>
        childOneResult = new Dictionary<string, string>(),
        childTwoResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");
      childTwoResult.Add("childTwo", "childTwo result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(true);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));
      childTwo
        .Setup(x => x.Serialize(It.IsAny<object>(), out childTwoResult, new int[0]))
        .Returns(true);

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);
      mapping.FlagKey = "flag";
      mapping.FlagValue = "val";

      IDictionary<string,string> result;
      bool success = mapping.Serialize(new Foo(), out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(3, result.Count, "Result count");
      Assert.AreEqual("childOne result", result["childOne"], "Result one");
      Assert.AreEqual("childTwo result", result["childTwo"], "Result two");
      Assert.AreEqual("val", result["flag"], "Flag value");
    }

    [Test]
    public void TestSerializeOneNullProperty()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);
      var childTwo = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string>
        childOneResult = new Dictionary<string, string>(),
        childTwoResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");
      childTwoResult.Add("childTwo", "childTwo result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(true);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));
      childTwo
        .Setup(x => x.Serialize(It.IsAny<object>(), out childTwoResult, new int[0]))
        .Returns(false);

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(new Foo(), out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Count, "Result count");
      Assert.AreEqual("childOne result", result["childOne"], "Result one");
    }

    [Test]
    public void TestSerializeMandatoryChildMandatoryFailure()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);
      var childTwo = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string>
        childOneResult = new Dictionary<string, string>(),
        childTwoResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");
      childTwoResult.Add("childTwo", "childTwo result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(true);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));
      childTwo
        .Setup(x => x.Serialize(It.IsAny<object>(), out childTwoResult, new int[0]))
        .Throws(new MandatorySerializationException(childTwo.Object));

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(new Foo(), out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result nullability");
    }

    [Test]
    public void TestSerializeAllPropertiesNull()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);
      var childTwo = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string>
        childOneResult = new Dictionary<string, string>(),
        childTwoResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");
      childTwoResult.Add("childTwo", "childTwo result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(false);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));
      childTwo
        .Setup(x => x.Serialize(It.IsAny<object>(), out childTwoResult, new int[0]))
        .Returns(false);

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(new Foo(), out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result nullability");
    }

    [Test]
    public void TestSerializeNullData()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);
      var childTwo = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string>
        childOneResult = new Dictionary<string, string>(),
        childTwoResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");
      childTwoResult.Add("childTwo", "childTwo result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(true);
      childTwo
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestInteger));
      childTwo
        .Setup(x => x.Serialize(It.IsAny<object>(), out childTwoResult, new int[0]))
        .Returns(true);

      mapping.Mappings.Add(childOne.Object);
      mapping.Mappings.Add(childTwo.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(null, out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result nullability");
    }

    [Test]
    public void TestSerializeMapAs()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string> childOneResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(true);

      mapping.MapAs = childOne.Object;

      IDictionary<string,string> result;
      bool success = mapping.Serialize(new Foo(), out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Count, "Result count");
      Assert.AreEqual("childOne result", result["childOne"], "Result one");
    }

    [Test]
    public void TestSerializeMissingMapAs()
    {
      ClassMapping<Foo> mapping = new ClassMapping<Foo>();
      var childOne = new Mock<IMapping>(MockBehavior.Strict);

      IDictionary<string,string> childOneResult = new Dictionary<string, string>();
      childOneResult.Add("childOne", "childOne result");

      childOne
        .SetupGet(x => x.Property)
        .Returns(Reflect.Property<Foo>(x => x.TestProperty));
      childOne
        .Setup(x => x.Serialize(It.IsAny<object>(), out childOneResult, new int[0]))
        .Returns(false);

      mapping.MapAs = childOne.Object;

      IDictionary<string,string> result;
      bool success = mapping.Serialize(new Foo(), out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result nullability");
    }

    #endregion
  }
}

