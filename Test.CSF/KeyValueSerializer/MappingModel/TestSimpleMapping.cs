using System;
using NUnit.Framework;
using CSF.KeyValueSerializer.MappingModel;
using Moq;
using CSF.Reflection;
using System.Collections.Generic;

namespace Test.CSF.KeyValueSerializer.MappingModel
{
  [TestFixture]
  public class TestSimpleMapping
  {
    #region general tests

    [Test]
    public void TestDefaultSerializationDeserializationMethods()
    {
      var parent = new Mock<IMapping>();
      var property = Reflect.Property<Foo>(x => x.TestInteger);

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object, property);

      Assert.AreEqual(5, mapping.DeserializationFunction("5"), "Deserialize");
      Assert.AreEqual("-56", mapping.SerializationFunction(-56), "Serialize");
    }

    [Test]
    [ExpectedException(typeof(InvalidMappingException),
                       ExpectedMessage = "Property mapping does not have either a serialization or deserialization function - this is invalid (a useless mapping).")]
    public void TestValidate()
    {
      var parent = new Mock<IMapping>();
      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.SerializationFunction = null;
      mapping.DeserializationFunction = null;

      mapping.Validate();
    }

    #endregion

    #region serialization

    [Test]
    public void TestSerializeSuccess()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);

      IDictionary<string,string> result = new Dictionary<string, string>();
      bool success = mapping.Serialize(5, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Count, "Result count");
      Assert.AreEqual("5", result["Foo"], "Result");
    }

    [Test]
    public void TestSerializeWriteFlag()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);
      mapping.FlagKey = "flag";
      mapping.FlagValue = "val";

      IDictionary<string,string> result = new Dictionary<string, string>();
      bool success = mapping.Serialize(5, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(2, result.Count, "Result count");
      Assert.AreEqual("5", result["Foo"], "Result");
      Assert.AreEqual("val", result["flag"], "Flag");
    }

    [Test]
    public void TestSerializeFailure()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);
      mapping.SerializationFunction = x => null;

      IDictionary<string,string> result = new Dictionary<string, string>();
      bool success = mapping.Serialize(5, out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result nullability");
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestSerializeMissingSerializationFunction()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);
      mapping.SerializationFunction = null;

      IDictionary<string,string> result = new Dictionary<string, string>();
      mapping.Serialize(5, out result, new int[0]);
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(MandatorySerializationException))]
    public void TestSerializeMandatoryFailure()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);
      mapping.SerializationFunction = x => null;
      mapping.Mandatory = true;

      IDictionary<string,string> result = new Dictionary<string, string>();
      mapping.Serialize(5, out result, new int[0]);
    }

    #endregion

    #region deserialization

    [Test]
    public void TestDeserializeSuccess()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("Foo", "-12");

      int result;
      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.AreEqual(-12, result, "Result");
    }

    [Test]
    public void TestDeserializeFlagFailure()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);
      mapping.FlagKey = "Flag";

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("Foo", "-12");

      int result;
      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsFalse(success, "Success");
    }

    [Test]
    public void TestDeserializeException()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);
      mapping.DeserializationFunction = x => {
        throw new Exception("This is a test exception");
      };

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("Foo", "-12");

      int result;
      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsFalse(success, "Success");
    }

    [Test]
    public void TestDeserializeMissingData()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);

      IDictionary<string,string> data = new Dictionary<string, string>();

      int result;
      bool success = mapping.Deserialize(data, out result, new int[0]);

      Assert.IsFalse(success, "Success");
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(MandatorySerializationException))]
    public void TestDeserializeMandatoryFailure()
    {
      var parent = new Mock<IMapping>();
      var policy = new Mock<IKeyNamingPolicy>();

      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      SimpleMapping<int> mapping = new SimpleMapping<int>(parent.Object,
                                                          Reflect.Property<Foo>(x => x.TestInteger));
      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(map => policy.Object);
      mapping.Mandatory = true;

      IDictionary<string,string> data = new Dictionary<string, string>();

      int result;
      mapping.Deserialize(data, out result, new int[0]);
    }

    #endregion
  }
}

