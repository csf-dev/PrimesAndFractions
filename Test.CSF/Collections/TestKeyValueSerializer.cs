using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections.Generic;
using Test.CSF.Mocks;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestKeyValueSerializer
  {
    #region mapping tests
    
    [Test]
    public void TestMap()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer.Map(x => x.PropertyOne);
      serializer.Map(x => x.PropertyTwo);
      
      Assert.AreEqual(2, serializer.Mappings.Count, "Correct count of mappings");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestMapDuplicateProperty()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer.Map(x => x.PropertyOne);
      serializer.Map(x => x.PropertyOne);
    }
    
    #endregion
    
    #region deserialization tests
    
    [Test]
    public void TestDeserialize()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer.Map(x => x.PropertyOne, "first");
      serializer.Map(x => x.PropertyTwo, "second", true);
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      collection.Add("first", "Foo bar baz");
      collection.Add("second", "5");
      
      SampleObject testObject = serializer.Deserialize(collection);
      
      Assert.IsNotNull(testObject, "Not null");
      Assert.AreEqual("Foo bar baz", testObject.PropertyOne, "Property one OK");
      Assert.AreEqual(5, testObject.PropertyTwo, "Property two OK");
    }
    
    [Test]
    [Description("Testing the deserialization process with no object data")]
    public void TestDeserializeEmptyCollection()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo);
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      collection.Add("first", "Foo bar baz");
      collection.Add("second", "5");
      
      SampleObject testObject = serializer.Deserialize(collection);
      
      Assert.IsNull(testObject, "Object is null");
    }
    
    [Test]
    [Description("Testing the deserialization process with only partial data")]
    public void TestDeserializePartialData()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo);
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      collection.Add("first", "Foo bar baz");
      collection.Add("second", "5");
      collection.Add("PropertyTwo", "37");
      
      SampleObject testObject = serializer.Deserialize(collection);
      
      Assert.IsNotNull(testObject, "Object is not null");
      Assert.IsNull(testObject.PropertyOne, "Property one is null");
      Assert.AreEqual(37, testObject.PropertyTwo, "Property two is correct");
    }
    
    [Test]
    [Description("Testing the deserialization process with missing mandatory data")]
    public void TestDeserializeMandatoryFailure()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer
        .Map(x => x.PropertyOne, true)
        .Map(x => x.PropertyTwo);
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      collection.Add("first", "Foo bar baz");
      collection.Add("second", "5");
      collection.Add("PropertyTwo", "37");
      
      SampleObject testObject = serializer.Deserialize(collection);
      
      Assert.IsNull(testObject, "Object is null");
    }
    
    [Test]
    public void TestDeserializeMany()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer.Map(x => x.PropertyOne, "first");
      serializer.Map(x => x.PropertyTwo, "second", true);
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      collection.Add("first_5", "Foo bar baz");
      collection.Add("second_5", "5");
      
      collection.Add("first_blag", "Spong wibble spong");
      collection.Add("second_blag", "25");
      
      IList<SampleObject> testObjects = serializer.DeserializeMany(collection);
      
      Assert.AreEqual(2, testObjects.Count, "Correct count");
      
      Assert.IsNotNull(testObjects[0], "First object not null");
      Assert.AreEqual("Foo bar baz", testObjects[0].PropertyOne, "Object 1, property one OK");
      Assert.AreEqual(5, testObjects[0].PropertyTwo, "Object 1, property two OK");
      Assert.AreEqual("Spong wibble spong", testObjects[1].PropertyOne, "Object 2, property one OK");
      Assert.AreEqual(25, testObjects[1].PropertyTwo, "Object 2, property two OK");
    }
    
    [Test]
    [Description("Further tests on the 'deserialize many' functionality, using a non-default format string")]
    public void TestDeserializeManyWithFormat()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo)
        .SetListFormat("{0}[{1}]");
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      collection.Add("PropertyOne[0]", "Foo bar baz");
      collection.Add("PropertyTwo[0]", "5");
      collection.Add("PropertyOne[1]", "Sample data");
      collection.Add("PropertyTwo[2]", "100");
      
      IList<SampleObject> testObjects = serializer.DeserializeMany(collection);
      
      Assert.AreEqual(3, testObjects.Count, "Correct count");
      
      Assert.IsNotNull(testObjects[0], "First object not null");
      Assert.IsNotNull(testObjects[1], "Second object not null");
      Assert.IsNotNull(testObjects[2], "Third object not null");
      
      Assert.AreEqual("Foo bar baz", testObjects[0].PropertyOne, "Object 1, property 1 OK");
      Assert.AreEqual(5, testObjects[0].PropertyTwo, "Object 1, property 2 OK");
      
      Assert.AreEqual("Sample data", testObjects[1].PropertyOne, "Object 2, property 1 OK");
      Assert.AreEqual(0, testObjects[1].PropertyTwo, "Object 2, property 2 OK");
      
      Assert.IsNull(testObjects[2].PropertyOne, "Object 3, property 1 OK");
      Assert.AreEqual(100, testObjects[2].PropertyTwo, "Object 3, property 2 OK");
    }
    
    [Test]
    [Description("Testing the 'deserialize many' functionality with an object that becomes null")]
    public void TestDeserializeManyMandatory()
    {
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      
      serializer
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo, true)
        .SetListFormat("{0}[{1}]");
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      collection.Add("PropertyOne[0]", "Foo bar baz");
      collection.Add("PropertyTwo[0]", "5");
      collection.Add("PropertyOne[1]", "Sample data");
      collection.Add("PropertyTwo[2]", "100");
      
      IList<SampleObject> testObjects = serializer.DeserializeMany(collection);
      
      Assert.AreEqual(2, testObjects.Count, "Correct count");
      
      Assert.IsNotNull(testObjects[0], "First object not null");
      Assert.IsNotNull(testObjects[1], "Second object not null");
      
      Assert.AreEqual("Foo bar baz", testObjects[0].PropertyOne, "Object 1, property 1 OK");
      Assert.AreEqual(5, testObjects[0].PropertyTwo, "Object 1, property 2 OK");
      
      Assert.IsNull(testObjects[1].PropertyOne, "Object 2, property 1 OK");
      Assert.AreEqual(100, testObjects[1].PropertyTwo, "Object 2, property 2 OK");
    }
    
    #endregion
    
    #region serialization tests
    
    [Test]
    public void TestSerialize()
    {
      SampleObject obj = new SampleObject() {
        PropertyOne = "Flim flam wobble",
        PropertyTwo = 28
      };
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      serializer
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo);
      
      serializer.Serialize(obj, collection);
      
      Assert.IsTrue(collection.ContainsKey("PropertyOne"), "Contains property one");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo"), "Contains property two");
      
      Assert.AreEqual(obj.PropertyOne, collection["PropertyOne"], "Property one correct");
      Assert.AreEqual(obj.PropertyTwo.ToString(), collection["PropertyTwo"], "Property two correct");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestSerializeMissingMandatory()
    {
      SampleObject obj = new SampleObject() {
        PropertyTwo = 28
      };
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      new KeyValueSerializer<SampleObject>()
        .Map(x => x.PropertyOne, true)
        .Map(x => x.PropertyTwo)
        .Serialize(obj, collection);
      
      Assert.Fail("Test should not reach this point");
    }
    
    [Test]
    [Description("Attempts to serialize an object that has a null property value.")]
    public void TestSerializeNull()
    {
      SampleObject obj = new SampleObject() {
        PropertyTwo = 28
      };
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      IKeyValueSerializer<SampleObject> serializer = new KeyValueSerializer<SampleObject>();
      serializer
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo);
      
      serializer.Serialize(obj, collection);
      
      Assert.IsFalse(collection.ContainsKey("PropertyOne"), "Does not contain contain property one");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo"), "Contains property two");
      
      Assert.AreEqual(obj.PropertyTwo.ToString(), collection["PropertyTwo"], "Property two correct");
    }
    
    [Test]
    public void TestSerializeMany()
    {
      IList<SampleObject> objects = new SampleObject[] {
        new SampleObject() {
          PropertyOne = "Sample data",
          PropertyTwo = 5
        },
        new SampleObject() {
          PropertyOne = "Foo bar baz",
          PropertyTwo = -69
        },
        new SampleObject() {
          PropertyOne = "Spong wibble spong",
          PropertyTwo = 3
        }
      };
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      new KeyValueSerializer<SampleObject>()
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo)
        .SerializeMany(objects, collection);
      
      Assert.IsTrue(collection.ContainsKey("PropertyOne_0"), "Contains PropertyOne key for first object");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo_0"), "Contains PropertyTwo key for first object");
      Assert.IsTrue(collection.ContainsKey("PropertyOne_1"), "Contains PropertyOne key for second object");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo_1"), "Contains PropertyTwo key for second object");
      Assert.IsTrue(collection.ContainsKey("PropertyOne_2"), "Contains PropertyOne key for third object");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo_2"), "Contains PropertyTwo key for third object");
      
      Assert.AreEqual("Sample data", collection["PropertyOne_0"], "Object 1, property 1 correct");
      Assert.AreEqual("5", collection["PropertyTwo_0"], "Object 1, property 2 correct");
      
      Assert.AreEqual("Foo bar baz", collection["PropertyOne_1"], "Object 2, property 1 correct");
      Assert.AreEqual("-69", collection["PropertyTwo_1"], "Object 2, property 2 correct");
      
      Assert.AreEqual("Spong wibble spong", collection["PropertyOne_2"], "Object 3, property 1 correct");
      Assert.AreEqual("3", collection["PropertyTwo_2"], "Object 3, property 2 correct");
    }
    
    [Test]
    public void TestSerializeManyWithFormat()
    {
      IList<SampleObject> objects = new SampleObject[] {
        new SampleObject() {
          PropertyOne = "Sample data",
          PropertyTwo = 5
        },
        new SampleObject() {
          PropertyOne = "Foo bar baz",
          PropertyTwo = -69
        },
        new SampleObject() {
          PropertyOne = "Spong wibble spong",
          PropertyTwo = 3
        }
      };
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      new KeyValueSerializer<SampleObject>()
        .Map(x => x.PropertyOne)
        .Map(x => x.PropertyTwo)
        .SetListFormat("{0}:({1})")
        .SerializeMany(objects, collection);
      
      Assert.IsTrue(collection.ContainsKey("PropertyOne:(0)"), "Contains PropertyOne key for first object");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo:(0)"), "Contains PropertyTwo key for first object");
      Assert.IsTrue(collection.ContainsKey("PropertyOne:(1)"), "Contains PropertyOne key for second object");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo:(1)"), "Contains PropertyTwo key for second object");
      Assert.IsTrue(collection.ContainsKey("PropertyOne:(2)"), "Contains PropertyOne key for third object");
      Assert.IsTrue(collection.ContainsKey("PropertyTwo:(2)"), "Contains PropertyTwo key for third object");
      
      Assert.AreEqual("Sample data", collection["PropertyOne:(0)"], "Object 1, property 1 correct");
      Assert.AreEqual("5", collection["PropertyTwo:(0)"], "Object 1, property 2 correct");
      
      Assert.AreEqual("Foo bar baz", collection["PropertyOne:(1)"], "Object 2, property 1 correct");
      Assert.AreEqual("-69", collection["PropertyTwo:(1)"], "Object 2, property 2 correct");
      
      Assert.AreEqual("Spong wibble spong", collection["PropertyOne:(2)"], "Object 3, property 1 correct");
      Assert.AreEqual("3", collection["PropertyTwo:(2)"], "Object 3, property 2 correct");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestSerializeManyMissingMandatory()
    {
      IList<SampleObject> objects = new SampleObject[] {
        new SampleObject() {
          PropertyOne = "Sample data",
          PropertyTwo = 5
        },
        new SampleObject() {
          PropertyTwo = -69
        },
        new SampleObject() {
          PropertyOne = "Spong wibble spong",
          PropertyTwo = 3
        }
      };
      
      IDictionary<string, string> collection = new Dictionary<string, string>();
      
      new KeyValueSerializer<SampleObject>()
        .Map(x => x.PropertyOne, true)
        .Map(x => x.PropertyTwo)
        .SerializeMany(objects, collection);
      
      Assert.Fail("Test should not reach this point");
    }
    
    #endregion
  }
}

