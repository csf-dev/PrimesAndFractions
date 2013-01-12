using System;
using NUnit.Framework;
using CSF.KeyValueSerializer.MappingModel;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF.KeyValueSerializer.MappingModel
{
  [TestFixture]
  public class TestCollectionMapping
  {
    #region validation tests

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException),
                       ExpectedMessage = "Collection-type mappings require a 'child' mapping, mapping the items of the collection.")]
    public void TestValidateNoMapAs()
    {
      CollectionMapping<Foo> mapping = new StubCollectionMapping();

      mapping.Validate();
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException),
                       ExpectedMessage = "This collection-type mapping is configured to use aggregate values with a separator character.  The only valid configuration is to map the collection using a 'simple' mapping but no mapping was found.")]
    public void TestValidateAggregateMissing()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        AggregateMapAs = null,
        CollectionKeyType = CollectionKeyType.Aggregate
      };

      mapping.Validate();
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException),
                       ExpectedMessage = "This collection-type mapping is configured to use aggregate values with a separator character.  The only valid configuration is to map the collection using a 'simple' mapping but a different mapping-type was found.")]
    public void TestValidateAggregateWrongType()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        AggregateMapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate
      };

      mapping.Validate();
    }

    #endregion

    #region serialization testing

    [Test]
    public void TestSerializeSeparateKeys()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string>
        result = new Dictionary<string, string>(),
        result1 = new Dictionary<string, string>(),
        result2 = new Dictionary<string, string>(),
        result3 = new Dictionary<string, string>();

      result1.Add("[0].Foo", "Bar 0");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result1, It.Is<int[]>(a => a[0] == 0))).Returns(true);

      result2.Add("[1].Foo", "Bar 1");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result2, It.Is<int[]>(a => a[0] == 1))).Returns(true);

      result3.Add("[2].Foo", "Bar 2");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result3, It.Is<int[]>(a => a[0] == 2))).Returns(true);

      ICollection<Foo> collection = new Foo[] {
        new Foo(),
        new Foo(),
        new Foo()
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result existence");
      Assert.AreEqual(3, result.Count, "Result count");
      Assert.AreEqual("Bar 0", result["[0].Foo"], "First result");
      Assert.AreEqual("Bar 1", result["[1].Foo"], "Second result");
      Assert.AreEqual("Bar 2", result["[2].Foo"], "Third result");
    }

    [Test]
    public void TestSerializeAggregateKey()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        AggregateMapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate
      };

      mapAs.SetupGet(x => x.SerializationFunction).Returns(() => x => x.TestProperty);
      namingPolicy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(x => { return namingPolicy.Object; });

      IDictionary<string,string>
        result = new Dictionary<string, string>();


      ICollection<Foo> collection = new Foo[] {
        new Foo() { TestProperty = "Bar 0" },
        new Foo() { TestProperty = "Bar 1" },
        new Foo() { TestProperty = "Bar 2" }
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result existence");
      Assert.AreEqual(1, result.Count, "Result count");
      Assert.AreEqual("Bar 0,Bar 1,Bar 2", result["Foo"], "Results");
    }

    [Test]
    public void TestSerializeSeparateKeysWithFlag()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate,
        FlagKey = "Flag",
        FlagValue = "!Flag Value!"
      };

      IDictionary<string,string>
        result = new Dictionary<string, string>(),
        result1 = new Dictionary<string, string>(),
        result2 = new Dictionary<string, string>(),
        result3 = new Dictionary<string, string>();

      result1.Add("[0].Foo", "Bar 0");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result1, It.Is<int[]>(a => a[0] == 0))).Returns(true);

      result2.Add("[1].Foo", "Bar 1");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result2, It.Is<int[]>(a => a[0] == 1))).Returns(true);

      result3.Add("[2].Foo", "Bar 2");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result3, It.Is<int[]>(a => a[0] == 2))).Returns(true);

      ICollection<Foo> collection = new Foo[] {
        new Foo(),
        new Foo(),
        new Foo()
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result existence");
      Assert.AreEqual(4, result.Count, "Result count");
      Assert.AreEqual("Bar 0", result["[0].Foo"], "First result");
      Assert.AreEqual("Bar 1", result["[1].Foo"], "Second result");
      Assert.AreEqual("Bar 2", result["[2].Foo"], "Third result");
      Assert.AreEqual("!Flag Value!", result["Flag"], "Flag result");
    }

    [Test]
    public void TestSerializeAggregateKeyWithFlag()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        AggregateMapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        FlagKey = "Flag",
        FlagValue = "!Flag Value!"
      };

      mapAs.SetupGet(x => x.SerializationFunction).Returns(() => x => x.TestProperty);
      namingPolicy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(x => { return namingPolicy.Object; });

      IDictionary<string,string>
        result = new Dictionary<string, string>();


      ICollection<Foo> collection = new Foo[] {
        new Foo() { TestProperty = "Bar 0" },
        new Foo() { TestProperty = "Bar 1" },
        new Foo() { TestProperty = "Bar 2" }
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result existence");
      Assert.AreEqual(2, result.Count, "Result count");
      Assert.AreEqual("Bar 0,Bar 1,Bar 2", result["Foo"], "Results");
      Assert.AreEqual("!Flag Value!", result["Flag"], "Flag result");
    }

    [Test]
    public void TestSerializeSeparateKeysMandatoryFailure()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string>
        result = new Dictionary<string, string>(),
        result1 = new Dictionary<string, string>(),
        result2 = new Dictionary<string, string>(),
        result3 = new Dictionary<string, string>();

      result1.Add("[0].Foo", "Bar 0");
      mapAs
        .Setup(x => x.Serialize(It.IsAny<object>(), out result1, It.Is<int[]>(a => a[0] == 0)))
        .Returns(true);

      result2.Add("[1].Foo", "Bar 1");
      mapAs
        .Setup(x => x.Serialize(It.IsAny<object>(), out result2, It.Is<int[]>(a => a[0] == 1)))
        .Throws(new MandatorySerializationException(mapAs.Object));

      result3.Add("[2].Foo", "Bar 2");
      mapAs
        .Setup(x => x.Serialize(It.IsAny<object>(), out result3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(true);

      ICollection<Foo> collection = new Foo[] {
        new Foo(),
        new Foo(),
        new Foo()
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result existence");
    }

    [Test]
    public void TestSerializeAggregateKeyMandatoryFailure()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        AggregateMapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate
      };

      mapAs.SetupGet(x => x.SerializationFunction).Returns(() => x => x.TestProperty);
      mapAs.SetupGet(x => x.Mandatory).Returns(true);
      namingPolicy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(x => { return namingPolicy.Object; });

      IDictionary<string,string>
        result = new Dictionary<string, string>();


      ICollection<Foo> collection = new Foo[] {
        new Foo() { TestProperty = "Bar 0" },
        new Foo() { TestProperty = null },
        new Foo() { TestProperty = "Bar 2" }
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result existence");
    }

    [Test]
    public void TestSerializeAggregateKeySerializationException()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      int callCount = 0;
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        AggregateMapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate
      };

      mapAs
        .SetupGet(x => x.SerializationFunction)
        .Callback(() => {
          if(++callCount == 2)
          {
            throw new InvalidOperationException("An exception was raised by the serialization function");
          }
        })
        .Returns(() => x => x.TestProperty);
      namingPolicy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(x => { return namingPolicy.Object; });

      IDictionary<string,string>
        result = new Dictionary<string, string>();


      ICollection<Foo> collection = new Foo[] {
        new Foo() { TestProperty = "Bar 0" },
        new Foo() { TestProperty = "Bar 1" },
        new Foo() { TestProperty = "Bar 2" }
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result existence");
      Assert.AreEqual(1, result.Count, "Result count");
      Assert.AreEqual("Bar 0,Bar 2", result["Foo"], "Results");
    }

    [Test]
    public void TestSerializeAggregateKeyMissingValue()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();

      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        AggregateMapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate
      };

      mapAs
        .SetupGet(x => x.SerializationFunction)
        .Returns(() => x => x.TestProperty);
      namingPolicy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("Foo");

      mapping.AttachKeyNamingPolicy<IKeyNamingPolicy>(x => { return namingPolicy.Object; });

      IDictionary<string,string>
        result = new Dictionary<string, string>();


      ICollection<Foo> collection = new Foo[] {
        new Foo() { TestProperty = "Bar 0" },
        new Foo() { TestProperty = null },
        new Foo() { TestProperty = "Bar 2" }
      };

      bool success = mapping.Serialize(collection, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result existence");
      Assert.AreEqual(1, result.Count, "Result count");
      Assert.AreEqual("Bar 0,Bar 2", result["Foo"], "Results");
    }

    [Test]
    public void TestSerializeNullValue()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string>
        result = new Dictionary<string, string>(),
        result1 = new Dictionary<string, string>(),
        result2 = new Dictionary<string, string>(),
        result3 = new Dictionary<string, string>();

      result1.Add("[0].Foo", "Bar 0");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result1, It.Is<int[]>(a => a[0] == 0))).Returns(true);

      result2.Add("[1].Foo", "Bar 1");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result2, It.Is<int[]>(a => a[0] == 1))).Returns(true);

      result3.Add("[2].Foo", "Bar 2");
      mapAs.Setup(x => x.Serialize(It.IsAny<object>(), out result3, It.Is<int[]>(a => a[0] == 2))).Returns(true);

      bool success = mapping.Serialize(null, out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result existence");
    }

    #endregion

    #region deserialization testing

    [Test]
    public void TestDeserializeSeparateKeys()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      object
        output1 = new Foo() { TestInteger = 0 },
        output2 = new Foo() { TestInteger = 1 },
        output3 = new Foo() { TestInteger = 2 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output1, It.Is<int[]>(a => a[0] == 0)))
        .Returns(true);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output2, It.Is<int[]>(a => a[0] == 1)))
        .Returns(true);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(true);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(3, output.Count, "Output count");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output1, x)), "First result");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output2, x)), "Second result");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output3, x)), "Third result");
    }

    [Test]
    public void TestDeserializeAggregateKey()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        AggregateMapAs = mapAs.Object
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("Foo", "1,2,3");
      object testOutput = new Foo() { TestInteger = 0 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out testOutput, null))
        .Returns(true);
      namingPolicy.Setup(x => x.GetKeyName(null)).Returns("Foo");

      mapping.AttachKeyNamingPolicy(x => namingPolicy.Object);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(3, output.Count, "Output count");
      Assert.AreEqual(3, output.Where(x => Object.ReferenceEquals(testOutput, x)).Count(), "Results all present");
    }

    [Test]
    public void TestDeserializeSeparateKeysFlagFailure()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate,
        FlagKey = "flag",
        FlagValue = "specific"
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("flag", "wrong");
      object
        output1 = new Foo() { TestInteger = 0 },
        output2 = new Foo() { TestInteger = 1 },
        output3 = new Foo() { TestInteger = 2 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output1, It.Is<int[]>(a => a[0] == 0)))
        .Returns(true);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output2, It.Is<int[]>(a => a[0] == 1)))
        .Returns(true);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(true);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(output, "Output null");
    }

    [Test]
    public void TestDeserializeAggregateKeyFlagFailure()
    {

      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        AggregateMapAs = mapAs.Object,
        FlagKey = "flag",
        FlagValue = "specific"
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("flag", "wrong");
      collection.Add("Foo", "1,2,3");
      object testOutput = new Foo() { TestInteger = 0 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out testOutput, null))
        .Returns(true);
      namingPolicy.Setup(x => x.GetKeyName(null)).Returns("Foo");

      mapping.AttachKeyNamingPolicy(x => namingPolicy.Object);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(output, "Output null");
    }

    [Test]
    public void TestDeserializeSeparateKeysFlagSuccess()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate,
        FlagKey = "flag",
        FlagValue = "specific"
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("flag", "specific");
      object
        output1 = new Foo() { TestInteger = 0 },
        output2 = new Foo() { TestInteger = 1 },
        output3 = new Foo() { TestInteger = 2 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output1, It.Is<int[]>(a => a[0] == 0)))
        .Returns(true);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output2, It.Is<int[]>(a => a[0] == 1)))
        .Returns(true);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(true);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(3, output.Count, "Output count");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output1, x)), "First result");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output2, x)), "Second result");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output3, x)), "Third result");
    }

    [Test]
    public void TestDeserializeAggregateKeyFlagSuccess()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        AggregateMapAs = mapAs.Object,
        FlagKey = "flag",
        FlagValue = "specific"
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("flag", "specific");
      collection.Add("Foo", "1,2,3");
      object testOutput = new Foo() { TestInteger = 0 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out testOutput, null))
        .Returns(true);
      namingPolicy.Setup(x => x.GetKeyName(null)).Returns("Foo");

      mapping.AttachKeyNamingPolicy(x => namingPolicy.Object);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(3, output.Count, "Output count");
      Assert.AreEqual(3, output.Where(x => Object.ReferenceEquals(testOutput, x)).Count(), "Results all present");
    }

    [Test]
    public void TestDeserializeSeparateKeysMandatoryFailure()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      object
        output1 = new Foo() { TestInteger = 0 },
        output2 = new Foo() { TestInteger = 1 },
        output3 = new Foo() { TestInteger = 2 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output1, It.Is<int[]>(a => a[0] == 0)))
        .Returns(true);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output2, It.Is<int[]>(a => a[0] == 1)))
        .Throws(new MandatorySerializationException(mapAs.Object));
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(true);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(output, "Output null");
    }

    [Test]
    public void TestDeserializeAggregateKeyMandatoryFailure()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        AggregateMapAs = mapAs.Object
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("Foo", "1,2,3");
      object testOutput = new Foo() { TestInteger = 0 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out testOutput, null))
        .Throws(new MandatorySerializationException(mapAs.Object));
      namingPolicy.Setup(x => x.GetKeyName(null)).Returns("Foo");

      mapping.AttachKeyNamingPolicy(x => namingPolicy.Object);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(output, "Output null");
    }

    [Test]
    public void TestDeserializeSeparateKeysException()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      object
        output1 = new Foo() { TestInteger = 0 },
        output2 = new Foo() { TestInteger = 1 },
        output3 = new Foo() { TestInteger = 2 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output1, It.Is<int[]>(a => a[0] == 0)))
        .Throws(new InvalidOperationException());
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output2, It.Is<int[]>(a => a[0] == 1)))
        .Throws(new InvalidOperationException());
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(true);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(1, output.Count, "Output count");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output3, x)), "Single result");
    }

    [Test]
    public void TestDeserializeAggregateKeyException()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        AggregateMapAs = mapAs.Object
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("Foo", "1,2,3");
      object testOutput = new Foo() { TestInteger = 0 };
      int testCount = 0;

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out testOutput, null))
        .Callback(() => {
          if(++testCount == 2)
          {
            throw new InvalidOperationException();
          }
        })
        .Returns(true);
      namingPolicy.Setup(x => x.GetKeyName(null)).Returns("Foo");

      mapping.AttachKeyNamingPolicy(x => namingPolicy.Object);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(2, output.Count, "Output count");
      Assert.AreEqual(2, output.Where(x => Object.ReferenceEquals(testOutput, x)).Count(), "Results all present");
    }

    [Test]
    public void TestDeserializeSeparateKeysNoItems()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      object
        output1 = new Foo() { TestInteger = 0 },
        output2 = new Foo() { TestInteger = 1 },
        output3 = new Foo() { TestInteger = 2 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output1, It.Is<int[]>(a => a[0] == 0)))
        .Returns(false);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output2, It.Is<int[]>(a => a[0] == 1)))
        .Returns(false);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(false);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(output, "Output null");
    }

    [Test]
    public void TestDeserializeAggregateKeyNoItems()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        AggregateMapAs = mapAs.Object
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      collection.Add("Foo", String.Empty);
      object testOutput = new Foo() { TestInteger = 0 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out testOutput, null))
        .Returns(true);
      namingPolicy.Setup(x => x.GetKeyName(null)).Returns("Foo");

      mapping.AttachKeyNamingPolicy(x => namingPolicy.Object);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(1, output.Count, "Output count");
    }

    [Test]
    public void TestDeserializeAggregateKeyKeyNotPresent()
    {
      var mapAs = new Mock<ISimpleMapping<Foo>>();
      var namingPolicy = new Mock<IKeyNamingPolicy>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Aggregate,
        AggregateMapAs = mapAs.Object
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      object testOutput = new Foo() { TestInteger = 0 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out testOutput, null))
        .Returns(true);
      namingPolicy.Setup(x => x.GetKeyName(null)).Returns("Foo");

      mapping.AttachKeyNamingPolicy(x => namingPolicy.Object);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(output, "Output null");
    }

    [Test]
    public void TestDeserializeSeparateKeysNulls()
    {
      var mapAs = new Mock<IMapping>();
      CollectionMapping<Foo> mapping = new StubCollectionMapping() {
        MapAs = mapAs.Object,
        CollectionKeyType = CollectionKeyType.Separate
      };

      IDictionary<string,string> collection = new Dictionary<string, string>();
      object
        output1 = null,
        output2 = null,
        output3 = new Foo() { TestInteger = 2 };

      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output1, It.Is<int[]>(a => a[0] == 0)))
        .Returns(false);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output2, It.Is<int[]>(a => a[0] == 1)))
        .Returns(false);
      mapAs
        .Setup(x => x.Deserialize(It.IsAny<IDictionary<string,string>>(), out output3, It.Is<int[]>(a => a[0] == 2)))
        .Returns(true);

      ICollection<Foo> output;
      bool success = mapping.Deserialize(collection, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(output, "Output null");
      Assert.AreEqual(1, output.Count, "Output count");
      Assert.IsTrue(output.Any(x => Object.ReferenceEquals(output3, x)), "Single result");
    }

    #endregion

    #region stub type

    public class StubCollectionMapping : CollectionMapping<Foo>
    {
      public IMapping AggregateMapAs;

      protected override IMapping GetAggregateMapAs()
      {
        return AggregateMapAs;
      }

      public IMapping MapAs
      {
        get { return base.BaseMapAs; }
        set { base.BaseMapAs = value; }
      }

      public StubCollectionMapping() : base(null, null, true) {}
    }

    #endregion
  }
}

